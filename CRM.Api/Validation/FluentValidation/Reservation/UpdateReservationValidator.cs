using CRM.Entity.DTO.Reservation;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Reservation
{
    public class UpdateReservationValidator:AbstractValidator<ReservationDTORequest>
    {
        public UpdateReservationValidator()
        {
            RuleFor(q => q.RoomId).NotEmpty().WithMessage("RoomId Boş Olamaz");
            RuleFor(q => q.CheckInDate).NotEmpty().WithMessage("CheckInDate Boş Olamaz");
            RuleFor(q => q.CheckOutDate).NotEmpty().WithMessage("CheckOutDate Boş Olamaz");

            //RuleFor(q => q.GUID).NotEmpty().WithMessage("Guid Boş Olamaz");
        }
    }
}
