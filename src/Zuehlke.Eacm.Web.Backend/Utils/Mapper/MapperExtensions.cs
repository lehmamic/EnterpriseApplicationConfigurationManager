using System;
using AutoMapper;
using Zuehlke.Eacm.Web.Backend.Commands;

namespace Zuehlke.Eacm.Web.Backend.Utils.Mapper
{
    public static class MapperExtensions
    {
        public static TDestination Map<TDestination>(this IMapper mapper, object source, Guid aggregateId, int aggregateVersion) where TDestination : IDomainCommand
        {
            return mapper.Map<TDestination>(source, o => o.AfterMap<object, object>((src, dest) =>
            {
                var command = (TDestination) dest;
                command.Id = aggregateId;
                command.ExpectedVersion = aggregateVersion;
            }));
        }
    }
}
