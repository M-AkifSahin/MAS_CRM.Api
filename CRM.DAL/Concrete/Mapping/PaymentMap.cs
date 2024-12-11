using CRM.DAL.Concrete.Mapping.Base;
using CRM.Entity.Poco;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Concrete.Mapping
{
    public class PaymentMap:BaseMap<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");
            builder.Property(q=>q.TotalPrice).IsRequired().HasMaxLength(50);
            builder.Property(q => q.PaymentDate).IsRequired().HasMaxLength(50);
            builder.Property(q => q.PaymentMethod).IsRequired().HasMaxLength(50);

            base.Configure(builder);
        }
    }
}
