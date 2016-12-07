using AutoMapper;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.ReadModel
{
    public class ReadModelProfile : Profile
    {
        public ReadModelProfile()
        {
            this.CreateMap<ProjectCreated, ConfigurationProject>();
            this.CreateMap<ProjectModified, ConfigurationProject>();
        }
    }
}