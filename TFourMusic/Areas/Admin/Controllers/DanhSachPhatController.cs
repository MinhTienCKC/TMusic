
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
  
    public class DanhSachPhatController : Controller
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
        public DanhSachPhatController(IHostingEnvironment env)
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
        public async Task<IActionResult> CreateDanhSachPhat([FromBody]danhsachphatModel item)
        {
            bool success = true;
            item.thoigian = DateTime.Now;
            try
            {

                var firebase = new FirebaseClient(Key);
                
                var dino = await firebase
                  .Child("danhsachphat")
                  .PostAsync(item)
                  ;
              
                string kk = dino.Key.ToString();
                item.id = kk;
                 await firebase
                    .Child("danhsachphat")
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
        public async Task<IActionResult> DeleteDanhSachPhat([FromBody] danhsachphatModel item)
        {
            bool success = true;
          
            try
            {

                var firebase = new FirebaseClient(Key);
              
                await firebase
                   .Child("danhsachphat")
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
        public async Task<IActionResult> EditDanhSachPhat([FromBody] danhsachphatModel item)
        {
          bool success = true;
           
            try
            {

                var firebase = new FirebaseClient(Key);
             
                await firebase
                   .Child("danhsachphat")
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
        public async Task<IActionResult> LoadDanhSachPhat()
        {
            
                var firebase = new FirebaseClient(Key);
               
                var dino = await firebase
                  .Child("danhsachphat")
                  .OnceAsync<danhsachphatModel>();
        

            //var dino1 = await firebase
            //             .Child("baihat")
            //             .OnceAsync<baihatModel>();
            //var ok = from bh in dino1
            //         orderby bh.Object.luottaixuong descending
            //         select bh;
            //var ok1 = ok.Take(3);
            //baihatModel item = new baihatModel();
            //chitietdanhsachphatModel ctdsp = new chitietdanhsachphatModel();

            //foreach (var dinos in ok1)
            //{
            //    ctdsp.baihat_id = dinos.Object.id;
            //    ctdsp.danhsachphat_id = "-Mbu6oam0hs82p-uXZ9";
            //    var dino123 = await firebase
            //       .Child("chitietdanhsachphat")
            //       .PostAsync(ctdsp);
            //    string kk = dino123.Key.ToString();
            //    ctdsp.id = kk;
            //    await firebase
            //       .Child("chitietdanhsachphat")
            //       .Child(kk)
            //       .PutAsync(ctdsp);
            //    item = new baihatModel();
            //}
            return Json(dino);
        }
        [HttpPost]
        public async Task<IActionResult> LoadChiTietDanhSachPhat([FromBody] danhsachphatModel item)
        {

            var firebase = new FirebaseClient(Key);

            var danhsachphat = await firebase
              .Child("danhsachphat")
              .OnceAsync<danhsachphatModel>();
            var baihat = await firebase
             .Child("baihat")
             .OnceAsync<baihatModel>();
            var chitietdanhsachphat = await firebase
            .Child("chitietdanhsachphattheloai")
            .OnceAsync<chitietdanhsachphattheloaiModel>();
            var dino = from ctdsp in chitietdanhsachphat
                       join dsp in danhsachphat on ctdsp.Object.danhsachphattheloai_id equals dsp.Object.id
                       join bh in baihat on ctdsp.Object.baihat_id equals bh.Object.id
                       where ctdsp.Object.danhsachphattheloai_id.Equals(item.id)
                       //orderby bh.Object.luotthich descending
                        select new 
                       {
                           
                           id = ctdsp.Object.id,
                           baihat_id = bh.Object.id,
                           danhsachphat_id = dsp.Object.id,
                           tenbaihat = bh.Object.tenbaihat,
                           casi = bh.Object.casi,
                           linkhinhanh = bh.Object.linkhinhanh

                       };
            var ok = from bh in baihat
                     select new
                     {


                         baihat_id = bh.Object.id,

                         tenbaihat = bh.Object.tenbaihat,
                         casi = bh.Object.casi,
                         linkhinhanh = bh.Object.linkhinhanh

                     };
            //Top 100 Bài Hát Có Nhiều Lượt Thích Nhất
            if (item.id == "-MbtuuVsiqHdwa6cT6Ly")
            {
                 ok = from bh in baihat
                         orderby bh.Object.luotthich descending
                      select new
                      {

                
                          baihat_id = bh.Object.id,
                  
                          tenbaihat = bh.Object.tenbaihat,
                          casi = bh.Object.casi,
                          linkhinhanh = bh.Object.linkhinhanh

                      };
            }
            //Top 100 Bài Hát Có Nhiều Lượt Nghe Nhất
            if (item.id == "-Mbu6m9-CEX_oFOcFVwV")
            {
                 ok = from bh in baihat
                         orderby bh.Object.luotnghe descending
                      select new
                      {

                       
                          baihat_id = bh.Object.id,
                            
                          tenbaihat = bh.Object.tenbaihat,
                          casi = bh.Object.casi,
                          linkhinhanh = bh.Object.linkhinhanh

                      };
            }
            //Top 100 Bài Hát Có Nhiều Lượt Tải Nhất
            if (item.id == "-Mbu6oam0hs82p-uXZ91")
            {
                 ok = from bh in baihat
                         orderby bh.Object.luottaixuong descending
                      select new
                      {

                   
                          baihat_id = bh.Object.id,
                     
                          tenbaihat = bh.Object.tenbaihat,
                          casi = bh.Object.casi,
                          linkhinhanh = bh.Object.linkhinhanh

                      };
            }
            //
            //
            var dulieu = ok.Take(3);
            return Json(dulieu);
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
                    string tg = "(ChuDe)" + aDateTime.Day.ToString() + aDateTime.Month.ToString()
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
            }).Child("image").Child("danhsachphat").Child(filename).PutAsync(stream, cancel.Token);
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

