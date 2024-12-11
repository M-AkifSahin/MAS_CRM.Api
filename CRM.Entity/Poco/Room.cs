using CRM.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entity.Poco
{
    public class Room:AuditableEntity
    {
        public Room()
        {
            Reservations = new HashSet<Reservation>();
        }
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }
        public string RoomNumber { get; set; }
        public string Type { get; set; }
        public int PricePerNight { get; set; }
        public bool Availability { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
