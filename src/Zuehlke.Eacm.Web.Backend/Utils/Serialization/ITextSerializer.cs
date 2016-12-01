using System.IO;

namespace Zuehlke.Eacm.Web.Backend.Utils.Serialization
{
    public interface ITextSerializer
    {
        void Serialize(TextWriter writer, object graph);

        object Deserialize(TextReader reader);
    }
}