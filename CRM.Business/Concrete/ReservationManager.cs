using CRM.Business.Abstract;
using CRM.DAL.Abstract.DataManagement;
using CRM.Entity.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Business.Concrete
{
    public class ReservationManager : IReservationService
    {
        private readonly IUnitOfWork _uow;

        public ReservationManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Reservation> AddAsync(Reservation entity)
        {
            await _uow.ReservationRepository.AddAsync(entity);
            await _uow.SaveChangeAsync();
            return entity;
        }

        public async Task DeleteAsync(Reservation entity)
        {
            await _uow.ReservationRepository.DeleteAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync(Expression<Func<Reservation, bool>> Filter = null, params string[] IncludeParameters)
        {
            return await _uow.ReservationRepository.GetAllAsync(Filter, IncludeParameters);
        }

        public async Task<Reservation> GetAsync(Expression<Func<Reservation, bool>> Filter, params string[] IncludeParameters)
        {
            return await _uow.ReservationRepository.GetAsync(Filter, IncludeParameters);
        }

        public async Task UpdateAsync(Reservation entity)
        {
            await _uow.ReservationRepository.UpdateAsync(entity);   
            await _uow.SaveChangeAsync();
        }
    }
}
