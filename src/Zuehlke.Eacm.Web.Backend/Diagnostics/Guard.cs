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

        public static string ArgumentNotNullOrEmpty(this string source, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException($"The parameter {parameterName} should not be null or empty.", parameterName);
            }

            return source;
        }
    }
}