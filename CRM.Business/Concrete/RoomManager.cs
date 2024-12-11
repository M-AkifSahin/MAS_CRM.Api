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
    public class RoomManager : IRoomService
    {
        private readonly IUnitOfWork _uow;

        public RoomManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Room> AddAsync(Room entity)
        {
            await _uow.RoomRepository.AddAsync(entity);
            await _uow.SaveChangeAsync();
            return entity;
        }

        public async Task DeleteAsync(Room entity)
        {
            await _uow.RoomRepository.DeleteAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<Room>> GetAllAsync(Expression<Func<Room, bool>> Filter = null, params string[] IncludeParameters)
        {
           return await _uow.RoomRepository.GetAllAsync(Filter, IncludeParameters);
        }

        public async Task<Room> GetAsync(Expression<Func<Room, bool>> Filter, params string[] IncludeParameters)
        {
            return await _uow.RoomRepository.GetAsync(Filter, IncludeParameters);
        }

        public async Task UpdateAsync(Room entity)
        {
            await _uow.RoomRepository.UpdateAsync(entity);
            await _uow.SaveChangeAsync();
        }
    }
}
