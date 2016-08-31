using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class ConfigurationValue : ModelNode
    {
        public ConfigurationValue(IEventAggregator eventAggregator, PropertyDefinition property, object value)
            : base(eventAggregator, property.ArgumentNotNull(nameof(property)).Id)
        {
            this.Property = property;
            this.Value = value;
        }

        public PropertyDefinition Property { get; }

        public object Value { get; private set; }
    }
}
