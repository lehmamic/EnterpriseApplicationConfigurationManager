using System;
using AutoMapper;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.Utils.Mapper
{
    public static class MappingOperationOptionsExtensions
    {
        public static void AfterMap<TSource, TDestination>(this IMappingOperationOptions options, Action<TSource, TDestination> afterFunction)
        {
            options.ArgumentNotNull(nameof(options));
            afterFunction.ArgumentNotNull(nameof(afterFunction));

            options.AfterMap((src, dest) => afterFunction((TSource) src, (TDestination) dest));
        }
    }
}
