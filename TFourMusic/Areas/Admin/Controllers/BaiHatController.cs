
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
//using FirebaseConfig = Firebase.Auth.FirebaseConfig;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using FireSharp.Interfaces;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
  
    public class BaiHatController : Controller
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
        private string Key = " https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";
       
        //public BaiHatController(ILogger<BaiHatController> logger)
        //{
        //    _logger = logger;
        //}
        public BaiHatController(IHostingEnvironment env)
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
        public async Task<IActionResult> taoBaiHat([FromBody]baihatModel item)
        {
          
            bool success = true;
            item.chedo = 1;//sữa chế độ thành 1 công khai
            item.thoigian = DateTime.Now;
            item.daxoa = 0;
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var dino = await firebase
                  .Child("baihat")
                  .PostAsync(item)
                  ;
              
                string kk = dino.Key.ToString();
                item.id = kk;
                 await firebase
                    .Child("baihat")
                    .Child(kk)
                    .PutAsync(item);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> xoaBaiHat(string idbaihat = "")
        {
            bool success = true;
            //item.thoigian = DateTime.Now;
            //item.chedo = 0;
            try
            {
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("baihat/" + idbaihat + "/" + "daxoa", 1);
                success = true;

            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        public class BienTam
        {
            public string linkhinhanhmoi { get; set; }
            public string linkhinhanhcu { get; set; }         
            public string baihat_id { get; set; }
           
        }
        [HttpPost]
        public async Task<IActionResult> suaLinkHinhAnhBaiHat([FromBody]BienTam item)
        {
            bool success = true;
            try
            {
               
                if (item.linkhinhanhcu != "")
                {
                    string link = item.linkhinhanhcu;
                    string[] list = link.Split("baihat%2F");
                    string fileName = list[1];
                    list = fileName.Split("?alt");
                    fileName = list[0];
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                    var cancel = new CancellationTokenSource();
                    var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }).Child("image").Child("baihat").Child(fileName).DeleteAsync();
                }
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("baihat/" + item.baihat_id + "/" + "linkhinhanh", item.linkhinhanhmoi);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> suaBaiHat([FromBody] baihatModel item)
        {

            bool success = true;
            //item.thoigian = DateTime.Now;
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)

                await firebase
                   .Child("baihat")
                   .Child(item.id)
                  .PutAsync(item);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
           

            //var observable = firebase
            //    .Child("baihat")
            //    .AsObservable<baihatModel>();
        }
       
        [HttpPost]
     
        public async Task<IActionResult> taiBaiHat([FromBody]baihatModel item)
        {           
            var firebase = new FirebaseClient(Key);
               
                 var baihat = await firebase
                  .Child("baihat")
                  .OnceAsync<baihatModel>();
            var data = from bh in baihat                      
                      //  orderby bh.Object.thoigian descending
                        select bh.Object;
          
            //var data = from bh in baihat
            //           where (string.IsNullOrEmpty(item.tenbaihat) || (bh.Object.tenbaihat != null && bh.Object.tenbaihat.ToLower().Contains(item.tenbaihat.ToLower())))
            //           && (string.IsNullOrEmpty(item.casi) || (bh.Object.casi != null && bh.Object.casi.ToLower().Contains(item.casi.ToLower())))
            //           select new { 
            //            bh.Object
            //           };
            return Json(data.OrderByDescending(x => x.thoigian));
        }
        [HttpPost]
        public async Task<IActionResult> taiTheLoai()
        {

            var firebase = new FirebaseClient(Key);

            var dino = await firebase
              .Child("theloai")
              .OnceAsync<theloaiModel>();

            return Json(dino);
        }
        [HttpPost]
        public async Task<IActionResult> taiDanhSachPhatTheLoai([FromBody]Text item)
        {

            var firebase = new FirebaseClient(Key);

            var danhsachphattheloai = await firebase
              .Child("danhsachphattheloai")
              .OnceAsync<danhsachphattheloaiModel>();

            var data = from dsptl in danhsachphattheloai
                       where dsptl.Object.theloai_id.Equals(item.key.ToString())
                       select new
                       {
                           dsptl.Object
                       };


            return Json(data);
        }
        public class Text
        {
            public string key { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> taiChuDe()
        {
          //    string text = dltr.key.ToString();
            var firebase = new FirebaseClient(Key);

            var dino = await firebase
              .Child("chude")
              .OnceAsync<chudeModel>();

           
            return Json(dino);
        }
        [HttpPost]
        public async Task<IActionResult> GetLink([FromForm] IFormCollection file)
        {

            long size = file.Files.Sum(f => f.Length);
            
            string pathName = "Access";
            string link = "";
           //var type = file.Files.FirstOrDefault(f => f.ContentType == "image/jpeg").ContentType ;

            var path = Path.Combine(_env.WebRootPath, $"music/{pathName}");
            try
            {
                foreach (var item in file.Files)
                {
                    DateTime aDateTime = DateTime.Now;
                    string tg = "admin" + aDateTime.Day.ToString() + aDateTime.Month.ToString()
                        + aDateTime.Year.ToString() + aDateTime.Hour.ToString()
                        + aDateTime.Minute.ToString() + aDateTime.Second.ToString() + aDateTime.DayOfYear.ToString()+".mp3";
                    if (item.Length > 0)
                    {
                        if (Directory.Exists(path))
                        {
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
                            {
                                await item.CopyToAsync(fs);
                             
                            }
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                               
                             
                                link = await Task.Run(() => Upload(fs,tg));
                              
                            }
                            System.IO.File.Delete(Path.Combine(path, item.FileName));
                        }
                        else
                        {
                            Directory.CreateDirectory(path);
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
                            {
                                await item.CopyToAsync(fs);
                            }
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                               
                                link = await Task.Run(() => Upload(fs,tg));
                               
                            }
                            System.IO.File.Delete(Path.Combine(path, item.FileName));
                        }

                    }


                }
              
            }
            catch (Exception e)
            {

            }
            return Json(link);
        }
        
       
        public async Task<string> Upload(FileStream stream, string filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            // cancel upload midway
            
            var cancel = new CancellationTokenSource();
           
                var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }).Child("music").Child("admin").Child(filename).PutAsync(stream, cancel.Token);
                try
                {
                    string link = await task;
                    return link;

                }
                catch (Exception e)
                {
                    return "";
                }
          
             
            
           

        }
        [HttpPost]
        public async Task<IActionResult> GetLinkHinhAnh([FromForm] IFormCollection file)
        {

            long size = file.Files.Sum(f => f.Length);
            string pathName = "Access";
            string link = "";
            var path = Path.Combine(_env.WebRootPath, $"image/{pathName}");
            try
            {
                foreach (var item in file.Files)
                {
                    DateTime aDateTime = DateTime.Now;
                    string tg = "admin" + aDateTime.Day.ToString() + aDateTime.Month.ToString()
                        + aDateTime.Year.ToString() + aDateTime.Hour.ToString()
                        + aDateTime.Minute.ToString() + aDateTime.Second.ToString() + aDateTime.DayOfYear.ToString();
                    if (item.Length > 0)
                    {
                        if (Directory.Exists(path))
                        {
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
                            {
                                await item.CopyToAsync(fs);
                            }
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {


                                link = await Task.Run(() => UploadHinhAnh(fs, tg));
                            }
                            System.IO.File.Delete(Path.Combine(path, item.FileName));
                        }
                        else
                        {
                            Directory.CreateDirectory(path);
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Create))
                            {
                                await item.CopyToAsync(fs);
                            }
                            using (FileStream fs = new FileStream(Path.Combine(path, item.FileName), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {

                                link = await Task.Run(() => UploadHinhAnh(fs, tg));
                            }
                            System.IO.File.Delete(Path.Combine(path, item.FileName));
                        }

                    }


                }
            }
            catch (Exception e)
            {

            }
            return Json(link);
        }
        public async Task<string> UploadHinhAnh(FileStream stream, string filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            // cancel upload midway

            var cancel = new CancellationTokenSource();
            var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            }).Child("image").Child("baihat").Child(filename).PutAsync(stream, cancel.Token);
            try
            {
                string link = await task;
                return link;

            }
            catch (Exception e)
            {
                return "";
            }

        }
    }
}

