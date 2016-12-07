﻿using System.Linq;
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

            var project = mapper.Map<ConfigurationProject>(message);
            this.dbContext.Projects.Add(project);

            this.dbContext.SaveChanges();
        }

        public void Handle(ProjectModified message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.dbContext.Projects.Single(p => p.Id == message.Id);
            mapper.Map(message, project);

            this.dbContext.SaveChanges();
        }

        public void Handle(EntityDefinitionAdded message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(EntityDefinitionModified message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(EntityDefinitionDeleted message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(PropertyDefinitionAdded message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(PropertyDefinitionModified message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(PropertyDefinitionDeleted message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(ConfigurationEntryAdded message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(ConfigurationEntryModified message)
        {
            message.ArgumentNotNull(nameof(message));
        }

        public void Handle(ConfigurationEntryDeleted message)
        {
            message.ArgumentNotNull(nameof(message));
        }
    }
}