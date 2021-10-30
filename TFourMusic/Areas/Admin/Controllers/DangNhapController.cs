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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using FirebaseConfig123 = Firebase.Auth.FirebaseConfig;
using Firebase.Database.Query;
using FireSharp.Interfaces;

namespace TFourMusic.Controllers
{
    [Area("Admin")]

    public class DangNhapController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "vHcXcNH4jYpiScpS8Fw3mSJhUj6lX3zp4kgpIM7T",
            BasePath = "https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        //IFirebaseConfig config = new FirebaseConfig
        //{
        //    AuthSecret = "MGsNSiHdXu6J2xSZoGqfod4KLmpg9dG0PSEOyoEe",
        //    BasePath = "https://musictt-9aa5f-default-rtdb.firebaseio.com/"

        //};

        //IFirebaseClient client;
        //private readonly ILogger<BaiHatController> _logger;
        private readonly IHostingEnvironment _env;
        //private static string ApiKey = "AIzaSyDXD0kXjDA_uoPLbK7fIIuxtKJC94AUnrQ";
        private static string ApiKey = "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q";
        //private static string Bucket = "musictt-9aa5f.appspot.com";
        private static string Bucket = "tfourmusic-1e3ff.appspot.com";
        private static string AuthEmail = "dang60780@gmail.com";
        private static string AuthPassword = "0362111719@TTai";
        private string Key = "https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";
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
            var firebase = new FirebaseClient(Key);

            var dino = await firebase
                .Child("csdlmoi")
                .Child("taikhoanquantri")
                .OnceAsync<taikhoanquantriModel>();
            var data = (from tkqt in dino
                       where tkqt.Object.taikhoan == item.email && tkqt.Object.matkhau == item.password
                       select tkqt.Object).ToList();
            bool ok = false;
        
            if (data.Count > 0)
            {
                if (data[0].phanquyen == 1)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim("username", item.email));
                    claims.Add(new Claim("uid", data[0].id));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, item.email));
                    claims.Add(new Claim(ClaimTypes.Name, item.email));
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
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
                else
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim("username", item.email));
                    claims.Add(new Claim("uid", data[0].id));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, item.email));
                    claims.Add(new Claim(ClaimTypes.Name, item.email));
                    claims.Add(new Claim(ClaimTypes.Role, "NhanVien"));
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
                
            }
            else
            {
                return Json(ok);
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
