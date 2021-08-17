using Firebase.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TFourMusic.Models;

namespace TFourMusic.Controllers
{
    [Area("Admin")]

    public class DangNhapController : Controller
    {
        private readonly ILogger<DangNhapController> _logger;
        public static UserModel okok = new UserModel();
        public DangNhapController(ILogger<DangNhapController> logger)
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
        [Authorize]
        public IActionResult Login()
        {

          
            return Ok();
        }
        [HttpPost]
        public IActionResult SatThuc([FromBody] UserModel data)
        {
            okok = data;
            
            return Ok();
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
