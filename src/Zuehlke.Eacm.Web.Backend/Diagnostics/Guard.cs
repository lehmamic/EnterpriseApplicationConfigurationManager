using System;
using System.Collections.Generic;
using System.Linq;

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


        public static IEnumerable<T> ItemsNotNull<T>(this IEnumerable<T> source, string parameterName)
        {
            source.ArgumentNotNull(parameterName);

            if (source.Any(i => i == null))
            {
                throw new ArgumentException($"The parameter {parameterName} should not contain items which are null.", parameterName);
            }

            return source;
        }

        public static IEnumerable<string> ItemsNotNullOrEmpty(this IEnumerable<string> source, string parameterName)
        {
            source.ArgumentNotNull(parameterName);

            if (source.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException($"The parameter {parameterName} should not contain items which are null or empty.", parameterName);
            }

            return source;
        }


        public static T InvalidCondition<T>(this T source, Predicate<T> expectedCondition, string message, string parameterName)
        {
            source.ArgumentNotNull(parameterName);

            if (!expectedCondition(source))
            {
                throw new ArgumentException(message, parameterName);
            }

            return source;
        }
    }
}