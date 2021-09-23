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

namespace TFourMusic.Controllers
{
    [Area("Admin")]

    public class NguoiDungController : Controller
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
        
        public NguoiDungController(IHostingEnvironment env)
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
        public async Task<IActionResult> taiNguoiDung()
        {

            var firebase = new FirebaseClient(Key);

            var NguoiDung = LayBangNguoiDung();

            var data = from t20 in NguoiDung
                         
                       select t20;
            var auth = new FirebaseConfig1(ApiKey);
            

            //UserRecordArgs args = new UserRecordArgs()
            //{
            //    Uid = userRecord.Uid,
            //    Email = userRecord.Email,
            //    PhoneNumber = userRecord.PhoneNumber,
            //    EmailVerified = userRecord.EmailVerified,
            //    Password = userRecord.Passw,
            //    DisplayName = "Jane Doe",
            //    PhotoUrl = "http://www.example.com/12345678/photo.png",
            //    Disabled = true,
            //};

            //   UserRecord userRecord123 = await FirebaseAuth123.DefaultInstance.UpdateUserAsync(args);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> voHieuHoa([FromBody] nguoidungModel item)
        {


            bool success = true;
            try
            {
                
                if (item.vohieuhoa == 0)
                {
                    UserRecordArgs args = new UserRecordArgs()
                    {
                        Uid = item.uid,
                        Disabled = false,
                    };
                    UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(args);
                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/nguoidung/" + item.uid + "/" + "vohieuhoa", item.vohieuhoa);
                }
                else
                {
                    UserRecordArgs args = new UserRecordArgs()
                    {
                        Uid = item.uid,
                        Disabled = true,
                    };
                    UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(args);
                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/nguoidung/" + item.uid + "/" + "vohieuhoa", item.vohieuhoa);
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> voHieuHoaBaiHatNguoiDung([FromBody]baihatModel item)
        {


            bool success = true;
            try
            {

                if (item.vohieuhoa == 0)
                {
                    
                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);
                }
                else
                {
                   
                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> voHieuHoaDanhSachPhatNguoiDung([FromBody]danhsachphatnguoidungModel item)
        {


            bool success = true;
            try
            {

                if (item.vohieuhoa == 0)
                {
                   
                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/danhsachphatnguoidung/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);
                }
                else
                {
                   
                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/danhsachphatnguoidung/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        public List<danhsachphatnguoidungModel> LayBangDanhSachPhatNguoiDung(string uid = null)
        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/danhsachphatnguoidung");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<danhsachphatnguoidungModel>();

                if (data != null)
                {

                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<danhsachphatnguoidungModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }


                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/danhsachphatnguoidung/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<danhsachphatnguoidungModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<danhsachphatnguoidungModel>(((JProperty)item).Value.ToString()));

                    }
                }
                return list;
            }



        }
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
        public async Task<IActionResult> xemBaiHatNguoiDung([FromBody] nguoidungModel item)
        {
            try
            {
                if (item.uid != null && item.uid != "null")
                {
                    var baihat = LayBangBaiHat(item.uid);
                    return Json(baihat);
                }
                else
                {
                    return Json("null");
                }
             
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> xemDanhSachPhatNguoiDung([FromBody] nguoidungModel item)
        {


            try
            {
                if (item.uid != null && item.uid != "null")
                {
                    var danhsachphat = LayBangDanhSachPhatNguoiDung(item.uid);
                    return Json(danhsachphat);
                }
                else
                {
                    return Json("null");
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
    }
}

