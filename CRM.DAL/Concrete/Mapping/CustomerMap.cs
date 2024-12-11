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
    public class CustomerMap:BaseMap<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");
            builder.Property(q => q.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(q => q.LastName).IsRequired().HasMaxLength(100);
            builder.Property(q => q.PhoneNumber).IsRequired().HasMaxLength(100);
            builder.Property(q => q.Email).IsRequired().HasMaxLength(100);
            builder.Property(q => q.UserName).IsRequired().HasMaxLength(100);
            builder.Property(q => q.Password).IsRequired().HasMaxLength(100);



            base.Configure(builder);
        }
    }
}
