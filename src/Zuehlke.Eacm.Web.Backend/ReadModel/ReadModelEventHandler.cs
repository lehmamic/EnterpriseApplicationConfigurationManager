using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CQRSlite.Events;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.ReadModel
{
    public class ReadModelEventHandler :
        IEventHandler<ProjectCreated>,
        IEventHandler<ProjectModified>,
        IEventHandler<EntityDefinitionAdded>,
        IEventHandler<EntityDefinitionModified>,
        IEventHandler<EntityDefinitionDeleted>,
        IEventHandler<PropertyDefinitionAdded>,
        IEventHandler<PropertyDefinitionModified>,
        IEventHandler<PropertyDefinitionDeleted>,
        IEventHandler<ConfigurationEntryAdded>,
        IEventHandler<ConfigurationEntryModified>,
        IEventHandler<ConfigurationEntryDeleted>
    {
        private readonly IMapper mapper;
        private readonly EacmDbContext dbContext;

        public ReadModelEventHandler(EacmDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext.ArgumentNotNull(nameof(dbContext));
            this.mapper = mapper.ArgumentNotNull(nameof(mapper));
        }

        public async Task Handle(ProjectCreated message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.mapper.Map<ConfigurationProject>(message);
            await this.dbContext.Projects.AddAsync(project);

			await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(ProjectModified message)
        {
            message.ArgumentNotNull(nameof(message));

			await this.UpdateProjectAsync(message);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(EntityDefinitionAdded message)
        {
            message.ArgumentNotNull(nameof(message));

			await this.AddEntityAsync<EntityDefinitionAdded, ConfigurationEntity>(message);
			await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(EntityDefinitionModified message)
        {
            message.ArgumentNotNull(nameof(message));

            await this.UpdateEntityAsync<EntityDefinitionModified, ConfigurationEntity>(message, m => m.EntityId);
			await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(EntityDefinitionDeleted message)
        {
            message.ArgumentNotNull(nameof(message));

			await this.DeleteEntityAsync<EntityDefinitionDeleted, ConfigurationEntity>(message, m => m.EntityId);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(PropertyDefinitionAdded message)
        {
            message.ArgumentNotNull(nameof(message));

            await this.AddEntityAsync<PropertyDefinitionAdded, ConfigurationProperty>(message);

            var entries = this.dbContext.Entries.Where(e => e.EntityId == message.ParentEntityId);
            var newPropertyValues = entries.Select(entry => new ConfigurationValue
            {
                Id = Guid.NewGuid(),
                EntryId = entry.Id,
                PropertyId = message.PropertyId
            });

            this.dbContext.Values.AddRange(newPropertyValues);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(PropertyDefinitionModified message)
        {
            message.ArgumentNotNull(nameof(message));

			await this.UpdateEntityAsync<PropertyDefinitionModified, ConfigurationProperty>(message, m => m.PropertyId);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(PropertyDefinitionDeleted message)
        {
            message.ArgumentNotNull(nameof(message));

            await this.DeleteEntityAsync<PropertyDefinitionDeleted, ConfigurationProperty>(message, m => m.PropertyId);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(ConfigurationEntryAdded message)
        {
            message.ArgumentNotNull(nameof(message));

			await this.UpdateProjectAsync(message);

            var entry = this.mapper.Map<ConfigurationEntry>(message);
            this.dbContext.Entries.Add(entry);

            var values = this.mapper.Map<IEnumerable<ConfigurationValue>>(message);
            this.dbContext.Values.AddRange(values);

			await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(ConfigurationEntryModified message)
        {
            message.ArgumentNotNull(nameof(message));

			await this.UpdateProjectAsync(message);

            IQueryable<ConfigurationValue> oldValues = this.dbContext.Values.Where(p => p.EntryId == message.EntryId);
            this.dbContext.Values.RemoveRange(oldValues);

            var newValues = this.mapper.Map<IEnumerable<ConfigurationValue>>(message);
            this.dbContext.Values.AddRange(newValues);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task Handle(ConfigurationEntryDeleted message)
        {
            message.ArgumentNotNull(nameof(message));

			await this.UpdateProjectAsync(message);

            ConfigurationEntry entry = this.dbContext.Entries.Single(p => p.Id == message.EntryId);
            this.dbContext.Entries.Remove(entry);

            await this.dbContext.SaveChangesAsync();
        }

		private async Task AddEntityAsync<TEvent, TEntity>(TEvent message)
            where TEvent : IEvent
            where TEntity : class, IDataModel
        {
			await this.UpdateProjectAsync(message);

            var entity = this.mapper.Map<TEntity>(message);
            await this.dbContext.Set<TEntity>().AddAsync(entity);
        }

		private async Task UpdateEntityAsync<TEvent, TEntity>(TEvent message, Func<TEvent, Guid> entityIdSelector)
            where TEvent : IEvent
            where TEntity : class, IDataModel
        {
			await this.UpdateProjectAsync(message);

            Guid entityId = entityIdSelector(message);

			TEntity entity = await this.dbContext.Set<TEntity>()
			                           .ToAsyncEnumerable()
			                           .Single(p => p.Id == entityId);
			
            this.mapper.Map(message, entity);
        }

		private async Task DeleteEntityAsync<TEvent, TEntity>(TEvent message, Func<TEvent, Guid> entityIdSelector)
            where TEvent : IEvent
            where TEntity : class, IDataModel
        {
			await this.UpdateProjectAsync(message);

            Guid entityId = entityIdSelector(message);

			TEntity entity = await this.dbContext.Set<TEntity>()
			                     .ToAsyncEnumerable()
								 .Single(p => p.Id == entityId);
			
			this.dbContext.Set<TEntity>().Remove(entity);
        }

		private async Task UpdateProjectAsync<TEvent>(TEvent message) where TEvent : IEvent
        {
            var project = await this.dbContext.Projects
			                  .ToAsyncEnumerable()
			                  .Single(p => p.Id == message.Id);
			
            this.mapper.Map(message, project);
        }
    }
}