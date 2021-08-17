
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



using FirebaseConfig = Firebase.Auth.FirebaseConfig;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Authorization;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
  
    public class TongQuatController : Controller
    {
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
        private string Key = " https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";
       
        //public BaiHatController(ILogger<BaiHatController> logger)
        //{
        //    _logger = logger;
        //}
        public TongQuatController(IHostingEnvironment env)
        {
            _env = env;
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
        public async Task<IActionResult> LayBangBaiHat()
        {
            var firebase = new FirebaseClient(Key);       
            var baihat = await firebase
                 .Child("baihat")
                 .OnceAsync<baihatModel>();
            var data = (from bh in baihat
                       where bh.Object.daxoa == 0
                       select bh.Object).ToList();
            return Json(data);
        }
        public async Task<IActionResult> LayBangDanhSachPhatNguoiDung()
        {

            var firebase = new FirebaseClient(Key);
            var danhsachphatnguoidung = await firebase
                 .Child("danhsachphatnguoidung")
                 .OnceAsync<danhsachphatnguoidungModel>();
            var data = (from dspnd in danhsachphatnguoidung
                        select dspnd.Object).ToList();
            return Json(data);
        }
        public async Task<IActionResult> LayBangDanhSachPhatTheLoai()
        {

            var firebase = new FirebaseClient(Key);
            var danhsachphattheloai = await firebase
                 .Child("danhsachphattheloai")
                 .OnceAsync<danhsachphattheloaiModel>();
            var data = (from dsptl in danhsachphattheloai
                        select dsptl.Object ).ToList();
            return Json(data);
        }
        public async Task<IActionResult> LayBangNguoiDung()
        {
            var firebase = new FirebaseClient(Key);
            var nguoidung = await firebase
                 .Child("nguoidung")
                 .OnceAsync<nguoidungModel>();
            var data = (from nd in nguoidung
                        select nd.Object).ToList();
            return Json(data);
        }



    }
}

