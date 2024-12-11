using CRM.Entity.DTO.Hotel;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Hotel
{
    public class UpdateHotelValidator : AbstractValidator<HotelDTORequest>
    {
        public UpdateHotelValidator()
        {
            RuleFor(q => q.Guid).NotEmpty().WithMessage("Güncellenecek Otel Bilgisi Gelmedi");
            RuleFor(q => q.Name).NotEmpty().WithMessage("Otel Adı Boş Olamaz");
            

            //Validatorleri HotelController'ın içine tanımlama kısmında kaldık.//
        }
    }
}
