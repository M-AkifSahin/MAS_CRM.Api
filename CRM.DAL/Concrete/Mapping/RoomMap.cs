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
    public class RoomMap:BaseMap<Room>
    {
        public override void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Room");
            builder.Property(q=>q.HotelId).IsRequired().HasMaxLength(50);
            builder.Property(q => q.RoomNumber).IsRequired().HasMaxLength(50);
            builder.Property(q => q.Type).IsRequired().HasMaxLength(50);
            builder.Property(q => q.PricePerNight).IsRequired().HasMaxLength(50);
            builder.Property(q => q.Availability).IsRequired().HasMaxLength(50);

            builder.HasOne(q=>q.Hotel).WithMany(q=>q.Rooms).HasForeignKey(q=>q.HotelId).OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
