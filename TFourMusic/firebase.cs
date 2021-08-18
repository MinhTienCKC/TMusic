using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFourMusic.Models;

namespace TFourMusic
{
    public static class firebase
    {
        static IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "vHcXcNH4jYpiScpS8Fw3mSJhUj6lX3zp4kgpIM7T",
            BasePath = "https:tfourmusic-1e3ff-default-rtdb.firebaseio.com/"
        };
        static IFirebaseClient client;

        // static readonly IFirebaseClient client;
        //[Obsolete]
        //private static IHostingEnvironment _env;

        //[Obsolete]


        //private static readonly string ApiKey = "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q";
        //private static string Bucket = "musictt-9aa5f.appspot.com";
        //private static FirebaseClient client;
        //private static readonly string Bucket = "tfourmusic-1e3ff.appspot.com";
        //private static readonly string AuthEmail = "dang60780@gmail.com";
        //private static readonly string AuthPassword = "0362111719@TTai";
        //private static readonly string Key = " https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";
        //17-08 Đã sữa CSDL mới
        public static List<chitietdanhsachphatnguoidungModel> LayBangChiTietDanhSachPhatNguoiDung(string uid = null)
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
        public static List<baihatModel> LayBangBaiHat(string uid = null)
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

        //17-08 Đã sữa CSDL mới
        public static List<chudeModel> LayBangChuDe(string idchude = null)
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
                   // client = new FireSharp.FirebaseClient(config);
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
        public static List<top20Model> LayBangTop20(string idtheloai = null)
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
        public static List<hoadonthanhtoanModel> LayBangHoaDonThanhToan(string uid = null)
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
                //client = new FireSharp.FirebaseClient(config);
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

        public static List<theodoiModel> LayBangTheoDoi(string uid = null)
        {
            if (uid == null)
            {
                //client = new FireSharp.FirebaseClient(config);
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
        public static List<danhsachphatnguoidungModel> LayBangDanhSachPhatNguoiDung(string uid = null)
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
        public static List<danhsachphattheloaiModel> LayBangDanhSachPhatTheLoai(string idtheloai = null)
        {
            if (idtheloai == null)
            {
                //client = new FireSharp.FirebaseClient(config);
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
                //client = new FireSharp.FirebaseClient(config);
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
        public static List<dataixuongModel> LayBangDaTaiXuong(string uid = null)

        {
            if (uid == null)
            {
                //client = new FireSharp.FirebaseClient(config);
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
                //client = new FireSharp.FirebaseClient(config);
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
        public static List<goivipModel> LayBangGoiVip(string idgoivip = null)
        {
            try
            {
                if (idgoivip == null)
                {
                    //client = new FireSharp.FirebaseClient(config);
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
                  //  client = new FireSharp.FirebaseClient(config);
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
        public static List<nguoidungModel> LayBangNguoiDung(string uid = null)
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
                    //client = new FireSharp.FirebaseClient(config);
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
        public static List<theloaiModel> LayBangTheLoai(string idtheloai = null)
        {
            try
            {
                if (idtheloai == null)
                {
                  //  client = new FireSharp.FirebaseClient(config);
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
        public static List<quangcaoModel> LayBangQuangCao(string idquangcao = null)
        {
            try
            {
                if (idquangcao == null)
                {
                    //client = new FireSharp.FirebaseClient(config);
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
        public static List<yeuthichModel> LayBangYeuThichBaiHat(string uid = null)
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
        public static List<yeuthichdanhsachphatnguoidung> layBangYeuThichDanhSachPhatNguoiDung(string uid = null)
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
              //  client = new FireSharp.FirebaseClient(config);
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
        public static List<yeuthichdanhsachphattheloaiModel> LayBangYeuThichDSPTheLoai(string uid = null)
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
        public static List<yeuthichtop20Model> LayBangYeuThichTop20(string uid = null)
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
        public static List<chitietdanhsachphattheloaiModel> LayBangChiTietDanhSachPhatTheLoai(string iddsptheloai = null)
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
    }
}
