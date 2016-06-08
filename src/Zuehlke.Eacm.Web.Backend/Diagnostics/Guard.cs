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

        public static string ArgumentNotEmpty(this string source, string parameterName)
        {
            if (source!= null && source.Trim() == string.Empty)
            {
                throw new ArgumentException($"The parameter {parameterName} should not be empty.", parameterName);
            }

            return source;
        }
    }
}