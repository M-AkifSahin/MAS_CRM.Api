using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.Entity.DTO.Reservation
{
    public class ReservationDTORequest
    {
        public int RoomId { get; set; }
        public int CustomerId { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        
        public Guid CustomerGUID { get; set; }
        
        public Guid RoomGuid { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        
        public Guid GUID { get; set; }
    }
}
