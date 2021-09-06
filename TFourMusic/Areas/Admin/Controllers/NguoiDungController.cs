
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

            return Json(data);
        }
       
    }
}

