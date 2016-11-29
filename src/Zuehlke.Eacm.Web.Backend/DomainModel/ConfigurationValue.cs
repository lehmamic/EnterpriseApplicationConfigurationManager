using System;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class ConfigurationValue : ModelNode
    {
        private string currentPropertyType;

        public ConfigurationValue(IEventAggregator eventAggregator, PropertyDefinition property, object value)
            : base(eventAggregator, Guid.NewGuid())
        {
            this.Property = property.ArgumentNotNull(nameof(property));
            this.Value = value;

            this.currentPropertyType = this.Property.PropertyType;

            this.EventAggregator.Subscribe<PropertyDefinitionModified>(this.OnPropertyDefinitionModified, e => e.PropertyId == this.Property.Id);
        }

        public PropertyDefinition Property { get; }

        public object Value { get; private set; }

        private void OnPropertyDefinitionModified(PropertyDefinitionModified e)
        {
            if (this.currentPropertyType != e.PropertyType)
            {
                this.currentPropertyType = this.Property.PropertyType;
                this.Value = null;
            }
        }
    }
}
