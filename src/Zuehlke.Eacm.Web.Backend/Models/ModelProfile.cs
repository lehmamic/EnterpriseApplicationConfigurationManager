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
            this.CreateMap<ProjectDto, ModifyProjectAttributesCommand>()
                .ForMember(dest => dest.ExpectedVersion, opt => opt.Ignore());
        }
    }
}