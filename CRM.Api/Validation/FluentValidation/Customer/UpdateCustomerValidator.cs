using CRM.Entity.DTO.Customer;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Customer
{
    public class UpdateCustomerValidator : AbstractValidator<CustomerDTOModel>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(q => q.FirstName).NotEmpty().WithMessage("Ad Boş Olamaz");
            RuleFor(q => q.LastName).NotEmpty().WithMessage("Soyad Boş Olamaz");
            RuleFor(q => q.PhoneNumber).NotEmpty().WithMessage("Telefon Numarası Boş Olamaz");
            RuleFor(q => q.Email).NotEmpty().WithMessage("E-Posta Boş Olamaz");
            RuleFor(q => q.GUID).NotEmpty().WithMessage("Customer Boş Olamaz");
        }
    }
}
