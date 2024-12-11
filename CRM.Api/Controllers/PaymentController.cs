using CRM.Api.Validation.FluentValidation.Hotel;
using CRM.Api.Validation.FluentValidation.Payment;
using CRM.Business.Abstract;
using CRM.Entity.DTO.Hotel;
using CRM.Entity.DTO.Payment;
using CRM.Entity.Poco;
using CRM.Entity.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Emit;

namespace CRM.Api.Controllers
{
    [ApiController]
    [Route("CRMApi/[action]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("/Payment")]
        public async Task<IActionResult> AddPayment(PaymentRequestDTO paymentDTO)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            AddPaymentValidator addPaymentValidator = new AddPaymentValidator();

            if (addPaymentValidator.Validate(paymentDTO).IsValid)
            {
                Payment payment = new Payment();
                payment.PaymentDate = paymentDTO.PaymentDate;
                payment.PaymentMethod = paymentDTO.PaymentMethod;
                payment.TotalPrice = paymentDTO.TotalPrice;
                
                await _paymentService.AddAsync(payment);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = 200;
                return Ok(sonuc);
            }
            else
            {
                List<string> validationMessage = new List<string>();

                for (int i = 0; i < addPaymentValidator.Validate(paymentDTO).Errors.Count; i++)
                {
                    validationMessage.Add(addPaymentValidator.Validate(paymentDTO).Errors[i].ErrorMessage);
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

        [HttpGet("/Payments")]
        public async Task<IActionResult> GetPayments()
        {
            Sonuc<IEnumerable<PaymentResponseDTO>> sonuc = new Sonuc<IEnumerable<PaymentResponseDTO>>();

            var payments = await _paymentService.GetAllAsync(q => q.IsActive == true);

            if (payments == null)
            {
                sonuc.Data = Enumerable.Empty<PaymentResponseDTO>();
                sonuc.Mesaj = "Ödeme Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            List<PaymentResponseDTO> paymentListResponse = new List<PaymentResponseDTO>();

            foreach (var payment in payments)
            {
                PaymentResponseDTO paymentResponseDTO = new PaymentResponseDTO();
                paymentResponseDTO.TotalPrice = payment.TotalPrice;
                paymentResponseDTO.PaymentDate = payment.PaymentDate;
                paymentResponseDTO.PaymentMethod = payment.PaymentMethod;
                paymentResponseDTO.Guid = payment.Guid;
               
                paymentListResponse.Add(paymentResponseDTO);
            }
            sonuc.Data = paymentListResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpGet("/Payment/{paymentGUID}")]
        public async Task<IActionResult> GetPayment(Guid paymentGUID)
        {
            Sonuc<PaymentResponseDTO> sonuc = new Sonuc<PaymentResponseDTO>();

            var payment = await _paymentService.GetAsync(q => q.Guid == paymentGUID);

            if (payment == null)
            {
                sonuc.Data = null;
                sonuc.Mesaj = "Ödeme Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }
            else if (payment is not null && payment.IsDeleted is true)
            {
                sonuc.Data = null;
                sonuc.Mesaj = "Bu Ödeme Silinmişir.Detayları Görmek İçin Tekrar Aktif Edniniz";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            PaymentResponseDTO paymentResponseDTO = new PaymentResponseDTO();

            paymentResponseDTO.TotalPrice = payment.TotalPrice;
            paymentResponseDTO.PaymentDate = payment.PaymentDate;
            paymentResponseDTO.PaymentMethod = payment.PaymentMethod;
            paymentResponseDTO.Guid = payment.Guid;




            sonuc.Data = paymentResponseDTO;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpDelete("/Payment/{paymentGUID}")]
        public async Task<IActionResult> DeletePayment(Guid paymentGUID)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            var payment = await _paymentService.GetAsync(q => q.Guid == paymentGUID);

            if (payment is null)
            {
                sonuc.Data = false;
                sonuc.Mesaj = "Ödeme Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            payment.IsActive = false;
            payment.IsDeleted = true;
            await _paymentService.UpdateAsync(payment);

            sonuc.Data = true;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpPut("/Payment")]
        public async Task<IActionResult> UpdatePayment(PaymentRequestDTO paymentDTO)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            UpdatePaymentValidator updatePaymentValidator = new UpdatePaymentValidator();

            if (updatePaymentValidator.Validate(paymentDTO).IsValid)
            {
                var payment = await _paymentService.GetAsync(q => q.Guid == paymentDTO.Guid);

                if (payment is null)
                {
                    sonuc.Data = false;
                    sonuc.Mesaj = "Hata Oluştu";
                    sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                    sonuc.HataBilgisi = new HataBilgisi()
                    {
                        Hata = "Parametre Hatası",
                        HataAciklama = new List<string> { "Ödeme Bilgisi Bulunmadığından Dolayı Güncelleme İşlemi Yapılamadı" }
                    };
                    return NotFound(sonuc);
                }
                //payment.IsActive = paymentDTO.IsActive;
                //payment.IsDeleted = paymentDTO.IsDeleted;
                payment.TotalPrice = paymentDTO.TotalPrice;
                payment.PaymentDate = paymentDTO.PaymentDate;
                payment.PaymentMethod = paymentDTO.PaymentMethod;
                payment.Guid=paymentDTO.Guid;
                await _paymentService.UpdateAsync(payment);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = (int)HttpStatusCode.OK;
                return Ok(sonuc);
            }
            else
            {
                List<string> errors = new List<string>();

                for (int i = 0; i < updatePaymentValidator.Validate(paymentDTO).Errors.Count; i++)
                {
                    errors.Add(updatePaymentValidator.Validate(paymentDTO).Errors[i].ErrorMessage);
                }
                return BadRequest(errors);
            }
        }
    }
}
