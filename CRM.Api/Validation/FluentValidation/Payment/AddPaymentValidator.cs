using CRM.Entity.DTO.Payment;
using FluentValidation;

namespace CRM.Api.Validation.FluentValidation.Payment
{
    public class AddPaymentValidator:AbstractValidator<PaymentRequestDTO>
    {
        public AddPaymentValidator()
        {
            RuleFor(q => q.TotalPrice).NotEmpty().WithMessage("Toplam Tutar Boş Olamaz");
            RuleFor(q => q.PaymentDate).NotEmpty().WithMessage("Ödeme Tarihi Boş Olamaz");
            RuleFor(q => q.PaymentMethod).NotEmpty().WithMessage("Ödeme Şekli Boş Olamaz");

        }
    }
}
