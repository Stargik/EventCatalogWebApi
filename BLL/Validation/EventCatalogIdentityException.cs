using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validation
{
    public class EventCatalogIdentityException : Exception
    {
        public string Property { get; protected set; }
        public EventCatalogIdentityException()
            : base()
        {
            
        }
        public EventCatalogIdentityException(string message, string prop)
            : base(message)
        {
            Property = prop;
        }
    }
}
