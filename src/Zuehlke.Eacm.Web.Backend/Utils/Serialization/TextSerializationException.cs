using System;

namespace Zuehlke.Eacm.Web.Backend.Utils.Serialization
{
    public class TextSerializationException : Exception
    {
        public TextSerializationException()
        {
        }

        public TextSerializationException(string message)
            : base(message)
        {
        }
        
        public TextSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}