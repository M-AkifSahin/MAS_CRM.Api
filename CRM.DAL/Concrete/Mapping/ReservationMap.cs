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
    public class ReservationMap:BaseMap<Reservation>
    {
        public override void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservation");
            builder.Property(q=>q.RoomId).IsRequired().HasMaxLength(100);
            builder.Property(q => q.CustomerId).IsRequired().HasMaxLength(100);
            builder.Property(q => q.CheckInDate).IsRequired().HasMaxLength(100);
            builder.Property(q => q.CheckOutDate).IsRequired().HasMaxLength(100);
            
            builder.HasOne(q=>q.Customer).WithMany(q=>q.Reservations).HasForeignKey(q=>q.CustomerId).OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
