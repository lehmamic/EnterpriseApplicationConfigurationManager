using AutoMapper;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Models;
using Zuehlke.Eacm.Web.Backend.ReadModel;

namespace Zuehlke.Eacm.Web.Backend.Utils.Mapper
{
    public static class MapperConfigurationExtensions
    {
        public static void AddProfiles(this IMapperConfigurationExpression config)
        {
            config.ArgumentNotNull(nameof(config));

            config.AddProfile<ReadModelProfile>();
            config.AddProfile<ModelProfile>();
        }
    }
}