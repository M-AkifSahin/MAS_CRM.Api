using CRM.Entity.DTO.Room;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Room
{
    public class AddRoomValidator:AbstractValidator<RoomAddRequestDTO>
    {
        public AddRoomValidator()
        {
           
            RuleFor(q => q.RoomNumber).NotEmpty().WithMessage("Room Number Boş Olamaz.");
            RuleFor(q => q.Type).NotEmpty().WithMessage("Type Boş Olamaz.");
            RuleFor(q => q.PricePerNight).NotEmpty().WithMessage("PricePerNight Boş Olamaz.");
            RuleFor(q => q.Availability).NotEmpty().WithMessage("Availability Boş Olamaz.");

        }
    }
}
