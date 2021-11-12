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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
    [Authorize]
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

            var data = (from t20 in NguoiDung

                        select t20).ToList() ;
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
            return Json(data.OrderByDescending(x => x.thoigian));
        }

        [HttpPost]
        public async Task<IActionResult> voHieuHoa([FromBody] nguoidungModel item)
        {
            var heThong = User.Identity as ClaimsIdentity;
            var phanQuyen = heThong.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            if (phanQuyen == "Admin")
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
                        message.Subject = "TMUSIC - TÀI KHOẢN VI PHẠM ĐIỀU KHOẢN CỘNG ĐỒNG";
                        message.Body = new TextPart("html")
                        {
                            //Text = "Chào: Anh/chị " + item.hoten + " \n\n" +
                            //"Có vẻ như tài khoản của bạn đã bị vô hiệu hóa do nhầm lẫn. Chúng tôi đã mở tài khoản của bạn và xin lỗi vì sự bất tiện này.\n\n" +
                            //" Bây giờ, bạn có thể đăng nhập. \n" +
                            //"Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                            //"Cám ơn bạn đã xem. \n" +
                            //"Admin TMUSIC"
                            Text = $@"<h3>Chào: {item.hoten}</h3>

                                    <p>Có vẻ như tài khoản của bạn đã bị vô hiệu hóa do nhầm lẫn. </p> </br>
                                    <p>Chúng tôi đã mở tài khoản của bạn, xin lỗi vì sự bất tiện này. </p> </br>
                                    <p>Bây giờ, bạn có thể đăng nhập. </p> </br>
                                    <p>Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này. </p> </br>
                                    </br>
                                    <p>Cám ơn bạn.</p> </br>
                                    <h5>Admin TMUSIC.</h5> </br></br></br></br></br>
                                    <table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:top'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='text-align:center'><img src='https://firebasestorage.googleapis.com/v0/b/music-77ac9.appspot.com/o/8.png?alt=media&amp;token=ae9c1de7-661c-4b35-a04b-c4ab02bce360' width='130' style='max-width:128px;display:block'></td></tr><tr><td height='30'></td></tr><tr><td style='text-align:center'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;display:inline-block'><tbody><tr><td><a href='https://www.facebook.com/minhtien6120/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/facebook-icon-2x.png' alt='Facebook' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td><td><a href='https://www.instagram.com/hominhtienckc/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/instagram-icon-2x.png' alt='instagram' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td></tr></tbody></table></td></tr></tbody></table></td><td width='46'><div></div></td><td style='padding:0px;vertical-align:middle'><h3 color='#333333' style='margin:0px;font-size:18px;color:rgb(51,51,51)'><font style='vertical-align:inherit'>&nbsp; Hồ&nbsp;</font><font style='vertical-align:inherit'>Minh Tiến</font>&nbsp;<font style='vertical-align:inherit'></font></h3><p color='#333333' style='margin:0px;color:rgb(51,51,51);font-size:14px;line-height:22px'><font style='vertical-align:inherit'>&nbsp; &nbsp;T-Music</font></p><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;width:644px'><tbody><tr><td height='30'></td></tr><tr><td color='#ff5f88' height='1' style='width:644px;border-bottom:1px solid rgb(255,95,136);border-left:none;display:block'></td></tr><tr><td height='30'></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/phone-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px;color:rgb(51,51,51)'><a href='tel:0981275911' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>0981275911</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/email-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='mailto:hominhtienxyz@gmail.com' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>hominhtienxyz@gmail.com</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/link-icon-2x.png' color='#ff5f88' width='13' style='display: block; margin-right: 0px;' height='13'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='https://tmusic-1.herokuapp.com/' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>https://tmusic-1.herokuapp.com/</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/address-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><span color='#333333' style='font-size:12px;color:rgb(51,51,51)'><font style='vertical-align:inherit'><font style='vertical-align:inherit'>41 Bàu Sen, khu phố 6, TT Dầu Tiếng, H. Dầu Tiếng, Tỉnh Bình Dương, 117/157/30 Nguyễn Hữu Cảnh, Phường 22, Quận Bình Thạnh, TP.&nbsp;</font><font style='vertical-align:inherit'>Hồ Chí Minh</font></font></span></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td height='30'></td></tr></tbody></table><a href='https://www.hubspot.com/email-signature-generator?utm_source=create-signature' rel='noopener noreferrer' style='font-size:12px;display:block;color:rgb(51,51,51)' target='_blank'><br></a></td></tr></tbody></table>
                                        "

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
                        message.Subject = "TMUSIC - TÀI KHOẢN VI PHẠM ĐIỀU KHOẢN CỘNG ĐỒNG";
                        message.Body = new TextPart("html")
                        {
                            //Text = "Chào: Anh/chị " + item.hoten + " \n\n" +
                            //"Chúng tôi đã khóa tài khoản của bạn do vi phạm điều khoản cộng đồng. \n" +
                            //"Nếu có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                            //"Cám ơn bạn đã xem. \n" +
                            //"Admin TMUSIC"
                            Text = $@"<h3>Chào: {item.hoten}</h3>

                                    <p>Chúng tôi đã khóa tài khoản của bạn do vi phạm điều khoản cộng đồng.</p> </br>
                                    <p>Nếu có thắc mắc vui lòng trả lời thư này. </p> </br>
                                    </br>
                                    <p>Cám ơn bạn.</p> </br>
                                    <h5>Admin TMUSIC.</h5> </br></br></br></br></br>
                                    <table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:top'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='text-align:center'><img src='https://firebasestorage.googleapis.com/v0/b/music-77ac9.appspot.com/o/8.png?alt=media&amp;token=ae9c1de7-661c-4b35-a04b-c4ab02bce360' width='130' style='max-width:128px;display:block'></td></tr><tr><td height='30'></td></tr><tr><td style='text-align:center'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;display:inline-block'><tbody><tr><td><a href='https://www.facebook.com/minhtien6120/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/facebook-icon-2x.png' alt='Facebook' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td><td><a href='https://www.instagram.com/hominhtienckc/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/instagram-icon-2x.png' alt='instagram' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td></tr></tbody></table></td></tr></tbody></table></td><td width='46'><div></div></td><td style='padding:0px;vertical-align:middle'><h3 color='#333333' style='margin:0px;font-size:18px;color:rgb(51,51,51)'><font style='vertical-align:inherit'>&nbsp; Hồ&nbsp;</font><font style='vertical-align:inherit'>Minh Tiến</font>&nbsp;<font style='vertical-align:inherit'></font></h3><p color='#333333' style='margin:0px;color:rgb(51,51,51);font-size:14px;line-height:22px'><font style='vertical-align:inherit'>&nbsp; &nbsp;T-Music</font></p><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;width:644px'><tbody><tr><td height='30'></td></tr><tr><td color='#ff5f88' height='1' style='width:644px;border-bottom:1px solid rgb(255,95,136);border-left:none;display:block'></td></tr><tr><td height='30'></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/phone-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px;color:rgb(51,51,51)'><a href='tel:0981275911' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>0981275911</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/email-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='mailto:hominhtienxyz@gmail.com' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>hominhtienxyz@gmail.com</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/link-icon-2x.png' color='#ff5f88' width='13' style='display: block; margin-right: 0px;' height='13'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='https://tmusic-1.herokuapp.com/' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>https://tmusic-1.herokuapp.com/</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/address-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><span color='#333333' style='font-size:12px;color:rgb(51,51,51)'><font style='vertical-align:inherit'><font style='vertical-align:inherit'>41 Bàu Sen, khu phố 6, TT Dầu Tiếng, H. Dầu Tiếng, Tỉnh Bình Dương, 117/157/30 Nguyễn Hữu Cảnh, Phường 22, Quận Bình Thạnh, TP.&nbsp;</font><font style='vertical-align:inherit'>Hồ Chí Minh</font></font></span></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td height='30'></td></tr></tbody></table><a href='https://www.hubspot.com/email-signature-generator?utm_source=create-signature' rel='noopener noreferrer' style='font-size:12px;display:block;color:rgb(51,51,51)' target='_blank'><br></a></td></tr></tbody></table>
                                        "
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
            else
            {
                return Json("admin");
            }

           
        }
        [HttpPost]
        public async Task<IActionResult> voHieuHoaBaiHatNguoiDung([FromBody]baihatModel item)
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
                    message.Body = new TextPart("html")
                    {
                        //Text = "Chào: Anh/chị " + nguoidung[0].hoten + " \n\n" +
                        //"Chúng tôi mở khóa bài hát của bạn, bây giờ bài hát có thể xuất hiện trước cộng đồng. \n" +
                        //"Nếu có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        //"Cám ơn bạn đã xem. \n" +
                        //"Admin TMUSIC"

                         Text = $@"<h3>Chào: {nguoidung[0].hoten}</h3>

                                    <p>Có vẻ như bài hát của bạn đã bị vô hiệu hóa do nhầm lẫn. </p> </br>
                                    <p>Chúng tôi đã mở khóa bài hát của bạn, xin lỗi vì sự bất tiện này. </p> </br>
                                    <p>Bây giờ, bài hát của bạn có thể công khai với mọi người. </p> </br>
                                    <p>Nếu bạn gặp bất kì sự cố với bài của mình hay có thắc mắc vui lòng trả lời thư này. </p> </br>
                                    </br>
                                    <p>Cám ơn bạn.</p> </br>
                                    <h5>Admin TMUSIC.</h5> </br></br></br></br></br>
                                    <table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:top'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='text-align:center'><img src='https://firebasestorage.googleapis.com/v0/b/music-77ac9.appspot.com/o/8.png?alt=media&amp;token=ae9c1de7-661c-4b35-a04b-c4ab02bce360' width='130' style='max-width:128px;display:block'></td></tr><tr><td height='30'></td></tr><tr><td style='text-align:center'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;display:inline-block'><tbody><tr><td><a href='https://www.facebook.com/minhtien6120/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/facebook-icon-2x.png' alt='Facebook' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td><td><a href='https://www.instagram.com/hominhtienckc/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/instagram-icon-2x.png' alt='instagram' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td></tr></tbody></table></td></tr></tbody></table></td><td width='46'><div></div></td><td style='padding:0px;vertical-align:middle'><h3 color='#333333' style='margin:0px;font-size:18px;color:rgb(51,51,51)'><font style='vertical-align:inherit'>&nbsp; Hồ&nbsp;</font><font style='vertical-align:inherit'>Minh Tiến</font>&nbsp;<font style='vertical-align:inherit'></font></h3><p color='#333333' style='margin:0px;color:rgb(51,51,51);font-size:14px;line-height:22px'><font style='vertical-align:inherit'>&nbsp; &nbsp;T-Music</font></p><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;width:644px'><tbody><tr><td height='30'></td></tr><tr><td color='#ff5f88' height='1' style='width:644px;border-bottom:1px solid rgb(255,95,136);border-left:none;display:block'></td></tr><tr><td height='30'></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/phone-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px;color:rgb(51,51,51)'><a href='tel:0981275911' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>0981275911</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/email-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='mailto:hominhtienxyz@gmail.com' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>hominhtienxyz@gmail.com</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/link-icon-2x.png' color='#ff5f88' width='13' style='display: block; margin-right: 0px;' height='13'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='https://tmusic-1.herokuapp.com/' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>https://tmusic-1.herokuapp.com/</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/address-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><span color='#333333' style='font-size:12px;color:rgb(51,51,51)'><font style='vertical-align:inherit'><font style='vertical-align:inherit'>41 Bàu Sen, khu phố 6, TT Dầu Tiếng, H. Dầu Tiếng, Tỉnh Bình Dương, 117/157/30 Nguyễn Hữu Cảnh, Phường 22, Quận Bình Thạnh, TP.&nbsp;</font><font style='vertical-align:inherit'>Hồ Chí Minh</font></font></span></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td height='30'></td></tr></tbody></table><a href='https://www.hubspot.com/email-signature-generator?utm_source=create-signature' rel='noopener noreferrer' style='font-size:12px;display:block;color:rgb(51,51,51)' target='_blank'><br></a></td></tr></tbody></table>
                                        "
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
                    message.Body = new TextPart("html")
                    {
                        //Text = "Chào: Anh/chị " + nguoidung[0].hoten + " \n\n" +
                        //"Chúng tôi đã khóa bài hát của bạn, bây giờ bài hát sẽ không được xuất hiện trước cộng đồng. \n" +
                        //"Vui lòng bạn thay đổi thông tin bài hát cho phù hợp với tiêu chuẩn cộng đồng. \n" +
                        //"Nếu có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        //"Cám ơn bạn đã xem. \n" +
                        //"Admin TMUSIC"

                        Text = $@"<h3>Chào: {nguoidung[0].hoten}</h3>

                                    <p>Chúng tôi đã khóa bài hát của bạn. </p> </br>
                                    <p>Bây giờ bài hát sẽ không được xuất hiện trước cộng đồng. </p> </br>
                                    <p>Vui lòng bạn thay đổi thông tin bài hát cho phù hợp với tiêu chuẩn cộng đồng. </p> </br>
                                    <p>Nếu bạn có thắc mắc vui lòng trả lời thư này. </p> </br>
                                    </br>
                                    <p>Cám ơn bạn.</p> </br>
                                    <h5>Admin TMUSIC.</h5> </br></br></br></br></br>
                                    <table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:top'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='text-align:center'><img src='https://firebasestorage.googleapis.com/v0/b/music-77ac9.appspot.com/o/8.png?alt=media&amp;token=ae9c1de7-661c-4b35-a04b-c4ab02bce360' width='130' style='max-width:128px;display:block'></td></tr><tr><td height='30'></td></tr><tr><td style='text-align:center'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;display:inline-block'><tbody><tr><td><a href='https://www.facebook.com/minhtien6120/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/facebook-icon-2x.png' alt='Facebook' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td><td><a href='https://www.instagram.com/hominhtienckc/' color='#243edd' style='display:inline-block;padding:0px;background-color:rgb(36,62,221)' target='_blank'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/instagram-icon-2x.png' alt='instagram' color='#243edd' height='24' style='max-width:135px;display:block'></a></td><td width='5'><div></div></td></tr></tbody></table></td></tr></tbody></table></td><td width='46'><div></div></td><td style='padding:0px;vertical-align:middle'><h3 color='#333333' style='margin:0px;font-size:18px;color:rgb(51,51,51)'><font style='vertical-align:inherit'>&nbsp; Hồ&nbsp;</font><font style='vertical-align:inherit'>Minh Tiến</font>&nbsp;<font style='vertical-align:inherit'></font></h3><p color='#333333' style='margin:0px;color:rgb(51,51,51);font-size:14px;line-height:22px'><font style='vertical-align:inherit'>&nbsp; &nbsp;T-Music</font></p><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial;width:644px'><tbody><tr><td height='30'></td></tr><tr><td color='#ff5f88' height='1' style='width:644px;border-bottom:1px solid rgb(255,95,136);border-left:none;display:block'></td></tr><tr><td height='30'></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/phone-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px;color:rgb(51,51,51)'><a href='tel:0981275911' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>0981275911</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/email-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='mailto:hominhtienxyz@gmail.com' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>hominhtienxyz@gmail.com</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/link-icon-2x.png' color='#ff5f88' width='13' style='display: block; margin-right: 0px;' height='13'></span></td></tr></tbody></table></td><td style='padding:0px'><a href='https://tmusic-1.herokuapp.com/' color='#333333' style='color:rgb(51,51,51);font-size:12px' target='_blank'>https://tmusic-1.herokuapp.com/</a></td></tr><tr height='25' style='vertical-align:middle'><td width='30' style='vertical-align:middle'><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td style='vertical-align:bottom'><span color='#ff5f88' width='11' style='display:block;background-color:rgb(255,95,136)'><img src='https://cdn2.hubspot.net/hubfs/53/tools/email-signature-generator/icons/address-icon-2x.png' color='#ff5f88' width='13' style='display:block'></span></td></tr></tbody></table></td><td style='padding:0px'><span color='#333333' style='font-size:12px;color:rgb(51,51,51)'><font style='vertical-align:inherit'><font style='vertical-align:inherit'>41 Bàu Sen, khu phố 6, TT Dầu Tiếng, H. Dầu Tiếng, Tỉnh Bình Dương, 117/157/30 Nguyễn Hữu Cảnh, Phường 22, Quận Bình Thạnh, TP.&nbsp;</font><font style='vertical-align:inherit'>Hồ Chí Minh</font></font></span></td></tr></tbody></table><table cellpadding='0' cellspacing='0' style='vertical-align:-webkit-baseline-middle;font-family:Arial'><tbody><tr><td height='30'></td></tr></tbody></table><a href='https://www.hubspot.com/email-signature-generator?utm_source=create-signature' rel='noopener noreferrer' style='font-size:12px;display:block;color:rgb(51,51,51)' target='_blank'><br></a></td></tr></tbody></table>
                                        "
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
                    //var message = new MimeMessage();
                    //message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    //message.To.Add(new MailboxAddress("Người Dùng", "dang60780@gmail.com"));
                    //message.Subject = "TMUSIC - TÀI KHOẢN VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    //message.Body = new TextPart("plain")
                    //{
                    //    Text = "Kính gửi: Anh/chị \n\n" +
                    //    "Chúng tôi đã mở khóa tài khoản của bạn. \n" +
                    //    "Nếu có thắc mắc vui lòng liên hệ SĐT:0981275911 hoặc Fanpage: TMUSIC Nghe Nhạc Trức Tuyến \n\n" +
                    //    "Cám ơn bạn đã xem. \n" +
                    //    "Admin TMUSIC"
                    //};
                    //using (var client = new SmtpClient())
                    //{
                    //    // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    //    client.CheckCertificateRevocation = false;
                    //    // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                    //    client.Connect("smtp.gmail.com", 587, false);
                    //    //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                    //    client.Authenticate("0306181067@caothang.edu.vn", "281258964");

                    //    client.Send(message);
                    //    client.Disconnect(true);
                    //}
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

