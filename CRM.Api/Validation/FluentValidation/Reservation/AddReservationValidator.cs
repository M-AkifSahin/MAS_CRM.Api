using CRM.Entity.DTO.Reservation;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Reservation
{
    public class AddReservationValidator:AbstractValidator<ReservationDTORequest>
    {
        public AddReservationValidator()
        {

            //RuleFor(q => q.RoomGuid).NotEmpty().WithMessage("RoomGuid Boş Olamaz");
            //RuleFor(q => q.CustomerGUID).NotEmpty().WithMessage("CustomerGuid Boş Olamaz");
            RuleFor(q => q.CheckInDate).NotEmpty().WithMessage("CheckInDate Boş Olamaz");
            RuleFor(q => q.CheckOutDate).NotEmpty().WithMessage("CheckOutDate Boş Olamaz");
        }
    }
}
