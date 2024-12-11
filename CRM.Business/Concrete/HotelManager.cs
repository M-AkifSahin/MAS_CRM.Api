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
    public class HotelManager : IHotelService
    {
        private readonly IUnitOfWork _uow;

        public HotelManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Hotel> AddAsync(Hotel entity)
        {
            await _uow.HotelRepository.AddAsync(entity);
            await _uow.SaveChangeAsync();
            return entity;
        }

        public async Task DeleteAsync(Hotel entity)
        {
            await _uow.HotelRepository.DeleteAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync(Expression<Func<Hotel, bool>> Filter = null, params string[] IncludeParameters)
        {
            return await _uow.HotelRepository.GetAllAsync(Filter, IncludeParameters);
        }

        public async Task<Hotel> GetAsync(Expression<Func<Hotel, bool>> Filter, params string[] IncludeParameters)
        {
            return await _uow.HotelRepository.GetAsync(Filter, IncludeParameters);
        }

        public async Task UpdateAsync(Hotel entity)
        {
           await _uow.HotelRepository.UpdateAsync(entity);
           await _uow.SaveChangeAsync();
        }
    }
}
