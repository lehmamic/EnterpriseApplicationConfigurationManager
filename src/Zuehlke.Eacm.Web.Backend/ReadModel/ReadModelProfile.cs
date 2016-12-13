﻿using AutoMapper;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.ReadModel
{
    public class ReadModelProfile : Profile
    {
        public ReadModelProfile()
        {
            this.CreateMap<ProjectCreated, ConfigurationProject>()
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<ProjectModified, ConfigurationProject>()
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<EntityDefinitionAdded, ConfigurationEntity>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => src.EntityId))
                .ForMember(p => p.ProjectId, opt => opt.MapFrom(src => src.Id))
                .ForMember(p => p.Project, opt => opt.Ignore())
                .ForMember(p => p.Properties, opt => opt.Ignore())
                .ForMember(p => p.Entries, opt => opt.Ignore());

            this.CreateMap<EntityDefinitionAdded, ConfigurationProject>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<EntityDefinitionModified, ConfigurationEntity>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.ProjectId, opt => opt.Ignore())
                .ForMember(p => p.Project, opt => opt.Ignore())
                .ForMember(p => p.Properties, opt => opt.Ignore())
                .ForMember(p => p.Entries, opt => opt.Ignore());

            this.CreateMap<EntityDefinitionModified, ConfigurationProject>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<EntityDefinitionDeleted, ConfigurationProject>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<PropertyDefinitionAdded, ConfigurationProperty>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => src.PropertyId))
                .ForMember(p => p.EntityId, opt => opt.MapFrom(src => src.ParentEntityId))
                .ForMember(p => p.Entity, opt => opt.Ignore());

            this.CreateMap<PropertyDefinitionAdded, ConfigurationProject>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<PropertyDefinitionModified, ConfigurationProperty>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => src.PropertyId))
                .ForMember(p => p.Entity, opt => opt.Ignore())
                .ForMember(p => p.EntityId, opt => opt.Ignore());

            this.CreateMap<PropertyDefinitionModified, ConfigurationProject>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<PropertyDefinitionDeleted, ConfigurationProperty>()
                .ForMember(p => p.Id, opt => opt.MapFrom(src => src.PropertyId))
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.PropertyType, opt => opt.Ignore())
                .ForMember(p => p.Entity, opt => opt.Ignore())
                .ForMember(p => p.EntityId, opt => opt.Ignore());

            this.CreateMap<PropertyDefinitionDeleted, ConfigurationProject>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());
        }
    }
}