using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFourMusic.Controllers
{
    public class QuenMatKhauController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
