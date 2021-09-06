using Firebase.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TFourMusic.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;
namespace TFourMusic.Controllers
{
    [Area("Admin")]

    public class DangNhapController : Controller
    {
        private readonly ILogger<DangNhapController> _logger;
        public static UserModel okok = new UserModel();
        public string url = "";
        public DangNhapController(ILogger<DangNhapController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            url = returnUrl;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync();
            return Redirect("/admin");
        }
       
        public async Task<IActionResult> kiemTra([FromBody] kiemtraModel item)
        {
            bool ok = false;
            if ( item.email =="tai@gmail.com" && item.password == "123123")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username",item.email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, item.email));
                claims.Add(new Claim(ClaimTypes.Name, item.email));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                ok = true;
                if (item.returnUrl == "")
                {
                    item.returnUrl = "/admin/tongquat";
                    return Json(item);
                }
               return Json(item);

            }

            return Json(ok);
        }
        [HttpPost]
        public IActionResult SatThuc([FromBody] UserModel data)
        {
            okok = data;
            
            return Ok();
        }
        
        public class kiemtraModel
        {
           
            public string email { set; get; }
            public string password { set; get; }
            public string returnUrl { set; get; }
        }
        public class UserModel
        {
            public string localId { set; get; }
            public string idToKen { set; get; }
            public string email { set; get; }
            public string refreshToken { set; get; }
            public string displayName { set; get; }
            public Boolean registered { set; get; }
            public string expiresIn { set; get; }
                   
        }
    }
}
