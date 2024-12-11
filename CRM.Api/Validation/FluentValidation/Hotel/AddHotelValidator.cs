using CRM.Entity.DTO.Hotel;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Hotel
{
    public class AddHotelValidator : AbstractValidator<HotelAddRequestDTO>
    {
        public AddHotelValidator()
        {
            RuleFor(q => q.Name).NotEmpty().WithMessage("Otel Adı Boş Olamaz");
            RuleFor(q => q.Address).NotEmpty().WithMessage("Adres Boş Olamaz");
            RuleFor(q => q.PhoneNumber).NotEmpty().WithMessage("Telefon Numarası  Boş Olamaz");
            RuleFor(q => q.Email).NotEmpty().WithMessage("E-Posta Boş Olamaz");
            RuleFor(q => q.Website).NotEmpty().WithMessage("Web Site Boş Olamaz");
            RuleFor(q => q.Description).NotEmpty().WithMessage("Açıklama Boş Olamaz");
        }
    }
}
