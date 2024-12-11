using CRM.Api.Validation.FluentValidation.Hotel;
using CRM.Business.Abstract;
using CRM.Entity.DTO.Hotel;
using CRM.Entity.Poco;
using CRM.Entity.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace CRM.Api.Controllers
{
    [ApiController]
    [Route("CRMApi/[aciton]")]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost("/Hotel")]
        public async Task<IActionResult> AddHotel(HotelAddRequestDTO hotelAddRequestDTO)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            AddHotelValidator addHotelValidator = new AddHotelValidator();



            if (addHotelValidator.Validate(hotelAddRequestDTO).IsValid)
            {
                Hotel hotel = new Hotel();
                hotel.Name = hotelAddRequestDTO.Name;
                hotel.Address = hotelAddRequestDTO.Address;
                hotel.PhoneNumber = hotelAddRequestDTO.PhoneNumber;
                hotel.Email = hotelAddRequestDTO.Email;
                hotel.Website = hotelAddRequestDTO.Website;
                hotel.Description = hotelAddRequestDTO.Description;
                await _hotelService.AddAsync(hotel);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode=(int)HttpStatusCode.OK;
                return Ok(sonuc);
            }
            else
            {
                List<string> validationMessage = new List<string>();

                for (int i = 0; i < addHotelValidator.Validate(hotelAddRequestDTO).Errors.Count; i++)
                {
                    validationMessage.Add(addHotelValidator.Validate(hotelAddRequestDTO).Errors[i].ErrorMessage);
                }
                sonuc.Data = false;
                sonuc.Mesaj = "Hata Oluştu";
                sonuc.StatusCode=(int)HttpStatusCode.BadRequest;
                sonuc.HataBilgisi = new HataBilgisi()
                {
                    Hata="Eksik Alanlar Mevcut",
                    HataAciklama= validationMessage
                };
                return BadRequest(sonuc);
            }
        }


        
        [HttpGet("/Hotels")]
        [ProducesResponseType<Sonuc<List<HotelDTOResponse>>>(200)] //Swaggerda response döner.//
        public async Task<IActionResult> GetHotels()
        {
            Sonuc<IEnumerable<HotelDTOResponse>> sonuc = new();

            var hotels = await _hotelService.GetAllAsync(q=>q.IsActive==true);

            if (hotels==null)
            {
                sonuc.Data=Enumerable.Empty<HotelDTOResponse>();
                sonuc.Mesaj = "Hotel Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;

                return NotFound(sonuc);
            }

            List<HotelDTOResponse> hotelListResponse = new List<HotelDTOResponse>();

            foreach (var hotel in hotels)
            {
                HotelDTOResponse hotelDTOResponse = new HotelDTOResponse();

                hotelDTOResponse.Name = hotel.Name;
                hotelDTOResponse.HotelGUID = hotel.Guid;
                hotelDTOResponse.Address = hotel.Address;
                hotelDTOResponse.PhoneNumber = hotel.PhoneNumber;
                hotelDTOResponse.Email = hotel.Email;
                hotelDTOResponse.Website= hotel.Website;
                hotelDTOResponse.Description = hotel.Description;
                


                hotelListResponse.Add(hotelDTOResponse);
            }
            sonuc.Data=hotelListResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);

        }

        [HttpGet("/Hotel/{guid}")]
        public async Task<IActionResult> GetHotel(Guid guid)
        {
            Sonuc<HotelDTOResponse> sonuc = new();
            var hotel = await _hotelService.GetAsync(q=>q.Guid==guid);

            if (hotel is null)
            {
                sonuc.Data= null;
                sonuc.Mesaj = "Hotel Bulunamadı";
                sonuc.StatusCode=(int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }
            else if (hotel is not null && hotel.IsDeleted is true)
            {
                sonuc.Data= null;
                sonuc.Mesaj = "Bu Hotel Silinmişir.Detayları Görmek İçin Tekrar Aktif Edniniz";
                sonuc.StatusCode=(int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }
            HotelDTOResponse hotelDTOResponse = new HotelDTOResponse();

            hotelDTOResponse.Name = hotel.Name;
            hotelDTOResponse.HotelGUID = hotel.Guid;
            hotelDTOResponse.Address = hotel.Address;
            hotelDTOResponse.PhoneNumber = hotel.PhoneNumber;
            hotelDTOResponse.Email= hotel.Email;
            hotelDTOResponse.Website = hotel.Website;
            hotelDTOResponse.Description= hotel.Description;

            sonuc.Data = hotelDTOResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpDelete("/Hotel/{guid}")]
        public async Task<IActionResult> DeleteHotel(Guid guid)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            var hotel = await _hotelService.GetAsync (q=>q.Guid==guid);

            if (hotel is null)
            {
                sonuc.Data=false;
                sonuc.Mesaj = "Hotel Bulunamadı";
                sonuc.StatusCode =(int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            hotel.IsActive = false;
            hotel.IsDeleted = true;
            await _hotelService.UpdateAsync(hotel);

            sonuc.Data=true;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode=(int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpPut("/Hotel")]
        public async Task<IActionResult> UpdateHotel(HotelDTORequest hotelDTORequest)
        
        {
            Sonuc<bool> sonuc =new Sonuc<bool>();
            UpdateHotelValidator updateHotelValidator = new UpdateHotelValidator();

            if (updateHotelValidator.Validate(hotelDTORequest).IsValid)
            {
                var hotel = await _hotelService.GetAsync(q => q.Guid == hotelDTORequest.Guid);

                if (hotel is null)
                {
                    sonuc.Data=false;
                    sonuc.Mesaj = "Hata Oluştu";
                    sonuc.StatusCode=(int)HttpStatusCode.NotFound ;
                    sonuc.HataBilgisi = new HataBilgisi()
                    {
                        Hata="Parametre Hatası",
                        HataAciklama= new List<string> {"Hotel Bilgisi Bulunmadığından Dolayı Güncelleme İşlemi Yapılamadı" }
                    };
                    return NotFound(sonuc);
                }
                
                hotel.Name = hotelDTORequest.Name;
                hotel.Address = hotelDTORequest.Address;
                hotel.PhoneNumber = hotelDTORequest.PhoneNumber;
                hotel.Email = hotelDTORequest.Email;
                hotel.Website = hotelDTORequest.Website;
                hotel.Description = hotelDTORequest.Description;
                await _hotelService.UpdateAsync(hotel);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = (int)HttpStatusCode.OK;
                return Ok(sonuc);
            }
            else
            {
                List<string> errors = new List<string>();

                for (int i = 0; i < updateHotelValidator.Validate(hotelDTORequest).Errors.Count; i++)
                {
                    errors.Add(updateHotelValidator.Validate(hotelDTORequest).Errors[i].ErrorMessage);
                }
                return BadRequest(errors);
            }
        }
    }
}
