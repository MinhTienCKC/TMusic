
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
  
    public class QuangCaoController : Controller
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
        public QuangCaoController(IHostingEnvironment env)
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
        public async Task<IActionResult> CreateQuangCao([FromBody]quangcaoModel item)
        {
            bool success = true;
            item.thoigian = DateTime.Now;
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var dino = await firebase
                  .Child("quangcao")
                  .PostAsync(item)
                  ;
              
                string kk = dino.Key.ToString();
                item.id = kk;
                 await firebase
                    .Child("quangcao")
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
        public async Task<IActionResult> DeleteQuangCao([FromBody] quangcaoModel item)
        {
            bool success = true;
           
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)

                await firebase
                   .Child("quangcao")
                   .Child(item.id)
                   .DeleteAsync();

                // Create a reference to the file to delete
                // var   auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                //var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                //var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
                //{
                //    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                //    ThrowOnCancel = true
                //}).Child("image").Child("sdfdsf").DeleteAsync();
               
                
                // Delete the file
                
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
       
        [HttpPost]
        public async Task<IActionResult> EditQuangCao([FromBody] quangcaoModel item)
        {

          bool success = true;
          
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)

                await firebase
                   .Child("quangcao")
                   .Child(item.id)
                  .PutAsync(item);

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
        public async Task<IActionResult> LoadQuangCao()
        {
            
                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var dino = await firebase
                  .Child("quangcao")
                  .OnceAsync<quangcaoModel>();
            //    var theloai = await firebase
            //     .Child("theloai")
            //     .OnceAsync<theloaiModel>();
            //var dino = from chude1 in chude
            //            join theloai1 in theloai on chude1.Object.theloai_id equals theloai1.Object.id                       
            //            select new
            //            {
            //                id = chude1.Object.id,
            //                tenchude = chude1.Object.tenchude,
            //                linkhinhanh = chude1.Object.linkhinhanh,
            //                tentheloai = theloai1.Object.tentheloai

            //            };

            //var data = data1.OrderBy(c => c.Id).DistinctBy(i => new { i.Id });
          
            //var observable = firebase
            //    .Child("baihat")
            //    .AsObservable<baihatModel>();


            return Json(dino);
        }
        [HttpPost]
        public async Task<IActionResult> LoadTheLoai()
        {

            var firebase = new FirebaseClient(Key);

            var dino = await firebase
              .Child("theloai")     
              .OnceAsync<theloaiModel>();

            return Json(dino);
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
                    string tg = "(quangcao)" + aDateTime.Day.ToString() + aDateTime.Month.ToString()
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
            }).Child("image").Child("quangcao").Child(filename).PutAsync(stream, cancel.Token);
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

