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
    public class ThongKeController : Controller
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
        public ThongKeController(IHostingEnvironment env)
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
        [HttpPost]
        public async Task<IActionResult> taiBangBaiHat()
        {
            var firebase = new FirebaseClient(Key);       
            var baihat = await firebase
                 .Child("baihat")
                 .OnceAsync<baihatModel>();
            var data = (from bh in baihat
                       where bh.Object.daxoa == 0
                       select bh.Object).ToList();
            var databinhluancha = await firebase
                 .Child("text")
                 .Child("binhluancha")
                 .Child("idbaihat1")
                 .OnceAsync<binhluanchaModel>();
            var binhluancha = (from bl in databinhluancha
                        select bl.Object).ToList();
            var databinhluancon = await firebase
                .Child("text")
                  .Child("binhluancon")
                 .Child("idbaihat1")
                 .OnceAsync<binhluanconModel>();
            var binhluancon = (from bl in databinhluancon
                               select bl.Object).ToList();
            var datanguoidung = await firebase
                .Child("nguoidung")
                 .OnceAsync<nguoidungModel>();
            var nguoidung = (from ng in datanguoidung
                             select ng.Object).ToList();
            var blcon = (from blc in binhluancon
                        join ng in nguoidung on blc.nguoidung_id equals ng.uid
                        select new
                        {
                          binhluancon =  blc,
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
                                            select blc1 ).ToList()
        }).ToList();
          
          

            return Json(data);
        }
        public class modelthongke { 
            public DateTime ngaybatdau { get; set; }
            public DateTime ngayketthuc { get; set; }
            public DateTime theothang { get; set; }
            public DateTime theonam { get; set; }
            public string hientimkiem { get; set; }

        }
        [HttpPost]
        public async Task<IActionResult> taiThongKe([FromBody] modelthongke item)
        {
            var hoadonthanhtoan1 = LayBangHoaDonThanhToan();
            var nguoidung = LayBangNguoiDung();
            var hoadon = from hdtt in hoadonthanhtoan1
                         join nd in nguoidung on hdtt.nguoidung_id equals nd.uid
                         select new { hdtt, email = nd.email };
            var hoadonthanhtoan = (from hdtt in hoadon
                                   select hdtt).ToList();
            //var hdtt = (from bh in hoadonthanhtoan
            //            where DateTime.Parse(bh.thoigian.ToString("MM-yyyy")) == DateTime.Parse(DateTime.Now.ToString("MM/yyyy"))
            //            select bh).ToList();
            //jnnj
            try
            {
                if (item.hientimkiem == "theongay")
                {
                    //DateTime ngaybatdau = DateTime.Parse(item.ngaybatdau.ToString("dd-MM-yyyy"));
                    //DateTime ngayketthuc = DateTime.Parse(item.ngayketthuc.ToString("dd-MM-yyyy"));
                    //var hdttd = (from hoadon1 in hoadonthanhtoan
                    //            where DateTime.Parse(hoadon1.hdtt.thoigian.ToString("dd-MM-yyyy")) >= ngaybatdau 
                    //                  && DateTime.Parse(hoadon1.hdtt.thoigian.ToString("dd-MM-yyyy")) <= ngayketthuc
                    //             select hoadon1).ToList();
                    var hdttd = (from hoadon1 in hoadonthanhtoan
                                 where hoadon1.hdtt.thoigian.Date >= item.ngaybatdau.Date
                                    && hoadon1.hdtt.thoigian.Date <= item.ngayketthuc.Date
                                 select hoadon1).ToList();
                   

                    return Json(hdttd.OrderByDescending(x => x.hdtt.thoigian));
                }
                else if(item.hientimkiem == "theothang") 
                {
                    //DateTime theothang = DateTime.Parse(item.theothang.ToString("MM-yyyy"));
                    //var hdttd = (from hoadon1 in hoadonthanhtoan
                    //            where DateTime.Parse(hoadon1.hdtt.thoigian.ToString("MM-yyyy")) == theothang
                    //             select hoadon1).ToList();
                    int theothang_nam = item.theothang.Year;
                    int theothang_thang = item.theothang.Month;
                 
                    var hdttd = (from hoadon1 in hoadonthanhtoan
                                  where hoadon1.hdtt.thoigian.Year == theothang_nam && hoadon1.hdtt.thoigian.Month == theothang_thang
                                  select hoadon1).ToList();

                    return Json(hdttd.OrderByDescending(x => x.hdtt.thoigian));
                }
                else
                {
                    //int theonam = item.theonam.Year;
                    //var hdttd = (from hoadon1 in hoadonthanhtoan
                    //             where hoadon1.hdtt.thoigian.Year == theonam
                    //             select hoadon1).ToList();
                    int theonam = item.theonam.Year;
                    var hdttd = (from hoadon1 in hoadonthanhtoan
                                  where hoadon1.hdtt.thoigian.Year == theonam
                                  select hoadon1).ToList();
                    return Json(hdttd.OrderByDescending(x=>x.hdtt.thoigian));
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
           
            
        }
        public class doanhthu
        {
            public float tongtien { get; set; }
            public float tonghoadon { get; set; }

        }
        [HttpPost]
        public async Task<IActionResult> taiThongKeDoanhThu([FromBody] modelthongke item)
        {
            var hoadonthanhtoan1 = LayBangHoaDonThanhToan();
            var nguoidung = LayBangNguoiDung();
            var hoadon = from hdtt in hoadonthanhtoan1
                         join nd in nguoidung on hdtt.nguoidung_id equals nd.uid
                         select new { hdtt, email = nd.email };
            var hoadonthanhtoan = (from hdtt in hoadon
                                   select hdtt).ToList();
            //var hdtt = (from bh in hoadonthanhtoan
            //            where DateTime.Parse(bh.thoigian.ToString("MM-yyyy")) == DateTime.Parse(DateTime.Now.ToString("MM/yyyy"))
            //            select bh).ToList();
            try
            {
                if (item.hientimkiem == "theongay")
                {
                    DateTime ngaybatdau = DateTime.Parse(item.ngaybatdau.ToString("dd-MM-yyyy"));
                    DateTime ngayketthuc = DateTime.Parse(item.ngayketthuc.ToString("dd-MM-yyyy"));
                    var hdttd = (from hoadon1 in hoadonthanhtoan
                                 where DateTime.Parse(hoadon1.hdtt.thoigian.ToString("dd-MM-yyyy")) >= ngaybatdau
                                       && DateTime.Parse(hoadon1.hdtt.thoigian.ToString("dd-MM-yyyy")) <= ngayketthuc
                                 select hoadon1).ToList();

                    doanhthu dt = new doanhthu();
                   

                    


                    int ng = 0;
               
                    foreach (var n in hdttd)
                    {
                        ng += n.hdtt.giatien;
                    }
                    dt.tongtien = ng;
                    dt.tonghoadon = hdttd.Count();
                    return Json(dt);
                }
                else if (item.hientimkiem == "theothang")
                {
                    DateTime theothang = DateTime.Parse(item.theothang.ToString("MM-yyyy"));
                    var hdttd = (from hoadon1 in hoadonthanhtoan
                                 where DateTime.Parse(hoadon1.hdtt.thoigian.ToString("MM-yyyy")) == theothang
                                 select hoadon1).ToList();
                    int ng = 0;
                    doanhthu dt = new doanhthu();
                    foreach (var n in hdttd)
                    {
                        ng += n.hdtt.giatien;
                    }
                    dt.tongtien = ng;
                    dt.tonghoadon = hdttd.Count();
                    return Json(dt);
                  //  return Json(hdttd.OrderByDescending(x => x.hdtt.thoigian));
                }
                else
                {
                    int theonam = item.theonam.Year;
                    var hdttd = (from hoadon1 in hoadonthanhtoan
                                 where hoadon1.hdtt.thoigian.Year == theonam
                                 select hoadon1).ToList();

                    int ng = 0;
                    doanhthu dt = new doanhthu();
                    foreach (var n in hdttd)
                    {
                        ng += n.hdtt.giatien;
                    }
                    dt.tongtien = ng;
                    dt.tonghoadon = hdttd.Count();
                    return Json(dt);
                  //  return Json(hdttd.OrderByDescending(x => x.hdtt.thoigian));
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }


        }
        [HttpPost]
        public async Task<IActionResult> taiBangDanhSachPhatTheLoai()
        {

            var firebase = new FirebaseClient(Key);
            var danhsachphattheloai = await firebase
                 .Child("danhsachphattheloai")
                 .OnceAsync<danhsachphattheloaiModel>();
            var data = (from dsptl in danhsachphattheloai
                        select dsptl.Object ).ToList();
            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> taiBangNguoiDung()
        {
            var firebase = new FirebaseClient(Key);
            var nguoidung = await firebase
                 .Child("nguoidung")
                 .OnceAsync<nguoidungModel>();
            var data = (from nd in nguoidung
                        select nd.Object).ToList();
            return Json(data);
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


    }
}

