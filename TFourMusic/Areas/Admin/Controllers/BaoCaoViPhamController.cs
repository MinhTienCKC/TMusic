
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
using MimeKit;
using MailKit.Net.Smtp;
using FirebaseAdmin.Auth;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
    public class BaoCaoViPhamController : Controller
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
        public BaoCaoViPhamController(IHostingEnvironment env)
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
        public async Task<IActionResult> taiBaiHatViPham()
        {
            try
            {
                var firebase = new FirebaseClient(Key);

                var dino = await firebase
                    .Child("csdlmoi")
                  .Child("baocao")
                  .Child("baihat")
                  .OnceAsync<baocaobaihatModel>();

                return Json(dino);
              //  return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        [HttpPost]
        public async Task<IActionResult> taiNguoiDungViPham()
        {
            try
            {
                var firebase = new FirebaseClient(Key);

                var dino = await firebase
                    .Child("csdlmoi")
                  .Child("baocao")
                  .Child("nguoidung")
                  .OnceAsync<baocaobaihatModel>();

                return Json(dino);
               // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        public class ShareModel
        {
            public string IdAlbum { get; set; }
            public string IdUsers { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> taiBaiHatViPhamChuaXuLy()
        {
            try
            {
                //string  a = item.IdUsers;
                //  dynamic json = JsonConvert.DeserializeObject(a);

                //var firebase = new FirebaseClient(Key);

                //var dino = await firebase
                //    .Child("csdlmoi")
                //  .Child("baocao")
                //  .Child("baihatvipham")
                //   .Child("chuaxuly")
                //  .Child("Zrbe0PBKXOPpi6QluEPi4YZPUi32")
                //  .Child("-MkWFfJxQTT1ZWP7-8F3")
                //  .OnceAsync<Dictionary<string, baocaobaihatModel>>();

                var baihatbaocao = LayBangBaoCaoBaiHatChuaXuLy();
                //var dulieu = (from bc in dino
                //              select bc.Object).ToList();
                var data = (from bcvp in baihatbaocao
                            select new
                            {
                                // loibaihat = JsonConvert.DeserializeObject(ok123.Object.noidung)
                                id = bcvp.id,
                                noidung = JsonConvert.DeserializeObject<string[]>(bcvp.noidung),
                                motavande = bcvp.motavande,
                                nguoidung_id = bcvp.nguoidung_id,
                                nguoidung_baocao_id = bcvp.nguoidung_baocao_id,
                                baihat_id = bcvp.baihat_id,
                                baihat_baocao_id = bcvp.baihat_baocao_id,
                             //   baihatvipham = LayBaiHatQuaID(bcvp.nguoidung_baocao_id,bcvp.baihat_baocao_id),
                                thoigian = bcvp.thoigian,
                                ngayxuly = bcvp.ngayxuly,
                                daxoa = bcvp.daxoa,
                                trangthai = bcvp.trangthai,
                                email_nguoidung = LayEmailQuaNguoiDungID(bcvp.nguoidung_id),
                                email_nguoidung_baocao = LayEmailQuaNguoiDungID(bcvp.nguoidung_baocao_id)
                                //                                email_nguoidung = LayBangNguoiDung(bcvp.nguoidung_id), //LayEmailQuaNguoiDungID(bcvp.nguoidung_id),
                                //email_nguoidung_baocao = LayBangNguoiDung(bcvp.nguoidung_baocao_id)
                            }).ToList();
                return Json(data.OrderByDescending(x => x.thoigian));
               // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


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
        public List<baocaobaihatModel> LayBangBaoCaoBaiHatDaXuLy_KhongViPham(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/baihatvipham/daxuly/khongvipham");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaobaihatModel>();


                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            foreach (var x in item)
                            {
                                foreach (var y in x)

                                {
                                    list.Add(JsonConvert.DeserializeObject<baocaobaihatModel>(((JProperty)y).Value.ToString()));

                                }

                            }

                        }
                    }
                    return list;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/baihatvipham/daxuly/khongvipham" + uid);
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaobaihatModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baocaobaihatModel>(((JProperty)item).Value.ToString()));

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
        public List<baocaobaihatModel> LayBangBaoCaoBaiHatDaXuLy_ViPham(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/baihatvipham/daxuly/vipham");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaobaihatModel>();


                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            foreach (var x in item)
                            {
                                foreach (var y in x)

                                {
                                    list.Add(JsonConvert.DeserializeObject<baocaobaihatModel>(((JProperty)y).Value.ToString()));

                                }

                            }

                        }
                    }
                    return list;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/baihatvipham/daxuly/vipham" + uid);
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaobaihatModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baocaobaihatModel>(((JProperty)item).Value.ToString()));

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




        public List<baocaobaihatModel> LayBangBaoCaoBaiHatChuaXuLy(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/baihatvipham/chuaxuly");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaobaihatModel>();


                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            foreach (var x in item)
                            {
                                foreach (var y in x)

                                {
                                    list.Add(JsonConvert.DeserializeObject<baocaobaihatModel>(((JProperty)y).Value.ToString()));

                                }

                            }

                        }
                    }
                    return list;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/baihatvipham/chuaxuly/" + uid);
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaobaihatModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baocaobaihatModel>(((JProperty)item).Value.ToString()));

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

        public class BaiHatBangQuyen
        {
            public string baihat_id { get; set; }

            public string nguoidung_id { get; set; }
        }



        
             [HttpPost]
        public async Task<IActionResult> taiBaiHatBangQuyen([FromBody] BaiHatBangQuyen item )
        {
            try
            {
                //string  a = item.IdUsers;
                //  dynamic json = JsonConvert.DeserializeObject(a);

                //var firebase = new FirebaseClient(Key);

                //var dino = await firebase
                //    .Child("csdlmoi")
                //  .Child("baocao")
                //  .Child("baihatvipham")
                //   .Child("chuaxuli")
                //  //.Child("hFxjlTD1nzeMgmtJiIBQN9xGJ8D3")
                //  .OnceAsync<Dictionary<string,baocaobaihatModel>>();

                //   var baihatbaocao = LayBangBaoCaoBaiHatChuaXuLy();
                //var dulieu = (from bc in dino
                //              select bc.Object).ToList();
                
                var data = LayBaiHatQuaID(item.nguoidung_id, item.baihat_id);
                return Json(data);
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        public class PheDuyetModel:modelTrangThai
        {
            //public string nguoidung_id { get; set; }

            //public string baocao_id { get; set; }
            public int vhh_baihat { get; set; }

            public int vhh_nguoidung { get; set; }
        }
        public baocaobaihatModel LayChiTietBangBaoCaoBaiHatChuaXuLy(string nguoidungid, string idbh)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse rp = client.Get("csdlmoi/baocao/baihatvipham/chuaxuly/" + nguoidungid.ToString() + "/" + idbh.ToString());
            var datarp = JsonConvert.DeserializeObject<baocaobaihatModel>(rp.Body);
            baocaobaihatModel bh = new baocaobaihatModel();
            if (datarp != null)
            {
                bh = datarp;
            }
            return bh;
        }

        [HttpPost]
        public async Task<IActionResult> pheDuyetBaiHatViPham([FromBody] PheDuyetModel item)
        {
            bool success = true;
            try
            {
                var firebase = new FirebaseClient(Key);
                var data = LayChiTietBangBaoCaoBaiHatChuaXuLy(item.nguoidung_id, item.id);
                var nguoidung = LayBangNguoiDung(data.nguoidung_id);
                if (item.vhh_baihat == 1 || item.vhh_nguoidung == 1)
                {
                    data.trangthai = 2;
                    await firebase
                   .Child("csdlmoi")
                  .Child("baocao")
                  .Child("baihatvipham")
                  .Child("daxuly")
                  .Child("vipham")
                  .Child(item.nguoidung_id)
                  .Child(item.id)
                 .PutAsync(data);


                       await firebase
                  .Child("csdlmoi")
                  .Child("baocao")
                  .Child("baihatvipham")
                  .Child("chuaxuly")
                  .Child(item.nguoidung_id)
                  .Child(item.id)
                  .DeleteAsync();
                 
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                    message.Subject = "TMUSIC - BÀI HÁT VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Chào: Anh/chị "+ nguoidung[0].hoten +"\n\n" +
                        "Chúng tôi đã vô hiệu hóa bài hát bạn báo cáo.\n" +
                        "Chúng tôi đã xem xét bài hát bạn báo cáo. Vì bài hát đã vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi đã vô hiệu hóa bài hát đó. " +
                        "Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic.. Chúng tôi thông báo cho chủ bài biết rằng bài hát của họ đã bị vô hiệu hóa. \n\n" +
                        //"Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        "Cám ơn bạn đã xem. \n" +
                        "Admin TMUSIC"
                    };
                    using (var client = new SmtpClient())
                    {
                        // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.CheckCertificateRevocation = false;
                        // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                        client.Connect("smtp.gmail.com", 587, false);
                        //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                        client.Authenticate("0306181067@caothang.edu.vn", "281258964");

                        client.Send(message);
                        client.Disconnect(true);
                    }

                }
                else if (item.vhh_baihat == 0 && item.vhh_nguoidung == 0)
                {
                    data.trangthai = 2;
                    await firebase
                  .Child("csdlmoi")
                  .Child("baocao")
                  .Child("baihatvipham")
                  .Child("daxuly")
                  .Child("khongvipham")
                  .Child(item.nguoidung_id)
                  .Child(item.id)
                  .PutAsync(data);

                        await firebase
                 .Child("csdlmoi")
                 .Child("baocao")
                 .Child("baihatvipham")
                 .Child("chuaxuly")
                 .Child(item.nguoidung_id)
                 .Child(item.id)
                 .DeleteAsync();

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                    message.Subject = "TMUSIC - BÀI HÁT VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Chào: Anh/chị " + nguoidung[0].hoten + "\n\n" +
                        //"Chúng tôi đã vô hiệu hóa bài hát bạn báo cáo.\n" +
                        "Chúng tôi đã xem xét bài hát bạn báo cáo. Vì bài hát không vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi nghĩ rằng bạn có chút nhầm lẫn. " +
                        "Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. \n\n" +
                        //"Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        "Cám ơn bạn đã xem. \n" +
                        "Admin TMUSIC"
                    };
                    using (var client = new SmtpClient())
                    {
                        // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.CheckCertificateRevocation = false;
                        // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                        client.Connect("smtp.gmail.com", 587, false);
                        //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                        client.Authenticate("0306181067@caothang.edu.vn", "281258964");

                        client.Send(message);
                        client.Disconnect(true);
                    }
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
        public async Task<IActionResult> voHieuHoaNguoiDung([FromBody] nguoidungModel item)
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
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", item.email));
                    message.Subject = "TMUSIC - TÀI KHOẢN VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Chào: Anh/chị "+ item.hoten +" \n\n" +
                        "Có vẻ như tài khoản của bạn đã bị vô hiệu hóa do nhầm lẫn. Chúng tôi đã mở tài khoản của bạn và xin lỗi vì sự bất tiện này.\n\n" +
                        " Bây giờ, bạn có thể đăng nhập. \n" +
                        "Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        "Cám ơn bạn đã xem. \n" +
                        "Admin TMUSIC"
                    };
                    using (var client = new SmtpClient())
                    {
                        // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.CheckCertificateRevocation = false;
                        // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                        client.Connect("smtp.gmail.com", 587, false);
                        //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                        client.Authenticate("0306181067@caothang.edu.vn", "281258964");

                        client.Send(message);
                        client.Disconnect(true);
                    }
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
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", item.email));
                    message.Subject = "TMUSIC - TÀI KHOẢN VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Chào: Anh/chị " + item.hoten + " \n\n" +
                        "Chúng tôi đã khóa tài khoản của bạn do vi phạm điều khoản cộng đồng. \n" +
                        "Nếu có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        "Cám ơn bạn đã xem. \n" +
                        "Admin TMUSIC"
                    };
                    using (var client = new SmtpClient())
                    {
                        // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.CheckCertificateRevocation = false;
                        // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                        client.Connect("smtp.gmail.com", 587, false);
                        //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                        client.Authenticate("0306181067@caothang.edu.vn", "281258964");

                        client.Send(message);
                        client.Disconnect(true);
                    }
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
        public async Task<IActionResult> voHieuHoaBaiHatNguoiDung([FromBody] baihatModel item)
        {
            

            bool success = true;
            try
            {
                var nguoidung = LayBangNguoiDung(item.nguoidung_id);

                if (item.vohieuhoa == 0)
                {

                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                    message.Subject = "TMUSIC - BÀI HÁT VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Chào: Anh/chị " + nguoidung[0].hoten + " \n\n" +
                        "Chúng tôi mở khóa bài hát của bạn, bây giờ bài hát có thể xuất hiện trước cộng đồng. \n" +
                        "Nếu có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        "Cám ơn bạn đã xem. \n" +
                        "Admin TMUSIC"
                    };
                    using (var client = new SmtpClient())
                    {
                        // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.CheckCertificateRevocation = false;
                        // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                        client.Connect("smtp.gmail.com", 587, false);
                        //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                        client.Authenticate("0306181067@caothang.edu.vn", "281258964");

                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                else
                {

                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);
                   
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                    message.Subject = "TMUSIC - BÀI HÁT VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Chào: Anh/chị " + nguoidung[0].hoten + " \n\n" +
                        "Chúng tôi đã khóa bài hát của bạn, bây giờ bài hát sẽ không được xuất hiện trước cộng đồng. \n" +
                        "Vui lòng bạn thay đổi thông tin bài hát cho phù hợp với tiêu chuẩn cộng đồng. \n" +
                        "Nếu có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        "Cám ơn bạn đã xem. \n" +
                        "Admin TMUSIC"
                    };
                    using (var client = new SmtpClient())
                    {
                        // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                        client.CheckCertificateRevocation = false;
                        // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                        client.Connect("smtp.gmail.com", 587, false);
                        //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                        client.Authenticate("0306181067@caothang.edu.vn", "281258964");

                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        public class modelTrangThai
        {
                    
            public string nguoidung_id { get; set; }

            public string id { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> capNhatTrangThai([FromBody] modelTrangThai item)
        {


            bool success = true;
            try
            {

                

                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/baocao/baihatvipham/chuaxuly/" + item.nguoidung_id + "/" + item.id + "/" + "trangthai", 1);
               
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> taiChiTietNguoiDungViPham([FromBody] BaiHatBangQuyen item)
        {
            try
            {
              
                var data = LayBangNguoiDung(item.nguoidung_id);
                return Json(data);
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        [HttpPost]

        
        public string LayEmailQuaNguoiDungID(string nguoidung_id)
        {
            try
            {
                // string a = item.IdUsers;
                // dynamic json = JsonConvert.DeserializeObject(a);
                if(nguoidung_id == "admin"){
                    return "admin";
                }
                var data = LayBangNguoiDung(nguoidung_id);

               
                return data[0].email;
                // return Json("null");
            }
            catch (Exception ex)
            {
                return "";
            }


        }
        [HttpPost]
        public List<baihatModel> LayBaiHatQuaID(string nguoidung_baocao_id, string baihat_baocao_id)
        {
            try
            {
     
       
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/baihat/" + nguoidung_baocao_id + "/" + baihat_baocao_id);
                var data = JsonConvert.DeserializeObject<baihatModel>(response.Body);
                List<baihatModel> list = new List<baihatModel>();
                if (data != null)
                {
                    list.Add(data);
                }

                
                return list;
                // return Json("null");
            }
            catch (Exception ex)
            {
                return null; 
            }


        }
      
    }
}
