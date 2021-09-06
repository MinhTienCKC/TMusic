
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

//using LiteDB;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
  [Authorize]
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
        private string Key = "https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";
       
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
        public async Task<IActionResult> thayDoiCheDoBaiHat([FromBody] baihatModel item)
        {

            bool success = true;
            try
            {
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "chedo", item.chedo);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> taoBaiHat([FromBody]baihatModel item)
        {
          
            bool success = true;
            item.chedo = 1;//sữa chế độ thành 1 công khai
            item.thoigian = DateTime.Now;
            item.daxoa = 0;
            item.nguoidung_id = "admin";
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var dino = await firebase
                    .Child("csdlmoi")
                  .Child("baihat")
                  .Child(item.nguoidung_id)
                  .PostAsync(item)
                  ;
              
                string kk = dino.Key.ToString();
                item.id = kk;
                 await firebase
                    .Child("csdlmoi")
                    .Child("baihat")
                    .Child(item.nguoidung_id)
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
        public async Task<IActionResult> xoaBaiHat([FromBody] baihatModel item)
        {
            bool success = true;
            //item.thoigian = DateTime.Now;
            //item.chedo = 0;
            try
            {

                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "daxoa", 1);
                object tg = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "thoigianxoa", DateTime.Now.AddMonths(1));
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

            public string nguoidung_id { get; set; }
           
        }
        [HttpPost]
        public async Task<IActionResult> suaLinkHinhAnhBaiHat([FromBody]BienTam item)
        {
            bool success = true;
            try
            {
               
                if (item.linkhinhanhcu != "")
                {
                    var xoaHinhAnhStorage = xoaStorageBangLink(item.linkhinhanhcu.ToString());                   
                }
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.baihat_id + "/" + "linkhinhanh", item.linkhinhanhmoi);
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

                

                await firebase
                    .Child("csdlmoi")
                   .Child("baihat")
                   .Child(item.nguoidung_id)
                   .Child(item.id)
                  .PutAsync(item);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
                
        }
        //static FirebaseAuthLink auth = null;
        //static string _userId = null;
        //public static FirebaseClient ClienteAutenticadoConEmail()
        //{
        //    var firebaseClient = new FirebaseClient(
        //          "<YourDatabaseUrl>",
        //          new FirebaseOptions
        //          {
        //              AuthTokenAsyncFactory = () => LoginWithEmailAndPasswordAsync(false)
        //          });
        //    return firebaseClient;
        //}
        //public static async Task<string> LoginWithEmailAndPasswordAsync(bool createUser)
        //{
        //    if (auth != null)
        //        return auth.FirebaseToken;

        //    // manage oauth login to Google / Facebook etc.
        //    // call FirebaseAuthentication.net library to get the Firebase Token
        //    // return the token
        //    string email = "dang60780@gmail.com";
        //    string password = "0362111719@TTai"; // "testPassword";
        //    var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q"));
        //    // Definido a nivel de clase. Si ya tenemos AuthLink se retorna el que se hizo inicialmente
        //    // FirebaseAuthLink auth = null;
        //    try
        //    {
        //        if (createUser)
        //        {
        //            auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
        //            // The auth Object will contain auth.User and the Authentication Token from the request
        //        }
        //        else
        //        {
        //            auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex);
        //    }

        //    System.Diagnostics.Debug.WriteLine(auth.FirebaseToken);
        //    _userId = auth.User.LocalId;
        //    return auth.FirebaseToken;
        //}
        public List<baihatModel> LayBangBaiHat(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baihat");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baihatModel>();


                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            foreach (var x in item)
                            {
                                foreach (var y in x)

                                {
                                    list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)y).Value.ToString()));

                                }

                            }

                        }
                    }
                    return list;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baihat/" + uid);
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baihatModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)item).Value.ToString()));

                        }
                    }
                    return list;
                }
            }
            catch
            {
                return null;
            }

        }
        [HttpPost]
     
        public async Task<IActionResult> taiBaiHat([FromBody]baihatModel item)
        {
          
                    
            var firebase = new FirebaseClient(Key);
            
            var baihat = LayBangBaiHat();
            var data = from bh in baihat
                       where bh.daxoa.Equals(0)
                       select bh;



            DateTime ok = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy"));
            var dsbhdaxoa = (from bh in baihat
                             where bh.daxoa.Equals(1) && DateTime.Parse(bh.thoigianxoa.ToString("dd-MM-yyyy")) == DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"))
                             select bh).ToList();
            if (dsbhdaxoa.Count > 0)
            {
                foreach (baihatModel bhxoa in dsbhdaxoa)
                {
                    XoaBaiHatVinhVien(bhxoa);
                }

            }

            //var data = from bh in baihat
            //           where (string.IsNullOrEmpty(item.tenbaihat) || (bh.Object.tenbaihat != null && bh.Object.tenbaihat.ToLower().Contains(item.tenbaihat.ToLower())))
            //           && (string.IsNullOrEmpty(item.casi) || (bh.Object.casi != null && bh.Object.casi.ToLower().Contains(item.casi.ToLower())))
            //           select new { 
            //            bh.Object
            //           };
            return Json(data.OrderByDescending(x => x.thoigian));
        }
        [HttpPost]

        public async Task<IActionResult> taiDanhSachBaiHatDaXoa_ThungRac()
        {
            var baihat = LayBangBaiHat();
            var data = from bh in baihat
                       where bh.daxoa.Equals(1)
                       select bh;   
            return Json(data.OrderByDescending(x => x.thoigianxoa));
        }
        [HttpPost]
        public async Task<IActionResult> khoiPhucBaiHat_ThungRac([FromBody] baihatModel item)
        {
            bool success = true;
            try
            {
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "daxoa", 0);
                object tg = client.Set("csdlmoi/baihat/" + item.nguoidung_id  + "/" + item.id + "/" + "thoigianxoa", DateTime.Now);
                success = true;

            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> xoaBaiHatVinhVien_ThungRac([FromBody] baihatModel item)
        {
            bool success = true;
            try
            {
                XoaBaiHatVinhVien(item);
               
                success = true;

            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(success);
        }
        public async Task XoaBaiHatVinhVien(baihatModel item)
        {
                if (item.link != "")
                {
                    var xoaBaiHatStorage = xoaStorageBangLink(item.link);
                }
                if (item.linkhinhanh != "")
                {
                    var xoaHinhAnhStorage = xoaStorageBangLink(item.linkhinhanh);
                }
                var firebase = new FirebaseClient(Key);
                     await firebase
                    .Child("csdlmoi")
                   .Child("baihat")
                   .Child(item.nguoidung_id)
                   .Child(item.id)
                   .DeleteAsync();
        }
        public async Task<IActionResult> xoaStorageBangLink(string link)
        {
            bool success = true;
            try
            {
                if (link != "")
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                    var opiton = new FirebaseStorage(Bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    });
                    var resultContent = "N/A";
                    // var link = "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2Fnguoidung%2Fa%20whole%20new%20world.mp3?alt=media&token=bececcb8-1a5b-4a5e-bff5-2df24235c621";
                    using (var http = await opiton.Options.CreateHttpClientAsync().ConfigureAwait(false))
                    {
                        var result = await http.DeleteAsync(link).ConfigureAwait(false);

                        resultContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

                        result.EnsureSuccessStatusCode();
                    }
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
        public async Task<IActionResult> taiTheLoai()
        {

            var firebase = new FirebaseClient(Key);

            var dino = await firebase
                .Child("csdlmoi")
              .Child("theloai")
              .OnceAsync<theloaiModel>();

            return Json(dino);
        }
        [HttpPost]
        public async Task<IActionResult> taiDanhSachPhatTheLoai([FromBody]Text item)
        {

            var firebase = new FirebaseClient(Key);

            var danhsachphattheloai = await firebase
                .Child("csdlmoi")
                .Child("danhsachphat")
              .Child("danhsachphattheloai")
              .Child(item.key.ToString())
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
                .Child("csdlmoi")
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
            var auth = new FirebaseAuthProvider(new FirebaseConfig123(ApiKey));
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
            var auth = new FirebaseAuthProvider(new FirebaseConfig123(ApiKey));
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

