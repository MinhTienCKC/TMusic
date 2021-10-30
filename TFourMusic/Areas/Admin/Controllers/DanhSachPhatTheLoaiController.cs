
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
    public class DanhSachPhatTheLoaiController : Controller
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
        public async Task<IActionResult> taoDanhSachPhatTheLoai([FromBody] danhsachphattheloaiModel item)
        {
            bool success = true;
          
            try
            {
                if (item.tendanhsachphattheloai != "" && item.theloai_id != "" && item.linkhinhanh != "")
                {
                    var firebase = new FirebaseClient(Key);

                    // add new item to list of data and let the client generate new key for you (done offline)
                    var dino = await firebase
                        .Child("csdlmoi")
                        .Child("danhsachphat")
                        .Child("danhsachphattheloai")
                        .Child(item.theloai_id)
                      .PostAsync(item)
                      ;
                    string keyDSPTL = dino.Key.ToString();
                    item.id = keyDSPTL;
                    await firebase
                        .Child("csdlmoi")
                       .Child("danhsachphat")
                        .Child("danhsachphattheloai")
                        .Child(item.theloai_id)
                        .Child(keyDSPTL)
                       .PutAsync(item);
                    top20Model t20 = new top20Model();
                    t20.danhsachphattheloai_id = item.id;
                    t20.linkhinhanh = item.linkhinhanh;
                    t20.tentop20 = "Top 20 " + item.tendanhsachphattheloai;
                    t20.mota = item.mota;
                    t20.theloai_id = item.theloai_id;
                    t20.daxoa = 0;
                    t20.id = "";
                    var taoTop20 = await firebase
                        .Child("csdlmoi")
                        .Child("danhsachphat")
                        .Child("danhsachphattop20")
                        .Child(item.theloai_id)
                      .PostAsync(t20)
                      ;
                    string keytop20 = taoTop20.Key.ToString();
                    t20.id = keytop20;
                    await firebase
                        .Child("csdlmoi")
                       .Child("danhsachphat")
                        .Child("danhsachphattop20")
                        .Child(item.theloai_id)
                        .Child(keytop20)
                       .PutAsync(t20);
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
                var baihat = LayBangBaiHat();
                var baihatmacdinh = (from bh in baihat
                                     where bh.danhsachphattheloai_id.Equals(item.id.ToString())
                                     select bh ).ToList();
                var baihattong = baihatmacdinh;
                var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai(item.id.ToString());
                var baihatdathem = (from bh in baihat
                               join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                               select bh).ToList();
                var datatop20 = LayBangTop20(item.theloai_id);
                var top20 = (from t20 in datatop20
                             where t20.danhsachphattheloai_id.Equals(item.id)
                             select t20).ToList();
                if (baihatdathem.Count > 0)
                {
                    foreach (var data in baihatdathem)
                    {
                        baihattong.Add(data);
                    }
                }
                var okok = baihattong;
                if (baihattong.Count == 0)
                {
                    if (item.linkhinhanh != "")
                    {
                        var xoaHinhAnhStorage = xoaStorageBangLink(item.linkhinhanh.ToString());
                    }
                    await firebase
                       .Child("csdlmoi")
                           .Child("danhsachphat")
                            .Child("danhsachphattheloai")
                            .Child(item.theloai_id)
                            .Child(item.id)
                       .DeleteAsync();
                    var xoaHinhAnhStorage123 = xoaStorageBangLink(top20[0].linkhinhanh.ToString());
                    await firebase
                       .Child("csdlmoi")
                           .Child("danhsachphat")
                            .Child("danhsachphattop20")
                            .Child(item.theloai_id)
                            .Child(top20[0].id)
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
       
        [HttpPost]
        public async Task<IActionResult> suaDanhSachPhatTheLoai([FromBody] danhsachphattheloaiModel item)
        {

          bool success = true;
          
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var datatop20 = LayBangTop20(item.theloai_id);
                var top20 = (from t20 in datatop20
                             where t20.danhsachphattheloai_id.Equals(item.id)
                             select t20).ToList();
                top20[0].mota = item.mota;
                top20[0].tentop20 = "Top 20 " + item.tendanhsachphattheloai;
                await firebase
                  .Child("csdlmoi")
                      .Child("danhsachphat")
                       .Child("danhsachphattop20")
                       .Child(item.theloai_id)
                       .Child(top20[0].id)
                 .PutAsync(top20[0]);

                await firebase
                   .Child("csdlmoi")
                       .Child("danhsachphat")
                        .Child("danhsachphattheloai")
                        .Child(item.theloai_id)
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
        public List<chitietdanhsachphattheloaiModel> LayBangChiTietDanhSachPhatTheLoai(string iddsptheloai = null)
        {
            if (iddsptheloai == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/chitietdanhsachphattheloai");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<chitietdanhsachphattheloaiModel>();
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<chitietdanhsachphattheloaiModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }



                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/chitietdanhsachphattheloai/" + iddsptheloai);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<chitietdanhsachphattheloaiModel>();
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<chitietdanhsachphattheloaiModel>(((JProperty)item).Value.ToString()));

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
        public List<top20Model> LayBangTop20(string idtheloai = null)
        {
            if (idtheloai == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/danhsachphat/danhsachphattop20");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<top20Model>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<top20Model>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }



                return list;

            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/danhsachphat/danhsachphattop20/" + idtheloai);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<top20Model>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<top20Model>(((JProperty)item).Value.ToString()));
                    }
                }
                return list;
            }

        }
        public List<danhsachphattheloaiModel> LayBangDanhSachPhatTheLoai(string idtheloai = null)
        {
            if (idtheloai == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/danhsachphat/danhsachphattheloai");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<danhsachphattheloaiModel>();
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<danhsachphattheloaiModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/danhsachphat/danhsachphattheloai/" + idtheloai);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<danhsachphattheloaiModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<danhsachphattheloaiModel>(((JProperty)item).Value.ToString()));

                    }
                }
                return list;
            }



        }
        [HttpPost]
        public async Task<IActionResult> taiDanhSachPhatTheLoai()
        {              
            var danhsachphattheloai = LayBangDanhSachPhatTheLoai();
            var data = (from dsptl in danhsachphattheloai
                       select dsptl).ToList();         
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> taiTheLoai()
        {

            var firebase = new FirebaseClient(Key);
            var theloai = await firebase
               .Child("csdlmoi")
              .Child("theloai")
              .OnceAsync<theloaiModel>();
            var data = (from tl in theloai
                        select tl.Object).ToList();
            return Json(data);
        }
        public class BienTam
        {
            public string linkhinhanhmoi { get; set; }
            public string linkhinhanhcu { get; set; }
            public string danhsachphattheloai_id { get; set; }

            public string theloai_id { get; set; }

        }
        [HttpPost]
        public async Task<IActionResult> suaLinkHinhAnhDanhSachPhatTheLoai([FromBody] BienTam item)
        {
            bool success = true;
            try
            {
                if (item.linkhinhanhcu != "")
                {
                    var xoaHinhAnhStorage = xoaStorageBangLink(item.linkhinhanhcu.ToString());
                }
                var datatop20 = LayBangTop20(item.theloai_id);
                var top20 = (from t20 in datatop20
                             where t20.danhsachphattheloai_id.Equals(item.danhsachphattheloai_id)
                             select t20).ToList();
                if (top20[0].linkhinhanh == item.linkhinhanhcu)
                {
                    client = new FireSharp.FirebaseClient(config);
                    object p1 = client.Set("csdlmoi/danhsachphat/danhsachphattop20/" + item.theloai_id + "/" + top20[0].id + "/" + "linkhinhanh", item.linkhinhanhmoi);
                }
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/danhsachphat/danhsachphattheloai/" + item.theloai_id + "/" + item.danhsachphattheloai_id + "/" + "linkhinhanh", item.linkhinhanhmoi);
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
        // tải danh sách bài hát để thêm vào dsp thể loại ko có bài hát đã tồn tại trong thể loại
        public async Task<IActionResult> taiDanhSachBaiHatDeThem_DSPTL(string key = "")
        {
            var firebase = new FirebaseClient(Key);
            var baihat = LayBangBaiHat();
            var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai(key);
            var baihat2 =(from bh in baihat
                          join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                          select bh).ToList();
            var baihat1 = (from bh in baihat
                          select bh).ToList();
            if (baihat2.Count > 0)
            {
                foreach (var item in baihat2)
                {

                    baihat1.Remove(item);
                }
            }
            var di123lieu = (from bh1 in baihat1                    
                          where bh1.danhsachphattheloai_id != key.ToString() && bh1.daxoa == 0 && bh1.chedo == 1
                          select bh1 ).ToList();

            return Json(di123lieu.OrderByDescending(x => x.thoigian));
        }
        [HttpPost]
        // tải danh sách bài hát đã thêm vào dsp thể loại bằng tay (Đã Thêm)
        public async Task<IActionResult> taiDanhSachBaiHatDaThem_DSPTL(string key = "")
        {
            var firebase = new FirebaseClient(Key);
            var baihat = LayBangBaiHat();
            var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai(key.ToString());
          
            var baihatdathem = (from bh in baihat
                           join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                           where ctdsptl.danhsachphattheloai_id.Equals(key.ToString())
                           select bh).ToList();
            return Json(baihatdathem);
        }
        [HttpPost]
        // tải danh sách bài hát Mặc Định (Mặc định là khi tạo bài hát là đã thêm vào dsptl bằng cắt chọn khóa ngoại id dsp_tl)
        public async Task<IActionResult> taiDanhSachBaiHatMacDinh_DSPTL(string key = "")
        {
            var firebase = new FirebaseClient(Key);
            var baihat = LayBangBaiHat();
            var baihatmacdinh = (from bh in baihat
                                where bh.danhsachphattheloai_id.Equals(key.ToString())
                                select bh ).ToList();
            return Json(baihatmacdinh);
        }
        [HttpPost]
        // thêm bài bài vào dsp thể loại bằng tay id bài hát vs id dsp tl
        public async Task<IActionResult> themBaiHatVaoDSPTL_DSPTL([FromBody] chitietdanhsachphattheloaiModel item)
        {
            var firebase = new FirebaseClient(Key);

            bool success = true;
            try
            {
                if (item.baihat_id != null && item.baihat_id != "" || item.danhsachphattheloai_id != null && item.danhsachphattheloai_id != "")
                {
                    var dino = await firebase
                        .Child("csdlmoi")
                      .Child("chitietdanhsachphattheloai")
                      .Child(item.danhsachphattheloai_id)
                      .PostAsync(item)
                      ;
                    string kk = dino.Key.ToString();
                    item.id = kk;
                    await firebase
                        .Child("csdlmoi")
                       .Child("chitietdanhsachphattheloai")
                       .Child(item.danhsachphattheloai_id)
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
        // xóa bài bài khỏi dsp thể loại bằng tay id bài hát vs id dsp tl
        public async Task<IActionResult> xoaBaiHatkhoiDSPTL_DSPTL([FromBody] Text item)
        {
            bool success = true;
            try
            {
                if (item.key != null || item.uid != null) {
                    var firebase = new FirebaseClient(Key);
                    var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai(item.uid);
                    var data = (from ctdsptl in chitietdanhsachphattheloai
                                where ctdsptl.danhsachphattheloai_id.Equals(item.uid.ToString()) &&
                                ctdsptl.baihat_id.Equals(item.key.ToString())
                                select ctdsptl
                                ).ToList();
                    if (data.Count == 1)
                    {
                        await firebase
                       .Child("csdlmoi")
                      .Child("chitietdanhsachphattheloai")
                      .Child(data[0].danhsachphattheloai_id)
                      .Child(data[0].id)
                      .DeleteAsync();
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
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
            var auth = new FirebaseAuthProvider(new FirebaseConfig123(ApiKey));
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

