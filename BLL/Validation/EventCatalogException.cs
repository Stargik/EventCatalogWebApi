
namespace BLL.Validation
{
    public class EventCatalogException : Exception
    {
        public string Property { get; protected set; }
        public EventCatalogException(string message, string prop)
            : base(message)
        {
            Property = prop;
        }
    }
}
