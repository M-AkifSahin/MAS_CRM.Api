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
    public class HotelMap:BaseMap<Hotel>
    {
        public override void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("Hotel");
            builder.Property(q => q.Name).IsRequired().HasMaxLength(50);
            builder.Property(q => q.Address).IsRequired().HasMaxLength(50);
            builder.Property(q => q.PhoneNumber).IsRequired().HasMaxLength(50);
            builder.Property(q => q.Email).IsRequired().HasMaxLength(50);
            builder.Property(q => q.Website).IsRequired().HasMaxLength(50);
            builder.Property(q => q.Description).IsRequired().HasMaxLength(50);

            

            base.Configure(builder);
        }
    }
}
