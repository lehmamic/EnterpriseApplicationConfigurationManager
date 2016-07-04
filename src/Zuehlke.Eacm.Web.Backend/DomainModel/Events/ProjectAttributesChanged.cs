namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class ProjectAttributesChanged : EventBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
