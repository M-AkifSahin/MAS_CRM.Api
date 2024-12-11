using CRM.Api.Validation.FluentValidation.Customer;
using CRM.Business.Abstract;
using CRM.Entity.DTO.Customer;
using CRM.Entity.Poco;
using CRM.Entity.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace CRM.Api.Controllers
{
    [ApiController]
    [Route("CRMApi/[action]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("/Customer")]
        public async Task<IActionResult> AddCustomer(CustomerRegistrationDTORequest customerDTORequest)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();

            CustomerDTOModel model = new CustomerDTOModel()
            {
                FirstName = customerDTORequest.FirstName,
                LastName = customerDTORequest.LastName,
                Email = customerDTORequest.Email,
                PhoneNumber = customerDTORequest.PhoneNumber,
                Password= customerDTORequest.Password,
                UserName= customerDTORequest.UserName,
                
            };

            AddCustomerValidator addCustomerValidator = new AddCustomerValidator();

            if (addCustomerValidator.Validate(model).IsValid)
            {
                Customer customer = new Customer();

                customer.FirstName = customerDTORequest.FirstName;
                customer.LastName = customerDTORequest.LastName;
                customer.PhoneNumber = customerDTORequest.PhoneNumber;
                customer.Email = customerDTORequest.Email;
                customer.Password = customerDTORequest.Password;
                customer.UserName = customerDTORequest.UserName;

                await _customerService.AddAsync(customer);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = (int)HttpStatusCode.OK;
                return Ok(sonuc);

            }
            else
            {
                List<string> validationMessage = new List<string>();

                for (int i = 0; i < addCustomerValidator.Validate(model).Errors.Count; i++)
                {
                    validationMessage.Add(addCustomerValidator.Validate(model).Errors[i].ErrorMessage);
                }

                sonuc.Data = false;
                sonuc.Mesaj = "Hata Oluştu";
                sonuc.StatusCode = (int)HttpStatusCode.BadRequest;
                sonuc.HataBilgisi = new HataBilgisi()
                {
                    Hata = "Eksik Alanlar Mevcut",
                    HataAciklama = validationMessage
                };

                return BadRequest(sonuc);
            }

        }

        [HttpGet("/Customers")]
        public async Task<IActionResult> GetCustomers()
        {
            Sonuc<IEnumerable<CustomerDTOResponse>> sonuc = new Sonuc<IEnumerable<CustomerDTOResponse>>();

            var customers = await _customerService.GetAllAsync(q => q.IsActive == true);

            if (customers == null)
            {
                sonuc.Data = Enumerable.Empty<CustomerDTOResponse>();
                sonuc.Mesaj = "Kullanıcı Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            List<CustomerDTOResponse> customerListResponse = new List<CustomerDTOResponse>();

            foreach (var customer in customers)
            {
                CustomerDTOResponse customerDTOResponse = new CustomerDTOResponse();

                customerDTOResponse.Guid = customer.Guid;
                customerDTOResponse.FirstName = customer.FirstName;
                customerDTOResponse.LastName = customer.LastName;
                customerDTOResponse.Email = customer.Email;
                customerDTOResponse.PhoneNumber = customer.PhoneNumber;
                customerDTOResponse.Password = customer.Password;
                customerDTOResponse.UserName = customer.UserName;
                
                

                customerListResponse.Add(customerDTOResponse);
            }

            sonuc.Data = customerListResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }


        [HttpGet("/Customer/{customerGuid}")]
        public async Task<IActionResult> GetCustomer(Guid customerGuid)
        {
            Sonuc<CustomerDTOResponse> sonuc = new Sonuc<CustomerDTOResponse>();

            var customer = await _customerService.GetAsync(q => q.Guid == customerGuid);

            if (customer == null)
            {
                sonuc.Data = new CustomerDTOResponse();
                sonuc.Mesaj = "Müşteri Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }
            else if (customer is not null && customer.IsDeleted is true)
            {
                sonuc.Data = null;
                sonuc.Mesaj = "BU Müşteri Silinmiştir.Detayları Görmek İçin Tekrar Aktif Ediniz.";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            CustomerDTOResponse customerDTOResponse = new CustomerDTOResponse();

            customerDTOResponse.Guid = customer.Guid;
            customerDTOResponse.FirstName = customer.FirstName;
            customerDTOResponse.LastName = customer.LastName;
            customerDTOResponse.Email = customer.Email;
            customerDTOResponse.PhoneNumber = customer.PhoneNumber;
            customerDTOResponse.Password = customer.Password;
            customerDTOResponse.UserName= customer.UserName;

            sonuc.Data = customerDTOResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }




        [HttpPut("/Customer")] //Response istiyor 
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateDTORequest customerDTO)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            var customer = await _customerService.GetAsync(q => q.Guid == customerDTO.GUID);
            CustomerDTOModel model = new CustomerDTOModel()
            {
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                Email = customerDTO.Email,
                PhoneNumber = customerDTO.PhoneNumber,
                GUID = customerDTO.GUID,
                Password = customerDTO.Password,
                UserName = customerDTO.UserName,
            };

            UpdateCustomerValidator updateCustomerValidation = new UpdateCustomerValidator();

            if (updateCustomerValidation.Validate(model).IsValid)
            {

                if (customer == null)
                {
                    sonuc.Data = false;
                    sonuc.Mesaj = "Hata Oluştu";
                    sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                    sonuc.HataBilgisi = new HataBilgisi()
                    {
                        Hata = "Parametre Hatası",
                        HataAciklama = new List<string> { "Customer Bilgisi Bulunamadığından Dolayı Güncelleme İşlemi Yapılamadı." }
                    };
                    return NotFound(sonuc);
                }

                customer.FirstName = customerDTO.FirstName;
                customer.LastName = customerDTO.LastName;
                customer.Email = customerDTO.Email;
                customer.PhoneNumber = customerDTO.PhoneNumber;
                customer.Password= customerDTO.Password;
                customer.UserName = customerDTO.UserName;
                await _customerService.UpdateAsync(customer);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = (int)HttpStatusCode.OK;
                return Ok(sonuc);
            }
            else
            {
                List<string> errors = new List<string>();

                for (int i = 0; i < updateCustomerValidation.Validate(model).Errors.Count; i++)
                {
                    errors.Add(updateCustomerValidation.Validate(model).Errors[i].ErrorMessage);
                }
                return BadRequest(errors);
            }

        }

        [HttpDelete("/Customer/{customerGUID}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerGUID)
        {
            Sonuc<bool> sonuc = new();

            var customer = await _customerService.GetAsync(q => q.Guid == customerGUID);

            customer.IsActive = false;
            customer.IsDeleted = true;

            await _customerService.UpdateAsync(customer);

            sonuc.Data = true;
            sonuc.Mesaj = "İşlem Başarılı ";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }




    }
}
