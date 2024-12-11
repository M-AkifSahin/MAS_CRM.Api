using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entity.DTO.Payment
{
    public class PaymentResponseDTO
    {
        public int TotalPrice { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public Guid Guid { get; set; }
    }
}
