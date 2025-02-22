﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entity.Base
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Guid = Guid.NewGuid();
        }
        public int id { get; set; }
        public Guid Guid { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
