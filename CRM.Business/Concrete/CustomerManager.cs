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
    public class CustomerManager : ICustomerService
    {
        private readonly IUnitOfWork _uow;

        public CustomerManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            var existCustomer_UserName = await _uow.CustomerRepository.GetAsync(q=>q.UserName==entity.UserName);

            var existCustmer_Email = await _uow.CustomerRepository.GetAsync(q=>q.Email==entity.Email);

            if (existCustomer_UserName is null && existCustmer_Email is null)
            {
                await _uow.CustomerRepository.AddAsync(entity);
                await _uow.SaveChangeAsync();
            }
            else if (existCustomer_UserName is not null)
            {
                throw new Exception("Bu Kullanıcı Adı Sistemde Mevcut");
            }
            else if(existCustmer_Email is not null)
            {
                throw new Exception("Bu E-Posta Adresi Sistemde Mevcut");
            }
            return entity;

        }

        public async Task DeleteAsync(Customer entity)
        {
            await _uow.CustomerRepository.DeleteAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(Expression<Func<Customer, bool>> Filter = null, params string[] IncludeParameters)
        {
            return await _uow.CustomerRepository.GetAllAsync(Filter, IncludeParameters);
        }

        public async Task<Customer> GetAsync(Expression<Func<Customer, bool>> Filter, params string[] IncludeParameters)
        {
            return await _uow.CustomerRepository.GetAsync(Filter, IncludeParameters);
        }

        public async Task UpdateAsync(Customer entity)
        {
            await _uow.CustomerRepository.UpdateAsync(entity);
            await _uow.SaveChangeAsync();
        }
    }
}
