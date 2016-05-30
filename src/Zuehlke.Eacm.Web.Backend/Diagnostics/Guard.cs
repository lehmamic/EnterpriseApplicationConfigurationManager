using System;

namespace Zuehlke.Eacm.Web.Backend.Diagnostics
{
    public static class Guard
    {
        public static T ArgumentNotNull<T>(this T source, string parameterName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return source;
        }
    }
}