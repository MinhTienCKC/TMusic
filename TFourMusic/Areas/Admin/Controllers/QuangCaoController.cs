﻿
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
    [Authorize]
    public class QuangCaoController : Controller
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
        public async Task<IActionResult> taoQuangCao([FromBody] quangcaoModel item)
        {
            bool success = true;
           // item.trangthai = 0;
            item.thoigian = DateTime.Now;
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var dino = await firebase
                    .Child("csdlmoi")
                    .Child("quangcao")
                    .PostAsync(item);

                string kk = dino.Key.ToString();
                item.id = kk;
                await firebase
                   .Child("csdlmoi")
                   .Child("quangcao")
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
        public async Task<IActionResult> xoaQuangCao([FromBody] quangcaoModel item)
        {
            bool success = true;

            try
            {
                if (item.id != "" && item.id != null)
                {
                    var xoaHinhAnhStorage = xoaStorageBangLink(item.linkhinhanh.ToString());

                    var firebase = new FirebaseClient(Key);
                    await firebase
                        .Child("csdlmoi")
                        .Child("quangcao")
                       .Child(item.id)
                       .DeleteAsync();
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
        public class BienTam
        {
            public string linkhinhanhmoi { get; set; }
            public string linkhinhanhcu { get; set; }
            public string quangcao_id { get; set; }


        }
        [HttpPost]
        public async Task<IActionResult> suaLinkHinhAnhQuangCao([FromBody] BienTam item)
        {
            bool success = true;
            try
            {

                if (item.linkhinhanhcu != "")
                {
                    var xoaHinhAnhStorage = xoaStorageBangLink(item.linkhinhanhcu.ToString());
                }
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/quangcao/" + item.quangcao_id + "/" + "linkhinhanh", item.linkhinhanhmoi);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
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
        public async Task<IActionResult> suaQuangCao([FromBody] quangcaoModel item)
        {
            bool success = true;
            try
            {
                var firebase = new FirebaseClient(Key);
                await firebase
                    .Child("csdlmoi")
                   .Child("quangcao")
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
        public async Task<IActionResult> taiQuangCao()
        {

            var firebase = new FirebaseClient(Key);
            var baihat = LayBangBaiHat();
            // add new item to list of data and let the client generate new key for you (done offline)
            var quangcao = await firebase
              .Child("csdlmoi")
              .Child("quangcao")
              .OnceAsync<quangcaoModel>();
            var dataqc = (from cd in quangcao
                        select cd.Object).ToList();
            var data = (from qc in dataqc
                        join bh in baihat on qc.baihat_id equals bh.id
                        select new
                        {
                            quangcao = qc,
                            baihat = bh
                        }).ToList();
            return Json(data);
        }
        [HttpPost]
        // tải danh sách bài hát để thêm vào dsp thể loại ko có bài hát đã tồn tại trong thể loại
        public async Task<IActionResult> taiDanhSachBaiHatDeThem_QuangCao()
        {
           
            var baihat = LayBangBaiHat();
            var firebase = new FirebaseClient(Key);
            var quangcao = await firebase
             .Child("csdlmoi")
             .Child("quangcao")
             .OnceAsync<quangcaoModel>();
            var dataqc = (from cd in quangcao
                          select cd.Object).ToList();
            var baihatquangcao = (from bh in baihat
                           join qc in dataqc on bh.id equals qc.baihat_id
                           select bh).ToList();      
            if (baihatquangcao.Count > 0)
            {
                foreach (var item in baihatquangcao)
                {

                    baihat.Remove(item);
                }
            } 
            return Json(baihat.OrderByDescending(x => x.thoigian));
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
                    string tg = "(QuangCao)" + aDateTime.Day.ToString() + aDateTime.Month.ToString()
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

