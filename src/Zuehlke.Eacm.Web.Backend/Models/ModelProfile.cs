using System;
using AutoMapper;
using Zuehlke.Eacm.Web.Backend.Commands;
using Zuehlke.Eacm.Web.Backend.DataAccess;

namespace Zuehlke.Eacm.Web.Backend.Models
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            this.CreateMap<ConfigurationProject, ProjectDto>();

            this.CreateMap<ProjectDto, CreateProjectCommand>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing(src => Guid.NewGuid()))
                .ForMember(dest => dest.ExpectedVersion, opt => opt.Ignore());

            this.CreateMap<ProjectDto, ModifyProjectCommand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExpectedVersion, opt => opt.Ignore());

            this.CreateMap<ConfigurationEntity, EntityDto>();

            this.CreateMap<EntityDto, CreateEntityCommand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExpectedVersion, opt => opt.Ignore());

            this.CreateMap<EntityDto, ModifyEntityCommand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExpectedVersion, opt => opt.Ignore())
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.Id));
        }
    }
}