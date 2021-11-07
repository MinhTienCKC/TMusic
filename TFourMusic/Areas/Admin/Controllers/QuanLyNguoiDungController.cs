
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using TFourMusic.Models;

using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Firebase.Auth;
using System.Threading;
using Firebase.Storage;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Firebase.Database;


using FirebaseConfig123 = Firebase.Auth.FirebaseConfig;
//using FirebaseConfig = Firebase.Auth.FirebaseConfig;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuanLyNguoiDungController : Controller
    {
        private readonly ILogger<QuanLyNguoiDungController> _logger;

        public QuanLyNguoiDungController(ILogger<QuanLyNguoiDungController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
        [HttpPost]
        public async Task<IActionResult> kiemTraPhanQuyen()
        {


            var heThong = User.Identity as ClaimsIdentity;
            var phanQuyen = heThong.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            if (phanQuyen == "Admin")
            {
                return Json(1);
            }
            else
            {
                return Json(2);
            }

                
        }
    }
}
