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
    public class AdminMap:BaseMap<Admin>
    {
        public override void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admin");
            builder.Property(q => q.UserName).IsRequired().HasMaxLength(100);
            builder.Property(q => q.Password).IsRequired().HasMaxLength(100);
            base.Configure(builder);
        }
    }
}
