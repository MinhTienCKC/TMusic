
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
  
    public class DanhSachPhatTheLoaiController : Controller
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
        public DanhSachPhatTheLoaiController(IHostingEnvironment env)
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
        public async Task<IActionResult> TaoDanhSachPhatTheLoai([FromBody] danhsachphattheloaiModel item)
        {
            bool success = true;
          
            try
            {
                if (item.tendanhsachphattheloai != "" && item.theloai_id != "" && item.linkhinhanh != "")
                {
                    var firebase = new FirebaseClient(Key);

                    // add new item to list of data and let the client generate new key for you (done offline)
                    var dino = await firebase
                      .Child("danhsachphattheloai")
                      .PostAsync(item)
                      ;

                    string kk = dino.Key.ToString();
                    item.id = kk;
                    await firebase
                       .Child("danhsachphattheloai")
                       .Child(kk)
                       .PutAsync(item);

                    success = true;
                }
                else
                {
                    success = false;
                }

               
                
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> XoaDanhSachPhatTheLoai([FromBody] danhsachphattheloaiModel item)
        {
            bool success = true;
           
            try
            {

                var firebase = new FirebaseClient(Key);


                await firebase
                   .Child("danhsachphattheloai")
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
        public async Task<IActionResult> SuaDanhSachPhatTheLoai([FromBody] danhsachphattheloaiModel item)
        {

          bool success = true;
          
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)

                await firebase
                   .Child("danhsachphattheloai")
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
        public class Text
        {
            public string key { get; set; }
            public string uid { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> TaiDanhSachPhatTheLoai()
        {
                var firebase = new FirebaseClient(Key);
                var danhsachphattheloai = await firebase
                  .Child("danhsachphattheloai")
                  .OnceAsync<danhsachphattheloaiModel>();
            var data = from dsptl in danhsachphattheloai
                     
                       select new
                       {
                           dsptl.Object
                       };         
            return Json(data);
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
        // tải danh sách bài hát để thêm vào dsp thể loại ko có bài hát đã tồn tại trong thể loại
        public async Task<IActionResult> taiDanhSachBaiHatDeThem_DSPTL(string key = "")
        {
            var firebase = new FirebaseClient(Key);
            var baihat = await firebase
             .Child("baihat")
             .OnceAsync<baihatModel>();
            var chitietdanhsachphattheloai = await firebase
            .Child("chitietdanhsachphattheloai")
            .OnceAsync<chitietdanhsachphattheloaiModel>();
            var baihat2 =(from bh in baihat
                          join ctdsptl in chitietdanhsachphattheloai on bh.Object.id equals ctdsptl.Object.baihat_id
                          select new
                          {
                              bh.Object
                          }).ToList();
            var baihat1 = (from bh in baihat
                          select new
                           {
                               bh.Object
                           }).ToList();
            if (baihat2.Count > 0)
            {
                foreach (var item in baihat2)
                {

                    baihat1.Remove(item);
                }
            }
            var di123lieu = (from bh1 in baihat1                    
                          where bh1.Object.danhsachphattheloai_id != key.ToString() 
                          select new
                          {

                              bh1.Object

                          }).ToList();

            return Json(di123lieu);
        }
        [HttpPost]
        // tải danh sách bài hát đã thêm vào dsp thể loại bằng tay (Đã Thêm)
        public async Task<IActionResult> taiDanhSachBaiHatDaThem_DSPTL(string key = "")
        {
            var firebase = new FirebaseClient(Key);
            var baihat = await firebase
             .Child("baihat")
             .OnceAsync<baihatModel>();
            var chitietdanhsachphattheloai = await firebase
            .Child("chitietdanhsachphattheloai")
            .OnceAsync<chitietdanhsachphattheloaiModel>();
            var baihatdathem = (from bh in baihat
                           join ctdsptl in chitietdanhsachphattheloai on bh.Object.id equals ctdsptl.Object.baihat_id
                           where ctdsptl.Object.danhsachphattheloai_id.Equals(key.ToString())
                           select new
                           {
                               bh.Object
                           }).ToList();
            return Json(baihatdathem);
        }
        [HttpPost]
        // tải danh sách bài hát Mặc Định (Mặc định là khi tạo bài hát là đã thêm vào dsptl bằng cắt chọn khóa ngoại id dsp_tl)
        public async Task<IActionResult> taiDanhSachBaiHatMacDinh_DSPTL(string key = "")
        {
            var firebase = new FirebaseClient(Key);
            var baihat = await firebase
             .Child("baihat")
             .OnceAsync<baihatModel>();
            var baihatmacdinh = (from bh in baihat
                                where bh.Object.danhsachphattheloai_id.Equals(key.ToString())
                                select new
                                {
                                    bh.Object
                                }).ToList();
            return Json(baihatmacdinh);
        }
        [HttpPost]
        // thêm bài bài vào dsp thể loại bằng tay id bài hát vs id dsp tl
        public async Task<IActionResult> themBaiHatVaoDSPTL_DSPTL([FromBody] Text item)
        {
            var firebase = new FirebaseClient(Key);


            bool success = true;

            try
            {
                // add new item to list of data and let the client generate new key for you (done offline)
                chitietdanhsachphattheloaiModel ctdsptl = new chitietdanhsachphattheloaiModel();
                ctdsptl.baihat_id = item.key;
                ctdsptl.danhsachphattheloai_id = item.uid;
                ctdsptl.id = "";
                var dino = await firebase
                  .Child("chitietdanhsachphattheloai")
                  .PostAsync(ctdsptl)
                  ;

                string kk = dino.Key.ToString();
                ctdsptl.id = kk;
                await firebase
                   .Child("chitietdanhsachphattheloai")
                   .Child(kk)
                   .PutAsync(ctdsptl);

            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        // xóa bài bài khỏi dsp thể loại bằng tay id bài hát vs id dsp tl
        public async Task<IActionResult> xoaBaiHatkhoiDSPTL_DSPTL([FromBody] Text item)
        {
            var firebase = new FirebaseClient(Key);
            var chitietdanhsachphattheloai = await firebase
           .Child("chitietdanhsachphattheloai")
           .OnceAsync<chitietdanhsachphattheloaiModel>();
            var data = (from ctdsptl in chitietdanhsachphattheloai
                                where ctdsptl.Object.danhsachphattheloai_id.Equals(item.uid.ToString()) &&
                                ctdsptl.Object.baihat_id.Equals(item.key.ToString())
                                select new
                                {
                                    ctdsptl.Object
                                }).ToList();

            bool success = true;

            try
            {
                // add new item to list of data and let the client generate new key for you (done offline)
                await firebase
                   .Child("chitietdanhsachphattheloai")
                   .Child(data[0].Object.id)
                   .DeleteAsync();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
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
                    string tg = "(danhsachphattheloai)" + aDateTime.Day.ToString() + aDateTime.Month.ToString()
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
            }).Child("image").Child("danhsachphattheloai").Child(filename).PutAsync(stream, cancel.Token);
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

