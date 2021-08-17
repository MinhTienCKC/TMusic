using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> ThanhToanThanhCongPayPal()
        {

            captureOrder(orderIdPaypal);
            try
            {
                hoadontam.mota = "Thanh toán vip bằng paypal thành công." + "-" + "Ngày thanh toán: " + hoadontam.thoigian + "  -" + "Giá tiền: " + hoadontam.giatien;

                client = new FireSharp.FirebaseClient(config);
                PushResponse response_dsphat = client.Push("hoadonthanhtoan/", hoadontam);
                hoadontam.id = response_dsphat.Result.name;
                SetResponse setResponse = client.Set("hoadonthanhtoan/" + hoadontam.id, hoadontam);


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
                SetResponse response = client.Set("nguoidung/" + nguoidung.id, nguoidung);

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
                FirebaseResponse response = client.Delete("hoadonthanhtoan/" + hoadon.id);

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
                PushResponse response_dsphat = client.Push("hoadonthanhtoan/", hoadontam);
                hoadontam.id = response_dsphat.Result.name;
                SetResponse setResponse = client.Set("hoadonthanhtoan/" + hoadontam.id, hoadontam);


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
                SetResponse response = client.Set("nguoidung/" + nguoidung.id, nguoidung);

                return Redirect("/#/thanhtoanthanhcong/" + hoadontam.id);
            }
            else
            {
                firstSign = null;
                return Redirect("/#/thanhtoanthatbai/");
            }

        }
        public IActionResult ReceiveRequestMomoPost()
        {
            var status = Request.Form["errorCode"].ToString();
            var idbill = Request.Form["orderId"].ToString();

            if (status == "0")
            {
                hoadontam.mota = "Thanh toán vip bằng MOMO thành công." + "  -  " + "Ngày thanh toán: " + hoadontam.thoigian + "  -  " + "Giá tiền: " + hoadontam.giatien;

                client = new FireSharp.FirebaseClient(config);
                PushResponse response_dsphat = client.Push("hoadonthanhtoan/", hoadontam);
                hoadontam.id = response_dsphat.Result.name;
                SetResponse setResponse = client.Set("hoadonthanhtoan/" + hoadontam.id, hoadontam);


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
                SetResponse response = client.Set("nguoidung/" + nguoidung.id, nguoidung);
                return Ok();
            }
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse responsez = client.Delete("hoadonthanhtoan/" + hoadontam.id);
            return Ok();
        }


        // end momo

        //zalo pay

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
        public List<chudeModel> LayBangChuDe()
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
        /// 29/07 
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
                FirebaseResponse response = client.Get("csdlmoi/danhsachphat/danhsachphattheloai" + idtheloai);
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
        public List<goivipModel> LayBangGoiVip()
        {
            try
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
            catch
            {
                return null;
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

                    list.Add(data);
                    return list;
                }
            }
            catch
            {
                return null;
            }

        }
        public List<theloaiModel> LayBangTheLoai()
        {
            try
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
            catch
            {
                return null;
            }


        }
        public List<quangcaoModel> LayBangQuangCao()
        {
            try
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
            catch
            {
                return null;
            }


        }
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
        [HttpPost]
        public object getUser(string uid)
        {

            try
            {
                var list = LayBangNguoiDung(uid);
                var user = (from nd in list
                            where nd.uid == uid
                            select nd).FirstOrDefault();
                if (user != null)
                {
                    if (user.hansudungvip < DateTime.Now)
                    {
                        user.vip = 0;
                        client = new FireSharp.FirebaseClient(config);
                        SetResponse response = client.Set("nguoidung/" + user.id, user);
                    }
                    return user;
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
        public async Task<IActionResult> taiTheLoai()
        {

            var firebase = new FirebaseClient(Key);

            var dino = await firebase
              .Child("theloai")
              .OnceAsync<theloaiModel>();

            return Json(dino);
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
        public class Text
        {
            public string key { get; set; }
            public string uid { get; set; }
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
        // 22/07 chưa sữa api để hiện yêu thích
        [HttpPost]
        public async Task<IActionResult> taiDanhSachPhatTheLoai([FromBody] Text item)
        {
            var danhsachphattheloai = LayBangDanhSachPhatTheLoai();
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

        [HttpPost]
        public async Task<IActionResult> taiChuDe()
        {
            //    string text = dltr.key.ToString();
            var firebase = new FirebaseClient(Key);

            var dino = await firebase
              .Child("chude")
              .OnceAsync<chudeModel>();


            return Json(dino);
        }
        // 03/08 sữa bổ sung
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
                  .Child("baihat")
                  .PostAsync(item)
                  ;

                string kk = dino.Key.ToString();
                item.id = kk;
                await firebase
                   .Child("baihat")
                   .Child(kk)
                   .PutAsync(item);
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        public class uidBaiHat
        {
            public string uid { get; set; }
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
                        baihatModel qery = (from bh in listbaihat
                                            where bh.id == yeuthich.baihat_id
                                            select bh).FirstOrDefault();
                        if (kq.Count() > 0)
                        {
                            client = new FireSharp.FirebaseClient(config);
                            FirebaseResponse response = client.Delete("yeuthich/" + kq[0].id);


                            qery.luotthich = qery.luotthich - 1;
                            client = new FireSharp.FirebaseClient(config);
                            SetResponse responsebaihat = client.Set("baihat/" + qery.id, qery);
                            return false;
                        }
                        else
                        {
                            qery.luotthich = qery.luotthich + 1;
                            client = new FireSharp.FirebaseClient(config);
                            SetResponse responsebaihat = client.Set("baihat/" + qery.id, qery);
                            yeuthich.thoigian = DateTime.Now;
                            var firebase = new FirebaseClient(Key);

                            // add new item to list of data and let the client generate new key for you (done offline)
                            var dino = await firebase
                              .Child("yeuthich")
                              .PostAsync(yeuthich)
                              ;

                            string idkey = dino.Key.ToString();
                            yeuthich.id = idkey;
                            await firebase
                               .Child("yeuthich")
                               .Child(idkey)
                               .PutAsync(yeuthich);
                            return true;
                        }

                    }
                    else
                        return false;

                }
                else
                    return false;

            }
            catch
            {
                return false;
            }

        }
        [HttpPost]
        public async Task<bool> theoDoiNguoiDung([FromBody] theodoiModel theodoi)
        {
            try
            {
                var list = LayBangTheoDoi();
                if (list.Count > 0)
                {
                    var kq = (from theodoilist in list
                              where theodoilist.nguoidung_id.Equals(theodoi.nguoidung_id) && theodoilist.nguoidung_theodoi_id.Equals(theodoi.nguoidung_theodoi_id)
                              select theodoilist).ToList();
                    if (kq.Count() > 0)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = client.Delete("theodoi/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        theodoi.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                          .Child("theodoi")
                          .PostAsync(theodoi)
                          ;

                        string idkey = dino.Key.ToString();
                        theodoi.id = idkey;
                        await firebase
                           .Child("theodoi")
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
                      .Child("theodoi")
                      .PostAsync(theodoi)
                      ;

                    string idkey = dino.Key.ToString();
                    theodoi.id = idkey;
                    await firebase
                       .Child("theodoi")
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
        public List<baihatModel> getListBaiHat()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("csdlmoi/baihat");
            var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<baihatModel>();


            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<baihatModel>(((JProperty)item).Value.ToString()));

            }


            return list;

        }
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
                nguoidung.cover = list[item].cover;
                nguoidung.daxacthuc = list[item].daxacthuc;
                nguoidung.email = list[item].email;
                nguoidung.facebook = list[item].facebook;
                nguoidung.id = list[item].id;
                nguoidung.gioitinh = list[item].gioitinh;
                nguoidung.hansudungvip = list[item].hansudungvip;
                nguoidung.hinhdaidien = list[item].hinhdaidien;
                nguoidung.hoten = list[item].hoten;
                nguoidung.matkhau = list[item].matkhau;
                nguoidung.mota = list[item].mota;
                nguoidung.ngaysinh = list[item].ngaysinh;
                nguoidung.online = list[item].online;
                nguoidung.quocgia = list[item].quocgia;
                nguoidung.thanhpho = list[item].thanhpho;
                nguoidung.thoigian = list[item].thoigian;
                nguoidung.uid = list[item].uid;
                nguoidung.vip = list[item].vip;
                nguoidung.website = list[item].website;
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
        public List<nguoidungchualogincustomModel> convertNguoiDungChuaLogin(List<nguoidungModel> list)
        {
            var listyeuthichtong = LayBangTheoDoi();

            List<nguoidungchualogincustomModel> listkq = new List<nguoidungchualogincustomModel>();

            for (int item = 0; item < list.Count(); item++)
            {
                nguoidungchualogincustomModel nguoidung = new nguoidungchualogincustomModel();
                nguoidung.cover = list[item].cover;
                nguoidung.daxacthuc = list[item].daxacthuc;
                nguoidung.email = list[item].email;
                nguoidung.facebook = list[item].facebook;
                nguoidung.id = list[item].id;
                nguoidung.gioitinh = list[item].gioitinh;
                nguoidung.hansudungvip = list[item].hansudungvip;
                nguoidung.hinhdaidien = list[item].hinhdaidien;
                nguoidung.hoten = list[item].hoten;
                nguoidung.matkhau = list[item].matkhau;
                nguoidung.mota = list[item].mota;
                nguoidung.ngaysinh = list[item].ngaysinh;
                nguoidung.online = list[item].online;
                nguoidung.quocgia = list[item].quocgia;
                nguoidung.thanhpho = list[item].thanhpho;
                nguoidung.thoigian = list[item].thoigian;
                nguoidung.uid = list[item].uid;
                nguoidung.vip = list[item].vip;
                nguoidung.website = list[item].website;
                nguoidung.soluongtheodoi = (from yeuthich in listyeuthichtong
                                            where yeuthich.nguoidung_theodoi_id == list[item].uid
                                            select yeuthich).Count();
                listkq.Add(nguoidung);
            }
            return listkq;
        }
        [HttpPost]
        public object GetListBaiHatMoi(string uid = "")
        {
            try
            {
                if (uid != null)
                {

                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1
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
        [HttpPost]
        public object getListTheoDoi(string uid)
        {
            try
            {

                var listbh = getListBaiHat();
                var listnd = LayBangNguoiDung(uid);
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
                                                where bh.chedo == 1 && bh.nguoidung_id == ng.uid
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
        public async Task<IActionResult> TaoTaiKhoan([FromBody] nguoidungModel model)
        {
            try
            {

                if (model != null && model.uid != "")
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("nguoidung");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<nguoidungModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<nguoidungModel>(((JProperty)item).Value.ToString()));

                        }
                        var datakq = from nguoidung in list
                                     where nguoidung.email.Equals(model.email)
                                     select nguoidung;

                        if (datakq.Count() > 0)
                        {
                            return Json(false);
                        }
                        else
                        {
                            model.thoigian = DateTime.Now;
                            var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
                            var a = await auth.CreateUserWithEmailAndPasswordAsync(model.email, model.matkhau, model.hoten, true);
                            client = new FireSharp.FirebaseClient(config);
                            model.uid = a.User.LocalId.ToString();
                            PushResponse response1 = client.Push("nguoidung/", model);
                            model.id = response1.Result.name;
                            SetResponse setResponse = client.Set("nguoidung/" + model.id, model);
                            return Json(true);
                        }
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
        [HttpPost]
        public object taiDanhSachPhatTop20_khampha(string uid = "")
        {



            var top20 = LayBangTop20();
            var data = (from t20 in top20
                        where t20.daxoa == 0
                        select t20).ToList();
            try
            {
                if (uid != null && uid != "")
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
        public object getListNgheNhieuNhat(string uid)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1
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


        public object getListLikeNhieuNhat(string uid)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1
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

        public object getListDowloadNhieuNhat(string uid)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1
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
        [HttpPost]
        public object getListBaiHatYeuThich(string uid)
        {
            try
            {
                if (uid != null)
                {
                    var listbaihat = getListBaiHat();
                    var listyeuthich = LayBangYeuThichBaiHat(uid);
                    var datakq = (from bh in listbaihat
                                  where (bh.chedo == 1)
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
                    PushResponse response_dsphat = client.Push("danhsachphatnguoidung/", model);
                    model.id = response_dsphat.Result.name;
                    SetResponse setResponse = client.Set("danhsachphatnguoidung/" + model.id, model);
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
        public async Task<object> TaoNguoiDungVoiXacThuc([FromBody] nguoidungModel model)
        {
            try
            {
                if (model.uid != null && model.uid != "")
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("nguoidung");

                    model.thoigian = DateTime.Now;
                    model.hansudungvip = DateTime.Now;
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<nguoidungModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<nguoidungModel>(((JProperty)item).Value.ToString()));

                        }
                        var datakq = from nguoidung in list
                                     where nguoidung.uid == model.uid
                                     select nguoidung;

                        if (datakq.Count() > 0)
                        {
                            return Json("datontai");
                        }
                        else
                        {
                            model.daxacthuc = 1;
                            model.thoigian = DateTime.Now;
                            var auth = new FirebaseAuthProvider(new FirebaseConfig1(ApiKey));
                            client = new FireSharp.FirebaseClient(config);
                            PushResponse response1 = client.Push("nguoidung/", model);
                            model.id = response1.Result.name;
                            SetResponse setResponse = client.Set("nguoidung/" + model.id, model);
                            return Json("dataomoi");
                        }

                    }
                    else
                    {
                        return Json("LoiFirebase");
                    }

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

        // thêm lượt nghe bài hát 
        [HttpPost]
        public bool themLuotNghe(string idbh)
        {
            try
            {
                if (idbh != null)
                {
                    var listbaihat = getListBaiHat();
                    baihatModel qery = (from bh in listbaihat
                                        where bh.id == idbh
                                        select bh).FirstOrDefault();
                    qery.luotnghe = qery.luotnghe + 1;
                    client = new FireSharp.FirebaseClient(config);
                    SetResponse response = client.Set("baihat/" + qery.id, qery);
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
        // thêm lượt nghe bài hát 
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
                    SetResponse response = client.Set("nguoidung/" + qery.id + "/email", qery.email);
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
        public bool SetBaiHat([FromBody] baihatModel bh)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("baihat/" + bh.id, bh);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        public async Task<bool> XoaDanhSachPhatNguoiDung(string iddsp)
        {
            try
            {


                //return list;
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("csdlmoi/nguoidung/" + "hFxjlTD1nzeMgmtJiIBQN9xGJ8D3");
                var data = JsonConvert.DeserializeObject<nguoidungModel>(response.Body);
                List<nguoidungModel> list = new List<nguoidungModel>();

                list.Add(data);
                //return dino.Select



                //var listbh = LayBangchi
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        //tìm kiếm playlist nguoi dung
        [HttpPost]
        public object timKiemDanhSachPhatNguoiDung([FromBody] timkiemmodel model)
        {
            try
            {
                if (model != null)
                {
                    var listPlaylist = LayBangDanhSachPhatNguoiDung();
                    var datakq = (from platlist in listPlaylist
                                  where platlist.tendanhsachphat.ToUpper().Contains(model.tuKhoa.ToUpper())
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
        [HttpPost]
        public object getListDanhSachPhatNguoiDung([FromBody] Text model)
        {
            try
            {
                if (model.uid != null)
                {
                    var listPlaylist = LayBangDanhSachPhatNguoiDung();
                    var datakq = (from platlist in listPlaylist
                                  where platlist.nguoidung_id == model.uid
                                  select platlist).ToList();
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
        [HttpPost]
        public async Task<object> TimKiemBaiHatAll([FromBody] timkiemmodel model)
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
                                  where baihat.tenbaihat.ToUpper().Contains(model.tuKhoa.ToUpper()) && baihat.chedo == 1 && baihat.daxoa == 0
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
        [HttpPost]
        public List<danhsachphattheloaiModel> getListYeuThichDSP_canhan(string uid)
        {
            if (uid != null)
            {
                var listdspyeuthich = LayBangYeuThichDSPTheLoai();
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
        [HttpPost]
        public object getListYeuThichDSPNgheSi_canhan(string uid)
        {
            if (uid != null)
            {
                var listdspyeuthich = layBangYeuThichDanhSachPhatNguoiDung();
                var listdsp = LayBangDanhSachPhatNguoiDung();

                var result = (from list in listdsp
                              join listyeuthich in listdspyeuthich on list.id equals listyeuthich.danhsachphat_id
                              where listyeuthich.nguoidung_id == uid
                              select list).ToList();

                return result;

            }
            else
                return null;

        }
        //tìm kiếm nguoi dùng
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
        [HttpPost]
        public object timKiemNgheSi([FromBody] modelTimKiemNgheSi model)
        {
            try
            {
                if (model.uidNgheSi != null)
                {

                    var listnguoidung = LayBangNguoiDung();
                    var datakq = (from nguoidung in listnguoidung
                                  where nguoidung.uid == model.uidNgheSi
                                  select nguoidung).ToList();

                    if (model.uidNguoiDung != null)
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

        // Cá Nhân
        public List<dataixuongModel> getListDaTaiXuong()
        {

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("dataixuong");
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
        public List<baihatcustomModel> getListDaTaiLen_CaNhan(string uid)
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

            }
            list = (from bh in list
                    where bh.nguoidung_id == uid
                    select bh).ToList();
            var result = convertBaiHat(list, uid);
            return result;

        }
        [HttpPost]
        public object getListDaTaiLen_NgheSi([FromBody] modelTimKiemNgheSi model)
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

            }
            list = (from bh in list
                    where bh.nguoidung_id == model.uidNgheSi && bh.chedo == 1
                    select bh).OrderByDescending(x => x.luotnghe).ToList();
            if (model.uidNguoiDung != null)
            {

                var result = convertBaiHat(list, model.uidNguoiDung);
                return result;
            }

            return list;




        }
        [HttpPost]
        public object getListHoaDon(string uid)
        {
            try
            {
                var hd = LayBangHoaDonThanhToan();
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

        public List<danhsachphatnguoidungModel> getListPlaylist_CaNhan()
        {

            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("danhsachphatnguoidung");
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
        [HttpPost]
        public List<baihatcustomModel> getListDaTaiXuong_CaNhan(string uid)
        {
            try
            {

                var listbh = getListBaiHat();
                var listdatai = getListDaTaiXuong();
                var qery = (from bh in listbh
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
        [HttpPost]
        public object getPlaylist_CaNhan(string uid)
        {
            try
            {

                var listdanhsachphat = getListPlaylist_CaNhan();
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
        // hàm lấy bài hát mới   ví dụ
        [HttpPost]
        public object taiDSPBaiHatMoi_NhacMoi(string uid = "")
        {
            try
            {
                if (uid != null)
                {

                    var listbaihat = getListBaiHat();
                    var datakq = (from baihat in listbaihat
                                  where baihat.chedo == 1
                                  select baihat).OrderByDescending(x => x.thoigian).Take(20).ToList();
                    //List<baihatModel> datakq = data.ToList();
                    var datareturn = convertBaiHat(datakq, uid);
                    return datareturn;
                }
                else
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
                                      where baihat.chedo == 1
                                      select baihat).OrderByDescending(x => x.thoigian).Take(20).ToList();

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
        // sữa 24 / 01 load lại dữ liệu
        // lấy bài theo dsp thể loại 11/07
        [HttpPost]
        public object taiDSPBaiHatTheoDSPTheLoai_DSP([FromBody] Text itemmodel)
        {
            try
            {
                var listbaihat = getListBaiHat();
                var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai();
                var datakq = (from baihat in listbaihat
                              where baihat.chedo == 1 && baihat.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
                              select baihat).ToList();
                var datathembaihat = (from bh in listbaihat
                                      join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                                      where bh.chedo == 1 && ctdsptl.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
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
                                    where bh.id == item.key
                                    select bh).FirstOrDefault();
                qery.luottaixuong = qery.luottaixuong + 1;
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("baihat/" + qery.id, qery);
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
                      .Child("dataixuong")
                      .PostAsync(dtx)
                      ;

                    string idTam = dino.Key.ToString();
                    dtx.id = idTam;
                    await firebase
                       .Child("dataixuong")
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
        //tải bài hát theo id
        // Sữa 16/07 tai bai hat
        [HttpPost]
        public object taiBaiHatTheoId([FromBody] Text item)
        {



            var baihat = getListBaiHat();

            var data = (from bh in baihat
                        where bh.id.Equals(item.key.ToString())
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
        //13/07 taiDanhSachPhatNguoiDungChiTiet_PlayList
        //16/07 sữa taiChiTietPlayList_PlayList
        // 03/08 sữa 
        [HttpPost]
        public object taiChiTietPlayList_PlayList([FromBody] Text item)
        {
            var danhsachphatnguoidung = LayBangDanhSachPhatNguoiDung();

            var data = (from dspnd in danhsachphatnguoidung
                        where dspnd.id.Equals(item.key.ToString())
                        select dspnd).ToList();
            try
            {
                if (item.uid != "" && item.uid != null)
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
        //16/07 sữa taiThongTinNguoiDungBangIdPlayList_PLayList
        [HttpPost]
        public async Task<IActionResult> taiThongTinNguoiDungBangIdPlayList_PLayList(string uid = "")
        {
            var nguoidung = LayBangNguoiDung();

            var data = (from nd in nguoidung
                        where nd.uid.Equals(uid.ToString())
                        select nd).ToList();


            return Json(data);
        }

        //14/07 taiBaiHatTheoIdDanhSachPhatNguoiDung_PlayList
        [HttpPost]
        public object taiDSBaiHatTheoDSPNguoiDung_PlayList([FromBody] Text item)
        {
            var danhsachphatnguoidung = LayBangDanhSachPhatNguoiDung();
            var baihat = getListBaiHat();
            var chitietdanhsachphatnguoidung = LayBangChiTietDanhSachPhatNguoiDung();
            var dino = (from ctdspnd in chitietdanhsachphatnguoidung
                        join dspnd in danhsachphatnguoidung on ctdspnd.danhsachphat_id equals dspnd.id
                        join bh in baihat on ctdspnd.baihat_id equals bh.id
                        where ctdspnd.danhsachphat_id.Equals(item.key.ToString())
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

        // 17/07
        // 16/07 sữa tai bài hat theo the loai thành list
        [HttpPost]
        public object taiDanhSachBaiHatTheoTheLoai([FromBody] Text item)
        {



            var baihat = getListBaiHat();
            var data = (from bh in baihat
                        where bh.theloai_id.Equals(item.key.ToString())
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
        // 03/08 bổ sung nười dùng id
        public List<danhsachphatnguoidungcustomModel> convertDanhSachPhatNguoiDung(List<danhsachphatnguoidungModel> list, string uid)
        {
            var listyeuthichdsp = layBangYeuThichDanhSachPhatNguoiDung();
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
                var list = LayBangYeuThichDSPTheLoai();
                if (list.Count > 0)
                {
                    var kq = (from yeuthichnew in list
                              where yeuthichnew.danhsachphat_id.Equals(yeuthichdanhsachphattheloai.danhsachphat_id) && yeuthichnew.nguoidung_id.Equals(yeuthichdanhsachphattheloai.nguoidung_id)
                              select yeuthichnew).ToList();
                    if (kq.Count() > 0)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = client.Delete("yeuthichdanhsachphattheloai/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        yeuthichdanhsachphattheloai.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                          .Child("yeuthichdanhsachphattheloai")
                          .PostAsync(yeuthichdanhsachphattheloai)
                          ;

                        string idkey = dino.Key.ToString();
                        yeuthichdanhsachphattheloai.id = idkey;
                        await firebase
                           .Child("yeuthichdanhsachphattheloai")
                           .Child(idkey)
                           .PutAsync(yeuthichdanhsachphattheloai);
                        return true;
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
                        FirebaseResponse response = client.Delete("yeuthichdanhsachphatnguoidung/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        yeuthichdanhsachphatnguoidung.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                          .Child("yeuthichdanhsachphatnguoidung")
                          .PostAsync(yeuthichdanhsachphatnguoidung)
                          ;

                        string idkey = dino.Key.ToString();
                        yeuthichdanhsachphatnguoidung.id = idkey;
                        await firebase
                           .Child("yeuthichdanhsachphatnguoidung")
                           .Child(idkey)
                           .PutAsync(yeuthichdanhsachphatnguoidung);
                        return true;
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
        [HttpPost]
        public object taiTheLoaiKetHopDanhSachPhatTheLoaiMoi(string uid = "")
        {


            var theloai = LayBangTheLoai();
            var danhsachphattheloai = LayBangDanhSachPhatTheLoai();
            //var data = from dsptl in danhsachphattheloai
            //           join tl1 in theloai on dsptl.Object.theloai_id equals tl1.Object.id
            //           where dsptl.Object.theloai_id.Equals(tl1.Object.id)
            //           select new
            //           {
            //               dsptl.Object
            //           };
            try
            {
                if (uid != null)
                {
                    var data1 = (from tl in theloai
                                 select new
                                 {
                                     key = tl.id,
                                     tentheloai = tl.tentheloai,
                                     Object = convertDanhSachPhatTheLoai((from dsptl in danhsachphattheloai
                                                                          join tl1 in theloai on dsptl.theloai_id equals tl1.id
                                                                          where dsptl.theloai_id.Equals(tl.id)
                                                                          select dsptl).ToList(), uid.ToString())
                                 }).Take(11).ToList();
                    return Json(data1);
                }
                else
                {
                    var data = (from tl in theloai
                                select new
                                {
                                    key = tl.id,
                                    tentheloai = tl.tentheloai,
                                    Object = (from dsptl in danhsachphattheloai
                                              join tl1 in theloai on dsptl.theloai_id equals tl1.id
                                              where dsptl.theloai_id.Equals(tl.id)
                                              select dsptl).ToList()
                                }).Take(11).ToList();
                    return Json(data);
                }
                // return Json("null");
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

        public async Task<IActionResult> xoaBaiHatKhoiDSPNguoiDung_Playlist([FromBody] Text item)
        {

            bool success = true;

            try
            {
                var chitietdspnguoidung = LayBangChiTietDanhSachPhatNguoiDung();

                var data = (from ctdspnd in chitietdspnguoidung
                            where ctdspnd.danhsachphat_id.Equals(item.uid.ToString()) && ctdspnd.baihat_id.Equals(item.key.ToString())
                            select ctdspnd).ToList();
                if (data.Count() > 0)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Delete("chitietdanhsachphatnguoidung/" + data[0].id);
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
        [HttpPost]
        public async Task<IActionResult> themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi([FromBody] Text item)
        {

            bool success = true;

            try
            {
                if (item.key == null && item.key == "" || item.uid == null && item.uid == "")
                {
                    success = false;
                    return Json(success);
                }
                var firebase = new FirebaseClient(Key);

                // add new item to list of data and let the client generate new key for you (done offline)
                chitietdanhsachphatnguoidungModel ctdspnd = new chitietdanhsachphatnguoidungModel();
                ctdspnd.baihat_id = item.key;
                ctdspnd.danhsachphat_id = item.uid;

                var dino = await firebase
                  .Child("chitietdanhsachphatnguoidung")
                  .PostAsync(ctdspnd)
                  ;

                string idTam = dino.Key.ToString();
                ctdspnd.id = idTam;
                await firebase
                   .Child("chitietdanhsachphatnguoidung")
                   .Child(idTam)
                   .PutAsync(ctdspnd);


            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        // 24/07 top 20 danh sach phat 1844 ///
        [HttpPost]
        public object taiDSPBaiHatTheoDSPTop20_DSPTop20([FromBody] Text itemmodel)
        {
            try
            {
                var listbaihat = getListBaiHat();
                var chitietdanhsachphattheloai = LayBangChiTietDanhSachPhatTheLoai();
                var datakq = (from baihat in listbaihat
                              where baihat.chedo == 1 && baihat.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
                              select baihat).OrderByDescending(x => x.luotnghe).Take(20).ToList();
                var datathembaihat = (from bh in listbaihat
                                      join ctdsptl in chitietdanhsachphattheloai on bh.id equals ctdsptl.baihat_id
                                      where bh.chedo == 1 && ctdsptl.danhsachphattheloai_id.Equals(itemmodel.key.ToString())
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



        // 03/08 bổ sung kt id nd và top20 id 
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
                var list = LayBangYeuThichTop20();
                if (list.Count > 0)
                {
                    var kq = (from yeuthichnew in list
                              where yeuthichnew.top20_id.Equals(yeuthichtop20.top20_id) && yeuthichnew.nguoidung_id.Equals(yeuthichtop20.nguoidung_id)
                              select yeuthichnew).ToList();
                    if (kq.Count() > 0)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = client.Delete("yeuthichtop20/" + kq[0].id);
                        return false;
                    }
                    else
                    {
                        yeuthichtop20.thoigian = DateTime.Now;
                        var firebase = new FirebaseClient(Key);

                        // add new item to list of data and let the client generate new key for you (done offline)
                        var dino = await firebase
                          .Child("yeuthichtop20")
                          .PostAsync(yeuthichtop20)
                          ;

                        string idkey = dino.Key.ToString();
                        yeuthichtop20.id = idkey;
                        await firebase
                           .Child("yeuthichtop20")
                           .Child(idkey)
                           .PutAsync(yeuthichtop20);
                        return true;
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
        public class top20customModel : top20Model
        {


            public int yeuthich { get; set; }
            // THÊM TRƯỜNG QUẢNG CÁO

        }
        public List<top20customModel> convertTop20(List<top20Model> list, string uid)
        {
            var listyeuthichtop20 = LayBangYeuThichTop20();
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
        public object taiTheLoaiKetHopTop20(string uid = "")
        {


            var theloai = LayBangTheLoai();
            var top20 = LayBangTop20();

            try
            {
                if (uid != null)
                {
                    var data1 = (from tl in theloai
                                 select new
                                 {
                                     key = tl.id,
                                     tentheloai = tl.tentheloai,
                                     Object = convertTop20((from t20 in top20
                                                            join tl1 in theloai on t20.theloai_id equals tl1.id
                                                            where t20.theloai_id.Equals(tl.id) && t20.daxoa == 0
                                                            select t20).ToList(), uid.ToString())
                                 }).Take(11).ToList();
                    return Json(data1);
                }
                else
                {
                    var data = (from tl in theloai
                                select new
                                {
                                    key = tl.id,
                                    tentheloai = tl.tentheloai,
                                    Object = (from t20 in top20
                                              join tl1 in theloai on t20.theloai_id equals tl1.id
                                              where t20.theloai_id.Equals(tl.id) && t20.daxoa == 0
                                              select t20).ToList()
                                }).Take(11).ToList();
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
        public async Task<IActionResult> taiTop20ChiTiet([FromBody] Text item)
        {

            var top20 = LayBangTop20();
            var data = (from t20 in top20
                        where t20.id.Equals(item.key.ToString()) && t20.daxoa == 0
                        select t20).ToList();
            try
            {
                if (item.uid != "")
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
        public List<chitietdanhsachphattheloaiModel> LayBangChiTietDanhSachPhatTheLoai()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("chitietdanhsachphattheloai");
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
                object p = client.Set("nguoidung/" + data[0].id + "/" + "hinhdaidien", item.linkhinhanh);
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
                object p = client.Set("nguoidung/" + data[0].id + "/" + "hoten", item.hoten);
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
                object p = client.Set("nguoidung/" + data[0].id + "/" + "mota", item.mota);
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
            if (uid != null)
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

    }

}
