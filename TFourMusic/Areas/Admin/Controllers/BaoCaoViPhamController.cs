
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
using System.Security.Claims;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
    [Authorize]
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
     
        public class ShareModel
        {
            public string IdAlbum { get; set; }
            public string IdUsers { get; set; }
        }
       
        //Lấy dữ liệu
        [HttpPost]
        public async Task<IActionResult> taiBaiHatViPhamChuaXuLy()
        {
            try
            {
                var BaoCaoBaiHatDaXuLy_KhongViPham = LayBangBaoCaoBaiHatDaXuLy_KhongViPham();
                var BaoCaoNguoiDungDaXuLy_KhongViPham = LayBangBaoCaoNguoiDungDaXuLy_KhongViPham();
                //var okok = from okok in baihatbaocao123
                //           where okok.thoigian
                //DateTime ok = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy"));
                var dsbhvpdaxoa = (from bh in BaoCaoBaiHatDaXuLy_KhongViPham
                                   where bh.trangthai == 2 && DateTime.Parse(bh.ngayxuly.AddDays(7).ToString("dd-MM-yyyy")) <= DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"))
                                 select bh).ToList();
                var dsndvpdaxoa = (from nd in BaoCaoNguoiDungDaXuLy_KhongViPham
                                   where nd.trangthai == 2 && DateTime.Parse(nd.ngayxuly.AddDays(7).ToString("dd-MM-yyyy")) <= DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"))
                                   select nd).ToList();
                var baihatbaocao = LayBangBaoCaoBaiHatChuaXuLy();
                var nguoidung = LayBangNguoiDung();
                nguoidungModel admin = new nguoidungModel();
                admin.uid = "admin";
                admin.email = "admin";
                nguoidung.Add(admin);
                var data1 = (from bcvp in baihatbaocao
                             join nd in nguoidung on bcvp.nguoidung_id equals nd.uid
                             join nd1 in nguoidung on bcvp.nguoidung_baocao_id equals nd1.uid
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
                                baihatvipham = LayBaiHatQuaID(bcvp.nguoidung_baocao_id, bcvp.baihat_baocao_id),
                                thoigian = bcvp.thoigian,
                                ngayxuly = bcvp.ngayxuly,
                                daxoa = bcvp.daxoa,
                                trangthai = bcvp.trangthai,
                                 //email_nguoidung = LayEmailQuaNguoiDungID(bcvp.nguoidung_id),
                                 //email_nguoidung_baocao = LayEmailQuaNguoiDungID(bcvp.nguoidung_baocao_id)
                                 email_nguoidung = nd.email,
                                 email_nguoidung_baocao = nd1.email
                                 //                                email_nguoidung = LayBangNguoiDung(bcvp.nguoidung_id), //LayEmailQuaNguoiDungID(bcvp.nguoidung_id),
                                 //email_nguoidung_baocao = LayBangNguoiDung(bcvp.nguoidung_baocao_id)
                             }).ToList();

                //var dulieu = (from bc in dino
                //              select bc.Object).ToList();
                //var data = (from bcvp in baihatbaocao
                //            select new
                //            {
                //                // loibaihat = JsonConvert.DeserializeObject(ok123.Object.noidung)
                //                id = bcvp.id,
                //                noidung = JsonConvert.DeserializeObject<string[]>(bcvp.noidung),
                //                motavande = bcvp.motavande,
                //                nguoidung_id = bcvp.nguoidung_id,
                //                nguoidung_baocao_id = bcvp.nguoidung_baocao_id,
                //                baihat_id = bcvp.baihat_id,
                //                baihat_baocao_id = bcvp.baihat_baocao_id,
                //                baihatvipham = LayBaiHatQuaID(bcvp.nguoidung_baocao_id,bcvp.baihat_baocao_id),
                //                thoigian = bcvp.thoigian,
                //                ngayxuly = bcvp.ngayxuly,
                //                daxoa = bcvp.daxoa,
                //                trangthai = bcvp.trangthai,
                //                email_nguoidung = LayEmailQuaNguoiDungID(bcvp.nguoidung_id),
                //                email_nguoidung_baocao = LayEmailQuaNguoiDungID(bcvp.nguoidung_baocao_id)
                //                //  email_nguoidung = LayBangNguoiDungEmail(bcvp.nguoidung_id),
                //                //email_nguoidung_baocao = LayBangNguoiDungEmail(bcvp.nguoidung_baocao_id)
                //                //                                email_nguoidung = LayBangNguoiDung(bcvp.nguoidung_id), //LayEmailQuaNguoiDungID(bcvp.nguoidung_id),
                //                //email_nguoidung_baocao = LayBangNguoiDung(bcvp.nguoidung_baocao_id)
                //            }).ToList();
                return Json(data1.OrderByDescending(x => x.thoigian));
               // return Json("null");
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        [HttpPost]
        public async Task<IActionResult> taiBaiHatViPhamDaXuLy_ViPham()
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

                var baihatbaocao = LayBangBaoCaoBaiHatDaXuLy_ViPham();
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
                                baihatvipham = LayBaiHatQuaID(bcvp.nguoidung_baocao_id,bcvp.baihat_baocao_id),
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
        [HttpPost]
        public async Task<IActionResult> taiBaiHatViPhamDaXuLy_KhongViPham()
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

                var baihatbaocao = LayBangBaoCaoBaiHatDaXuLy_KhongViPham();
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
                                baihatvipham = LayBaiHatQuaID(bcvp.nguoidung_baocao_id, bcvp.baihat_baocao_id),
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
        [HttpPost]
        public async Task<IActionResult> taiNguoiDungViPhamDaXuLy_ViPham()
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

                var baihatbaocao = LayBangBaoCaoNguoiDungDaXuLy_ViPham();
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
                            //    baihat_id = bcvp.baihat_id,
                            //    baihat_baocao_id = bcvp.baihat_baocao_id,
                             //   baihatvipham = LayBaiHatQuaID(bcvp.nguoidung_baocao_id, bcvp.baihat_baocao_id),
                                thoigian = bcvp.thoigian,
                                ngayxuly = bcvp.ngayxuly,
                              //  daxoa = bcvp.daxoa,
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
        [HttpPost]
        public async Task<IActionResult> taiNguoiDungViPhamDaXuLy_KhongViPham()
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

                var baihatbaocao = LayBangBaoCaoNguoiDungDaXuLy_KhongViPham();
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
                             //   baihat_id = bcvp.baihat_id,
                             //   baihat_baocao_id = bcvp.baihat_baocao_id,
                          //     baihatvipham = LayBaiHatQuaID(bcvp.nguoidung_baocao_id, bcvp.baihat_baocao_id),
                                thoigian = bcvp.thoigian,
                                ngayxuly = bcvp.ngayxuly,
                            //    daxoa = bcvp.daxoa,
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
        [HttpPost]
        public async Task<IActionResult> taiNguoiDungViPhamChuaXuLy()
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

                var nguoidungbaocao = LayBangBaoCaoNguoiDungChuaXuLy();
                //var dulieu = (from bc in dino
                //              select bc.Object).ToList();
                var data = (from bcvp in nguoidungbaocao
                            select new
                            {
                                // loibaihat = JsonConvert.DeserializeObject(ok123.Object.noidung)
                                id = bcvp.id,
                                noidung = JsonConvert.DeserializeObject<string[]>(bcvp.noidung),
                                motavande = bcvp.motavande,
                                nguoidung_id = bcvp.nguoidung_id,
                                nguoidung_baocao_id = bcvp.nguoidung_baocao_id,
                                thoigian = bcvp.thoigian,
                                ngayxuly = bcvp.ngayxuly,
                               // daxoa = bcvp.daxoa,
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
        [HttpPost]
        public async Task<IActionResult> taiBaiHatBangQuyen([FromBody] BaiHatBangQuyen item)
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
                if (nguoidung_id == "admin")
                {
                    return "admin";
                }
                var data = LayBangNguoiDung(nguoidung_id);
                //client = new FireSharp.FirebaseClient(config);
                //FirebaseResponse response = client.Get("csdlmoi/nguoidung/" + nguoidung_id);
                //var data1 = JsonConvert.DeserializeObject<nguoidungModel>(response.Body);
                //List<nguoidungModel> list = new List<nguoidungModel>();
                //if (data1 != null)
                //{
                //    list.Add(data1);
                //}

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

        // lấy bảng
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
        public string LayBangNguoiDungEmail(string uid = null)
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

                    return list[0].email;
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

                    return list[0].email;
                }
            }
            catch
            {
                List<nguoidungModel> list = new List<nguoidungModel>();
                return list[0].email;
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
        public List<baocaonguoidungModel> LayBangBaoCaoNguoiDungChuaXuLy(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/nguoidungvipham/chuaxuly");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaonguoidungModel>();


                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            foreach (var x in item)
                            {
                                foreach (var y in x)

                                {
                                    list.Add(JsonConvert.DeserializeObject<baocaonguoidungModel>(((JProperty)y).Value.ToString()));

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
                    var list = new List<baocaonguoidungModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baocaonguoidungModel>(((JProperty)item).Value.ToString()));

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
        public List<baocaonguoidungModel> LayBangBaoCaoNguoiDungDaXuLy_KhongViPham(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/nguoidungvipham/daxuly/khongvipham");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaonguoidungModel>();


                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            foreach (var x in item)
                            {
                                foreach (var y in x)

                                {
                                    list.Add(JsonConvert.DeserializeObject<baocaonguoidungModel>(((JProperty)y).Value.ToString()));

                                }

                            }

                        }
                    }
                    return list;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/nguoidungvipham/daxuly/khongvipham" + uid);
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaonguoidungModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baocaonguoidungModel>(((JProperty)item).Value.ToString()));

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
        public List<baocaonguoidungModel> LayBangBaoCaoNguoiDungDaXuLy_ViPham(string uid = null)
        {
            try
            {
                if (uid == null)
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/nguoidungvipham/daxuly/vipham");
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaonguoidungModel>();


                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            foreach (var x in item)
                            {
                                foreach (var y in x)

                                {
                                    list.Add(JsonConvert.DeserializeObject<baocaonguoidungModel>(((JProperty)y).Value.ToString()));

                                }

                            }

                        }
                    }
                    return list;
                }
                else
                {
                    client = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = client.Get("csdlmoi/baocao/nguoidungvipham/daxuly/vipham" + uid);
                    var data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var list = new List<baocaonguoidungModel>();

                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            list.Add(JsonConvert.DeserializeObject<baocaonguoidungModel>(((JProperty)item).Value.ToString()));

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
         
        public class PheDuyetModel:modelTrangThai
        {
            //public string nguoidung_id { get; set; }

            //public string baocao_id { get; set; }
            public int vhh_baihat { get; set; }

            public int vhh_nguoidung { get; set; }
        }
        public class modelTrangThai
        {

            public string nguoidung_id { get; set; }

            public string id { get; set; }
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

        public baocaobaihatModel LayChiTietBangBaoCaoBaiHatDaXuLy_ViPham(string nguoidungid, string idbh)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse rp = client.Get("csdlmoi/baocao/baihatvipham/daxuly/vipham/" + nguoidungid.ToString() + "/" + idbh.ToString());
            var datarp = JsonConvert.DeserializeObject<baocaobaihatModel>(rp.Body);
            baocaobaihatModel bh = new baocaobaihatModel();
            if (datarp != null)
            {
                bh = datarp;
            }
            return bh;
        }
        public baocaonguoidungModel LayChiTietBangBaoCaoNguoiDungChuaXuLy(string nguoidungid, string idbh)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse rp = client.Get("csdlmoi/baocao/nguoidungvipham/chuaxuly/" + nguoidungid.ToString() + "/" + idbh.ToString());
            var datarp = JsonConvert.DeserializeObject<baocaonguoidungModel>(rp.Body);
            baocaonguoidungModel bh = new baocaonguoidungModel();
            if (datarp != null)
            {
                bh = datarp;
            }
            return bh;
        }

        public baocaonguoidungModel LayChiTietBangBaoCaoNguoiDungDaXuLy_ViPham(string nguoidungid, string idbh)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse rp = client.Get("csdlmoi/baocao/nguoidungvipham/daxuly/vipham/" + nguoidungid.ToString() + "/" + idbh.ToString());
            var datarp = JsonConvert.DeserializeObject<baocaonguoidungModel>(rp.Body);
            baocaonguoidungModel bh = new baocaonguoidungModel();
            if (datarp != null)
            {
                bh = datarp;
            }
            return bh;
        }


        // chức năng
        [HttpPost]
        public async Task<IActionResult> pheDuyetBaiHatViPham([FromBody] PheDuyetModel item)
        {
            bool success = true;
            try
            {
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/baocao/baihatvipham/chuaxuly/" + item.nguoidung_id + "/" + item.id + "/" + "ngayxuly", DateTime.Now);
                var firebase = new FirebaseClient(Key);
                var data = LayChiTietBangBaoCaoBaiHatChuaXuLy(item.nguoidung_id, item.id);
                var nguoidung = LayBangNguoiDung(data.nguoidung_id);
                if (item.vhh_baihat == 1 )
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
                    message.Body = new TextPart("html")
                    {
                        //Text = "Chào: Anh/chị "+ nguoidung[0].hoten +"\n\n" + 
                                  //  <p>Chúng tôi đã vô hiệu hóa bài hát bạn báo cáo. </p> </br>
                        //"Chúng tôi đã vô hiệu hóa bài hát bạn báo cáo.\n" +
                        //"Chúng tôi đã xem xét bài hát bạn báo cáo. Vì bài hát đã vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi đã vô hiệu hóa bài hát đó. " +
                        //"Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic.. Chúng tôi thông báo cho chủ bài biết rằng bài hát của họ đã bị vô hiệu hóa. \n\n" +
                        ////"Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        //"Cám ơn bạn đã xem. \n" +
                        //"Admin TMUSIC"
                        Text = $@"<h3>Chào: {nguoidung[0].hoten}</h3>

                                    <p>Chúng tôi đã xem xét bài hát bạn báo cáo. </p> </br>
                                     <p>Vì bài hát đã vi phạm tiêu chuẩn cộng đồng nên chúng tôi đã vô hiệu hóa bài hát đó. </p> </br>
                                    <p>Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. </p> </br>
                                    <p>Chúng tôi thông báo cho chủ bài biết rằng bài hát của họ đã bị vô hiệu hóa. </p> </br>
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
                else if (item.vhh_baihat == 0 )
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
                    message.Body = new TextPart("html")
                    {
                        //Text = "Chào: Anh/chị " + nguoidung[0].hoten + "\n\n" +
                        ////"Chúng tôi đã vô hiệu hóa bài hát bạn báo cáo.\n" +
                        //"Chúng tôi đã xem xét bài hát bạn báo cáo. Vì bài hát không vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi nghĩ rằng bạn có chút nhầm lẫn. " +
                        //"Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. \n\n" +
                        ////"Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        //"Cám ơn bạn đã xem. \n" +
                        //"Admin TMUSIC"
                        Text = $@"<h3>Chào: {nguoidung[0].hoten}</h3>

                                
                                    <p>Chúng tôi đã xem xét bài hát bạn báo cáo. </p> </br>
                                     <p>Vì bài hát không vi phạm tiêu chuẩn cộng đồng nên chúng tôi nghĩ rằng bạn có chút nhầm lẫn. </p> </br>
                                    <p>Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. </p> </br>
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
        public async Task<IActionResult> khoiPhucBaiHatViPham([FromBody] PheDuyetModel item)
        {
            bool success = false;
            try
            {
                var firebase = new FirebaseClient(Key);
                var data = LayChiTietBangBaoCaoBaiHatDaXuLy_ViPham(item.nguoidung_id, item.id);
                client = new FireSharp.FirebaseClient(config);
                object p123 = client.Set("csdlmoi/baocao/baihatvipham/daxuly/vipham/" + item.nguoidung_id + "/" + item.id + "/" + "ngayxuly", DateTime.Now);
                var baihatbaocao = LayBaiHatQuaID(data.nguoidung_baocao_id, data.baihat_baocao_id);
                if ( baihatbaocao[0].vohieuhoa == 1)
                {
                    

                        client = new FireSharp.FirebaseClient(config);
                        object p = client.Set("csdlmoi/baihat/" + data.nguoidung_baocao_id + "/" + data.baihat_baocao_id + "/" + "vohieuhoa", 0);
                    if (data.nguoidung_baocao_id != "admin") {
                        var nguoidung = LayBangNguoiDung(data.nguoidung_baocao_id);
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                        message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                        message.Subject = "TMUSIC - BÀI HÁT VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                        message.Body = new TextPart("html")
                        {
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
                  
                    success = true;
                }
               
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
           .Child("daxuly")
           .Child("vipham")
           .Child(item.nguoidung_id)
           .Child(item.id)
           .DeleteAsync();

                success = true;
                return Json(success);

            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(success);

        }
        [HttpPost]
        public async Task<IActionResult> pheDuyetNguoiDungViPham([FromBody] PheDuyetModel item)
        {
            bool success = true;
            try
            {
                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/baocao/nguoidungvipham/chuaxuly/" + item.nguoidung_id + "/" + item.id + "/" + "ngayxuly", DateTime.Now);
                var firebase = new FirebaseClient(Key);
                var data = LayChiTietBangBaoCaoNguoiDungChuaXuLy(item.nguoidung_id, item.id);
                var nguoidung = LayBangNguoiDung(data.nguoidung_id);
                if (item.vhh_nguoidung == 1)
                {
                    data.trangthai = 2;
                    await firebase
                   .Child("csdlmoi")
                  .Child("baocao")
                  .Child("nguoidungvipham")
                  .Child("daxuly")
                  .Child("vipham")
                  .Child(item.nguoidung_id)
                  .Child(item.id)
                 .PutAsync(data);


                    await firebase
               .Child("csdlmoi")
               .Child("baocao")
               .Child("nguoidungvipham")
               .Child("chuaxuly")
               .Child(item.nguoidung_id)
               .Child(item.id)
               .DeleteAsync();

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                    message.Subject = "TMUSIC - NGƯỜI DÙNG VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("html")
                    {
                        //Text = "Chào: Anh/chị " + nguoidung[0].hoten + "\n\n" +   <p>Chúng tôi đã vô hiệu hóa người dùng bạn báo cáo.</p> </br>
                        //"Chúng tôi đã vô hiệu hóa người dùng bạn báo cáo.\n" +
                        //"Chúng tôi đã xem xét người dùng bạn báo cáo. Vì người dùng đã vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi đã vô hiệu hóa tài khoản người dùng đó. " +
                        //"Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. Chúng tôi thông báo cho chủ tài khoản người dùng, họ đã bị vô hiệu hóa. \n\n" +
                        ////"Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        //"Cám ơn bạn đã xem. \n" +
                        //"Admin TMUSIC"
                        Text = $@"<h3>Chào: {nguoidung[0].hoten}</h3>

                                  
                                    <p>Chúng tôi đã xem xét người dùng bạn báo cáo. </p> </br>
                                     <p>Vì người dùng đã vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi đã vô hiệu hóa tài khoản người dùng đó. </p> </br>
                                    <p>Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. </p> </br>
                                    <p>Chúng tôi thông báo cho chủ tài khoản người dùng, họ đã bị vô hiệu hóa. </p> </br>
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
                else if (item.vhh_nguoidung == 0)
                {
                    data.trangthai = 2;
                    await firebase
                  .Child("csdlmoi")
                  .Child("baocao")
                  .Child("nguoidungvipham")
                  .Child("daxuly")
                  .Child("khongvipham")
                  .Child(item.nguoidung_id)
                  .Child(item.id)
                  .PutAsync(data);

                    await firebase
             .Child("csdlmoi")
             .Child("baocao")
             .Child("nguoidungvipham")
             .Child("chuaxuly")
             .Child(item.nguoidung_id)
             .Child(item.id)
             .DeleteAsync();

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                    message.Subject = "TMUSIC - NGƯỜI DÙNG VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("html")
                    {
                        //Text = "Chào: Anh/chị " + nguoidung[0].hoten + "\n\n" +
                   
                        //"Chúng tôi đã xem xét người dùng bạn báo cáo. Vì người dùng không vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi nghĩ rằng bạn có chút nhầm lẫn. " +
                        //"Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. \n\n" +
                        ////"Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        //"Cám ơn bạn đã xem. \n" +
                        //"Admin TMUSIC"
                        Text = $@"<h3>Chào: {nguoidung[0].hoten}</h3>

                                   
                                    <p>Chúng tôi đã xem xét người dùng bạn báo cáo. </p> </br>
                                     <p>Vì người dùng không vi phạm tiêu chuẩn cộng đồng của chúng tôi nên chúng tôi nghĩ rằng bạn có chút nhầm lẫn. </p> </br>
                                    <p>Cảm ơn bạn đã báo cáo và sự đóng góp của bạn để phát triển cộng đồng Tmusic. </p> </br>
                                  
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
        [HttpPost]
        public async Task<IActionResult> khoiPhucNguoiDungViPham([FromBody] PheDuyetModel item)
        {
            bool success = false;
            try
            {
                var firebase = new FirebaseClient(Key);
                var data = LayChiTietBangBaoCaoNguoiDungDaXuLy_ViPham(item.nguoidung_id, item.id);
                var nguoidung = LayBangNguoiDung(data.nguoidung_baocao_id);
                client = new FireSharp.FirebaseClient(config);
                object p123 = client.Set("csdlmoi/baocao/nguoidungvipham/daxuly/vipham" + item.nguoidung_id + "/" + item.id + "/" + "ngayxuly", DateTime.Now);
                if ( nguoidung[0].vohieuhoa == 1)
                {

                    UserRecordArgs args = new UserRecordArgs()
                    {
                        Uid = data.nguoidung_baocao_id,
                        Disabled = false,
                    };
                    UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(args);
                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/nguoidung/" + data.nguoidung_baocao_id + "/" + "vohieuhoa", 0);
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Admin TMUSIC", "0306181067@caothang.edu.vn"));
                    message.To.Add(new MailboxAddress("Người Dùng", nguoidung[0].email));
                    message.Subject = "TMUSIC - TÀI KHOẢN VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
                    message.Body = new TextPart("html")
                    {
                        //Text = "Chào: Anh/chị " + nguoidung[0].hoten + " \n\n" +
                        //        "Có vẻ như tài khoản của bạn đã bị vô hiệu hóa do nhầm lẫn. Chúng tôi đã mở tài khoản của bạn và xin lỗi vì sự bất tiện này.\n\n" +
                        //        " Bây giờ, bạn có thể đăng nhập. \n" +
                        //        "Nếu bạn gặp bất kì sự cố khi đăng nhập vào tài khoản của mình hay có thắc mắc vui lòng trả lời thư này hoặc liên hệ fanpage: TMUSIC Nghe Nhạc Trực Tuyến \n\n" +
                        //        "Cám ơn bạn đã xem. \n" +
                        //        "Admin TMUSIC"
                        Text = $@"<h3>Chào: {nguoidung[0].hoten}</h3>

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
                    success = true;
                }
     
                data.trangthai = 2;
                await firebase
               .Child("csdlmoi")
              .Child("baocao")
              .Child("nguoidungvipham")
              .Child("daxuly")
              .Child("khongvipham")
              .Child(item.nguoidung_id)
              .Child(item.id)
             .PutAsync(data);


                await firebase
           .Child("csdlmoi")
           .Child("baocao")
           .Child("nguoidungvipham")
           .Child("daxuly")
           .Child("vipham")
           .Child(item.nguoidung_id)
           .Child(item.id)
           .DeleteAsync();

                success = true;
                return Json(success);

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
            var heThong = User.Identity as ClaimsIdentity;
            var phanQuyen = heThong.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            if (phanQuyen == "Admin")
            {
               
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
                        message.Subject = "TMUSIC - TÀI KHOẢN VI PHẠM TIÊU CHUẨN CỘNG ĐỒNG";
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
                return Json("");
            }
        }
        [HttpPost]
        public async Task<IActionResult> voHieuHoaBaiHatNguoiDung([FromBody] baihatModel item)
        {
            

            bool success = true;
            try
            {
               
               

                if (item.vohieuhoa == 0)
                {

                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);


                    if (item.nguoidung_id != "admin")
                    {
                        var nguoidung = LayBangNguoiDung(item.nguoidung_id);
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
                   
                }
                else
                {

                    client = new FireSharp.FirebaseClient(config);
                    object p = client.Set("csdlmoi/baihat/" + item.nguoidung_id + "/" + item.id + "/" + "vohieuhoa", item.vohieuhoa);
                    if (item.nguoidung_id != "admin")
                    {
                        var nguoidung = LayBangNguoiDung(item.nguoidung_id);
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
        // cập nhật trạng thái bài hát vi phạm 
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
        public async Task<IActionResult> capNhatTrangThaiNguoiDung([FromBody] modelTrangThai item)
        {


            bool success = true;
            try
            {



                client = new FireSharp.FirebaseClient(config);
                object p = client.Set("csdlmoi/baocao/nguoidungvipham/chuaxuly/" + item.nguoidung_id + "/" + item.id + "/" + "trangthai", 1);

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
        [HttpPost]
        public async Task<IActionResult> xoaTaiKhoanQuanTri([FromBody] taikhoanquantriModel item)
        {
            var heThong = User.Identity as ClaimsIdentity;
            var phanQuyen = heThong.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            var uid = heThong.Claims.FirstOrDefault(c => c.Type == "uid").Value;
            bool success = true;

            if (uid == item.id)
            {
                return Json("loi");
            }
            try
            {

                var firebase = new FirebaseClient(Key);
                await firebase
               .Child("csdlmoi")
               .Child("taikhoanquantri")
               .Child(item.id)
               .DeleteAsync();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Json(success);
        }
    }
}
