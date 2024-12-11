﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.Entity.DTO.Room
{
    public class RoomDTORequest
    {
        
        public int HotelId { get; set; }
        public string RoomNumber { get; set; }
        public string Type { get; set; }
        public int PricePerNight { get; set; }
        public bool Availability { get; set; }
        
        public Guid HotelGUID { get; set; }
        
        public Guid Guid { get; set; }

        public string HotelName { get; set; }

        public bool? IsActive { get; set; }
        
        public bool? IsDeleted { get; set; }
    }
}
