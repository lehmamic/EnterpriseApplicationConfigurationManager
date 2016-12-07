using AutoMapper;
using Zuehlke.Eacm.Web.Backend.DataAccess;

namespace Zuehlke.Eacm.Web.Backend.Models
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            this.CreateMap<ConfigurationProject, ProjectDto>();
        }
    }
}