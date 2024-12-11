using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entity.DTO.Customer
{
    public class CustomerDTOModel:CustomerDTORequest
    {
        public  Guid GUID { get; set; }
    }
}
