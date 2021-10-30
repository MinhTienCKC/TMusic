using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TFourMusic.Models;
using PayPalHttp;
using FirebaseConfig1 = Firebase.Auth.FirebaseConfig;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TaiKhoanQuanTriController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "vHcXcNH4jYpiScpS8Fw3mSJhUj6lX3zp4kgpIM7T",
            BasePath = "https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
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

        public TaiKhoanQuanTriController(IHostingEnvironment env)
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

        public async Task<IActionResult> xoaTaiKhoanQuanTri([FromBody] taikhoanquantriModel item)
        {
            var heThong = User.Identity as ClaimsIdentity;
            var phanQuyen = heThong.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            var uid = heThong.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            bool success = true;

            if (uid == item.id)
            {
                return Json("loi");
            }
            try
            {

                var firebase = new FirebaseClient(Key);
                 await firebase
                .Child("csdlmoi")
                .Child("taikhoanquantri")
                .Child(item.id)
                .DeleteAsync();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }

        //[HttpPost]
        //public async Task<IActionResult> thayDoiTrangThaiNguoiDung([FromBody] nguoidungModel item)
        //{

        //    bool success = true;
        //    try
        //    {
        //        client = new FireSharp.FirebaseClient(config);
        //        object p = client.Set("csdlmoi/danhsachphat/danhsachphatNguoiDung/" + item.theloai_id + "/" + item.id + "/" + "daxoa", item.daxoa);
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        success = false;
        //    }

        //    return Json(success);
        //}
        public class Text
        {
            public string key { get; set; }
        }
        public List<nguoidungModel> LayBangNguoiDung(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/nguoidung");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<nguoidungModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<nguoidungModel>(((JProperty)item).Value.ToString()));

                        }
                    }

                    return list;
                }
                else
                {

                    //return list;
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/nguoidung/" + uid);
                    var data = JsonConvert.DeserializeObject<nguoidungModel>(response.Body);
                    List<nguoidungModel> list = new List<nguoidungModel>();
                    if (data != null)
                    {
                        list.Add(data);
                    }

                    return list;
                }
            }
            catch
            {
                List<nguoidungModel> list = new List<nguoidungModel>();
                return list;
            }

        }
        [HttpPost]
        public async Task<IActionResult> taiTaiKhoanQuanTri()
        {

            var firebase = new FirebaseClient(Key);

            var dino = await firebase
                .Child("csdlmoi")
                .Child("taikhoanquantri")
                .OnceAsync<taikhoanquantriModel>();
            var data = from tkqt in dino

                       select tkqt.Object;

            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> voHieuHoa([FromBody] taikhoanquantriModel item)
        {
            var heThong = User.Identity as ClaimsIdentity;
            var phanQuyen = heThong.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            var uid = heThong.Claims.FirstOrDefault(c => c.Type == "uid").Value;
       

            if (uid == item.id)
            {
                return Json("loi");
            }
            if (phanQuyen == "Admin")
            {
                bool success = true;
                try
                {

                    if (item.vohieuhoa == 0)
                    {

                        client = new FireSharp.FirebaseClient(config);
                        object p = client.Set("csdlmoi/taikhoanquantri/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);

                    }
                    else
                    {

                        client = new FireSharp.FirebaseClient(config);
                        object p = client.Set("csdlmoi/taikhoanquantri/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);

                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }

                return Json(success);
            }
            else
            {
                return Json("");
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> taoTaiKhoanQuanTri([FromBody] taikhoanquantriModel item)
        {

            bool success = true;
            item.vohieuhoa = 0;
            item.thoigian = DateTime.Now;      
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var dino = await firebase
                    .Child("csdlmoi")
                  .Child("taikhoanquantri")
               //   .Child(item.nguoidung_id)
                  .PostAsync(item)
                  ;
                string kk = dino.Key.ToString();
                item.id = kk;
                await firebase
                    .Child("csdlmoi")
                  .Child("taikhoanquantri")                 
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
        public async Task<IActionResult> suaTaiKhoanQuanTri([FromBody] taikhoanquantriModel item)
        {

            bool success = true;
        
            try
            {

                var firebase = new FirebaseClient(Key);



                await firebase
                     .Child("csdlmoi")
                  .Child("taikhoanquantri")
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
    }
}

