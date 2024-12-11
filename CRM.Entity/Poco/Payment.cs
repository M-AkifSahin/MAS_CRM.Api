using CRM.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entity.Poco
{
    public class Payment:AuditableEntity
    {
        public int TotalPrice { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }

        
    }
}
