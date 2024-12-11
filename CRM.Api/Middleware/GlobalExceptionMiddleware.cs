using CRM.Entity.Result;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.Api.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                List<string>errors = new List<string>();
                errors.Add(ex.Message);
                Sonuc<bool> sonuc = new Sonuc<bool>();
                sonuc.Data = false;
                sonuc.Mesaj = "İşlem Başarısız";
                sonuc.HataBilgisi= new HataBilgisi()
                {
                    HataAciklama= errors
                };
                
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsJsonAsync(sonuc, new JsonSerializerOptions { PropertyNamingPolicy = null });

                
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
