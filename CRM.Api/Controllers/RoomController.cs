using CRM.Api.Validation.FluentValidation.Hotel;
using CRM.Api.Validation.FluentValidation.Room;
using CRM.Business.Abstract;
using CRM.Entity.DTO.Hotel;
using CRM.Entity.DTO.Room;
using CRM.Entity.Poco;
using CRM.Entity.Result;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRM.Api.Controllers
{
    [ApiController]
    [Route("CRMApi/[action]")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;

        public RoomController(IRoomService roomService, IHotelService hotelService)
        {
            _roomService = roomService;
            _hotelService = hotelService;
        }

        [HttpPost("/Room")]
        public async Task<IActionResult> AddRoom(RoomAddRequestDTO addRequestDTO) //product post !!!!!
        {

            Sonuc<bool> sonuc = new();
            var hotel = await _hotelService.GetAsync(q => q.Guid == addRequestDTO.HotelGUID);
            AddRoomValidator addRoomValidator = new AddRoomValidator();
            
            
 
            if (addRoomValidator.Validate(addRequestDTO).IsValid)
            {
                Room room = new Room();
               
                room.Availability = addRequestDTO.Availability;
                room.RoomNumber = addRequestDTO.RoomNumber;
                room.PricePerNight = addRequestDTO.PricePerNight;
                room.Type = addRequestDTO.Type;
                room.HotelId=hotel.id;

                await _roomService.AddAsync(room);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = 200;
                return Ok(sonuc);
            }
            else
            {
                List<string> validationMessage = new List<string>();

                for (int i = 0; i < addRoomValidator.Validate(addRequestDTO).Errors.Count; i++)
                {
                    validationMessage.Add(addRoomValidator.Validate(addRequestDTO).Errors[i].ErrorMessage);
                }

                sonuc.Data = false;
                sonuc.Mesaj = "Hata Oluştu.";
                sonuc.StatusCode=(int)HttpStatusCode.BadRequest;
                sonuc.HataBilgisi = new HataBilgisi()
                {
                    Hata="Eksik Alanlar Mevcut",
                    HataAciklama = validationMessage
                };
                return BadRequest(sonuc);
            }
        }

        [HttpGet("/Rooms")]
        public async Task<IActionResult> GetRooms()
        {
            Sonuc<IEnumerable<RoomDTOResponse>> sonuc = new Sonuc<IEnumerable<RoomDTOResponse>>();

            var rooms = await _roomService.GetAllAsync(q=>q.IsActive==true,"Hotel");
            

            if (rooms==null)
            {
                sonuc.Data = Enumerable.Empty<RoomDTOResponse>();
                sonuc.Mesaj = "Oda Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            List<RoomDTOResponse> roomListResponse = new List<RoomDTOResponse>();

            foreach (var room in rooms)
            {
                RoomDTOResponse roomDTO = new RoomDTOResponse();

                roomDTO.Availability = room.Availability;
                roomDTO.RoomNumber = room.RoomNumber;
                roomDTO.PricePerNight = room.PricePerNight;
                roomDTO.HotelId =room.Hotel.id;
                roomDTO.HotelName =room.Hotel.Name;
                roomDTO.Type = room.Type;
                roomDTO.Guid = room.Guid;
                
                
                
                
                

                roomListResponse.Add(roomDTO);
            }
            sonuc.Data = roomListResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpGet("/Room/{roomGuid}")]
        public async Task<IActionResult> GetRoom(Guid roomGuid)
        {
            Sonuc<RoomDTOResponse> sonuc = new Sonuc<RoomDTOResponse>();

            var room = await _roomService.GetAsync(q=>q.Guid==roomGuid,"Hotel");

            if (room == null)
            {
                sonuc.Data = null;
                sonuc.Mesaj = "Oda Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }
            else if (room is not null && room.IsDeleted is true)
            {
                sonuc.Data = null;
                sonuc.Mesaj = "Bu Hotel Silinmişir.Detayları Görmek İçin Tekrar Aktif Edniniz";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }
            RoomDTOResponse roomDTOResponse = new RoomDTOResponse();

            roomDTOResponse.HotelId = room.Hotel.id;
            roomDTOResponse.Guid = room.Guid;
            roomDTOResponse.RoomNumber = room.RoomNumber;
            roomDTOResponse.Type = room.Type;
            roomDTOResponse.PricePerNight = room.PricePerNight;
            roomDTOResponse.Availability = room.Availability;
            roomDTOResponse.HotelName = room.Hotel.Name;

            sonuc.Data = roomDTOResponse;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpDelete("/Room/{roomGuid}")]
        public async Task<IActionResult> DeleteRoom(Guid roomGuid)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();

            var room = await _roomService.GetAsync(q => q.Guid == roomGuid);

            if (room is null)
            {
                sonuc.Data = false;
                sonuc.Mesaj = "Oda Bulunamadı";
                sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                return NotFound(sonuc);
            }

            room.IsActive = false;
            room.IsDeleted = true;
            await _roomService.UpdateAsync(room);

            sonuc.Data = true;
            sonuc.Mesaj = "İşlem Başarılı";
            sonuc.StatusCode = (int)HttpStatusCode.OK;
            return Ok(sonuc);
        }

        [HttpPut("/Room")]
        public async Task<IActionResult> UpdateRoom(RoomDTORequest roomDTORequest)
        {
            Sonuc<bool> sonuc = new Sonuc<bool>();
            var hotel = await _hotelService.GetAsync(q => q.Guid == roomDTORequest.HotelGUID);
            var room = await _roomService.GetAsync(q => q.Guid == roomDTORequest.Guid);
            UpdateRoomValidator updateRoomValidator = new UpdateRoomValidator();

            if (updateRoomValidator.Validate(roomDTORequest).IsValid)
            {
                
                if (room is null)
                {
                    sonuc.Data = false;
                    sonuc.Mesaj = "Hata Oluştu";
                    sonuc.StatusCode = (int)HttpStatusCode.NotFound;
                    sonuc.HataBilgisi = new HataBilgisi()
                    {
                        Hata = "Parametre Hatası",
                        HataAciklama = new List<string> { "Oda Bilgisi Bulunmadığından Dolayı Güncelleme İşlemi Yapılamadı" }
                    };
                    return NotFound(sonuc);
                }
               
                room.HotelId = hotel.id;
                room.RoomNumber = roomDTORequest.RoomNumber;
                room.Type = roomDTORequest.Type;
                room.PricePerNight = roomDTORequest.PricePerNight;
                room.Availability = roomDTORequest.Availability;
                
                
                await _roomService.UpdateAsync(room);

                sonuc.Data = true;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = (int)HttpStatusCode.OK;
                return Ok(sonuc);
            }
            else
            {
                List<string> errors = new List<string>();

                for (int i = 0; i < updateRoomValidator.Validate(roomDTORequest).Errors.Count; i++)
                {
                    errors.Add(updateRoomValidator.Validate(roomDTORequest).Errors[i].ErrorMessage);
                }
                return BadRequest(errors);
            }
        }
    }
}
