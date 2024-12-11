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
    public class AdminManager:IAdminService
    {
        private readonly IUnitOfWork _uow;

        public AdminManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Admin> AddAsync(Admin entity)
        {
            var existUserName = await _uow.AdminRepository.GetAsync(q => q.UserName == entity.UserName);


            if (existUserName is null)
            {
                await _uow.AdminRepository.AddAsync(entity);
                await _uow.SaveChangeAsync();
            }
            else if (existUserName is not null)
            {
                throw new Exception("Bu Kullanıcı Adı Sistemde Mevcut");
            }           
            return entity;

        }

        public async Task DeleteAsync(Admin entity)
        {
            await _uow.AdminRepository.DeleteAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<Admin>> GetAllAsync(Expression<Func<Admin, bool>> Filter = null, params string[] IncludeParameters)
        {
            return await _uow.AdminRepository.GetAllAsync(Filter, IncludeParameters);
        }

        public async Task<Admin> GetAsync(Expression<Func<Admin, bool>> Filter, params string[] IncludeParameters)
        {
            return await _uow.AdminRepository.GetAsync(Filter, IncludeParameters);
        }

        public async Task UpdateAsync(Admin entity)
        {
            await _uow.AdminRepository.UpdateAsync(entity);
            await _uow.SaveChangeAsync();
        }
    }
}
