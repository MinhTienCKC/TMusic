
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
  
    public class GoiVipController : Controller
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
        public GoiVipController(IHostingEnvironment env)
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
        public async Task<IActionResult> CreateGoiVip([FromBody]goivipModel item)
        {
            bool success = true;
            item.thoigian = DateTime.Now;
            try
            {

                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                var dino = await firebase
                  .Child("goivip")
                  .PostAsync(item)
                  ;
              
                string kk = dino.Key.ToString();
                item.id = kk;
                 await firebase
                    .Child("goivip")
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
        public class goivipcustomModel : goivipModel
        {


            public int yeuthich { get; set; }
            public int daxoa { get; set; }
            // THÊM TRƯỜNG QUẢNG CÁO

        }
        public class baihatcustomModel : baihatModel
        {



            public DateTime thoigianxoa { get; set; }
            // THÊM TRƯỜNG QUẢNG CÁO

        }
        [HttpPost]
        public async Task<IActionResult> LoadQuangCao()
        {
            
                var firebase = new FirebaseClient(Key);
            goivipcustomModel goivip = new goivipcustomModel();
            //   firebase::database::DatabaseReference dbref = database->GetReference();
                 //await  firebase
                 // .Child("text")
                 // .Child("-MfWGEQQ3Xqt4c5r0v7H")
                 // .Client.Child("linkhinhanh").PutAsync("ok");
            //await firebase
            //     .Child("text")
            //     .Child("-MfWGEQQ3Xqt4c5r0v7H")
            //     .PutAsync(goivip);
            var dino = await firebase
                 .Child("text")
                 .OnceAsync<goivipModel>();
            var baihat = await firebase
                 .Child("baihat")
                 .OnceAsync<baihatModel>();

            //var danhsachphattheloai = await firebase
            //    .Child("danhsachphattheloai")
            //    .OnceAsync<danhsachphattheloaiModel>();
            //top20Model top20 = new top20Model();
            //foreach (var dsptl in danhsachphattheloai)
            //{
            //    top20.danhsachphattheloai_id = dsptl.Object.id;
            //    top20.daxoa = 0;
            //    top20.linkhinhanh = dsptl.Object.linkhinhanh;
            //    top20.mota = dsptl.Object.mota;
            //    top20.tentop20 = "Top 20 " + dsptl.Object.tendanhsachphattheloai;
            //    top20.theloai_id = dsptl.Object.theloai_id;
            //    var data = await firebase
            //    .Child("top20")
            //    .PostAsync(top20)
            //    ;
            //    string kk = data.Key.ToString();

            //    top20.id = kk;

            //    await firebase
            //       .Child("top20")
            //       .Child(kk)
            //       .PutAsync(top20);

            //    top20 = new top20Model();

            //}

            baihatcustomModel bh = new baihatcustomModel();
            foreach (var ok in baihat)
            {
                bh.id = ok.Object.id;
                bh.nguoidung_id = ok.Object.nguoidung_id;
                bh.tenbaihat = ok.Object.tenbaihat;
                bh.mota = ok.Object.mota;
                bh.luottaixuong = ok.Object.luottaixuong;
                bh.thoigian = ok.Object.thoigian;
                bh.chedo = ok.Object.chedo;
                bh.luotthich = ok.Object.luotthich;
                bh.casi = ok.Object.casi;
                bh.loibaihat = ok.Object.loibaihat;
                bh.luotnghe = ok.Object.luotnghe;
                bh.theloai_id = ok.Object.theloai_id;
                bh.chude_id = ok.Object.chude_id;
                bh.danhsachphattheloai_id = ok.Object.danhsachphattheloai_id;
                bh.quangcao = ok.Object.quangcao;
                bh.thoiluongbaihat = ok.Object.thoiluongbaihat;
                bh.link = ok.Object.link;
                bh.linkhinhanh = ok.Object.linkhinhanh;
                bh.daxoa = 0;
                bh.thoigianxoa = DateTime.Now;

                //public string id { get; set; }1
                //public string nguoidung_id { get; set; }2
                //public string tenbaihat { get; set; }3
                //public string mota { get; set; }4
                //public int luottaixuong { get; set; }5
                //public DateTime thoigian { get; set; }6
                //public int chedo { get; set; }7
                //public int luotthich { get; set; }8
                //public string casi { get; set; }9
                //public string loibaihat { get; set; }10
                //public int luotnghe { get; set; }11
                //public string theloai_id { get; set; }12
                //public string chude_id { get; set; }13
                //public string danhsachphattheloai_id { get; set; }14
                //public string quangcao { get; set; }15
                //public string thoiluongbaihat { get; set; }1 61
                //public string link { get; set; }17
                //public string linkhinhanh { get; set; }18
                await firebase
                   .Child("baihat")
                   .Child(bh.id)
                   .PutAsync(bh);
                bh = new baihatcustomModel();

            }



            return Json(dino);
        }
        //public List<danhsachphatcustomModel> convert(List<danhsachphattheloaiModel> list, string uid)
        //{
        //    var listyeuthichdsp = getListYeuThichDSPTheLoai();
        //    listyeuthichdsp = (from yeuthich in listyeuthichdsp
        //                       where yeuthich.nguoidung_id == uid
        //                       select yeuthich).ToList();
        //    List<danhsachphatcustomModel> listkq = new List<danhsachphatcustomModel>();
        //    for (int item = 0; item < list.Count(); item++)
        //    {
        //        danhsachphatcustomModel danhsachphat = new danhsachphatcustomModel();

        //        danhsachphat.id = list[item].id;
        //        danhsachphat.tendanhsachphattheloai = list[item].tendanhsachphattheloai;
        //        danhsachphat.linkhinhanh = list[item].linkhinhanh;
        //        danhsachphat.theloai_id = list[item].theloai_id;
        //        danhsachphat.mota = list[item].mota;
        //        bool checkYeuThich = false;
        //        for (int j = 0; j < listyeuthichdsp.Count(); j++)
        //        {

        //            if (list[item].id.Equals(listyeuthichdsp[j].danhsachphat_id))
        //            {
        //                checkYeuThich = true;
        //            }

        //        }
        //        if (checkYeuThich)
        //        {
        //            danhsachphat.yeuthich = 1;
        //            listkq.Add(danhsachphat);
        //        }
        //        else
        //        {
        //            danhsachphat.yeuthich = 0;
        //            listkq.Add(danhsachphat);
        //        }

        //    }
        //    return listkq;
        //}
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

