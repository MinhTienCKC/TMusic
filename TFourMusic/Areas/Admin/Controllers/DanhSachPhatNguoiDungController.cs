
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
  
    public class DanhSachPhatNguoiDungController : Controller
    {
        //IFirebaseConfig config = new FirebaseConfig
        //{
        //    AuthSecret = "MGsNSiHdXu6J2xSZoGqfod4KLmpg9dG0PSEOyoEe",
        //    BasePath = "https://musictt-9aa5f-default-rtdb.firebaseio.com/"

        //};

        //IFirebaseClient client;
        //private readonly ILogger<TheLoaiController> _logger;
        private readonly IHostingEnvironment _env;
        //private static string ApiKey = "AIzaSyDXD0kXjDA_uoPLbK7fIIuxtKJC94AUnrQ";
        private static string ApiKey = "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q";
        //private static string Bucket = "musictt-9aa5f.appspot.com";
        private static string Bucket = "tfourmusic-1e3ff.appspot.com";
        private static string AuthEmail = "dang60780@gmail.com";
        private static string AuthPassword = "0362111719@TTai";
        private string Key = " https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";
       
        //public TheLoaiController(ILogger<TheLoaiController> logger)
        //{
        //    _logger = logger;
        //}
        public DanhSachPhatNguoiDungController(IHostingEnvironment env)
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
        public async Task<IActionResult> CreateDanhSachPhatNguoiDung([FromBody]danhsachphatnguoidungModel item)
        {
            bool success = true;
            item.chedo = 0;
            item.thoigian = DateTime.Now;
            try
            {

                var firebase = new FirebaseClient(Key);
                
                var dino = await firebase
                  .Child("danhsachphatnguoidung")
                  .PostAsync(item)
                  ;
              
                string kk = dino.Key.ToString();
                item.id = kk;
                 await firebase
                    .Child("danhsachphatnguoidung")
                    .Child(kk)
                    .PutAsync(item);
                
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDanhSachPhatNguoiDung([FromBody] danhsachphatnguoidungModel item)
        {
            bool success = true;
          
            try
            {

                var firebase = new FirebaseClient(Key);
              
                await firebase
                   .Child("danhsachphatnguoidung")
                   .Child(item.id)
                   .DeleteAsync();

            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> EditDanhSachPhatNguoiDung([FromBody] danhsachphatnguoidungModel item)
        {
          bool success = true;
           
            try
            {

                var firebase = new FirebaseClient(Key);
             
                await firebase
                   .Child("danhsachphatnguoidung")
                   .Child(item.id)
                  .PutAsync(item);

            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
                   
        }
       
        [HttpPost]
        public async Task<IActionResult> LoadDanhSachPhatNguoiDung()
        {
            
                var firebase = new FirebaseClient(Key);
               
                var dino = await firebase
                  .Child("danhsachphatnguoidung")
                  .OnceAsync<danhsachphatnguoidungModel>();
    
            return Json(dino);
        }
         
    }
}

