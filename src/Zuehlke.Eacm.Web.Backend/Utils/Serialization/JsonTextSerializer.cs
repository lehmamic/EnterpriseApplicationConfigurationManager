using System.IO;
using Newtonsoft.Json;

namespace Zuehlke.Eacm.Web.Backend.Utils.Serialization
{
    public class JsonTextSerializer : ITextSerializer
    {
        private readonly JsonSerializer serializer;

        public JsonTextSerializer()
            : this(JsonSerializer.Create(new JsonSerializerSettings
            {
                // Allows deserializing to the actual runtime type
                TypeNameHandling = TypeNameHandling.All,

                // In a version resilient way
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            }))
        {
        }

        public JsonTextSerializer(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        #region Implementation of ITextSerializer
        public void Serialize(TextWriter writer, object graph)
        {
            var jsonWriter = new JsonTextWriter(writer);
#if DEBUG
            jsonWriter.Formatting = Formatting.Indented;
#endif

            this.serializer.Serialize(jsonWriter, graph);

            // We don't close the stream as it's owned by the message.
            writer.Flush();
        }

        public object Deserialize(TextReader reader)
        {
            var jsonReader = new JsonTextReader(reader);

            try
            {
                return this.serializer.Deserialize(jsonReader);
            }
            catch (JsonSerializationException e)
            {
                throw new TextSerializationException(e.Message, e);
            }
        }
        #endregion
    }
}