using CRM.Api.Validation.FluentValidation.Payment;
using CRM.Api.Validation.FluentValidation.Reservation;
using CRM.Api.Validation.FluentValidation.Room;
using CRM.Business.Abstract;
using CRM.Entity.DTO.Payment;
using CRM.Entity.DTO.Reservation;
using CRM.Entity.DTO.Room;
using CRM.Entity.Poco;
using CRM.Entity.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRM.Api.Controllers
{
    [ApiController]
    [Route("CRMApi/[action]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        public ReservationController(IReservationService reservationService, ICustomerService customerService, IRoomService roomService)
        {
            _reservationService = reservationService;
            _customerService = customerService;
            _roomService = roomService;
        }

        [HttpPost("/Reservation")]
        public async Task<IActionResult> AddReservation(ReservationDTORequest reservationDTORequest)
        {
            Sonuc<bool> sonuc = new();
            AddReservationValidator addReservationValidator = new AddReservationValidator();

            var customer = await _customerService.GetAsync(q=>q.id==reservationDTORequest.CustomerId);
            var room = await _roomService.GetAsync(q=>q.id==reservationDTORequest.RoomId);
            if (addReservationValidator.Validate(reservationDTORequest).IsValid)
            {
                Reservation reservation = new Reservation();
                reservation.RoomId = room.id;
                reservation.CheckInDate = reservationDTORequest.CheckInDate;
                reservation.CheckOutDate = reservationDTORequest.CheckOutDate;
                reservation.CustomerId=customer.id;

                await _reservationService.AddAsync(reservation);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = 200;
                return Ok(sonuc);
            }
            else
            {
                List<string> validationMessage = new List<string>();

                for (int i = 0; i < addReservationValidator.Validate(reservationDTORequest).Errors.Count; i++)
                {
                    validationMessage.Add(addReservationValidator.Validate(reservationDTORequest).Errors[i].ErrorMessage);
                }
                sonuc.Data = false;
                sonuc.Mesaj = "Hata Oluştu.";
                sonuc.StatusCode = (int)HttpStatusCode.BadRequest;
                sonuc.HataBilgisi = new HataBilgisi()
                {
                    Hata = "Eksik Alanlar Mevcut",
                    HataAciklama = validationMessage
                };
                return BadRequest(sonuc);
            }
        }

        [HttpGet("/Reservations")]
        public async Task<IActionResult> GetReservations()
        {
            Sonuc<IEnumerable<ReservationDTOResponse>> sonuc = new Sonuc<IEnumerable<ReservationDTOResponse>>();

            var reservations = await _reservationService.GetAllAsync(q => q.IsActive == true,"Customer");

            if (reservations == null)
            {
                sonuc.Data = Enumerable.Empty<ReservationDTOResponse>();
                sonuc.Mesaj = "Rezervasyon Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            List<ReservationDTOResponse> reservationsListResponse = new List<ReservationDTOResponse>();

            foreach (var reservation in reservations)
            {
                ReservationDTOResponse reservationResponseDTO = new ReservationDTOResponse();
                reservationResponseDTO.RoomId = reservation.RoomId;
                reservationResponseDTO.CustomerId = reservation.CustomerId;

                reservationResponseDTO.CheckInDate = reservation.CheckInDate;
                reservationResponseDTO.CheckOutDate = reservation.CheckOutDate;
                reservationResponseDTO.Guid = reservation.Guid;

                reservationResponseDTO.CustomerId=reservation.Customer.id; // Yeni yazıldı // 

                reservationsListResponse.Add(reservationResponseDTO);
            }
            sonuc.Data = reservationsListResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpGet("/Reservation/{reservationGUID}")]
        public async Task<IActionResult> GetReservation(Guid reservationGUID)
        {
            Sonuc<ReservationDTOResponse> sonuc = new Sonuc<ReservationDTOResponse>();

            var reservation = await _reservationService.GetAsync(q => q.Guid == reservationGUID);

            if (reservation == null)
            {
                sonuc.Data = null;
                sonuc.Mesaj = "Rezervasyon Bulunamadı Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }
            else if (reservation is not null && reservation.IsDeleted is true)
            {
                sonuc.Data = null;
                sonuc.Mesaj = "Bu Rezervasyon Silinmişir.Detayları Görmek İçin Tekrar Aktif Edniniz";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            ReservationDTOResponse reservationDTOResponse = new ReservationDTOResponse();

            reservationDTOResponse.RoomId = reservation.RoomId;
            reservationDTOResponse.CustomerId = reservation.CustomerId;
            reservationDTOResponse.CheckOutDate = reservation.CheckOutDate;
            reservationDTOResponse.CheckInDate = reservation.CheckInDate;
            reservationDTOResponse.Guid = reservation.Guid;

            sonuc.Data = reservationDTOResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpDelete("/Reservation/{reservationGUID}")]
        public async Task<IActionResult> DeleteReservation(Guid reservationGUID)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            var reservation = await _reservationService.GetAsync(q => q.Guid == reservationGUID);

            if (reservation is null)
            {
                sonuc.Data = false;
                sonuc.Mesaj = "Reservasyon Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            reservation.IsActive = false;
            reservation.IsDeleted = true;
            await _reservationService.UpdateAsync(reservation);

            sonuc.Data = true;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpPut("/Reservation")]
        public async Task<IActionResult> UpdateReservation(ReservationDTORequest reservationDTO)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            var reservation = await _reservationService.GetAsync(q => q.Guid == reservationDTO.GUID);
            UpdateReservationValidator updateReservationValidator = new UpdateReservationValidator();

            if (updateReservationValidator.Validate(reservationDTO).IsValid)
            {
               

                if (reservation is null)
                {
                    sonuc.Data = false;
                    sonuc.Mesaj = "Hata Oluştu";
                    sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                    sonuc.HataBilgisi = new HataBilgisi()
                    {
                        Hata = "Parametre Hatası",
                        HataAciklama = new List<string> { "Reservasyon Bilgisi Bulunmadığından Dolayı Güncelleme İşlemi Yapılamadı" }
                    };
                    return NotFound(sonuc);
                }
                reservation.RoomId = reservationDTO.RoomId;
                //reservation.IsActive = reservationDTO.IsActive;
                //reservation.IsDeleted = reservationDTO.IsDeleted;
                reservation.CheckOutDate = reservationDTO.CheckOutDate;
                reservation.CheckInDate = reservationDTO.CheckInDate;
                reservation.Guid=reservationDTO.GUID;
               
                await _reservationService.UpdateAsync(reservation);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = (int)HttpStatusCode.OK;
                return Ok(sonuc);
            }
            else
            {
                List<string> errors = new List<string>();

                for (int i = 0; i < updateReservationValidator.Validate(reservationDTO).Errors.Count; i++)
                {
                    errors.Add(updateReservationValidator.Validate(reservationDTO).Errors[i].ErrorMessage);
                }
                return BadRequest(errors);
            }
        }
    }
}
