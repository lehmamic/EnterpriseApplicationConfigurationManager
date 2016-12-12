using AutoMapper;
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
                .ForMember(p => p.Project, opt => opt.Ignore());

            this.CreateMap<EntityDefinitionAdded, ConfigurationProject>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Name, opt => opt.Ignore())
                .ForMember(p => p.Description, opt => opt.Ignore())
                .ForMember(p => p.Entities, opt => opt.Ignore());

            this.CreateMap<EntityDefinitionModified, ConfigurationEntity>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.ProjectId, opt => opt.Ignore())
                .ForMember(p => p.Project, opt => opt.Ignore());

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
        }
    }
}