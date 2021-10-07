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
using static MoreLinq.Extensions.LagExtension;
using static MoreLinq.Extensions.LeadExtension;
using MoreLinq;
using System.Globalization;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace TFourMusic.Controllers
{

    public class HomeController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "vHcXcNH4jYpiScpS8Fw3mSJhUj6lX3zp4kgpIM7T",
            BasePath = "https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        [Obsolete]
        private IHostingEnvironment _env;

        [Obsolete]
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        private string ApiKey = "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q";
        //private static string Bucket = "musictt-9aa5f.appspot.com";
        private string Bucket = "tfourmusic-1e3ff.appspot.com";
        private string AuthEmail = "dang60780@gmail.com";
        private string AuthPassword = "0362111719@TTai";
        private string Key = " https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";

        private static int ngayUpdatexuhuong = 16, namUpdatexuhuong = 2018;

        // paypal 
        static hoadonthanhtoanModel hoadontam;
        static String clientId = "AQmn28mP8riU8dfxBIvtPDk_Elpe8cmtm68j6uMR13qwuB9Z9JPSEfqgbyrixldj3L7nsM60QJJ8Vzfa";
        static String secret = "ELYN4AKVxpFdNkf239-tEH2Z5TDMousP7VN0K9nz5bMmvy90aCz_5OpNWjhGFbeY1DQKubaMJ1IdidFY";
        static string orderIdPaypal = "";

        public static string firstSign { get; set; }
        public static HttpClient Clientpaypal()
        {
            // Creating a sandbox environment
            PayPalEnvironment environment = new SandboxEnvironment(clientId, secret);

            // Creating a client for the environment
            PayPalHttpClient clientpaypal = new PayPalHttpClient(environment);
            return clientpaypal;
        }
        // 18/08 Đã sữa CSDL Mới
        public async Task<IActionResult> ThanhToanThanhCongPayPal()
        {

            captureOrder(orderIdPaypal);
            try
            {
                hoadontam.mota = "Thanh toán vip bằng paypal thành công." + "-" + "Ngày thanh toán: " + hoadontam.thoigian + "  -" + "Giá tiền: " + hoadontam.giatien;

                client = new FireSharp.FirebaseClient(config);
                PushResponse response_dsphat = client.Push("csdlmoi/hoadonthanhtoan/" + hoadontam.nguoidung_id.ToString(), hoadontam);
                hoadontam.id = response_dsphat.Result.name;
                SetResponse setResponse = client.Set("csdlmoi/hoadonthanhtoan/" + hoadontam.nguoidung_id + "/" + hoadontam.id, hoadontam);


                var congngayvip = LayBangNguoiDung(hoadontam.nguoidung_id);
                var nguoidung = (from congvip in congngayvip
                                 where congvip.uid == hoadontam.nguoidung_id
                                 select congvip).FirstOrDefault();
                var goivip = LayBangGoiVip();
                var goiviphoadon = (from goiviphoadonnguoidung in goivip
                                    where goiviphoadonnguoidung.id == hoadontam.loaigoivip_id
                                    select goiviphoadonnguoidung).FirstOrDefault();
                DateTime ngaymoi;
                if (nguoidung.hansudungvip.ToUniversalTime() < hoadontam.thoigian.ToUniversalTime())
                {
                    nguoidung.hansudungvip = hoadontam.thoigian;
                    ngaymoi = nguoidung.hansudungvip.AddMonths((int)goiviphoadon.sothang);

                }
                else
                {
                    ngaymoi = nguoidung.hansudungvip.AddMonths((int)goiviphoadon.sothang);

                }
                nguoidung.vip = 1;
                nguoidung.hansudungvip = ngaymoi;
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("csdlmoi/nguoidung/" + hoadontam.nguoidung_id.ToString(), nguoidung);

                return Redirect("/#/thanhtoanthanhcong/" + hoadontam.id);
            }
            catch (Exception ex)
            {
                return Redirect("/#/thanhtoanthatbai/");
            }


        }
        public async Task<IActionResult> ThanhToanThatBaiPayPal()
        {

            return Redirect("/#/thanhtoanthatbai/");
        }
        public async Task<IActionResult> SendRequestPaypal([FromBody] hoadonthanhtoanModel hoadon)
        {
            try
            {


                var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                PayPalHttp.HttpResponse response;
                // Construct a request object and set desired parameters
                // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
                var order = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>()
                {
                    new PurchaseUnitRequest()
                    {
                        AmountWithBreakdown = new AmountWithBreakdown()
                        {
                            CurrencyCode = "USD",
                            Value = (Convert.ToDouble(hoadon.giatien / 23000)).ToString()
                        }
                    }
                },
                    ApplicationContext = new ApplicationContext()
                    {
                        ReturnUrl = $"{hostname}/Home/ThanhToanThanhCongPayPal",
                        CancelUrl = $"{hostname}/Home/ThanhToanThatBaiPayPal"
                    }
                };
                // Call API with your client and get a response for your call
                var request = new OrdersCreateRequest();
                request.Prefer("return=representation");
                request.RequestBody(order);
                response = await Clientpaypal().Execute(request);
                var statusCode = response.StatusCode;
                Order result = response.Result<Order>();
                orderIdPaypal = result.Id;
                hoadon.hoadonthanhtoan_id = orderIdPaypal;


                hoadon.thoigian = DateTime.Now;
                hoadontam = hoadon;
                return Json(new
                {
                    payUrl = result.Links[1].Href
                });
            }
            catch (Exception e)
            {
                return BadRequest();
            }


        }
        public async static Task<PayPalHttp.HttpResponse> captureOrder(string orderId)
        {
            // Construct a request object and set desired parameters
            // Replace ORDER-ID with the approved order id from create order
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());
            PayPalHttp.HttpResponse response = await Clientpaypal().Execute(request);
            var statusCode = response.StatusCode;
            Order result = response.Result<Order>();
            return response;
        }



        //end paypal

        /// / thanh toán bằng momo 
        /// 


        // 18/08 đã sữa CSDL mới

        public async Task<IActionResult> SendRequestMomo([FromBody] hoadonthanhtoanModel hoadon)
        {
            try
            {
                hoadon.thoigian = DateTime.Now;
                var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                string partnerCode = "MOMOSTVY20210726";
                string accessKey = "ZtS9WUaOBjHFJF1W";
                string secretKey = "hWwCQjp2sWusIhnCjCsnWcNGSRyxreX4";
                string orderInfo = "Gói Vip " + hoadon.mota.ToString();
                string returnUrl = $"{hostname}/Home/ReceiveRequestMomo";
                string notifyUrl = $"{hostname}/Home/ReceiveRequestMomoPost";
                string amount = hoadon.giatien.ToString();
                string orderId = Guid.NewGuid().ToString();
                hoadon.id = orderId;
                hoadon.hoadonthanhtoan_id = "momo" + hoadon.nguoidung_id;
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderId + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyUrl + "&extraData=" +
                    extraData;
                MomoSecurity momo = new MomoSecurity();
                string signature = momo.signSHA256(rawHash, secretKey);
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyUrl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };
                string response = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(response);
                hoadontam = hoadon;
                return Json(new
                {
                    payUrl = jmessage.GetValue("payUrl").ToString()
                });
            }
            catch (Exception e)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Delete("csdlmoi/hoadonthanhtoan/" + hoadon.nguoidung_id + "/" + hoadon.id);

                return Json(new
                {
                    payUrl = ""
                });
            }

        }

        public IActionResult ReceiveRequestMomo()
        {
            if (firstSign == null)
            {
                firstSign = Request.QueryString.ToString();
            }
            var sign = Request.Query.TryGetValue("signature", out var vs);
            if (firstSign != Request.QueryString.ToString())
            {
                return BadRequest();
            }
            var status = Request.Query.TryGetValue("errorCode", out var stt);
            if (Convert.ToInt32(stt) == 0)
            {
                firstSign = null;
                hoadontam.mota = "Thanh toán vip bằng MOMO thành công." + "  -  " + "Ngày thanh toán: " + hoadontam.thoigian + "  -  " + "Giá tiền: " + hoadontam.giatien;

                client = new FireSharp.FirebaseClient(config);
                PushResponse response_dsphat = client.Push("csdlmoi/hoadonthanhtoan/" + hoadontam.nguoidung_id.ToString(), hoadontam);
                hoadontam.id = response_dsphat.Result.name;
                SetResponse setResponse = client.Set("csdlmoi/hoadonthanhtoan/" + hoadontam.nguoidung_id + " / " + hoadontam.id, hoadontam);


                var congngayvip = LayBangNguoiDung(hoadontam.nguoidung_id);
                var nguoidung = (from congvip in congngayvip
                                 where congvip.uid == hoadontam.nguoidung_id
                                 select congvip).FirstOrDefault();
                var goivip = LayBangGoiVip();
                var goiviphoadon = (from goiviphoadonnguoidung in goivip
                                    where goiviphoadonnguoidung.id == hoadontam.loaigoivip_id
                                    select goiviphoadonnguoidung).FirstOrDefault();
                DateTime ngaymoi;
                if (nguoidung.hansudungvip.ToUniversalTime() < hoadontam.thoigian.ToUniversalTime())
                {
                    nguoidung.hansudungvip = hoadontam.thoigian;
                    ngaymoi = nguoidung.hansudungvip.AddMonths((int)goiviphoadon.sothang);

                }
                else
                {
                    ngaymoi = nguoidung.hansudungvip.AddMonths((int)goiviphoadon.sothang);

                }
                nguoidung.vip = 1;
                nguoidung.hansudungvip = ngaymoi;
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("csdlmoi/nguoidung/" + hoadontam.nguoidung_id + "/" + nguoidung.id, nguoidung);

                return Redirect("/#/thanhtoanthanhcong/" + hoadontam.id);
            }
            else
            {
                firstSign = null;
                return Redirect("/#/thanhtoanthatbai/");
            }

        }
        // 18/08 Đã Sữa CSDL Mới
        public IActionResult ReceiveRequestMomoPost()
        {
            var status = Request.Form["errorCode"].ToString();
            var idbill = Request.Form["orderId"].ToString();

            if (status == "0")
            {
                hoadontam.mota = "Thanh toán vip bằng MOMO thành công." + "  -  " + "Ngày thanh toán: " + hoadontam.thoigian + "  -  " + "Giá tiền: " + hoadontam.giatien;

                client = new FireSharp.FirebaseClient(config);
                PushResponse response_dsphat = client.Push("csdlmoi/hoadonthanhtoan/" + hoadontam.nguoidung_id.ToString(), hoadontam);
                hoadontam.id = response_dsphat.Result.name;
                SetResponse setResponse = client.Set("csdlmoi/hoadonthanhtoan/" + hoadontam.nguoidung_id + "/" + hoadontam.id, hoadontam);


                var congngayvip = LayBangNguoiDung(hoadontam.nguoidung_id);
                var nguoidung = (from congvip in congngayvip
                                 where congvip.uid == hoadontam.nguoidung_id
                                 select congvip).FirstOrDefault();
                var goivip = LayBangGoiVip();
                var goiviphoadon = (from goiviphoadonnguoidung in goivip
                                    where goiviphoadonnguoidung.id == hoadontam.loaigoivip_id
                                    select goiviphoadonnguoidung).FirstOrDefault();
                DateTime ngaymoi;
                if (nguoidung.hansudungvip.ToUniversalTime() < hoadontam.thoigian.ToUniversalTime())
                {
                    nguoidung.hansudungvip = hoadontam.thoigian;
                    ngaymoi = nguoidung.hansudungvip.AddMonths((int)goiviphoadon.sothang);

                }
                else
                {
                    ngaymoi = nguoidung.hansudungvip.AddMonths((int)goiviphoadon.sothang);

                }
                nguoidung.vip = 1;
                nguoidung.hansudungvip = ngaymoi;
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("csdlmoi/nguoidung/" + hoadontam.nguoidung_id + "/" + nguoidung.id, nguoidung);
                return Ok();
            }
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse responsez = client.Delete("hoadonthanhtoan/" + hoadontam.id);
            return Ok();
        }

        // end momo
        public class baihatcustomModel : baihatModel
        {


            public int yeuthich { get; set; }
            // THÊM TRƯỜNG QUẢNG CÁO

        }
        public class nguoidungcustomModel : nguoidungModel
        {
            public int theodoi { get; set; }
            public int soluongtheodoi { get; set; }
        }
        public class nguoidungchualogincustomModel : nguoidungModel
        {

            public int soluongtheodoi { get; set; }
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
        //17-08 Đã sữa CSDL mới
        public List<chitietdanhsachphatnguoidungModel> LayBangChiTietDanhSachPhatNguoiDung(string uid = null)
        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/chitietdanhsachphatnguoidung");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<chitietdanhsachphatnguoidungModel>();
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<chitietdanhsachphatnguoidungModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }



                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/chitietdanhsachphatnguoidung/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<chitietdanhsachphatnguoidungModel>();
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<chitietdanhsachphatnguoidungModel>(((JProperty)item).Value.ToString()));

                    }


                }
                return list;
            }
        }
        //public baocaobaihatModel LayBaiHatChiTiet(string nguoidungid, string idbh)
        //{
        //    FirebaseResponse rp = client.Get("csdlmoi/baocao/chuaxuly" + nguoidungid.ToString() + "/" + idbh.ToString());
        //    var datarp = JsonConvert.DeserializeObject<baocaobaihatModel>(rp.Body);
        //    baocaobaihatModel bh = new baocaobaihatModel();
        //    if (datarp != null)
        //    {
        //        bh = datarp;
        //    }
        //    return bh;
        //}

        //17-08 Đã sữa CSDL mới
        public List<chudeModel> LayBangChuDe(string idchude = null)
        {
            try
            {
                if (idchude == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/chude");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<chudeModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<chudeModel>(((JProperty)item).Value.ToString()));

                        }
                    }

                    return list;
                }
                else
                {

                    //return list;
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/chude/" + idchude);
                    var data = JsonConvert.DeserializeObject<chudeModel>(response.Body);
                    List<chudeModel> list = new List<chudeModel>();
                    if (data != null)
                    {
                        list.Add(data);
                    }

                    return list;
                }
            }
            catch
            {
                return null;
            }

        }
        /// 29/07 
        ///    //17-08 Đã sữa CSDL mới
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
        //17-08 Đã sữa CSDL mới
        public List<hoadonthanhtoanModel> LayBangHoaDonThanhToan(string uid = null)
        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/hoadonthanhtoan");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<hoadonthanhtoanModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<hoadonthanhtoanModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }
                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/hoadonthanhtoan/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<hoadonthanhtoanModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<hoadonthanhtoanModel>(((JProperty)item).Value.ToString()));

                    }
                }
                return list;
            }
        }
        //17-08 Đã sữa CSDL mới
        public List<theodoiModel> LayBangTheoDoi(string uid = null)
        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/theodoi");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<theodoiModel>();


                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<theodoiModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }
                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/theodoi/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<theodoiModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<theodoiModel>(((JProperty)item).Value.ToString()));

                    }
                }
                return list;
            }
        }
        //17-08 Đã sữa CSDL mới
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
        //17-08 Đã sữa CSDL mới
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
        //17-08 Đã sữa CSDL mới
        public List<dataixuongModel> LayBangDaTaiXuong(string uid = null)

        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/dataixuong");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<dataixuongModel>();
                if (data != null)
                {

                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<dataixuongModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/dataixuong/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<dataixuongModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<dataixuongModel>(((JProperty)item).Value.ToString()));

                    }
                }
                return list;
            }
        }
        //17-08 Đã sữa CSDL mới
        public List<goivipModel> LayBangGoiVip(string idgoivip = null)
        {
            try
            {
                if (idgoivip == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/goivip");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<goivipModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<goivipModel>(((JProperty)item).Value.ToString()));

                        }
                    }

                    return list;
                }
                else
                {

                    //return list;
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/goivip/" + idgoivip);
                    var data = JsonConvert.DeserializeObject<goivipModel>(response.Body);
                    List<goivipModel> list = new List<goivipModel>();
                    if (data != null)
                    {
                        list.Add(data);
                    }

                    return list;
                }
            }
            catch
            {
                return null;
            }


        }
        //17-08 Đã sữa CSDL mới
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
        //17-08 Đã sữa CSDL mới
        public List<theloaiModel> LayBangTheLoai(string idtheloai = null)
        {
            try
            {
                if (idtheloai == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/theloai");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<theloaiModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<theloaiModel>(((JProperty)item).Value.ToString()));

                        }
                    }

                    return list;
                }
                else
                {

                    //return list;
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/theloai/" + idtheloai);
                    var data = JsonConvert.DeserializeObject<theloaiModel>(response.Body);
                    List<theloaiModel> list = new List<theloaiModel>();
                    if (data != null)
                    {
                        list.Add(data);
                    }

                    return list;
                }
            }
            catch
            {
                return null;
            }



        }
        //17-08 Đã sữa CSDL mới
        public List<quangcaoModel> LayBangQuangCao(string idquangcao = null)
        {
            try
            {
                if (idquangcao == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/quangcao");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<quangcaoModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<quangcaoModel>(((JProperty)item).Value.ToString()));

                        }
                    }

                    return list;
                }
                else
                {

                    //return list;
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/quangcao/" + idquangcao);
                    var data = JsonConvert.DeserializeObject<quangcaoModel>(response.Body);
                    List<quangcaoModel> list = new List<quangcaoModel>();
                    if (data != null)
                    {
                        list.Add(data);
                    }

                    return list;
                }
            }
            catch
            {
                return null;
            }


        }
        //17-08 Đã sữa CSDL mới
        public List<yeuthichModel> LayBangYeuThichBaiHat(string uid = null)
        {

            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichbaihat");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<yeuthichModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichbaihat/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<yeuthichModel>(((JProperty)item).Value.ToString()));
                    }
                }
                return list;
            }

        }
        public List<binhluanchaModel> LayBangBinhLuanCha(string idbh = null)
        {

            if (idbh == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/binhluancha");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<binhluanchaModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<binhluanchaModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/binhluancha/" + idbh);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<binhluanchaModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<binhluanchaModel>(((JProperty)item).Value.ToString()));
                    }
                }
                return list;
            }

        }
        public List<binhluanconModel> LayBangBinhLuanCon(string idbh = null)
        {

            if (idbh == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/binhluancon");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<binhluanconModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<binhluanconModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/binhluancon/" + idbh);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<binhluanconModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<binhluanconModel>(((JProperty)item).Value.ToString()));
                    }
                }
                return list;
            }

        }
        //17-08 Đã sữa CSDL mới
        public List<yeuthichdanhsachphatnguoidung> layBangYeuThichDanhSachPhatNguoiDung(string uid = null)
        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichdanhsachphatnguoidung");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichdanhsachphatnguoidung>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<yeuthichdanhsachphatnguoidung>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichdanhsachphatnguoidung/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichdanhsachphatnguoidung>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<yeuthichdanhsachphatnguoidung>(((JProperty)item).Value.ToString()));
                    }
                }
                return list;
            }


        }
        //17-08 Đã sữa CSDL mới
        public List<yeuthichdanhsachphattheloaiModel> LayBangYeuThichDSPTheLoai(string uid = null)
        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichdanhsachphattheloai");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichdanhsachphattheloaiModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<yeuthichdanhsachphattheloaiModel>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichdanhsachphattheloai/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichdanhsachphattheloaiModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<yeuthichdanhsachphattheloaiModel>(((JProperty)item).Value.ToString()));
                    }
                }
                return list;
            }


        }
        //17-08 Đã sữa CSDL mới
        public List<yeuthichtop20Model> LayBangYeuThichTop20(string uid = null)
        {
            if (uid == null)
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichdanhsachphattop20");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichtop20Model>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        foreach (var x in item)
                        {
                            foreach (var y in x)

                            {
                                list.Add(JsonConvert.DeserializeObject<yeuthichtop20Model>(((JProperty)y).Value.ToString()));

                            }

                        }

                    }
                }

                return list;
            }
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/yeuthich/yeuthichdanhsachphattop20/" + uid);
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<yeuthichtop20Model>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<yeuthichtop20Model>(((JProperty)item).Value.ToString()));
                    }
                }
                return list;
            }
        }
       public baocaobaihatModel LayBangBaoCaoBaiHatChuaXuLy(string nguoidungid,string idbaocao)
        {
            FirebaseResponse rp = client.Get("csdlmoi/baocao/chuaxuly/" + nguoidungid.ToString() + "/" + idbaocao.ToString());
            var datarp = JsonConvert.DeserializeObject<baocaobaihatModel>(rp.Body);
            baocaobaihatModel bh = new baocaobaihatModel();
            if (datarp != null)
            {
                bh = datarp;
            }
            return bh;
        }
        //17-08 Đã sữa CSDL mới
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

        //17-08 Đã sữa CSDL mới
        public async Task<IActionResult> DanhSachQuangCao(string uid)
        {
            try
            {
                var list = LayBangQuangCao();
                var listbh = getListBaiHat();
                if (uid != null && uid != "")
                {
                    var listbhuid = convertBaiHat(listbh, uid);
                    var datauid = (from qc in list
                                   join bh in listbhuid on qc.baihat_id equals bh.id
                                   select new
                                   {
                                       quangcao = qc,
                                       baihat = bh
                                   }).ToList();
                    return Json(datauid);
                }
                else
                {
                    var data = (from qc in list
                                join bh in listbh on qc.baihat_id equals bh.id
                                select new
                                {
                                    quangcao = qc,
                                    baihat = bh
                                }).ToList();
                    return Json(data);
                }
                return Json("null");
            }
            catch
            {
                return null;
            }



        }
        //17-08 Đã sữa CSDL mới
        [HttpPost]
        public object getUser(string uid = null)
        {

            try
            {
                var list = LayBangNguoiDung(uid);

                if (list.Count > 0 && list[0] != null)
                {
                    if (list[0].hansudungvip < DateTime.Now)
                    {
                        list[0].vip = 0;
                        client = new FireSharp.FirebaseClient(config);
                        SetResponse response = client.Set("csdlmoi/nguoidung/" + uid, list[0]);
                    }
                    return list[0];
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
        //17-08 Đã sữa CSDL mới
        [HttpPost]
        public async Task<IActionResult> taiTheLoai()
        {

            var dino = LayBangTheLoai();

            return Json(dino);
        }
        public class Text
        {
            public string key { get; set; }
            public string uid { get; set; }
        }
        //17-08 Đã sữa CSDL mới
        [HttpPost]
        public async Task<IActionResult> taiChuDe()
        {
            //    string text = dltr.key.ToString();
            var dino = LayBangChuDe();


            return Json(dino);
        }
        public class uidBaiHat
        {
            public string uid { get; set; }
        }
        // 17/08 Đã Sữa CSDL mới
        [HttpPost]
        public async Task<bool> YeuThichBaiHatAsync([FromBody] yeuthichModel yeuthich)

        {
            try
            {
                if (yeuthich.nguoidung_id != null && yeuthich.nguoidung_id != "")
                {
                    var list = LayBangYeuThichBaiHat(yeuthich.nguoidung_id);
                    if (list.Count > 0)
                    {
                        var kq = (from yeuthichnew in list
                                  where yeuthichnew.baihat_id.Equals(yeuthich.baihat_id) && yeuthichnew.nguoidung_id.Equals(yeuthich.nguoidung_id)
                                  select yeuthichnew).ToList();
                        var listbaihat = getListBaiHat();
                        baihatModel qery = (from bh in getListBaiHat()
                                            where bh.id == yeuthich.baihat_id
                                            select bh).FirstOrDefault();
                        if (kq.Count() > 0)
                        {
                            client = new FireSharp.FirebaseClient(config);
                            FirebaseResponse response = client.Delete("csdlmoi/yeuthich/yeuthichbaihat/" + yeuthich.nguoidung_id + "/" + kq[0].id);


                            qery.luotthich = qery.luotthich - 1;
                            client = new FireSharp.FirebaseClient(config);
                            SetResponse responsebaihat = client.Set("csdlmoi/baihat/" + qery.nguoidung_id + "/" + qery.id, qery);
                            return false;
                        }
                        else
                        {
                            qery.luotthich = qery.luotthich + 1;
                            client = new FireSharp.FirebaseClient(config);
                            SetResponse responsebaihat = client.Set("csdlmoi/baihat/" + qery.nguoidung_id + "/" + qery.id, qery);
                            yeuthich.thoigian = DateTime.Now;
                            var firebase = new FirebaseClient(Key);

                            // add new item to list of data and let the client generate new key for you (done offline)
                            var dino = await firebase
                              .Child("csdlmoi")
                              .Child("yeuthich")
                              .Child("yeuthichbaihat")
                              .Child(yeuthich.nguoidung_id.ToString())
                              .PostAsync(yeuthich)
                              ;

                            string idkey = dino.Key.ToString();
                            yeuthich.id = idkey;
                            await firebase
                               .Child("csdlmoi")
                              .Child("yeuthich")
                              .Child("yeuthichbaihat")
                              .Child(yeuthich.nguoidung_id.ToString())
                               .Child(idkey)
                               .PutAsync(yeuthich);
                            return true;
                        }

                    }
                    else
                    {
                        var listbaihat = getListBaiHat();
                        baihatModel qery = (from bh in getListBaiHat()
                                            where bh.id == yeuthich.baihat_id
                                            select bh).FirstOrDefault();
                        qery.luotthich = qery.luotthich + 1;
                        client = new FireSharp.FirebaseClient(config);
                        SetResponse responsebaihat = client.Set("csdlmoi/baihat/" + qery.nguoidung_id + "/" + qery.id, qery);
                        yeuthich.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                          .Child("csdlmoi")
                          .Child("yeuthich")
                          .Child("yeuthichbaihat")
                          .Child(yeuthich.nguoidung_id.ToString())
                          .PostAsync(yeuthich)
                          ;

                        string idkey = dino.Key.ToString();
                        yeuthich.id = idkey;
                        await firebase
                           .Child("csdlmoi")
                          .Child("yeuthich")
                          .Child("yeuthichbaihat")
                          .Child(yeuthich.nguoidung_id.ToString())
                           .Child(idkey)
                           .PutAsync(yeuthich);
                        return false;
                    }
                }
                else
                    return false;

            }
            catch
            {
                return false;
            }

        }
        //17-08 Đã sữa CSDL mới
        [HttpPost]
        public async Task<bool> theoDoiNguoiDung([FromBody] theodoiModel theodoi)
        {
            try
            {
                var list = LayBangTheoDoi(theodoi.nguoidung_id);
                if (list.Count > 0)
                {
                    var kq = (from theodoilist in list
                              where theodoilist.nguoidung_id.Equals(theodoi.nguoidung_id) && theodoilist.nguoidung_theodoi_id.Equals(theodoi.nguoidung_theodoi_id)
                              select theodoilist).ToList();
                    if (kq.Count() > 0)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = client.Delete("csdlmoi/theodoi/" + theodoi.nguoidung_id + "/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        theodoi.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                          .Child("csdlmoi")
                          .Child("theodoi")
                          .Child(theodoi.nguoidung_id.ToString())
                          .PostAsync(theodoi)
                          ;

                        string idkey = dino.Key.ToString();
                        theodoi.id = idkey;
                        await firebase
                           .Child("csdlmoi")
                           .Child("theodoi")
                           .Child(theodoi.nguoidung_id.ToString())
                           .Child(idkey)
                           .PutAsync(theodoi);

                        return true;
                    }

                }

                if (list.Count == 0)
                {
                    theodoi.thoigian = DateTime.Now;
                    var firebase = new FirebaseClient(Key);

                    // add new item to list of data and let the client generate new key for you (done offline)
                    var dino = await firebase
                      .Child("csdlmoi")
                      .Child("theodoi")
                      .Child(theodoi.nguoidung_id.ToString())
                      .PostAsync(theodoi)
                      ;

                    string idkey = dino.Key.ToString();
                    theodoi.id = idkey;
                    await firebase
                       .Child("csdlmoi")
                       .Child("theodoi")
                       .Child(theodoi.nguoidung_id.ToString())
                       .Child(idkey)
                       .PutAsync(theodoi);

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        //17-08 Đã sữa CSDL mới
        public List<baihatModel> getListBaiHat(string uid = null)
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
        public List<baocaobaihatModel> LayBangBaoCaoBaiHatChuaXuLi(string uid = null)
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
        //17-08 Đã sữa CSDL mới
        public List<baihatcustomModel> convertBaiHat(List<baihatModel> list, string uid)
        {
            var listyeuthich = LayBangYeuThichBaiHat(uid);
            listyeuthich = (from yeuthich in listyeuthich
                            where yeuthich.nguoidung_id == uid
                            select yeuthich).ToList();
            List<baihatcustomModel> listkq = new List<baihatcustomModel>();
            for (int item = 0; item < list.Count(); item++)
            {
                baihatcustomModel baihat = new baihatcustomModel();
                baihat.casi = list[item].casi;
                baihat.chedo = list[item].chedo;
                baihat.chude_id = list[item].chude_id;
                baihat.danhsachphattheloai_id = list[item].danhsachphattheloai_id;
                baihat.id = list[item].id;
                baihat.link = list[item].link;
                baihat.linkhinhanh = list[item].linkhinhanh;
                baihat.loibaihat = list[item].loibaihat;
                baihat.luotnghe = list[item].luotnghe;
                baihat.luottaixuong = list[item].luottaixuong;
                baihat.luotthich = list[item].luotthich;
                baihat.mota = list[item].mota;
                baihat.nguoidung_id = list[item].nguoidung_id;
                baihat.quangcao = list[item].quangcao;
                baihat.tenbaihat = list[item].tenbaihat;
                baihat.theloai_id = list[item].theloai_id;
                baihat.thoigian = list[item].thoigian;
                baihat.thoiluongbaihat = list[item].thoiluongbaihat;
                baihat.thoigianxoa = list[item].thoigianxoa;
                baihat.vohieuhoa = list[item].vohieuhoa;
                
                bool checkYeuThich = false;
                for (int j = 0; j < listyeuthich.Count(); j++)
                {

                    if (list[item].id.Equals(listyeuthich[j].baihat_id))
                    {
                        checkYeuThich = true;
                    }

                }
                if (checkYeuThich)
                {
                    baihat.yeuthich = 1;
                    listkq.Add(baihat);
                }
                else
                {
                    baihat.yeuthich = 0;
                    listkq.Add(baihat);
                }

            }
            return listkq;
        }
        //17-08 Đã sữa CSDL mới
        public List<nguoidungcustomModel> convertNguoiDung(List<nguoidungModel> list, string uid)
        {
            var listyeuthichtong = LayBangTheoDoi();
            var listyeuthich = (from yeuthich in listyeuthichtong
                                where yeuthich.nguoidung_id == uid
                                select yeuthich).ToList();
            List<nguoidungcustomModel> listkq = new List<nguoidungcustomModel>();

            for (int item = 0; item < list.Count(); item++)
            {
                nguoidungcustomModel nguoidung = new nguoidungcustomModel();
                //  nguoidung.cover = list[item].cover;
                nguoidung.daxacthuc = list[item].daxacthuc;
                nguoidung.email = list[item].email;
                // nguoidung.facebook = list[item].facebook;
                // nguoidung.id = list[item].id;
                nguoidung.gioitinh = list[item].gioitinh;
                nguoidung.hansudungvip = list[item].hansudungvip;
                nguoidung.hinhdaidien = list[item].hinhdaidien;
                nguoidung.hoten = list[item].hoten;
                nguoidung.matkhau = list[item].matkhau;
                nguoidung.mota = list[item].mota;
                nguoidung.ngaysinh = list[item].ngaysinh;
                nguoidung.online = list[item].online;
                //  nguoidung.quocgia = list[item].quocgia;
                //  nguoidung.thanhpho = list[item].thanhpho;
                nguoidung.thoigian = list[item].thoigian;
                nguoidung.uid = list[item].uid;
                nguoidung.vip = list[item].vip;
                nguoidung.vohieuhoa = list[item].vohieuhoa;
                
                //  nguoidung.website = list[item].website;
                bool checkYeuThich = false;
                for (int j = 0; j < listyeuthich.Count(); j++)
                {

                    if (list[item].uid.Equals(listyeuthich[j].nguoidung_theodoi_id))
                    {
                        checkYeuThich = true;
                    }

                }
                nguoidung.soluongtheodoi = (from yeuthich in listyeuthichtong
                                            where yeuthich.nguoidung_theodoi_id == list[item].uid
                                            select yeuthich).Count();
                if (checkYeuThich)
                {
                    nguoidung.theodoi = 1;
                    listkq.Add(nguoidung);
                }
                else
                {
                    nguoidung.theodoi = 0;
                    listkq.Add(nguoidung);
                }

            }
            return listkq;
        }
        //17-08 Đã sữa CSDL mới
        public List<nguoidungchualogincustomModel> convertNguoiDungChuaLogin(List<nguoidungModel> list)
        {
            var listyeuthichtong = LayBangTheoDoi();

            List<nguoidungchualogincustomModel> listkq = new List<nguoidungchualogincustomModel>();

            for (int item = 0; item < list.Count(); item++)
            {
                nguoidungchualogincustomModel nguoidung = new nguoidungchualogincustomModel();
                //   nguoidung.cover = list[item].cover;
                nguoidung.daxacthuc = list[item].daxacthuc;
                nguoidung.email = list[item].email;
                //   nguoidung.facebook = list[item].facebook;
                nguoidung.id = list[item].id;
                nguoidung.gioitinh = list[item].gioitinh;
                nguoidung.hansudungvip = list[item].hansudungvip;
                nguoidung.hinhdaidien = list[item].hinhdaidien;
                nguoidung.hoten = list[item].hoten;
                nguoidung.matkhau = list[item].matkhau;
                nguoidung.mota = list[item].mota;
                nguoidung.ngaysinh = list[item].ngaysinh;
                nguoidung.online = list[item].online;
                //   nguoidung.quocgia = list[item].quocgia;
                //   nguoidung.thanhpho = list[item].thanhpho;
                nguoidung.thoigian = list[item].thoigian;
                nguoidung.uid = list[item].uid;
                nguoidung.vip = list[item].vip;
                nguoidung.vohieuhoa = list[item].vohieuhoa;
                //   nguoidung.website = list[item].website;
                nguoidung.soluongtheodoi = (from yeuthich in listyeuthichtong
                                            where yeuthich.nguoidung_theodoi_id == list[item].uid
                                            select yeuthich).Count();
                listkq.Add(nguoidung);
            }
            return listkq;
        }
        //18/08 Đã sữa CSDL mới
        [HttpPost]
        public object GetListBaiHatMoi(string uid = null)
        {
            try
            {
                if (uid != null)
                {

                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1 && baihat.daxoa == 0 && baihat.vohieuhoa == 0
                                  select baihat).ToList().OrderByDescending(x => x.thoigian).Take(listbaihat.Count > 12 ? 12 : listbaihat.Count).ToList();
                    //List<baihatModel> datakq = data.ToList();
                    var datareturn = convertBaiHat(datakq, uid);
                    return datareturn;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baihat");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baihatModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)item).Value.ToString()));

                        }
                        var datakq = (from baihat in list
                                      where baihat.chedo == 1
                                      select baihat).ToList().OrderByDescending(x => x.thoigian).Take(list.Count > 12 ? 12 : list.Count);

                        return Json(datakq);
                    }

                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        //18/08 Đã sữa CSDL mới
        [HttpPost]
        public object getListTheoDoi(string uid = null)
        {
            try
            {

                var listbh = getListBaiHat();
                var listnd = LayBangNguoiDung();

                if (uid != null && uid != "")
                {
                    var convertnd = convertNguoiDung(listnd, uid);
                    var convertbh = convertBaiHat(listbh, uid);
                    var result = (from ng in convertnd
                                  select new
                                  {
                                      nguoidung = ng,
                                      baihat = (from ngcon in convertnd
                                                join bh in convertbh on ngcon.uid equals bh.nguoidung_id
                                                where bh.chedo == 1 && bh.nguoidung_id == ng.uid && bh.daxoa == 0 && bh.vohieuhoa == 0
                                                select bh)
                                                .ToList()
                                  }).OrderByDescending(x => x.nguoidung.theodoi).ThenByDescending(x => x.nguoidung.soluongtheodoi).ToList();
                    return result;
                }
                else
                {
                    var listconvert = convertNguoiDungChuaLogin(listnd);
                    var result = (from ng in listconvert
                                  select new
                                  {
                                      nguoidung = ng,
                                      baihat = (from ngcon in listnd
                                                join bh in listbh on ngcon.uid equals bh.nguoidung_id
                                                where bh.chedo == 1 && bh.nguoidung_id == ng.uid
                                                select bh)
                                                .ToList()
                                  }).OrderByDescending(x => x.nguoidung.soluongtheodoi).ToList();
                    return result;
                }



            }
            catch
            {
                return Json(null);
            }
        }
        // 17/08 Đã Sữa CSDL mới
        public async Task<IActionResult> TaoTaiKhoan([FromBody] nguoidungModel model)
        {
            try
            {

                if (model != null)
                {
                    var data = LayBangNguoiDung();
                    var datakq = (from kq in data
                                  where kq.email == model.email
                                  select kq).ToList();
                    if (datakq.Count() > 0)
                    {
                        return Json(false);
                    }
                    else
                    {
                        model.thoigian = DateTime.Now;
                        var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
                        var a = await auth.CreateUserWithEmailAndPasswordAsync(model.email, model.matkhau, model.hoten, true);
                        model.uid = a.User.LocalId.ToString();
                        var firebase = new FirebaseClient(Key);

                        await firebase
                               .Child("csdlmoi")
                               .Child("nguoidung")
                               .Child(a.User.LocalId.ToString())
                               .PutAsync(model);

                        return Json(true);
                    }
                }


            }
            catch (Exception ex)
            {

                return Json(false);
            }
            return Json(false);

        }
        public class login
        {
            public string email { get; set; }
            public string matkhau { get; set; }
        }
        // 17/08 Đã Sữa CSDL Mới
        public object getListNgheNhieuNhat(string uid = null)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1 && baihat.daxoa == 0 && baihat.vohieuhoa == 0
                                  select baihat).ToList().OrderByDescending(x => x.luotnghe).Take(listbaihat.Count > 8 ? 8 : listbaihat.Count).ToList();
                    var result = convertBaiHat(datakq, uid);

                    return result;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baihat");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baihatModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)item).Value.ToString()));

                        }
                        var datakq = (from baihat in list
                                      where baihat.chedo == 1
                                      select baihat).ToList().OrderByDescending(x => x.luotnghe).Take(list.Count > 8 ? 8 : list.Count);

                        return Json(datakq);
                    }
                    return Json("null");
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 17/08 Đã Sữa CSDL Mới
        public object getListLikeNhieuNhat(string uid = null)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1 && baihat.daxoa == 0 && baihat.vohieuhoa == 0
                                  select baihat).ToList().OrderByDescending(x => x.luotthich).Take(listbaihat.Count > 8 ? 8 : listbaihat.Count).ToList();
                    var result = convertBaiHat(datakq, uid);
                    return result;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baihat");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baihatModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)item).Value.ToString()));

                        }
                        var datakq = (from baihat in list
                                      where baihat.chedo == 1
                                      select baihat).ToList().OrderByDescending(x => x.luotthich).Take(list.Count > 8 ? 8 : list.Count);

                        return Json(datakq);
                    }
                    return Json("null");
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 17/08 Đã Sữa CSDL Mới
        public object getListDowloadNhieuNhat(string uid = null)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1 && baihat.daxoa == 0 && baihat.vohieuhoa == 0
                                  select baihat).OrderByDescending(x => x.luottaixuong).Take(listbaihat.Count > 8 ? 8 : listbaihat.Count).ToList();
                    var result = convertBaiHat(datakq, uid);
                    return result;
                }
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/baihat");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<baihatModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)item).Value.ToString()));

                    }
                    var datakq = (from baihat in list
                                  where baihat.chedo == 1
                                  select baihat).ToList().OrderByDescending(x => x.luottaixuong).Take(list.Count > 8 ? 8 : list.Count);

                    return Json(datakq);
                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 17/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object getListBaiHatYeuThich(string uid = null)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var listyeuthich = LayBangYeuThichBaiHat(uid);
                    var datakq = (from bh in listbaihat
                                  where (bh.chedo == 1 && bh.daxoa == 0 && bh.vohieuhoa == 0)
                                  join yt in listyeuthich on bh.id equals yt.baihat_id
                                  where yt.nguoidung_id == uid
                                  select bh).ToList();

                    var result = convertBaiHat(datakq, uid);
                    return result;
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 17/08 Đã Sữa CSDL Mới
        [HttpPost]
        public async Task<object> TimKiemBaiHat(string tuKhoa)
        {
            try
            {

                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("baihat");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<baihatModel>();

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)item).Value.ToString()));

                    }
                    var datakq = (from baihat in list
                                  where baihat.tenbaihat.ToUpper().Contains(tuKhoa.ToUpper()) && baihat.chedo == 1
                                  select baihat).ToList().Take(list.Count > 5 ? 5 : list.Count);

                    return Json(datakq);
                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public bool taoDanhSachPhat_NguoiDung([FromBody] danhsachphatnguoidungModel model)
        {
            try
            {
                if (model.nguoidung_id != null && model.nguoidung_id != "")
                {
                    model.linkhinhanh = "https://photo-zmp3.zadn.vn/album_default.png";
                    model.thoigian = DateTime.Now;
                    client = new FireSharp.FirebaseClient(config);
                    PushResponse response_dsphat = client.Push("csdlmoi/danhsachphatnguoidung/" + model.nguoidung_id, model);
                    model.id = response_dsphat.Result.name;
                    SetResponse setResponse = client.Set("csdlmoi/danhsachphatnguoidung/" + model.nguoidung_id + "/" + model.id, model);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public async Task<object> TaoNguoiDungVoiXacThuc([FromBody] nguoidungModel model)
        {
            try
            {
                if (model.uid != null && model.uid != "")
                {


                    model.thoigian = DateTime.Now;
                    model.hansudungvip = DateTime.Now;
                    var datakq = LayBangNguoiDung(model.uid);



                    if (datakq.Count() > 0)
                    {
                        return Json("datontai");
                    }
                    else
                    {
                        model.daxacthuc = 1;
                        model.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);
                        await firebase
                       .Child("csdlmoi")
                       .Child("nguoidung")
                       .Child(model.uid.ToString())
                       .PutAsync(model);
                        return Json("dataomoi");
                    }

                }
                else
                {
                    return Json("LoiFirebase");
                }

            }


            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }
        // thêm lượt nghe bài hát 
        // 1808 Đã Sữa CSDL Mới
        [HttpPost]
        public bool themLuotNghe(string idbh)
        {
            try
            {
                if (idbh != null && idbh.ToString() !=  "undefined")
                {
                    var listbaihat = getListBaiHat();
                    baihatModel qery = (from bh in listbaihat
                                        where bh.id == idbh && bh.daxoa == 0 && bh.vohieuhoa == 0
                                        select bh).FirstOrDefault();
                    qery.luotnghe = qery.luotnghe + 1;
                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("csdlmoi/baihat/" + qery.nguoidung_id + "/" + qery.id, qery);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<bool> themLuotNgheToptrending([FromBody] baihatModel baihat)
        {
            try
            {
                baihat.luotnghe = 1;
                xuhuongModel xuhuong = new xuhuongModel();
                xuhuong.baihat_id = baihat.id;
                xuhuong.luotnghe = baihat.luotnghe;
                xuhuong.nguoidung_id = baihat.nguoidung_id;

                //int week = DateTime.Now.Day;
                //int monthh = DateTime.Now.Month;
                //int year = DateTime.Now.Year;
                //var firebase = new FirebaseClient(Key);
                if (baihat != null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/xuhuong/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString());
                    //FirebaseResponse response = client.Get("csdlmoi/baihat");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<xuhuongModel>();
                    var firebase = new FirebaseClient(Key);

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<xuhuongModel>(((JProperty)item).Value.ToString()));

                        }

                        list = (from bh in list
                                where bh.baihat_id == xuhuong.baihat_id
                                select bh).ToList();
                        if (list.Count > 0)
                        {
                            list[0].luotnghe += 1;

                            SetResponse setresponse = client.Set("csdlmoi/xuhuong/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + list[0].id, list[0]);
                            return true;
                        }
                        else
                        {

                            var dino = await firebase
                          .Child("csdlmoi")
                     .Child("xuhuong")
                     .Child(DateTime.Now.Year.ToString())
                     .Child(DateTime.Now.Month.ToString())
                     .Child(DateTime.Now.Day.ToString())
                         .PostAsync(xuhuong)
                         ;

                            string idkey = dino.Key.ToString();
                            xuhuong.id = idkey;
                            await firebase
                              .Child("csdlmoi")
                     .Child("xuhuong")
                     .Child(DateTime.Now.Year.ToString())
                     .Child(DateTime.Now.Month.ToString())
                     .Child(DateTime.Now.Day.ToString())
                               .Child(idkey)
                               .PutAsync(xuhuong);
                            //  PushResponse setresponse = client.Push("csdlmoi / toptrending / " + DateTime.Now.Year.ToString() + " / " + DateTime.Now.Month.ToString() + " / " + DateTime.Now.Day.ToString() + "/" + baihat.id, baihat);
                            //await firebase.Child("csdlmoi").Child("xuhuong").Child(DateTime.Now.Year.ToString()).Child(DateTime.Now.Month.ToString()).Child(DateTime.Now.Day.ToString()).Child(xuhuong.id.ToString()).PutAsync(xuhuong);
                            return true;
                        }
                        //return false;

                    }
                    else
                    {
                        //await firebase.Child("csdlmoi").Child("xuhuong").Child(DateTime.Now.Year.ToString()).Child(DateTime.Now.Month.ToString()).Child(DateTime.Now.Day.ToString()).Child(baihat.id.ToString()).PutAsync(baihat);
                        var dino = await firebase
                        .Child("csdlmoi")
                   .Child("xuhuong")
                   .Child(DateTime.Now.Year.ToString())
                   .Child(DateTime.Now.Month.ToString())
                   .Child(DateTime.Now.Day.ToString())
                       .PostAsync(xuhuong)
                       ;

                        string idkey = dino.Key.ToString();
                        xuhuong.id = idkey;
                        await firebase
                          .Child("csdlmoi")
                 .Child("xuhuong")
                 .Child(DateTime.Now.Year.ToString())
                 .Child(DateTime.Now.Month.ToString())
                 .Child(DateTime.Now.Day.ToString())
                           .Child(idkey)
                           .PutAsync(xuhuong);
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<object> DanhSachTopTrending24h([FromBody] Text model)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var firebase = new FirebaseClient(Key);
                DateTime update = DateTime.Parse(model.key);
                //update = update.AddDays(-1);
                FirebaseResponse response = client.Get("csdlmoi/xuhuong/" + update.Year.ToString() + "/" + update.Month.ToString() + "/" + update.Day.ToString());
                //FirebaseResponse response = client.Get("csdlmoi/baihat");
                var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<xuhuongModel>();
                DateTime updatexh = DateTime.Now;
                var date = DateTime.Now;
                if (date.Day > ngayUpdatexuhuong)
                {

                    update = updatexh.AddDays(-1);
                    FirebaseResponse responsexh = client.Get("csdlmoi/xuhuong/" + updatexh.Year.ToString() + "/" + updatexh.Month.ToString() + "/" + updatexh.Day.ToString());
                    //FirebaseResponse response = client.Get("csdlmoi/baihat");
                    var dataxh = JsonConvert.DeserializeObject<dynamic>(responsexh.Body);
                    var listxh = new List<xuhuongModel>();
                    var firebasec = new FirebaseClient(Key);
                    if (dataxh != null)
                    {
                        foreach (var item in dataxh)
                        {
                            listxh.Add(JsonConvert.DeserializeObject<xuhuongModel>(((JProperty)item).Value.ToString()));

                        }

                        listxh = (from bh in listxh
                                  select bh).OrderByDescending(x => x.luotnghe).Take(20).ToList();
                        if (listxh.Count > 20)
                        {
                            FirebaseResponse responseupdate = client.Delete("csdlmoi/xuhuong/" + updatexh.Year.ToString() + "/" + updatexh.Month.ToString() + "/" + updatexh.Day.ToString());
                            for (int i = 0; i < listxh.Count; i++)
                            {
                                await firebasec.Child("csdlmoi").Child("xuhuong").Child(updatexh.Year.ToString()).Child(updatexh.Month.ToString()).Child(updatexh.Day.ToString()).Child(list[i].id).PutAsync(list[i]);

                            }
                        }
                    }
                    ngayUpdatexuhuong = date.Day;
                }
                if (date.Year - 3 > namUpdatexuhuong)
                {
                    //  var firebase = new FirebaseClient(Key);
                    FirebaseResponse responseupdate = client.Delete("csdlmoi/xuhuong/" + namUpdatexuhuong);
                    namUpdatexuhuong++;
                }


                if (data != null)
                {
                    List<baihatModel> listbh = new List<baihatModel>();
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<xuhuongModel>(((JProperty)item).Value.ToString()));

                    }
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            FirebaseResponse rp = client.Get("csdlmoi/baihat/" + item.nguoidung_id.ToString() + "/" + item.baihat_id.ToString());
                            var datarp = JsonConvert.DeserializeObject<baihatModel>(rp.Body);

                            if (datarp != null)
                            {
                                datarp.luotnghe = item.luotnghe;
                                listbh.Add(datarp);
                            }
                        }
                        if (listbh.Count > 0)
                        {

                            listbh = (from bh in listbh
                                      select bh).OrderByDescending(x => x.luotnghe).Take(20).ToList();

                            if (model.uid != null && model.uid != "null")
                            {
                                return convertBaiHat(listbh, model.uid);
                            }
                            else
                            {
                                return listbh;
                            }

                        }
                    }

                }
                return false;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost]
        public object DanhSachXuHuongThang([FromBody] Text model)
        {
            client = new FireSharp.FirebaseClient(config);
            var firebase = new FirebaseClient(Key);
            DateTime update = DateTime.Parse(model.key);
            //update = update.AddDays(-1);
            FirebaseResponse response = client.Get("csdlmoi/xuhuong/" + update.Year.ToString() + "/" + update.Month.ToString());
            //FirebaseResponse response = client.Get("csdlmoi/baihat");
            var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<xuhuongModel>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    foreach (var x in item)
                    {
                        foreach (var y in x)

                        {
                            list.Add(JsonConvert.DeserializeObject<xuhuongModel>(((JProperty)y).Value.ToString()));

                        }

                    }

                }
            }
            if (list.Count > 0)
            {
                List<xuhuongModel> listxh = new List<xuhuongModel>();
                List<baihatModel> listbh = new List<baihatModel>();
                var tongbh = (from xh in list
                              group xh by xh.baihat_id into idbaihat
                              select idbaihat).ToList();
                foreach (var item in tongbh)
                {
                    listxh.Add(congLuotNgheThang(list, item.Key.ToString()));
                }
                foreach (var item in listxh)
                {
                    FirebaseResponse rp = client.Get("csdlmoi/baihat/" + item.nguoidung_id.ToString() + "/" + item.baihat_id.ToString());
                    var datarp = JsonConvert.DeserializeObject<baihatModel>(rp.Body);

                    if (datarp != null)
                    {
                        datarp.luotnghe = item.luotnghe;
                        listbh.Add(datarp);
                    }
                }
                if (listbh.Count > 0)
                {

                    listbh = (from bh in listbh
                              select bh).OrderByDescending(x => x.luotnghe).Take(20).ToList();

                    if (model.uid != null && model.uid != "null")
                    {
                        return convertBaiHat(listbh, model.uid);
                    }
                    else
                    {
                        return listbh;
                    }

                }

            }
            return false;
        }
        public xuhuongModel congLuotNgheThang(List<xuhuongModel> xuhuongs, string idbh)
        {
            List<xuhuongModel> result = (from xh in xuhuongs
                                         where xh.baihat_id == idbh
                                         select xh).ToList();
            xuhuongModel xuhuong = result[0];
            foreach (var item in result)
            {
                xuhuong.luotnghe += item.luotnghe;
            }
            return xuhuong;

        }
        //[HttpPost]
        //public object
        // thêm lượt nghe bài hát 
        //1808 Đã Sữa CSDL Mới
        [HttpPost]
        public bool ChinhSuaEmail([FromBody] Text email)
        {
            try
            {
                if (email != null && email.key != "")
                {
                    var listnguoidung = LayBangNguoiDung(email.uid);
                    nguoidungModel qery = (from nd in listnguoidung
                                           where nd.uid == email.uid
                                           select nd).FirstOrDefault();
                    qery.email = email.key.ToString();
                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("csdlmoi/nguoidung/" + qery.uid + "/email", qery.email);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // 18/08 Đã Sữa CSDL Mới
        public bool SetBaiHat([FromBody] baihatModel bh)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("csdlmoi/baihat/" + bh.nguoidung_id + "/" + bh.id, bh);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        public async Task<bool> XoaDanhSachPhatNguoiDung([FromBody] Text model)
        {
            try
            {
                if (model != null)
                {
                    var listchitiet = LayBangChiTietDanhSachPhatNguoiDung(model.uid);
                    listchitiet = (from ct in listchitiet
                                   where ct.danhsachphat_id == model.key
                                   select ct).ToList();
                    client = new FireSharp.FirebaseClient(config);
                    if (listchitiet.Count() > 0)
                    {


                        foreach (var item in listchitiet)
                        {
                            FirebaseResponse response = client.Delete("csdlmoi/chitietdanhsachphatnguoidung/" + model.uid + "/" + item.id);
                        }
                        return false;
                    }
                    else
                    {
                        FirebaseResponse response = client.Delete("csdlmoi/danhsachphatnguoidung/" + model.uid + "/" + model.key);
                        return true;
                    }


                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        //tìm kiếm playlist nguoi dung
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object timKiemDanhSachPhatNguoiDung([FromBody] timkiemmodel model)
        {
            try
            {
                if (model != null)
                {
                    var listPlaylist = LayBangDanhSachPhatNguoiDung();
                    var datakq = (from platlist in listPlaylist
                                  where platlist.tendanhsachphat.ToUpper().Contains(model.tuKhoa.ToUpper()) && platlist.chedo == 1
                                  select platlist).OrderBy(x => x.tendanhsachphat).ToList();
                    if (model.uid != null)
                    {
                        var data = convertDanhSachPhatNguoiDung(datakq, model.uid);
                        return Json(data);
                    }
                    else
                        return datakq;
                }
                else
                    return Json(null);


            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        //tìm kiếm playlist nguoi dung

        //tìm kiếm playlist the loai
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object timKiemDanhSachPhatTheLoai([FromBody] timkiemmodel model)
        {
            try
            {
                if (model != null)
                {
                    var listPlaylist = LayBangDanhSachPhatTheLoai();
                    var datakq = (from platlist in listPlaylist
                                  where platlist.tendanhsachphattheloai.ToUpper().Contains(model.tuKhoa.ToUpper())
                                  select platlist).OrderBy(x => x.tendanhsachphattheloai).ToList();
                    if (model.uid != null)
                    {
                        var data = convertDanhSachPhatTheLoai(datakq, model.uid);
                        return Json(data);
                    }
                    else
                        return datakq;
                }
                else
                    return Json(null);


            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        //tìm kiếm playlist the loai
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object getListDanhSachPhatNguoiDung([FromBody] Text model)
        {
            try
            {
                if (model.uid != null)
                {
                    var datakq = LayBangDanhSachPhatNguoiDung(model.uid);

                    if (model.key != null && model.key != "")
                    {
                        var data = convertDanhSachPhatNguoiDung(datakq, model.key);
                        return Json(data);
                    }
                    else
                        return datakq;
                }
                else
                    return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        [HttpPost]
        public object getListDanhSachPhatNgheSi([FromBody] Text model)
        {
            try
            {
                if (model.uid != null)
                {
                    var datakq = LayBangDanhSachPhatNguoiDung(model.uid);
                    datakq = (from ds in datakq
                              where ds.chedo == 1
                              select ds).ToList();
                    if (model.key != null && model.key != "")
                    {
                        var data = convertDanhSachPhatNguoiDung(datakq, model.key);
                        return Json(data);
                    }
                    else
                        return datakq;
                }
                else
                    return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        public class timkiemmodel
        {
            public string tuKhoa { get; set; }
            public string uid { get; set; }
        }
        // tim kiem bai hat khong gioi han bh
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public async Task<object> TimKiemBaiHatAll([FromBody] timkiemmodel model)
        {
            try
            {
                var list = getListBaiHat();

                if (list.Count > 0 && list[0] != null)
                {

                    var datakq = (from baihat in list
                                  where baihat.tenbaihat.ToUpper().Contains(model.tuKhoa.ToUpper()) && baihat.chedo == 1 && baihat.daxoa == 0 && baihat.vohieuhoa == 0
                                  select baihat).OrderBy(x => x.tenbaihat).ToList();
                    if (model.uid != null)
                    {
                        var result = convertBaiHat(datakq, model.uid);
                        return Json(result);
                    }
                    else
                    {
                        return datakq;
                    }

                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public List<danhsachphattheloaiModel> getListYeuThichDSP_canhan(string uid)
        {
            if (uid != null)
            {
                var listdspyeuthich = LayBangYeuThichDSPTheLoai(uid);
                var listdsp = LayBangDanhSachPhatTheLoai();

                var result = (from list in listdsp
                              join ytdsp in listdspyeuthich on list.id equals ytdsp.danhsachphat_id
                              where ytdsp.nguoidung_id.Equals(uid)
                              select list).ToList();

                return result;

            }
            else
                return null;

        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object getListYeuThichDSPNgheSi_canhan(string uid)
        {
            if (uid != null)
            {
                var listdspyeuthich = layBangYeuThichDanhSachPhatNguoiDung(uid);
                var listdsp = LayBangDanhSachPhatNguoiDung();

                var result = (from list in listdsp
                              where list.chedo == 1
                              join listyeuthich in listdspyeuthich on list.id equals listyeuthich.danhsachphat_id
                              where listyeuthich.nguoidung_id == uid
                              select list).ToList();

                return result;

            }
            else
                return null;

        }
        //tìm kiếm nguoi dùng
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object timKiemNguoiDungCustom([FromBody] timkiemmodel model)
        {
            try
            {
                if (model != null)
                {

                    var listnguoidung = LayBangNguoiDung();
                    var datakq = (from nguoidung in listnguoidung
                                  where nguoidung.hoten.ToUpper().Contains(model.tuKhoa.ToUpper())
                                  select nguoidung).OrderBy(x => x.hoten).ToList();

                    if (model.uid != null)
                    {

                        var result = convertNguoiDung(datakq, model.uid);
                        return Json(result);
                    }
                    else
                    {
                        var result = convertNguoiDungChuaLogin(datakq);
                        return result;
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        //tìm kiếm nguoi dùng
        public class modelTimKiemNgheSi
        {
            public string uidNguoiDung { get; set; }
            public string uidNgheSi { get; set; }
        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object timKiemNgheSi([FromBody] modelTimKiemNgheSi model)
        {
            try
            {
                if (model.uidNgheSi != null)
                {

                    var listnguoidung = LayBangNguoiDung(model.uidNgheSi);
                    var datakq = (from nguoidung in listnguoidung
                                  where nguoidung.uid == model.uidNgheSi
                                  select nguoidung).ToList();

                    if (model.uidNguoiDung != null && model.uidNguoiDung != "")
                    {

                        var result = convertNguoiDung(datakq, model.uidNguoiDung);
                        return Json(result);
                    }
                    else
                    {
                        var result = convertNguoiDungChuaLogin(datakq);
                        return result;
                    }
                }
                else
                {
                    return Json(null);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object gettopNgheSi(string uid)
        {
            try
            {


                var listnguoidung = LayBangNguoiDung();

                if (uid != null)
                {

                    var result = convertNguoiDung(listnguoidung, uid);
                    result = (from nghesi in result
                              select nghesi).OrderByDescending(x => x.soluongtheodoi).Take(10).ToList();
                    return Json(result);
                }
                else
                {
                    var result = convertNguoiDungChuaLogin(listnguoidung);
                    result = (from nghesi in result
                              select nghesi).OrderByDescending(x => x.soluongtheodoi).Take(10).ToList();
                    return result;
                }


            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
        // 18/08 Đã Sữa CSDL Mới
        public List<baihatcustomModel> getListDaTaiLen_CaNhan(string uid)
        {
            if (uid != null)
            {
                var list = getListBaiHat(uid);
                list = (from bh in list
                        where bh.nguoidung_id == uid && bh.daxoa == 0 
                        select bh).ToList();
                var result = convertBaiHat(list, uid);
                return result;
            }
            else
            {
                List<baihatcustomModel> list = new List<baihatcustomModel>();
                return list;
            }
        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object getListDaTaiLen_NgheSi([FromBody] modelTimKiemNgheSi model)
        {

            var list = getListBaiHat(model.uidNgheSi);
            list = (from bh in list
                    where bh.nguoidung_id == model.uidNgheSi && bh.chedo == 1 && bh.daxoa == 0  && bh.vohieuhoa == 0
                    select bh).OrderByDescending(x => x.luotnghe).ToList();
            if (model.uidNguoiDung != null)
            {

                var result = convertBaiHat(list, model.uidNguoiDung);
                return result;
            }

            return list;
        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object getListHoaDon(string uid)
        {
            try
            {
                var hd = LayBangHoaDonThanhToan(uid);
                hd = (from hditem in hd
                      where hditem.nguoidung_id == uid
                      select hditem).OrderByDescending(x => x.thoigian).ToList();
                return hd;
            }
            catch
            {
                return Json(null);
            }


        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public List<baihatcustomModel> getListDaTaiXuong_CaNhan(string uid)
        {
            try
            {

                var listbh = getListBaiHat();
                var listdatai = LayBangDaTaiXuong(uid);
                var qery = (from bh in listbh where bh.daxoa == 0 && bh.vohieuhoa == 0 && bh.chedo == 1
                            join datai in listdatai.Where(x => x.nguoidung_id == uid) on bh.id equals datai.baihat_id
                            select bh).Distinct().ToList();
                var result = convertBaiHat(qery, uid);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object getPlaylist_CaNhan(string uid)
        {
            try
            {

                var listdanhsachphat = LayBangDanhSachPhatNguoiDung(uid);
                var qery = (from dsp in listdanhsachphat
                            where dsp.nguoidung_id.Equals(uid)
                            select dsp).ToList();
                return qery;
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public async Task<bool> YeuThichDanhSachPhatNguoiDung([FromBody] yeuthichdanhsachphatnguoidung yeuthichdanhsachphatnguoidung)

        {
            try
            {
                var list = layBangYeuThichDanhSachPhatNguoiDung();
                if (list.Count > 0)
                {
                    var kq = (from yeuthichnew in list
                              where yeuthichnew.danhsachphat_id.Equals(yeuthichdanhsachphatnguoidung.danhsachphat_id) && yeuthichnew.nguoidung_id.Equals(yeuthichdanhsachphatnguoidung.nguoidung_id)
                              select yeuthichnew).ToList();
                    if (kq.Count() > 0)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = client.Delete("csdlmoi/yeuthich/yeuthichdanhsachphatnguoidung/" + yeuthichdanhsachphatnguoidung.nguoidung_id + "/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        yeuthichdanhsachphatnguoidung.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                           .Child("csdlmoi")
                      .Child("yeuthich")
                      .Child("yeuthichdanhsachphatnguoidung")
                      .Child(yeuthichdanhsachphatnguoidung.nguoidung_id)
                          .PostAsync(yeuthichdanhsachphatnguoidung)
                          ;

                        string idkey = dino.Key.ToString();
                        yeuthichdanhsachphatnguoidung.id = idkey;
                        await firebase
                            .Child("csdlmoi")
                      .Child("yeuthich")
                      .Child("yeuthichdanhsachphatnguoidung")
                      .Child(yeuthichdanhsachphatnguoidung.nguoidung_id)
                           .Child(idkey)
                           .PutAsync(yeuthichdanhsachphatnguoidung);
                        return true;
                    }

                }
                else
                {
                    yeuthichdanhsachphatnguoidung.thoigian = DateTime.Now;
                    var firebase = new FirebaseClient(Key);

                    // add new item to list of data and let the client generate new key for you (done offline)
                    var dino = await firebase
                      .Child("csdlmoi")
                      .Child("yeuthich")
                      .Child("yeuthichdanhsachphatnguoidung")
                      .Child(yeuthichdanhsachphatnguoidung.nguoidung_id)
                      .PostAsync(yeuthichdanhsachphatnguoidung)
                      ;

                    string idkey = dino.Key.ToString();
                    yeuthichdanhsachphatnguoidung.id = idkey;
                    await firebase
                        .Child("csdlmoi")
                      .Child("yeuthich")
                      .Child("yeuthichdanhsachphatnguoidung")
                      .Child(yeuthichdanhsachphatnguoidung.nguoidung_id)
                       .Child(idkey)
                       .PutAsync(yeuthichdanhsachphatnguoidung);
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        // 18/08 Đã Sữa CSDL Mới
        public List<danhsachphatnguoidungcustomModel> convertDanhSachPhatNguoiDung(List<danhsachphatnguoidungModel> list, string uid)
        {
            var listyeuthichdsp = layBangYeuThichDanhSachPhatNguoiDung(uid);
            listyeuthichdsp = (from yeuthich in listyeuthichdsp
                               where yeuthich.nguoidung_id == uid
                               select yeuthich).ToList();
            List<danhsachphatnguoidungcustomModel> listkq = new List<danhsachphatnguoidungcustomModel>();
            for (int item = 0; item < list.Count(); item++)
            {
                danhsachphatnguoidungcustomModel danhsachphat = new danhsachphatnguoidungcustomModel();

                danhsachphat.id = list[item].id;
                danhsachphat.tendanhsachphat = list[item].tendanhsachphat;
                danhsachphat.linkhinhanh = list[item].linkhinhanh;
                danhsachphat.nguoidung_id = list[item].nguoidung_id;
                danhsachphat.thoigian = list[item].thoigian;
                danhsachphat.chedo = list[item].chedo;
                bool checkYeuThich = false;
                for (int j = 0; j < listyeuthichdsp.Count(); j++)
                {

                    if (list[item].id.Equals(listyeuthichdsp[j].danhsachphat_id))
                    {
                        checkYeuThich = true;
                    }

                }
                if (checkYeuThich)
                {
                    danhsachphat.yeuthich = 1;
                    listkq.Add(danhsachphat);
                }
                else
                {
                    danhsachphat.yeuthich = 0;
                    listkq.Add(danhsachphat);
                }

            }
            return listkq;
        }

        // 18/08 Đã Sữa CSDL Mới
        [HttpPost]
        public async Task<bool> YeuThichTop20Async([FromBody] yeuthichtop20Model yeuthichtop20)

        {
            try
            {
                if (yeuthichtop20.nguoidung_id == null || yeuthichtop20.nguoidung_id == "" &&
                   yeuthichtop20.top20_id == null || yeuthichtop20.top20_id == "")
                {
                    return false;
                }
                var list = LayBangYeuThichTop20(yeuthichtop20.nguoidung_id);
                if (list.Count > 0)
                {
                    var kq = (from yeuthichnew in list
                              where yeuthichnew.top20_id.Equals(yeuthichtop20.top20_id) && yeuthichnew.nguoidung_id.Equals(yeuthichtop20.nguoidung_id)
                              select yeuthichnew).ToList();
                    if (kq.Count() > 0)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = client.Delete("csdlmoi/yeuthich/yeuthichdanhsachphattop20/" + yeuthichtop20.nguoidung_id + "/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        yeuthichtop20.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                          .Child("csdlmoi")
                          .Child("yeuthich")
                          .Child("yeuthichdanhsachphattop20")
                          .Child(yeuthichtop20.nguoidung_id)
                          .PostAsync(yeuthichtop20)
                          ;

                        string idkey = dino.Key.ToString();
                        yeuthichtop20.id = idkey;
                        await firebase
                         .Child("csdlmoi")
                          .Child("yeuthich")
                          .Child("yeuthichdanhsachphattop20")
                          .Child(yeuthichtop20.nguoidung_id)
                           .Child(idkey)
                           .PutAsync(yeuthichtop20);
                        return true;
                    }

                }
                else
                {
                    yeuthichtop20.thoigian = DateTime.Now;
                    var firebase = new FirebaseClient(Key);

                    // add new item to list of data and let the client generate new key for you (done offline)
                    var dino = await firebase
                      .Child("csdlmoi")
                      .Child("yeuthich")
                      .Child("yeuthichdanhsachphattop20")
                      .Child(yeuthichtop20.nguoidung_id)
                      .PostAsync(yeuthichtop20)
                      ;

                    string idkey = dino.Key.ToString();
                    yeuthichtop20.id = idkey;
                    await firebase
                     .Child("csdlmoi")
                      .Child("yeuthich")
                      .Child("yeuthichdanhsachphattop20")
                      .Child(yeuthichtop20.nguoidung_id)
                       .Child(idkey)
                       .PutAsync(yeuthichtop20);
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        [HttpPost]
        public async Task<bool> BinhLuanBaiHat_Cha([FromBody] binhluanchaModel model)
        {
            try
            {
                if (model != null)
                {
                    model.thoigian = DateTime.Now;
                    var firebase = new FirebaseClient(Key);
                    var dino = await firebase.Child("csdlmoi").Child("binhluancha").Child(model.baihat_id).PostAsync(model);
                    string idkey = dino.Key.ToString();
                    model.id = idkey;
                    await firebase.Child("csdlmoi").Child("binhluancha").Child(model.baihat_id).Child(idkey).PutAsync(model);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<bool> ChinhSuaBinhLuanBaiHat_Cha([FromBody] binhluanchaModel model)
        {
            try
            {
                if (model != null)
                {

                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("csdlmoi/binhluancha/" + model.baihat_id + "/" + model.id + "/" + "noidung", model.noidung);
                    //var dino = await firebase.Child("csdlmoi").Child("binhluancha").Child(model.baihat_id).Child(model.id).Child("noidung").PostAsync(model.noidung);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public bool XoaBinhLuanBaiHat_Cha([FromBody] binhluanchaModel model)
        {
            try
            {
                if (model != null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Delete("csdlmoi/binhluancha/" + model.baihat_id + "/" + model.id);
                    var list = LayBangBinhLuanCon(model.baihat_id);
                    list = (from bl in list
                            where bl.binhluancha_id == model.id
                            select bl).ToList();
                    client = new FireSharp.FirebaseClient(config);
                    if (list.Count() > 0)
                    {


                        foreach (var item in list)
                        {
                            FirebaseResponse responsecon = client.Delete("csdlmoi/binhluancon/" + model.baihat_id + "/" + item.id);
                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public bool XoaBinhLuanBaiHat_Con([FromBody] binhluanchaModel model)
        {
            try
            {
                if (model != null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Delete("csdlmoi/binhluancon/" + model.baihat_id + "/" + model.id);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<bool> ChinhSuaBinhLuanConBaiHat([FromBody] binhluanchaModel model)
        {
            try
            {
                if (model != null)
                {

                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("csdlmoi/binhluancon/" + model.baihat_id + "/" + model.id + "/" + "noidung", model.noidung);
                    //var dino = await firebase.Child("csdlmoi").Child("binhluancha").Child(model.baihat_id).Child(model.id).Child("noidung").PostAsync(model.noidung);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public async Task<bool> BinhLuanBaiHat_Con([FromBody] binhluanconModel model)
        {
            try
            {
                if (model != null)
                {
                    model.thoigian = DateTime.Now;
                    var firebase = new FirebaseClient(Key);
                    var dino = await firebase.Child("csdlmoi").Child("binhluancon").Child(model.baihat_id).PostAsync(model);
                    string idkey = dino.Key.ToString();
                    model.id = idkey;
                    await firebase.Child("csdlmoi").Child("binhluancon").Child(model.baihat_id).Child(idkey).PutAsync(model);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<object> loadBinhLuan(string idbh)
        {
            try
            {
                if (idbh != null && idbh != "")
                {
                    var firebase = new FirebaseClient(Key);
                    var databinhluancha = await firebase
                        .Child("csdlmoi")
                        .Child("binhluancha")
                        .Child(idbh)
                        .OnceAsync<binhluanchaModel>();
                    var binhluancha = (from bl in databinhluancha
                                       select bl.Object).ToList();
                    var databinhluancon = await firebase
                        .Child("csdlmoi")
                          .Child("binhluancon")
                         .Child(idbh)
                         .OnceAsync<binhluanconModel>();
                    var binhluancon = (from bl in databinhluancon
                                       select bl.Object).ToList();
                    var datanguoidung = LayBangNguoiDung();
                    var nguoidung = (from ng in datanguoidung
                                     select ng).ToList();
                    var blcon = (from blc in binhluancon
                                 join ng in nguoidung on blc.nguoidung_id equals ng.uid
                                 select new
                                 {
                                     binhluancon = blc,
                                     nguoidungcon = ng
                                 }).ToList();
                    var blcha = (from blc in binhluancha
                                 join ng in nguoidung on blc.nguoidung_id equals ng.uid
                                 select new
                                 {
                                     binhluancha = blc,
                                     nguoidungcha = ng,
                                     binhluancon = (from blc1 in blcon
                                                    where blc1.binhluancon.binhluancha_id.Equals(blc.id)
                                                    select blc1).ToList()
                                 }).ToList();
                    return Json(blcha);
                }
                else
                {
                    return Json(false);
                }
            }
            catch
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<object> BaoCaoBaiHat([FromBody] baocaobaihatModel baocao)
        {
            //string output = JsonConvert.SerializeObject(baocao.noidung);
            var listbaocao = LayBangBaoCaoBaiHatChuaXuLi(baocao.nguoidung_id);
            listbaocao = (from bc in listbaocao
                          where bc.baihat_baocao_id == baocao.baihat_baocao_id
                          select bc).ToList();
            if (listbaocao.Count > 0)
            {

                return Json("Bạn Đã Báo Cáo Bài Hát Vui Lòng Đợi Nhân Viên Xử Lý");
            }
            else
            {
                baocao.thoigian = DateTime.Now;
                var firebase = new FirebaseClient(Key);

                if (baocao != null)
                {
                    var dino = await firebase.Child("csdlmoi").Child("baocao").Child("baihatvipham").Child("chuaxuly").Child(baocao.nguoidung_id).PostAsync(baocao);
                    string idkey = dino.Key.ToString();
                    baocao.id = idkey;
                    await firebase.Child("csdlmoi").Child("baocao").Child("baihatvipham").Child("chuaxuly").Child(baocao.nguoidung_id).Child(idkey).PutAsync(baocao);
                    var nguoibaocao = LayBangNguoiDung(baocao.nguoidung_id);
                    var nguoibibaocao = LayBangNguoiDung(baocao.nguoidung_baocao_id);


                    if (nguoibaocao.Count > 0 && nguoibaocao[0].email != "admin")
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181280@caothang.edu.vn"));
                        message.To.Add(new MailboxAddress(nguoibaocao[0].hoten, nguoibaocao[0].email));
                        message.Subject = "TMUSIC - CÁM ƠN BẠN ĐÃ BÁO CÁO";
                        message.Body = new TextPart("plain")
                        {
                            Text = "Xin chào: " + nguoibaocao[0].hoten + "\n" +
                            "Cám ơn bạn đã báo cáo bài hát mà bạn đã cho rằng vi phạm tiêu chuẩn cộng đồng. \n" +
                            "Chúng tôi đã tiếp nhận báo cáo của bạn và sẽ xử lý vi phạm trong thời gian sớm nhất. \n\n" +
                            "Chúng tôi sẽ thông báo cho bạn ngay khi có kết quả xử lý vi phạm.  \n" +
                            "Cám ơn bạn. \n" +
                            "Admin TMUSIC"
                        };
                        using (var client = new SmtpClient())
                        {
                            // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                            client.CheckCertificateRevocation = false;
                            // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                            client.Connect("smtp.gmail.com", 587, false);
                            //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                            client.Authenticate("0306181280@caothang.edu.vn", "0965873520");

                            client.Send(message);
                            client.Disconnect(true);
                        }

                    }
                    // gửi email


                    if (nguoibibaocao.Count > 0 && nguoibibaocao[0].email != "admin")
                    {
                        // gửi email
                        var messagenguoibibaocao = new MimeMessage();
                        messagenguoibibaocao.From.Add(new MailboxAddress("Admin TMUSIC", "0306181280@caothang.edu.vn"));
                        messagenguoibibaocao.To.Add(new MailboxAddress(nguoibibaocao[0].hoten, nguoibibaocao[0].email));
                        messagenguoibibaocao.Subject = "TMUSIC - Bài Hát Của Bạn Bị Báo Cáo Vi Phạm Tiêu Chuẩn Cộng Đồng";
                        messagenguoibibaocao.Body = new TextPart("plain")
                        {
                            Text = "Xin chào: " + nguoibibaocao[0].hoten + "\n" +
                            "Chúng tôi đã nhận được báo cáo bài hát của bạn vi phạm tiêu chuẩn cộng đồng của chúng tôi. \n" +
                            "Nhằm đảm bảo cho tiêu chuẩn cộng đồng chúng tôi sẽ xem xét về trường hợp của bạn. \n" +
                            "Nếu bạn cho rằng báo cáo nhầm lẫn hoặc không chính xác vui lòng  reply email cho chúng tôi với các thông tin chứng minh rằng bạn đã không vi phạm bất cứ tiêu chuẩn cộng đồng của chúng tôi. \n\n" +
                            "Chúng tôi luôn đảm bảo rằng TMusic là một cộng đồng người dùng văn minh.   \n" +
                            "Cám ơn bạn. \n" +
                            "Admin TMUSIC"
                        };
                        using (var client = new SmtpClient())
                        {
                            // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                            client.CheckCertificateRevocation = false;
                            // client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                            client.Connect("smtp.gmail.com", 587, false);
                            //  await client.ConnectAsync("smtp.gmail.com", 587, false);
                            client.Authenticate("0306181280@caothang.edu.vn", "0965873520");

                            client.Send(messagenguoibibaocao);
                            client.Disconnect(true);
                        }
                    }
                    return Json("Cảm Ơn Bạn Bạn Đã Báo Cáo. Chúng Tôi Đã Tiếp Nhận Báo Cáo Của Bạn. Chúng Tôi Sẽ Phản Hồi Tình Trạng Qua Email Của Bạn.");
                }
            }

            return false;
        }
        [HttpPost]
        public bool ChinhSuaTenDanhSachPhat([FromBody] danhsachphatnguoidungModel dsp)
        {
            try
            {
                if (dsp != null )
                {
                    //var listnguoidung = LayBangDanhSachPhatNguoiDung(dsp.nguoidung_id);
                    //danhsachphatnguoidungModel qery = (from nd in listnguoidung
                    //                       where nd.nguoidung_id == dsp.nguoidung_id
                    //                       select nd).FirstOrDefault();
                   // qery.tendanhsachphat = dsp.tendanhsachphat.ToString();
                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("csdlmoi/danhsachphatnguoidung/" + dsp.nguoidung_id +"/"+ dsp.id,dsp);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost]
        public bool ChinhSuaTenBaiHat([FromBody] baihatModel baihat)
        {
            try
            {
                if (baihat != null )
                {
                    
                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("csdlmoi/baihat/" + baihat.nguoidung_id+"/" + baihat.id , baihat);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        public bool xoaBaiHatNguoiDung([FromBody] baihatModel baihat)
        {
            try
            {
                if (baihat != null)
                {

                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("csdlmoi/baihat/" + baihat.nguoidung_id + "/" + baihat.id +"/"+ "daxoa", 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        // Của Tài
        // hàm lấy bài hát mới   ví dụ
        // 21/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object taiDSPBaiHatMoi_NhacMoi(string uid = "")
        {
            try
            {
                var listbaihat = getListBaiHat();
                var datakq = (from baihat in listbaihat
                              where baihat.chedo == 1 && baihat.daxoa == 0  && baihat.vohieuhoa == 0
                              select baihat).OrderByDescending(x => x.thoigian).Take(20).ToList();
                if (uid != null && uid != "" && uid != "null")
                {


                    //List<baihatModel> datakq = data.ToList();
                    var datareturn = convertBaiHat(datakq, uid);
                    return datareturn;
                }
                else
                {

                    return Json(datakq);
                }
                //return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        [HttpPost]
        public object taiDanhSachPhatTop20_khampha(string uid = "")
        {



            var top20 = LayBangTop20();
            var data = (from t20 in top20
                        where t20.daxoa == 0
                        select t20).ToList();
            try
            {
                if (uid != null && uid != "" && uid != "null")
                {
                    var data1 = convertTop20(data, uid.ToString()).OrderByDescending(x => x.id).ToList();
                    return Json(data1);
                }
                else
                {

                    return Json(data);
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // sữa 24 / 01 load lại dữ liệu
        // lấy bài theo dsp thể loại 11/07
        // 22/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object taiDSPBaiHatTheoDSPTheLoai_DSP([FromBody] Text itemmodel)
        {
            try
            {
                var listbaihat = getListBaiHat();
                var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai(itemmodel.key.ToString());
                var datakq = (from baihat in listbaihat
                              where baihat.chedo == 1 && baihat.daxoa == 0 && baihat.vohieuhoa == 0 && baihat.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
                              select baihat).ToList();
                var datathembaihat = (from bh in listbaihat
                                      join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                                      where bh.chedo == 1 && bh.daxoa == 0 && ctdsptl.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
                                      select bh).ToList();
                //var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai(key.ToString());

                //var baihatdathem = (from bh in baihat
                //                    join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                //                    where ctdsptl.danhsachphattheloai_id.Equals(key.ToString())
                //                    select bh).ToList();

                if (datathembaihat.Count > 0)
                {
                    foreach (var item in datathembaihat)
                    {
                        datakq.Add(item);
                    }
                }
                if (itemmodel.uid != "")
                {


                    var datareturn = convertBaiHat(datakq, itemmodel.uid);
                    return datareturn;
                }
                else
                {

                    return Json(datakq);
                }
                //  return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }


        // 21/08 Đã Sữa CSDL Mới
        // Tải xuống 11/07
        // 3/08 sữa kiểm uid 
        [HttpPost]
        public async Task<IActionResult> taiXuongBaiHat_NguoiDung([FromBody] Text item)
        {

            bool success = true;

            try
            {

                var listbaihat = getListBaiHat();
                baihatModel qery = (from bh in listbaihat
                                    where bh.id == item.key && bh.daxoa == 0 && bh.vohieuhoa == 0
                                    select bh).FirstOrDefault();
                qery.luottaixuong = qery.luottaixuong + 1;
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("csdlmoi/baihat/" + qery.nguoidung_id + "/" + qery.id, qery);
                if (item.uid == "" && item.uid == null)
                {
                    success = false;
                    return Json(success);
                }
                var list = LayBangDaTaiXuong();
                list = (from datai in list
                        where datai.baihat_id == item.key && datai.nguoidung_id == item.uid
                        select datai).ToList();
                if (list.Count > 0)
                {
                    success = false;
                }

                else
                {
                    var firebase = new FirebaseClient(Key);

                    // add new item to list of data and let the client generate new key for you (done offline)
                    dataixuongModel dtx = new dataixuongModel();
                    dtx.nguoidung_id = item.uid;
                    dtx.baihat_id = item.key;
                    dtx.thoigian = DateTime.Now;
                    var dino = await firebase
                        .Child("csdlmoi")
                      .Child("dataixuong")
                      .Child(item.uid)
                      .PostAsync(dtx)
                      ;

                    string idTam = dino.Key.ToString();
                    dtx.id = idTam;
                    await firebase
                         .Child("csdlmoi")
                       .Child("dataixuong")
                       .Child(item.uid)
                       .Child(idTam)
                       .PutAsync(dtx);


                    success = true;
                }



                //var listbaihat = getListBaiHat();
                //var datakq = (from baihat in listbaihat
                //              where baihat.chedo == 1 && baihat.id.Equals(item.key.ToString())
                //              select baihat).ToList();

                //using (WebClient wc = new WebClient())
                //{

                //    wc.DownloadFileAsync(
                //        // Param1 = Link of file
                //        new System.Uri(datakq[0].link),
                //        // Param2 = Path to save
                //        "D:\\" + datakq[0].tenbaihat + ".mp3"
                //    );
                //};
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> GetLink([FromForm] IFormCollection file)
        {

            long size = file.Files.Sum(f => f.Length);
            string uid = file["uid"].ToString();
            string pathName = "Access";
            string link = "";
            //var type = file.Files.FirstOrDefault(f => f.ContentType == "image/jpeg").ContentType ;

            var path = Path.Combine(_env.WebRootPath, $"music/{pathName}");
            try
            {
                foreach (var item in file.Files)
                {
                    DateTime aDateTime = DateTime.Now;
                    string tg = uid + aDateTime.Day.ToString() + aDateTime.Month.ToString()
                        + aDateTime.Year.ToString() + aDateTime.Hour.ToString()
                        + aDateTime.Minute.ToString() + aDateTime.Second.ToString() + aDateTime.DayOfYear.ToString() + ".mp3";
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


                                link = await Task.Run(() => Upload(fs, tg));

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

                                link = await Task.Run(() => Upload(fs, tg));
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
            var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            // cancel upload midway

            var cancel = new CancellationTokenSource();

            var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            }).Child("music").Child("nguoidung").Child(filename).PutAsync(stream, cancel.Token);
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
            string uid = file["uid"].ToString();
            string link = "";
            var path = Path.Combine(_env.WebRootPath, $"image/{pathName}");
            try
            {
                foreach (var item in file.Files)
                {
                    DateTime aDateTime = DateTime.Now;
                    string tg = uid + aDateTime.Day.ToString() + aDateTime.Month.ToString()
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
            var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            // cancel upload midway

            var cancel = new CancellationTokenSource();
            var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            }).Child("image").Child("baihat").Child("nguoidung").Child(filename).PutAsync(stream, cancel.Token);
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
        // 03/08 sữa bổ sung
        // 21/08 Đã Sữa CSDL Mới
        [HttpPost]
        public async Task<IActionResult> taoBaiHat([FromBody] baihatModel item)
        {

            bool success = true;
            item.chedo = 0;
            item.thoigian = DateTime.Now;
            try
            {
                if (item.nguoidung_id == null && item.nguoidung_id == "")
                {
                    success = false;
                    return Json(success);
                }
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
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> taiTheLoaiKetHopDanhSachPhatTheLoai()
        {

            var firebase = new FirebaseClient(Key);

            var dino = await firebase
              .Child("theloai")
              .OnceAsync<theloaiModel>();

            var danhsachphattheloai = await firebase
              .Child("danhsachphattheloai")
              .OnceAsync<danhsachphattheloaiModel>();
            var data = from dsptl in danhsachphattheloai
                       join tl1 in dino on dsptl.Object.theloai_id equals tl1.Object.id
                       where dsptl.Object.theloai_id.Equals(tl1.Object.id)
                       select new
                       {
                           dsptl.Object
                       };
            var data1 = from tl in dino
                        select new
                        {
                            key = tl.Key,
                            tentheloai = tl.Object.tentheloai,
                            Object = (from dsptl in danhsachphattheloai
                                      join tl1 in dino on dsptl.Object.theloai_id equals tl1.Object.id
                                      where dsptl.Object.theloai_id.Equals(tl.Key)
                                      select new
                                      {
                                          dsptl.Object
                                      }).Take(6)
                        };
            return Json(data1);
        }
        [HttpPost]
        public async Task<IActionResult> taiNhacMoi()
        {

            var firebase = new FirebaseClient(Key);

            // add new item to list of data and let the client generate new key for you (done offline)
            var baihat = await firebase
             .Child("baihat")
             .OnceAsync<baihatModel>();


            var data1 = from bh in baihat

                        orderby bh.Object.thoigian descending
                        select new
                        {
                            bh.Object
                        };


            return Json(data1);
        }
        // 22/07 chưa sữa api để hiện yêu thích
        [HttpPost]
        public async Task<IActionResult> taiDanhSachPhatTheLoaiChiTiet([FromBody] Text item)
        {

            var danhsachphattheloai = LayBangDanhSachPhatTheLoai();
            var data = (from dsptl in danhsachphattheloai
                        where dsptl.id.Equals(item.key.ToString())
                        select dsptl).ToList();
            try
            {
                if (item.uid != "" && item.uid != "null")
                {
                    var datachuyendoi = convertDanhSachPhatTheLoai(data, item.uid.ToString()).ToList();
                    return Json(datachuyendoi);
                }
                else
                {
                    return Json(data);
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 21/08 Đã Sữa CSDL Mới
        // 22/07 chưa sữa api để hiện yêu thích
        [HttpPost]
        public async Task<IActionResult> taiDanhSachPhatTheLoai([FromBody] Text item)
        {
            var danhsachphattheloai = LayBangDanhSachPhatTheLoai(item.key.ToString());
            var data = (from dsptl in danhsachphattheloai
                        where dsptl.theloai_id.Equals(item.key.ToString())
                        select dsptl).ToList();
            try
            {
                if (item.uid != "")
                {
                    var datachuyendoi = convertDanhSachPhatTheLoai(data, item.uid.ToString()).ToList();
                    return Json(datachuyendoi);
                }
                else
                {
                    return Json(data);
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        [HttpPost]
        public async Task<IActionResult> taiDanhSachBaiHat([FromBody] Text item)
        {

            var firebase = new FirebaseClient(Key);

            var baihat = await firebase
              .Child("baihat")
              .OnceAsync<baihatModel>();

            var data = from bh in baihat
                       where bh.Object.danhsachphattheloai_id.Equals(item.key.ToString())
                       select new
                       {
                           bh.Object
                       };
            //   int a = data.Sum(s => Convert.ToInt32(s.Object.thoiluongbaihat));
            return Json(data);
        }
        // 21/08 Đã Sữa CSDL Mới
        //tải bài hát theo id
        // Sữa 16/07 tai bai hat
        [HttpPost]
        public object taiBaiHatTheoId([FromBody] Text item)
        {

           

            var baihat = getListBaiHat();

            var data = (from bh in baihat
                        where bh.id.Equals(item.key.ToString()) && bh.daxoa == 0 && bh.vohieuhoa == 0
                        select bh).ToList();
            try
            {
                if (item.uid != "")
                {
                    var datareturn = convertBaiHat(data, item.uid);
                    return datareturn;
                }
                else
                {
                    return Json(data);
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        // 21/08 Đã Sữa CSDL Mới
        //13/07 taiDanhSachPhatNguoiDungChiTiet_PlayList
        //16/07 sữa taiChiTietPlayList_PlayList
        // 03/08 sữa 
        public class modelChiTietPlayList
        {
            public string dspnguoidung_id { get; set; }
            // người dùng sỡ hữu playlist dspnguoidung
            public string nguoidung_id { get; set; }
            //người dùng đang đăng nhập hiện tại
            public string uid { get; set; }
        }
        [HttpPost]
        public object taiChiTietPlayList_PlayList([FromBody] Text item)
        {
            var danhsachphatnguoidung = LayBangDanhSachPhatNguoiDung();

            var data = (from dspnd in danhsachphatnguoidung
                        where dspnd.id.Equals(item.key.ToString())
                        select dspnd).ToList();
            try
            {
                if (item.uid != "" && item.uid != null && item.uid != "null")
                {
                    var datareturn = convertDanhSachPhatNguoiDung(data, item.uid);
                    return datareturn;
                }
                else
                {
                    return data;
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

            // return Json(data);
        }
        // 21/08 Đã Sữa CSDL Mới
        //16/07 sữa taiThongTinNguoiDungBangIdPlayList_PLayList
        [HttpPost]
        public async Task<IActionResult> taiThongTinNguoiDungBangIdPlayList_PLayList(string uid = "")
        {
            var nguoidung = LayBangNguoiDung(uid);

            var data = (from nd in nguoidung
                        where nd.uid.Equals(uid.ToString())
                        select nd).ToList();


            return Json(data);
        }

        //14/07 taiBaiHatTheoIdDanhSachPhatNguoiDung_PlayList
        [HttpPost]
        public object taiDSBaiHatTheoDSPNguoiDung_PlayList([FromBody] modelChiTietPlayList item)
        {
            var danhsachphatnguoidung = LayBangDanhSachPhatNguoiDung(item.nguoidung_id);
            var baihat = getListBaiHat();
            var chitietdanhsachphatnguoidung = LayBangChiTietDanhSachPhatNguoiDung(item.nguoidung_id);
            var dino = (from ctdspnd in chitietdanhsachphatnguoidung
                        join dspnd in danhsachphatnguoidung on ctdspnd.danhsachphat_id equals dspnd.id
                        join bh in baihat on ctdspnd.baihat_id equals bh.id
                        where ctdspnd.danhsachphat_id.Equals(item.dspnguoidung_id.ToString()) && bh.chedo == 1 && bh.daxoa == 0
                        //orderby bh.Object.luotthich descending
                        select bh).ToList();


            // return Json(dino);
            try
            {
                if (item.uid != "")
                {
                    var datareturn = convertBaiHat(dino, item.uid);
                    return datareturn;
                }
                else
                {
                    return Json(dino);
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 21/08 Đã Sữa CSDL Mới
        // 17/07
        // 16/07 sữa tai bài hat theo the loai thành list
        [HttpPost]
        public object taiDanhSachBaiHatTheoTheLoai([FromBody] Text item)
        {
            var baihat = getListBaiHat();
            var data = (from bh in baihat
                        where bh.theloai_id.Equals(item.key.ToString()) && bh.daxoa == 0 && bh.chedo == 1
                        select bh).ToList();
            //var data1 = data.Take(15)
            try
            {
                if (item.uid != "")
                {
                    var datareturn = convertBaiHat(data, item.uid);
                    return datareturn;
                }
                else
                {
                    return Json(data);
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
            // return Json(data1);
        }
        // 17/07 thêm mới
        public class danhsachphatcustomModel : danhsachphattheloaiModel
        {


            public int yeuthich { get; set; }
            // THÊM TRƯỜNG QUẢNG CÁO

        }
        public class danhsachphatnguoidungcustomModel : danhsachphatnguoidungModel
        {


            public int yeuthich { get; set; }
            // THÊM TRƯỜNG QUẢNG CÁO

        }


        // 21/08 Đã Sữa CSDL Mới
        public List<danhsachphatcustomModel> convertDanhSachPhatTheLoai(List<danhsachphattheloaiModel> list, string uid)
        {
            var listyeuthichdsp = LayBangYeuThichDSPTheLoai();
            listyeuthichdsp = (from yeuthich in listyeuthichdsp
                               where yeuthich.nguoidung_id == uid
                               select yeuthich).ToList();
            List<danhsachphatcustomModel> listkq = new List<danhsachphatcustomModel>();
            for (int item = 0; item < list.Count(); item++)
            {
                danhsachphatcustomModel danhsachphat = new danhsachphatcustomModel();

                danhsachphat.id = list[item].id;
                danhsachphat.tendanhsachphattheloai = list[item].tendanhsachphattheloai;
                danhsachphat.linkhinhanh = list[item].linkhinhanh;
                danhsachphat.theloai_id = list[item].theloai_id;
                danhsachphat.mota = list[item].mota;
                bool checkYeuThich = false;
                for (int j = 0; j < listyeuthichdsp.Count(); j++)
                {

                    if (list[item].id.Equals(listyeuthichdsp[j].danhsachphat_id))
                    {
                        checkYeuThich = true;
                    }

                }
                if (checkYeuThich)
                {
                    danhsachphat.yeuthich = 1;
                    listkq.Add(danhsachphat);
                }
                else
                {
                    danhsachphat.yeuthich = 0;
                    listkq.Add(danhsachphat);
                }

            }
            return listkq;
        }
        // 21/08 Đã Sữa CSDL Mới
        // 03/08 bổ sung kiểm tra id nd và dsptl id
        [HttpPost]
        public async Task<bool> YeuThichDanhSachPhatAsync([FromBody] yeuthichdanhsachphattheloaiModel yeuthichdanhsachphattheloai)

        {
            try
            {
                if (yeuthichdanhsachphattheloai.nguoidung_id == null && yeuthichdanhsachphattheloai.danhsachphat_id == null ||
                    yeuthichdanhsachphattheloai.nguoidung_id == "" && yeuthichdanhsachphattheloai.danhsachphat_id == "")
                {
                    return false;
                }
                var list = LayBangYeuThichDSPTheLoai(yeuthichdanhsachphattheloai.nguoidung_id);
                if (list.Count > 0)
                {
                    var kq = (from yeuthichnew in list
                              where yeuthichnew.danhsachphat_id.Equals(yeuthichdanhsachphattheloai.danhsachphat_id) && yeuthichnew.nguoidung_id.Equals(yeuthichdanhsachphattheloai.nguoidung_id)
                              select yeuthichnew).ToList();
                    if (kq.Count() > 0)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = client.Delete("csdlmoi/yeuthich/yeuthichdanhsachphattheloai/" + yeuthichdanhsachphattheloai.nguoidung_id + "/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        yeuthichdanhsachphattheloai.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                            .Child("csdlmoi")
                            .Child("yeuthich")
                          .Child("yeuthichdanhsachphattheloai")
                          .Child(yeuthichdanhsachphattheloai.nguoidung_id)
                          .PostAsync(yeuthichdanhsachphattheloai)
                          ;

                        string idkey = dino.Key.ToString();
                        yeuthichdanhsachphattheloai.id = idkey;
                        await firebase
                            .Child("csdlmoi")
                            .Child("yeuthich")
                           .Child("yeuthichdanhsachphattheloai")
                           .Child(yeuthichdanhsachphattheloai.nguoidung_id)
                           .Child(idkey)
                           .PutAsync(yeuthichdanhsachphattheloai);
                        return true;
                    }

                }
                else
                {
                    yeuthichdanhsachphattheloai.thoigian = DateTime.Now;
                    var firebase = new FirebaseClient(Key);

                    // add new item to list of data and let the client generate new key for you (done offline)
                    var dino = await firebase
                        .Child("csdlmoi")
                        .Child("yeuthich")
                      .Child("yeuthichdanhsachphattheloai")
                      .Child(yeuthichdanhsachphattheloai.nguoidung_id)
                      .PostAsync(yeuthichdanhsachphattheloai)
                      ;

                    string idkey = dino.Key.ToString();
                    yeuthichdanhsachphattheloai.id = idkey;
                    await firebase
                        .Child("csdlmoi")
                        .Child("yeuthich")
                       .Child("yeuthichdanhsachphattheloai")
                       .Child(yeuthichdanhsachphattheloai.nguoidung_id)
                       .Child(idkey)
                       .PutAsync(yeuthichdanhsachphattheloai);
                    return true;
                }

            }
            catch
            {
                return false;
            }

        }
        public class ModelIdDSP
        {
            public string uid { get; set; }
            public string id_dsp { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> taiTheLoaiKetHopDanhSachPhatTheLoaiTheoThoiGian([FromBody] ModelIdDSP item)
        {


            var firebase = new FirebaseClient(Key);

            var dino = await firebase
                .Child("csdlmoi")
                .Child("theloai")
                .OrderByKey()
                .StartAt(item.id_dsp)
                .LimitToFirst(3)
             .OnceAsync<theloaiModel>();
            //var theloai = (from tl in dino
            //               select tl.Object).ToList();
            //var theloaitext = (from tl in dino
            //                   where tl.Key != item.id_dsp
            //                   select new
            //                   {
            //                       key = tl.Object.id,
            //                       tentheloai = tl.Object.tentheloai,
            //                       Object = new List<danhsachphattheloaiModel>()
            //                   }
            //               ).ToList();
            //  var danhsachphattheloai = LayBangDanhSachPhatTheLoai();
            //int i = 0;
            //foreach (var tl in theloaitext)
            //{
            //    var dsptl123 = LayBangDanhSachPhatTheLoai(tl.key);
            //    if (theloaitext[i].key == tl.key)
            //    {
            //        theloaitext[i].Object.AddRange(dsptl123);
            //    }
            //    i++;

            //}
            try
            {
                if (item.uid != null && item.uid != "null")
                {
                    var theloaitext = (from tl in dino
                                       where tl.Key != item.id_dsp
                                       select new
                                       {
                                           key = tl.Object.id,
                                           tentheloai = tl.Object.tentheloai,
                                           Object = new List<danhsachphatcustomModel>()
                                       }
                           ).ToList();
                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = convertDanhSachPhatTheLoai(LayBangDanhSachPhatTheLoai(tl.key), item.uid.ToString());
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }
                    return Json(theloaitext);
                }
                else
                {
                    var theloaitext = (from tl in dino
                                       where tl.Key != item.id_dsp
                                       select new
                                       {
                                           key = tl.Object.id,
                                           tentheloai = tl.Object.tentheloai,
                                           Object = new List<danhsachphattheloaiModel>()
                                       }
                          ).ToList();
                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = LayBangDanhSachPhatTheLoai(tl.key);
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }
                    return Json(theloaitext);
                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        [HttpPost]
        public async Task<IActionResult> taiTheLoaiKetHopDanhSachPhatTheLoaiMoi(string uid = "")
        {


            var firebase = new FirebaseClient(Key);

            var dino = await firebase
                .Child("csdlmoi")
                .Child("theloai")
                .OrderByKey()
                .LimitToFirst(3)
             .OnceAsync<theloaiModel>();



            try
            {
                if (uid != null && uid != "null")
                {
                    var theloaitext = (from tl in dino
                                       select new
                                       {
                                           key = tl.Object.id,
                                           tentheloai = tl.Object.tentheloai,
                                           Object = new List<danhsachphatcustomModel>()
                                       }
                          ).ToList();

                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = convertDanhSachPhatTheLoai(LayBangDanhSachPhatTheLoai(tl.key), uid.ToString());
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }

                    return Json(theloaitext);
                }
                else
                {
                    var theloaitext = (from tl in dino
                                       select new
                                       {
                                           key = tl.Object.id,
                                           tentheloai = tl.Object.tentheloai,
                                           Object = new List<danhsachphattheloaiModel>()
                                       }
                           ).ToList();
                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = LayBangDanhSachPhatTheLoai(tl.key);
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }
                    return Json(theloaitext);
                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        // 20 / 07 
        public class TextTaiXuong
        {
            public string linknhac { get; set; }
            public string tentaixuong { get; set; }
            public string key { get; set; }
        }
        [HttpPost]

        public async Task<IActionResult> downloadBaiHatVeMayNguoiDung([FromBody] TextTaiXuong item)
        {
            //var path = Path.Combine(_env.WebRootPath, $"music\\Download\\");
            var filename = item.tentaixuong;
            var link = item.linknhac;
            var pathq = Path.Combine(_env.WebRootPath, $"music\\Download\\");
            using (WebClient wc = new WebClient())
            {

                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new System.Uri(link),
                    // Param2 = Path to save
                    pathq + filename
                );
            };
            // long kt = 0;
            long size = 0;
            while (size < 1)
            {
                FileInfo finfo = new FileInfo(pathq + filename);
                size = finfo.Length;
            }
            return Json(filename);
        }
        [HttpPost]

        public async Task<IActionResult> xoaNhacDaTaiXuong([FromBody] TextTaiXuong item)
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
        }
        // 20 / 07 
        // 21/07 xóa bài hát khỏi playlist
        [HttpPost]

        public async Task<IActionResult> xoaBaiHatKhoiDSPNguoiDung_Playlist([FromBody] modelThemBaiHatVaoDSPNguoiDung item)
        {

            bool success = true;

            try
            {
                var chitietdspnguoidung = LayBangChiTietDanhSachPhatNguoiDung(item.uid);

                var data = (from ctdspnd in chitietdspnguoidung
                            where ctdspnd.danhsachphat_id.Equals(item.danhsachphatnguoidung_id.ToString()) && ctdspnd.baihat_id.Equals(item.baihat_id.ToString())
                            select ctdspnd).ToList();
                if (data.Count() > 0)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Delete("csdlmoi/chitietdanhsachphatnguoidung/" + item.uid + "/" + data[0].id);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(success);
        }
        //21/07 bổ sung 14/07  themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi
        //03/08 bổ sung kiểm tra id
        public class modelThemBaiHatVaoDSPNguoiDung
        {
            public string baihat_id { get; set; }
            public string uid { get; set; }

            public string danhsachphatnguoidung_id { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi([FromBody] modelThemBaiHatVaoDSPNguoiDung item)
        {

            bool success = true;

            try
            {
                if (item.baihat_id == null && item.baihat_id == ""
                    || item.danhsachphatnguoidung_id == null && item.danhsachphatnguoidung_id == ""
                    || item.uid == null && item.uid == ""
                    )
                {
                    success = false;
                    return Json(success);
                }
                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                chitietdanhsachphatnguoidungModel ctdspnd = new chitietdanhsachphatnguoidungModel();
                ctdspnd.baihat_id = item.baihat_id;
                ctdspnd.danhsachphat_id = item.danhsachphatnguoidung_id;

                var dino = await firebase
                    .Child("csdlmoi")
                  .Child("chitietdanhsachphatnguoidung")
                  .Child(item.uid)
                  .PostAsync(ctdspnd)
                  ;

                string idTam = dino.Key.ToString();
                ctdspnd.id = idTam;
                await firebase
                    .Child("csdlmoi")
                   .Child("chitietdanhsachphatnguoidung")
                    .Child(item.uid)
                   .Child(idTam)
                   .PutAsync(ctdspnd);

                var baihat = getListBaiHat();

                var databh = (from bh in baihat
                            where bh.id.Equals(item.baihat_id.ToString()) && bh.daxoa == 0 && bh.vohieuhoa == 0
                            select bh).ToList();
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/danhsachphatnguoidung/" + item.uid + "/" + item.danhsachphatnguoidung_id  + "/linkhinhanh", databh[0].linkhinhanh);

            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        // 24/07 top 20 danh sach phat 1844 ///
        // 22/08 Đã Sữa CSDL Mới
        [HttpPost]
        public object taiDSPBaiHatTheoDSPTop20_DSPTop20([FromBody] Text itemmodel)
        {
            try
            {
                var listbaihat = getListBaiHat();
                var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai(itemmodel.key.ToString());
                var datakq = (from baihat in listbaihat
                              where baihat.chedo == 1 && baihat.daxoa == 0 && baihat.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
                              select baihat).OrderByDescending(x => x.luotnghe).Take(20).ToList();
                var datathembaihat = (from bh in listbaihat
                                      join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                                      where bh.chedo == 1 && bh.daxoa == 0 && ctdsptl.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
                                      select bh).ToList();
                if (datathembaihat.Count > 0)
                {
                    foreach (var item in datathembaihat)
                    {
                        datakq.Add(item);
                    }
                }
                if (itemmodel.uid != "")
                {


                    var datareturn = convertBaiHat(datakq, itemmodel.uid);
                    return datareturn.OrderByDescending(x => x.luotnghe);
                }
                else
                {
                    return Json(datakq.OrderByDescending(x => x.luotnghe));
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }

        // thanh toán thành công thanh toán thất bại 2607
        [HttpPost]
        public object taiThongTinThanhToanThanhCong([FromBody] Text item)
        {
            try
            {
                var hoadonthanhtoan = LayBangHoaDonThanhToan();
                var goivip = LayBangGoiVip();
                var nguoidung = LayBangNguoiDung();
                var datahd = (from hdtt in hoadonthanhtoan
                              join gv in goivip on hdtt.loaigoivip_id equals gv.id
                              join nd in nguoidung on hdtt.nguoidung_id equals nd.uid
                              where hdtt.id.Equals(item.key.ToString())
                              select new
                              {
                                  hoadonthanhtoan = hdtt,
                                  hoten = nd.hoten,
                                  email = nd.email,
                                  tengoivip = gv.tengoivip



                              }




                              ).ToList();



                return datahd.ToList();

                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }




        public class top20customModel : top20Model
        {


            public int yeuthich { get; set; }
            // THÊM TRƯỜNG QUẢNG CÁO

        }
        // 18/08 Đã Sữa CSDL Mới
        public List<top20customModel> convertTop20(List<top20Model> list, string uid)
        {
            var listyeuthichtop20 = LayBangYeuThichTop20(uid);
            listyeuthichtop20 = (from yeuthich in listyeuthichtop20
                                 where yeuthich.nguoidung_id == uid
                                 select yeuthich).ToList();
            List<top20customModel> listkq = new List<top20customModel>();
            for (int item = 0; item < list.Count(); item++)
            {
                top20customModel top20 = new top20customModel();

                top20.id = list[item].id;
                top20.tentop20 = list[item].tentop20;
                top20.linkhinhanh = list[item].linkhinhanh;
                top20.danhsachphattheloai_id = list[item].danhsachphattheloai_id;
                top20.theloai_id = list[item].theloai_id;
                top20.mota = list[item].mota;
                top20.daxoa = list[item].daxoa;
                bool checkYeuThich = false;
                for (int j = 0; j < listyeuthichtop20.Count(); j++)
                {

                    if (list[item].id.Equals(listyeuthichtop20[j].top20_id))
                    {
                        checkYeuThich = true;
                    }

                }
                if (checkYeuThich)
                {
                    top20.yeuthich = 1;
                    listkq.Add(top20);
                }
                else
                {
                    top20.yeuthich = 0;
                    listkq.Add(top20);
                }

            }
            return listkq;
        }
        [HttpPost]
        public async Task<IActionResult> taiTheLoaiKetHopTop20TheoThoiGian([FromBody] ModelIdDSP item)
        {


            var firebase = new FirebaseClient(Key);

            var dino = await firebase
                .Child("csdlmoi")
                .Child("theloai")
                .OrderByKey()
                .EndAt(item.id_dsp)
                .LimitToLast(3)
             .OnceAsync<theloaiModel>();
            //var theloai = (from tl in dino
            //               select tl.Object).ToList();
            //var theloaitext = (from tl in dino
            //                   where tl.Key != item.id_dsp
            //                   select new
            //                   {
            //                       key = tl.Object.id,
            //                       tentheloai = tl.Object.tentheloai,
            //                       Object = new List<top20Model>()
            //                   }
            //               ).OrderByDescending(x => x.key).ToList();

            try
            {
                if (item.uid != null && item.uid != "null")
                {
                    var theloaitext = (from tl in dino
                                       where tl.Key != item.id_dsp
                                       select new
                                       {
                                           key = tl.Object.id,
                                           tentheloai = tl.Object.tentheloai,
                                           Object = new List<top20customModel>()
                                       }
                          ).OrderByDescending(x => x.key).ToList();
                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = convertTop20(LayBangTop20(tl.key), item.uid.ToString());
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }
                    return Json(theloaitext);
                }
                else
                {
                    var theloaitext = (from tl in dino
                                       where tl.Key != item.id_dsp
                                       select new
                                       {
                                           key = tl.Object.id,
                                           tentheloai = tl.Object.tentheloai,
                                           Object = new List<top20Model>()
                                       }
                          ).OrderByDescending(x => x.key).ToList();
                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = LayBangTop20(tl.key);
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }
                    return Json(theloaitext);
                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        [HttpPost]
        public async Task<IActionResult> taiTheLoaiKetHopTop20(string uid = "")
        {


            var firebase = new FirebaseClient(Key);

            var dino1 = await firebase
               .Child("csdlmoi")
               .Child("theloai")
               .OrderByKey()
               .LimitToLast(3)
               .OnceAsync<theloaiModel>();
            var dino = (from tl in dino1
                        select tl.Object).ToList();
            //  var top20 = LayBangTop20();

            List<theloaiModel> theloai = new List<theloaiModel>();
            for (int y = dino.Count - 1; y >= 0; y--)
            {
                theloai.Add(dino[y]);
            }

            //var theloaitext = (from tl in theloai

            //                   select new
            //                   {
            //                       key = tl.id,
            //                       tentheloai = tl.tentheloai,
            //                       Object = new List<top20Model>()
            //                   }
            //              ).ToList();
            try
            {
                //if (uid != null)
                //{
                //    var data1 = (from tl in theloai
                //                 select new
                //                 {
                //                     key = tl.id,
                //                     tentheloai = tl.tentheloai,
                //                     Object = convertTop20((from t20 in top20
                //                                            join tl1 in theloai on t20.theloai_id equals tl1.id
                //                                            where t20.theloai_id.Equals(tl.id) && t20.daxoa == 0
                //                                            select t20).ToList(), uid.ToString())
                //                 }).Take(11).ToList();
                //    return Json(data1);
                //}

                //else
                //{
                //    var data = (from tl in theloai
                //                select new
                //                {
                //                    key = tl.id,
                //                    tentheloai = tl.tentheloai,
                //                    Object = (from t20 in top20
                //                              join tl1 in theloai on t20.theloai_id equals tl1.id
                //                              where t20.theloai_id.Equals(tl.id) && t20.daxoa == 0
                //                              select t20).ToList()
                //                }).Take(11).ToList();
                //    return Json(data);
                //}
                // return Json("null");
                if (uid != null && uid != "null")
                {
                    var theloaitext = (from tl in theloai

                                       select new
                                       {
                                           key = tl.id,
                                           tentheloai = tl.tentheloai,
                                           Object = new List<top20customModel>()
                                       }
                         ).ToList();
                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = convertTop20(LayBangTop20(tl.key), uid.ToString());
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }
                    return Json(theloaitext);
                }
                else
                {
                    var theloaitext = (from tl in theloai

                                       select new
                                       {
                                           key = tl.id,
                                           tentheloai = tl.tentheloai,
                                           Object = new List<top20Model>()
                                       }
                         ).ToList();
                    int i = 0;
                    foreach (var tl in theloaitext)
                    {
                        var dsptl123 = LayBangTop20(tl.key);
                        if (theloaitext[i].key == tl.key)
                        {
                            theloaitext[i].Object.AddRange(dsptl123);
                        }
                        i++;
                    }
                    return Json(theloaitext);
                }
                return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }
        [HttpPost]
        public async Task<IActionResult> taiTop20ChiTiet([FromBody] Text item)
        {

            var top20 = LayBangTop20();
            var data = (from t20 in top20
                        where t20.id.Equals(item.key.ToString()) && t20.daxoa == 0
                        select t20).ToList();
            try
            {
                if (item.uid != "" && item.uid != "null")
                {
                    var datachuyendoi = convertTop20(data, item.uid.ToString()).ToList();
                    return Json(datachuyendoi);
                }
                else
                {
                    return Json(data);
                }
                // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        // 1/8


        // 2/8 sữa hình ảnh người dùng
        [HttpPost]
        public async Task<IActionResult> GetLinkHinhAnhNguoiDung([FromForm] IFormCollection file)
        {

            long size = file.Files.Sum(f => f.Length);
            string pathName = "Access";
            string uid = file["uid"].ToString();
            string link = "";
            var path = Path.Combine(_env.WebRootPath, $"image/{pathName}");
            try
            {
                foreach (var item in file.Files)
                {
                    DateTime aDateTime = DateTime.Now;
                    string tg = uid + aDateTime.Day.ToString() + aDateTime.Month.ToString()
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


                                link = await Task.Run(() => UploadHinhAnhNguoiDung(fs, tg));
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

                                link = await Task.Run(() => UploadHinhAnhNguoiDung(fs, tg));
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
        public async Task<string> UploadHinhAnhNguoiDung(FileStream stream, string filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            // cancel upload midway

            var cancel = new CancellationTokenSource();
            var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                ThrowOnCancel = true
            }).Child("image").Child("nguoidung").Child(filename).PutAsync(stream, cancel.Token);
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
        public class BienTam
        {
            public string linkhinhanh { get; set; }
            public string bangnguoidung_id { get; set; }
            public string uid { get; set; }
            public string baihat_id { get; set; }

            public string hoten { get; set; }

            public string mota { get; set; }
        }
        //public async Task<IActionResult> xoaAnhTrongStore(string thucmuc2, string thucmuc3, string linkhinhanh)
        //{
        //    string hinhanh = "";
        //    string link = linkhinhanh;
        //    var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
        //    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
        //    var cancel = new CancellationTokenSource();
        //    var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
        //    {
        //        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
        //        ThrowOnCancel = true
        //    }).Child("image").Child("nguoidung").Child("").DeleteAsync();
        //    if (thucmuc3 != "")
        //    {
        //        string[] list = link.Split(thucmuc3 + "%2Fh");
        //        string fileName = list[1];
        //        list = fileName.Split("?alt");
        //        hinhanh = list[0];
        //    }
        //    else
        //    {
        //        string[] list = link.Split(thucmuc2 + "%2Fh");
        //        string fileName = list[1];
        //        list = fileName.Split("?alt");
        //        hinhanh = list[0];
        //    }
        //    return hinhanh;
        //}
        [HttpPost]
        public async Task<IActionResult> suaLinkHinhAnhNguoiDung_CaNhan([FromBody] BienTam item)
        {
            bool success = true;
            try
            {
                var nguoidung = LayBangNguoiDung();
                var data = (from nd in nguoidung
                            where nd.uid.Equals(item.uid)
                            select nd).ToList();
                if (data[0].hinhdaidien != "")
                {
                    string link = data[0].hinhdaidien;
                    string[] list = link.Split("nguoidung%2F");
                    string fileName = list[1];
                    list = fileName.Split("?alt");
                    fileName = list[0];
                    var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                    var cancel = new CancellationTokenSource();
                    var task = new FirebaseStorage(Bucket, new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }).Child("image").Child("nguoidung").Child(fileName).DeleteAsync();
                }
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/nguoidung/" + data[0].uid + "/" + "hinhdaidien", item.linkhinhanh);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        //[HttpPost]
        //public async Task<IActionResult> taiDSPChuDe_TheLoai([FromBody] Text item)
        //{

        //    var top20 = LayBangTop20();
        //    var data = (from t20 in top20
        //                where t20.id.Equals(item.key.ToString()) && t20.daxoa == 0
        //                select t20).ToList();
        //    try
        //    {
        //        if (item.uid != "")
        //        {
        //            var datachuyendoi = convertTop20(data, item.uid.ToString()).ToList();
        //            return Json(datachuyendoi);
        //        }
        //        else
        //        {
        //            return Json(data);
        //        }
        //        // return Json("null");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message.ToString());
        //    }
        //}  3116// 883// 2353// 2469/// 2681  //2706  //2947 
        // 06/08 
        [HttpPost]
        public async Task<IActionResult> chinhSuaHoTen_CaNhan([FromBody] BienTam item)
        {
            bool success = true;
            try
            {
                if (item.uid == null && item.uid == "")
                {
                    success = false;
                    return Json(success);
                }
                var nguoidung = LayBangNguoiDung();
                var data = (from nd in nguoidung
                            where nd.uid.Equals(item.uid)
                            select nd).ToList();

                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/nguoidung/" + data[0].uid + "/" + "hoten", item.hoten);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> chinhSuaMoTa_CaNhan([FromBody] BienTam item)
        {
            bool success = true;
            try
            {
                if (item.uid == null && item.uid == "")
                {
                    success = false;
                    return Json(success);
                }
                var nguoidung = LayBangNguoiDung();
                var data = (from nd in nguoidung
                            where nd.uid.Equals(item.uid)
                            select nd).ToList();

                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/nguoidung/" + data[0].uid + "/" + "mota", item.mota);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public List<top20Model> getListYeuThichTop20_canhan(string uid)
        {
            if (uid != null && uid != "null")
            {
                var listdspyeuthich = LayBangYeuThichTop20();
                var listdsp = LayBangTop20();

                var result = (from list in listdsp
                              join yttop20 in listdspyeuthich on list.id equals yttop20.top20_id
                              where yttop20.nguoidung_id.Equals(uid)
                              select list).ToList();

                return result;

            }
            else
                return null;

        }

        [HttpPost]
        public async Task<IActionResult> taiTheLoaiTheoId_ChiTietTheLoai([FromBody] Text item)
        {
            var theloai = LayBangTheLoai(item.key.ToString());

            return Json(theloai);
        }
        [HttpPost]
        public async Task<IActionResult> taiChuDeTheoId_ChuDe([FromBody] Text item)
        {
            var chude = LayBangChuDe(item.key.ToString());

            return Json(chude);
        }
        [HttpPost]
        public async Task<IActionResult> taiDanhSachBaiHatTheoChuDe_ChuDe([FromBody] Text item)
        {


            try
            {
                var baihat = getListBaiHat();
                var datakq = (from bh in baihat
                              where bh.daxoa == 0 && bh.chedo == 1 && bh.chude_id.Equals(item.key.ToString())
                              select bh).ToList();
                if (item.uid != null && item.uid != "" && item.uid != "null")
                {


                    //List<baihatModel> datakq = data.ToList();
                    var datareturn = convertBaiHat(datakq, item.uid);
                    return Json(datareturn.OrderByDescending(x => x.thoigian).Take(20));
                }
                else
                {

                    return Json(datakq.OrderByDescending(x => x.thoigian).Take(20));
                }
                //return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }



    }

}
