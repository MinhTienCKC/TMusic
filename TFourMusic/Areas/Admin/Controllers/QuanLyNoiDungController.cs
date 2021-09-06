using Firebase.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFourMusic.Models;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
   
    public class QuanLyNoiDungController : Controller
    {
        //private readonly ILogger<QuanLyNoiDungController> _logger;
        //public QuanLyNoiDungController(ILogger<QuanLyNoiDungController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly IHostingEnvironment _env;

        public QuanLyNoiDungController(IHostingEnvironment env)
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
        public class Text
        {
            public string linknhac { get; set; }
            public string tentaixuong { get; set; }
            public string key { get; set; }
        }
        [HttpPost]

        public async Task<string> downloadBaiHatVeMayNguoiDung([FromBody] Text item)
        {
            var path = Path.Combine(_env.WebRootPath, $"music\\Download\\");
            var filename = item.tentaixuong;
            var link = item.linknhac;

            using (WebClient wc = new WebClient())
            {

                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new System.Uri(link),
                    // Param2 = Path to save
                    path + filename
                );
            };
            long kt = 0;
            long size = 0;
            while ( size < 1 )
            {
                FileInfo finfo = new FileInfo(path + filename);
                size = finfo.Length;
                
                //if( size > size - 2 )
                //{
                //    kt = 5;
                //}    
            }        
            return filename;
        }
        [HttpPost]
        public async Task<IActionResult> xoaNhacDaTaiXuong([FromBody] Text item)
        {
            
            bool success = true;
            var path = Path.Combine(_env.WebRootPath, $"music\\Download\\");
            try
            {
                System.IO.File.Delete(Path.Combine(path, item.tentaixuong));
                success = true;

            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);



            //var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            //var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            //var opiton = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            //{
            //    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
            //    ThrowOnCancel = true
            //});
            //var resultContent = "N/A";
            //var link = "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/image%2Fdanhsachphat%2Ft%E1%BA%A3i%20xu%E1%BB%91ng%20(1)%20-%20Copy.jpg?alt=media&token=f16727ee-1854-4e2a-b9e3-086010ce2d4f";
            //using (var http = await opiton.Options.CreateHttpClientAsync().ConfigureAwait(false))
            //{
            //    var result = await http.DeleteAsync(link).ConfigureAwait(false);

            //    resultContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            //    result.EnsureSuccessStatusCode();
            //}

        }
    }
}
