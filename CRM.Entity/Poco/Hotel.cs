using CRM.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entity.Poco
{
    public class Hotel:AuditableEntity
    {
        public Hotel()
        {
            Rooms = new HashSet<Room>();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }

        public IEnumerable<Room> Rooms { get; set; }
    }
}
