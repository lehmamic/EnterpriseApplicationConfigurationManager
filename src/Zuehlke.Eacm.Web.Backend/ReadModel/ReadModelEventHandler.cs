using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Handle(ProjectCreated message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.mapper.Map<ConfigurationProject>(message);
            this.dbContext.Projects.Add(project);

            this.dbContext.SaveChanges();
        }

        public void Handle(ProjectModified message)
        {
            message.ArgumentNotNull(nameof(message));

            this.UpdateProject(message);
            this.dbContext.SaveChanges();
        }

        public void Handle(EntityDefinitionAdded message)
        {
            message.ArgumentNotNull(nameof(message));

            this.AddEntity<EntityDefinitionAdded, ConfigurationEntity>(message);
            this.dbContext.SaveChanges();
        }

        public void Handle(EntityDefinitionModified message)
        {
            message.ArgumentNotNull(nameof(message));

            this.UpdateEntity<EntityDefinitionModified, ConfigurationEntity>(message, m => m.EntityId);
            this.dbContext.SaveChanges();
        }

        public void Handle(EntityDefinitionDeleted message)
        {
            message.ArgumentNotNull(nameof(message));

            this.DeleteEntity<EntityDefinitionDeleted, ConfigurationEntity>(message, m => m.EntityId);
            this.dbContext.SaveChanges();
        }

        public void Handle(PropertyDefinitionAdded message)
        {
            message.ArgumentNotNull(nameof(message));

            this.AddEntity<PropertyDefinitionAdded, ConfigurationProperty>(message);
            this.dbContext.SaveChanges();
        }

        public void Handle(PropertyDefinitionModified message)
        {
            message.ArgumentNotNull(nameof(message));

            this.UpdateEntity<PropertyDefinitionModified, ConfigurationProperty>(message, m => m.PropertyId);
            this.dbContext.SaveChanges();
        }

        public void Handle(PropertyDefinitionDeleted message)
        {
            message.ArgumentNotNull(nameof(message));

            this.DeleteEntity<PropertyDefinitionDeleted, ConfigurationProperty>(message, m => m.PropertyId);
            this.dbContext.SaveChanges();
        }

        public void Handle(ConfigurationEntryAdded message)
        {
            message.ArgumentNotNull(nameof(message));

            this.UpdateProject(message);

            var entry = this.mapper.Map<ConfigurationEntry>(message);
            this.dbContext.Entries.Add(entry);

            var values = this.mapper.Map<IEnumerable<ConfigurationValue>>(message);
            this.dbContext.Values.AddRange(values);

            this.dbContext.SaveChanges();
        }

        public void Handle(ConfigurationEntryModified message)
        {
            message.ArgumentNotNull(nameof(message));

            this.UpdateProject(message);

            IQueryable<ConfigurationValue> oldValues = this.dbContext.Values.Where(p => p.EntryId == message.EntryId);
            this.dbContext.Values.RemoveRange(oldValues);

            var newValues = this.mapper.Map<IEnumerable<ConfigurationValue>>(message);
            this.dbContext.Values.AddRange(newValues);

            this.dbContext.SaveChanges();
        }

        public void Handle(ConfigurationEntryDeleted message)
        {
            message.ArgumentNotNull(nameof(message));

            this.UpdateProject(message);

            ConfigurationEntry entry = this.dbContext.Entries.Single(p => p.Id == message.EntryId);
            this.dbContext.Entries.Remove(entry);

            this.dbContext.SaveChanges();
        }

        private void AddEntity<TEvent, TEntity>(TEvent message)
            where TEvent : IEvent
            where TEntity : class, IDataModel
        {
            this.UpdateProject(message);

            var entity = this.mapper.Map<TEntity>(message);
            this.dbContext.Set<TEntity>().Add(entity);
        }

        private void UpdateEntity<TEvent, TEntity>(TEvent message, Func<TEvent, Guid> entityIdSelector)
            where TEvent : IEvent
            where TEntity : class, IDataModel
        {
            this.UpdateProject(message);

            Guid entityId = entityIdSelector(message);

            TEntity entity = this.dbContext.Set<TEntity>().Single(p => p.Id == entityId);
            this.mapper.Map(message, entity);
        }

        private void DeleteEntity<TEvent, TEntity>(TEvent message, Func<TEvent, Guid> entityIdSelector)
            where TEvent : IEvent
            where TEntity : class, IDataModel
        {
            this.UpdateProject(message);

            Guid entityId = entityIdSelector(message);

            TEntity entity = this.dbContext.Set<TEntity>().Single(p => p.Id == entityId);
            this.dbContext.Set<TEntity>().Remove(entity);
        }

        private void UpdateProject<TEvent>(TEvent message) where TEvent : IEvent
        {
            var project = this.dbContext.Projects.Single(p => p.Id == message.Id);
            this.mapper.Map(message, project);
        }
    }
}