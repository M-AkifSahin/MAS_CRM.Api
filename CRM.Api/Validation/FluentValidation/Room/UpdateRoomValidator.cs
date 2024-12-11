using CRM.Entity.DTO.Room;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Room
{
    public class UpdateRoomValidator:AbstractValidator<RoomDTORequest>
    {
        public UpdateRoomValidator()
        {
            RuleFor(q => q.HotelId).NotEmpty().WithMessage("HotelId Boş Olamaz.");
            RuleFor(q => q.RoomNumber).NotEmpty().WithMessage("Room Number Boş Olamaz.");
            RuleFor(q => q.Type).NotEmpty().WithMessage("Type Boş Olamaz.");
            RuleFor(q => q.PricePerNight).NotEmpty().WithMessage("PricePerNight Boş Olamaz.");
            RuleFor(q => q.Availability).NotEmpty().WithMessage("Availability Boş Olamaz.");
            //RuleFor(q => q.IsActive).NotEmpty().WithMessage("IsActive Boş Olamaz.");
            //RuleFor(q => q.IsDeleted).NotEmpty().WithMessage("IsDeleted Boş Olamaz.");
            RuleFor(q => q.Guid).NotEmpty().WithMessage("Guid Boş Olamaz.");


        }
    }
}
