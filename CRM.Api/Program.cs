
using CRM.Api.Middleware;
using CRM.Business.Abstract;
using CRM.Business.Concrete;
using CRM.DAL.Abstract.DataManagement;
using CRM.DAL.Concrete;
using CRM.DAL.Concrete.EntityFramework.DataManagement;

namespace CRM.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CRMContext>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            builder.Services.AddScoped<IHotelService, HotelManager>();
            builder.Services.AddScoped<ICustomerService, CustomerManager>();
            builder.Services.AddScoped<IPaymentService, PaymentManager>();
            builder.Services.AddScoped<IRoomService, RoomManager>();
            builder.Services.AddScoped<IReservationService, ReservationManager>();
            builder.Services.AddScoped<IAdminService, AdminManager>();


            var app = builder.Build();

            app.UseGlobalExceptionMiddleware();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorizationMiddleware();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
