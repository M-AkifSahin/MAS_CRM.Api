using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Abstract.DataManagement
{
    public interface IUnitOfWork
    {
        public ICustomerRepository CustomerRepository { get;  }
        public IHotelRepository HotelRepository { get;  }
        public IPaymentRepository PaymentRepository { get;  }
        public IReservationRepository ReservationRepository { get; }
        public IRoomRepository RoomRepository { get;  }
        public IAdminRepository AdminRepository { get; }

        Task<int> SaveChangeAsync();

        
    }
}
