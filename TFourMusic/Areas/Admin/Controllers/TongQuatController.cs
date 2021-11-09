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
using FirebaseAdmin.Auth;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Authentication;
using System.Security.Claims;

namespace TFourMusic.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TongQuatController : Controller
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
        public TongQuatController(IHostingEnvironment env)
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

        public class nguoidungcustommodel 
        {
       
            public int daxacthuc { get; set; }
            public string matkhau { get; set; }
            public string email { get; set; }
            public string hoten { get; set; }                  
            public string mota { get; set; }
            public string ngaysinh { get; set; }
            public string hinhdaidien { get; set; }  
            public string gioitinh { get; set; }
            public DateTime thoigian { get; set; }
            public int online { get; set; }
            public int vip { get; set; }
            public DateTime hansudungvip { get; set; }
            public string uid { get; set; }
            public int daxoa { get; set; }
            public int vohieuhoa { get; set; }

        }
        public class baihatcustommodel:baihatModel
        {

            //public string id { get; set; }
            //public string nguoidung_id { get; set; }
            //public string tenbaihat { get; set; }
            //public string mota { get; set; }
            //public int luottaixuong { get; set; }
            //public DateTime thoigian { get; set; }
            //public int chedo { get; set; }
            //public int luotthich { get; set; }
            //public string casi { get; set; }
            //public string loibaihat { get; set; }
            //public int luotnghe { get; set; }
            //public string theloai_id { get; set; }
            //public string chude_id { get; set; }
            //public string danhsachphattheloai_id { get; set; }
            //public string quangcao { get; set; }
            //public string thoiluongbaihat { get; set; }
            //public string link { get; set; }
            //public string linkhinhanh { get; set; }
            //public int daxoa { get; set; }
            //public DateTime thoigianxoa { get; set; }

            public int vohieuhoa { get; set; }
        }
        public class dspnguoidungcustommodel
        {

            public string id { get; set; }
            public string nguoidung_id { get; set; }
            public string tendanhsachphat { get; set; }
            public int chedo { get; set; }
            public DateTime thoigian { get; set; }

            public string linkhinhanh { get; set; }

            public int vohieuhoa { get; set; }

        }
        public class binhluanchaModel
        {
            public string id { get; set; }
            public string baihat_id { get; set; }
            public string nguoidung_id { get; set; }
            public string noidung { get; set; }
       
        }
        public class binhluanconModel
        {
            public string id { get; set; }
            public string baihat_id { get; set; }
            public string nguoidung_id { get; set; }
            public string noidung { get; set; }
            public string binhluancha_id { get; set; }

        }
        public class modelthongke
        {
            public DateTime ngaybatdau { get; set; }
            public DateTime ngayketthuc { get; set; }
            public DateTime theothang { get; set; }
            public DateTime theonam { get; set; }
            public string hientimkiem { get; set; }

        }
        public class soBaiHatTheoNgay
        {
            public int ngaythu { get; set; }
            public int sobaihat { get; set; }
            public int sobaihatadmin { get; set; }
        }
        public class hoadon
        {
            public List<hoadonthanhtoanModel> ngay { get; set; }
            public List<hoadonthanhtoanModel> thang { get; set; }
            public List<hoadonthanhtoanModel> nam { get; set; }
        }
        public class doanhthu
        {
            public float tongtien { get; set; }
            public float tonghoadon { get; set; }
           
        }
        public class hoadon1
        {
            public doanhthu ngay { get; set; }
            public doanhthu thang { get; set; }
            public doanhthu nam { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> taiThongKe123([FromBody] modelthongke item)
        {


          
            var firebase = new FirebaseClient(Key);
           
            var hoadonthanhtoan1 = LayBangHoaDonThanhToan();
           




            try
            {
                DateTime ngaybatdau = DateTime.Parse(item.ngaybatdau.ToString("dd-MM-yyyy"));
                DateTime ngayketthuc = DateTime.Parse(item.ngayketthuc.ToString("dd-MM-yyyy"));
                var hdttd1 = (from hoadon1 in hoadonthanhtoan1
                              where DateTime.Parse(hoadon1.thoigian.ToString("dd-MM-yyyy")) >= ngaybatdau
                                    && DateTime.Parse(hoadon1.thoigian.ToString("dd-MM-yyyy")) <= ngayketthuc
                              select hoadon1).ToList();
                DateTime theothang = DateTime.Parse(item.theothang.ToString("MM-yyyy"));
                var hdttd2 = (from hoadon1 in hoadonthanhtoan1
                              where DateTime.Parse(hoadon1.thoigian.ToString("MM-yyyy")) == theothang
                              select hoadon1).ToList();

                int theonam = item.theonam.Year;
                var hdttd3 = (from hoadon1 in hoadonthanhtoan1
                              where hoadon1.thoigian.Year == theonam
                              select hoadon1).ToList();
                doanhthu dtngay = new doanhthu();
                doanhthu dtthang = new doanhthu();
                doanhthu dtnam = new doanhthu();

                hoadon1 data1 = new hoadon1();              
                
           
                int ng = 0;
                int th = 0;
                int na = 0;
                foreach (var n in hdttd1)
                {
                    ng += n.giatien;
                }
                foreach (var n in hdttd2)
                {
                    th += n.giatien;
                }
                foreach (var n in hdttd3)
                {
                    na += n.giatien;
                }
                dtngay.tongtien = ng;
                dtthang.tongtien = th;
                dtnam.tongtien = na;
                dtngay.tonghoadon = hdttd1.Count();
                dtthang.tonghoadon = hdttd2.Count();
                dtnam.tonghoadon = hdttd3.Count();
                data1.ngay = dtngay;
                data1.thang = dtthang;
                data1.nam = dtnam;
                return Json(data1);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        [HttpPost]
        public async Task<IActionResult> taiThongKe([FromBody] modelthongke item)
        {
           
           
            //  var auth = new FirebaseAuthProvider(new FirebaseConfig123(ApiKey));
            // var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            //var ok = await FirebaseAuth123
            //   UserRecord user = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserAsync("6arN66zteAM2PEeDlACJAjSiRD42");
            //UserRecordArgs args = new UserRecordArgs()
            //{
            //    Uid = "6arN66zteAM2PEeDlACJAjSiRD42",
            //    Disabled = false,
            //};
            //  UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(args);
            var firebase = new FirebaseClient(Key);
            //var nguoidung = await firebase
            //      .Child("csdlmoi")
            //      .Child("xuhuong")
            //      .Child("2021")
            //         .Child("9")
            //            .Child("16")
            //      .OnceAsync<baihatModel>();
            ////var dspnguoidung = LayBangDanhSachPhatNguoiDung();
            //xuhuongModel ng = new xuhuongModel();
            //foreach (var item1 in nguoidung)
            //{


            //    //ng.daxacthuc = item1.daxacthuc;
                
            //    //ng.email = item1.email;
            //    //ng.gioitinh = item1.gioitinh;
            //    //ng.hansudungvip = item1.hansudungvip;
            //    //ng.hinhdaidien = item1.hinhdaidien;
            //    ng.luotnghe = item1.Object.luotnghe;
            //    ng.baihat_id = item1.Object.id;              
            //    ng.nguoidung_id = item1.Object.nguoidung_id;

            //    //ng.matkhau = item1.matkhau;
            //    //ng.mota = item1.mota;
            //    //ng.ngaysinh = item1.ngaysinh;
            //    //ng.online = item1.online;
            //    //ng.uid = item1.uid;
            //    //ng.vip = item1.vip;
            //    //ng.thoigian = item1.thoigian;
            //    //ng.vohieuhoa = 0;
            //    var dino = await firebase
            //      .Child("csdlmoi")
            //      .Child("xuhuong")
            //      .Child("2021")
            //         .Child("9")
            //            .Child("16")    
            //     .PostAsync(ng)
            //     ;

            //    string kk = dino.Key.ToString();
            //    ng.id = kk;
            //    await firebase
            //    .Child("csdlmoi")
            //      .Child("xuhuong")
            //      .Child("2021")
            //         .Child("9")
            //            .Child("16")
            //       .Child(kk)
            //       .PutAsync(ng);
            //    await firebase
            //      .Child("csdlmoi")
            //      .Child("xuhuong")
            //      .Child("2021")
            //         .Child("9")
            //            .Child("16")
            //            .Child(ng.baihat_id)
            //      .DeleteAsync();

            //    //await firebase
            //    //   .Child("2021")
            //    //     .Child("9")
            //    //        .Child("15")                   
            //    //    .PutAsync(ng);
            //    ng = new xuhuongModel();
            //}
            //var nguoidung = LayBangNguoiDung();
            //nguoidungcustommodel ng = new nguoidungcustommodel();
            //foreach (var item1 in nguoidung)
            //{


            //    ng.daxacthuc = item1.daxacthuc;
            //    ng.daxoa = 0;
            //    ng.email = item1.email;
            //    ng.gioitinh = item1.gioitinh;
            //    ng.hansudungvip = item1.hansudungvip;
            //    ng.hinhdaidien = item1.hinhdaidien;
            //    ng.hoten = item1.hoten;
            //    ng.matkhau = item1.matkhau;
            //    ng.mota = item1.mota;
            //    ng.ngaysinh = item1.ngaysinh;
            //    ng.online = item1.online;
            //    ng.uid = item1.uid;
            //    ng.vip = item1.vip;
            //    ng.thoigian = item1.thoigian;
            //    ng.vohieuhoa = 0;
            //    await firebase
            //        .Child("csdlmoi")
            //        .Child("nguoidung")
            //        .Child(ng.uid)
            //        .PutAsync(ng);

            //    ng = new nguoidungcustommodel();
            //}
            //var nguoidung = LayBangBaiHat();
            //baihatcustommodel ng = new baihatcustommodel();
            //foreach (var item1 in nguoidung)
            //{
            //    ng.id = item1.id;
            //    ng.nguoidung_id = item1.nguoidung_id;
            //    ng.tenbaihat = item1.tenbaihat;
            //    ng.casi = item1.casi;
            //    ng.chedo = item1.chedo;
            //    ng.chude_id = item1.chude_id;
            //    ng.danhsachphattheloai_id = item1.danhsachphattheloai_id;
            //    ng.daxoa = item1.daxoa;
            //    ng.link = item1.link;
            //    ng.linkhinhanh = item1.linkhinhanh;
            //    ng.loibaihat = item1.loibaihat;
            //    ng.luotnghe = item1.luotnghe;
            //    ng.luottaixuong = item1.luottaixuong;
            //    ng.luotthich = item1.luotthich;
            //    ng.mota = item1.mota;
            //    ng.quangcao = item1.quangcao;
            //    ng.theloai_id = item1.theloai_id;
            //    ng.thoigian = item1.thoigian;
            //    ng.thoigianxoa = item1.thoigianxoa;
            //    ng.thoiluongbaihat = item1.thoiluongbaihat;
            //    ng.vohieuhoa = 0;



            //    await firebase
            //        .Child("csdlmoi")
            //        .Child("baihat")
            //        .Child(ng.nguoidung_id)
            //        .Child(ng.id)
            //        .PutAsync(ng);

            //    ng = new baihatcustommodel();
            //}
            var hoadonthanhtoan1 = LayBangHoaDonThanhToan();
         //   var nguoidung = LayBangNguoiDung();
          //  var hoadon = from hdtt in hoadonthanhtoan1
                       //  join nd in nguoidung on hdtt.nguoidung_id equals nd.uid
                      //   select new { hdtt, email = nd.email };
           // var hoadonthanhtoan = (from hdtt in hoadon
                                //   select hdtt).ToList();
            //var hdtt = (from bh in hoadonthanhtoan
            //            where DateTime.Parse(bh.thoigian.ToString("MM-yyyy")) == DateTime.Parse(DateTime.Now.ToString("MM/yyyy"))
            //            select bh).ToList();




            try
            {
                    DateTime ngaybatdau = DateTime.Parse(item.ngaybatdau.ToString("dd-MM-yyyy"));
                    DateTime ngayketthuc = DateTime.Parse(item.ngayketthuc.ToString("dd-MM-yyyy"));
                    var hdttd1 = (from hoadon1 in hoadonthanhtoan1
                                 where DateTime.Parse(hoadon1.thoigian.ToString("dd-MM-yyyy")) >= ngaybatdau
                                       && DateTime.Parse(hoadon1.thoigian.ToString("dd-MM-yyyy")) <= ngayketthuc
                                  select hoadon1).ToList();
                DateTime theothang = DateTime.Parse(item.theothang.ToString("MM-yyyy"));
                    var hdttd2 = (from hoadon1 in hoadonthanhtoan1
                                 where DateTime.Parse(hoadon1.thoigian.ToString("MM-yyyy")) == theothang
                                 select hoadon1).ToList();
                  
                    int theonam = item.theonam.Year;
                    var hdttd3 = (from hoadon1 in hoadonthanhtoan1
                                 where hoadon1.thoigian.Year == theonam
                                 select hoadon1).ToList();
                hoadon data = new hoadon();
                data.ngay = hdttd1;
                data.thang = hdttd2;
                data.nam = hdttd3;
                return Json(data);
            } 
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        [HttpPost]
        public async Task<IActionResult> taiDanhSachSoBHTheoNgay([FromBody] modelthongke item)
        {

            var baihat = LayBangBaiHat();                   
            try
            { 
                //DateTime theothang = DateTime.Parse(item.theothang.ToString("MM-yyyy"));
                //var databh = (from bh in baihat
                //                 where DateTime.Parse(bh.thoigian.ToString("MM-yyyy")) == theothang && bh.nguoidung_id != "Admin"
                //              select bh).ToList();
                var databh = (from bh in baihat
                              where bh.thoigian.Month == item.theothang.Month
                              select bh).ToList();
           
                var dataadmin = (from bh in databh
                              where bh.nguoidung_id != "admin"
                              select bh).ToList();
                var datand = (from bh in databh
                                where  bh.nguoidung_id == "admin"
                                select bh).ToList();
                List<soBaiHatTheoNgay> data = new List<soBaiHatTheoNgay>();
                soBaiHatTheoNgay baihattheongay = new soBaiHatTheoNgay();  
                if (item.theothang.Month == 1
                    || item.theothang.Month == 3
                    || item.theothang.Month == 5
                    || item.theothang.Month == 7
                    || item.theothang.Month == 8
                    || item.theothang.Month == 10
                        || item.theothang.Month == 12
                   )
                {
                   
                    for (int i =1; i < 32; i++)
                    {
                        baihattheongay.ngaythu = i;
                        baihattheongay.sobaihat = laySoBaiHatTheoNgay(databh, i);
                        baihattheongay.sobaihatadmin = laySoBaiHatTheoNgayadmin(databh, i);
                        data.Add(baihattheongay);
                        baihattheongay = new soBaiHatTheoNgay();
                    }
                    return Json(data);
                }
                else if (item.theothang.Month == 4
                    || item.theothang.Month == 6                
                    || item.theothang.Month == 9
                    || item.theothang.Month == 11)
                {
                    for (int i = 1; i < 31; i++)
                    {
                        baihattheongay.ngaythu = i;
                        baihattheongay.sobaihat = laySoBaiHatTheoNgay(databh, i);
                        baihattheongay.sobaihatadmin = laySoBaiHatTheoNgayadmin(databh, i);
                        data.Add(baihattheongay);
                        baihattheongay = new soBaiHatTheoNgay();
                    }
                    return Json(data);
                }
                else
                {
                    for (int i = 1; i < 30; i++)
                    {
                        baihattheongay.ngaythu = i;
                        baihattheongay.sobaihat = laySoBaiHatTheoNgay(databh, i);
                        baihattheongay.sobaihatadmin = laySoBaiHatTheoNgayadmin(databh, i);
                        data.Add(baihattheongay);
                        baihattheongay = new soBaiHatTheoNgay();
                    }
                    return Json(data);
                }

                    return Json(data);
               
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        public int laySoBaiHatTheoNgay(List<baihatModel> list , int ngaythu)
        {           
            try
            {
                DateTime dateGregorian = new DateTime(1, 1, ngaythu);
                var databh = (from bh in list
                              where bh.thoigian.Day == dateGregorian.Day && bh.nguoidung_id != "admin"
                              select bh).ToList();
                return databh.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int laySoBaiHatTheoNgayadmin(List<baihatModel> list, int ngaythu)
        {
            try
            {
                DateTime dateGregorian11 = new DateTime(ngaythu);
                DateTime dateGregorian = new DateTime(1,1, ngaythu);
                var databh = (from bh in list
                              where bh.thoigian.Day == dateGregorian.Day && bh.nguoidung_id == "admin"
                              select bh).ToList();
                return databh.Count;
            }
            catch (Exception ex)
            {
                return 0;
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

                    list.Add(data);
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

                    list.Add(data);
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

                    list.Add(data);
                    return list;
                }
            }
            catch
            {
                return null;
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

                    list.Add(data);
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

                    list.Add(data);
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

        [HttpPost]
        public async Task<IActionResult> taiTaiKhoanQuanTri()
        {

            var firebase = new FirebaseClient(Key);

            var dino = await firebase
                .Child("csdlmoi")
                .Child("taikhoanquantri")
                .OnceAsync<taikhoanquantriModel>();
            var data = from tkqt in dino

                       select tkqt.Object;

            var ok = User.Identity as ClaimsIdentity;
            var o = ok.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            return Json(data);       
        }

    }
}

