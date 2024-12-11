using CRM.Business.Abstract;
using CRM.Entity.DTO.Login;
using CRM.Entity.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CRM.Api.Controllers
{
    [ApiController]
    [Route("CRMApi/[action]")]
    public class AccountController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IConfiguration _configuration;

        public AccountController(IAdminService adminService, IConfiguration configuration)
        {
            _adminService = adminService;
            _configuration = configuration;
        }

        [HttpPost("/Login")]
        public async Task<IActionResult> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            var admin = await _adminService.GetAsync(q=>q.UserName == loginRequestDTO.KullaniciAdi && q.Password==loginRequestDTO.Sifre);

            if (admin is null)
            {
                Sonuc<LoginResponseDTO> sonuc = new Sonuc<LoginResponseDTO>();
                LoginResponseDTO loginResponseDTO = new LoginResponseDTO();

                sonuc.Data = loginResponseDTO;
                sonuc.Mesaj = "Hata Oluştur";
                sonuc.StatusCode=(int)HttpStatusCode.NotFound;
                sonuc.HataBilgisi = new HataBilgisi()
                {
                    Hata = "Login Hatası",
                    HataAciklama = new List<string>()
                    {
                        "Kullanıcı Bulunamadı"
                    }
                };
                return NotFound(sonuc);
            }
            else
            {
                var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:JWTKey"));

                var claims = new List<Claim>();

                claims.Add(new Claim("Ad", admin.FirstName));
                claims.Add(new Claim("Soyad", admin.LastName));
                claims.Add(new Claim("KullaniciAdi", admin.UserName));

                var jwt = new JwtSecurityToken(
                    expires:DateTime.Now.AddDays(30),
                    claims: claims,
                    issuer:"http://hotelreservationsystem.com",
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                    
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                Sonuc<LoginResponseDTO> sonuc = new Sonuc<LoginResponseDTO>();

                LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Email = admin.Email,
                    PhoneNumber = admin.PhoneNumber,
                    Token=token,
                };

                sonuc.Data = loginResponseDTO;
                sonuc.Mesaj = "İşlem Başarılı";
                sonuc.StatusCode = (int)HttpStatusCode.OK;
                return Ok(sonuc);
            }
        }
    }
}
