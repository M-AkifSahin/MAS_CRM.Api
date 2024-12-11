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
    public class PaymentManager : IPaymentService
    {
        private readonly IUnitOfWork _uow;

        public PaymentManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Payment> AddAsync(Payment entity)
        {
            
            await _uow.PaymentRepository.AddAsync(entity);
            await _uow.SaveChangeAsync();
            return entity;
        }

        public async Task DeleteAsync(Payment entity)
        {
            await _uow.PaymentRepository.DeleteAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(Expression<Func<Payment, bool>> Filter = null, params string[] IncludeParameters)
        {
            return await _uow.PaymentRepository.GetAllAsync(Filter, IncludeParameters);
        }

        public async Task<Payment> GetAsync(Expression<Func<Payment, bool>> Filter, params string[] IncludeParameters)
        {
            return await _uow.PaymentRepository.GetAsync(Filter, IncludeParameters);
        }

        public async Task UpdateAsync(Payment entity)
        {
            await _uow.PaymentRepository.UpdateAsync(entity);
            await _uow.SaveChangeAsync();
        }
    }
}
