using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class baihatModel
    {
        
        public string id { get; set; }
        public string nguoidung_id { get; set; }
        public string  tenbaihat { get; set; }
        public string mota { get; set; }
        public int luottaixuong { get; set; }
        public DateTime thoigian { get; set; }
        public int chedo { get; set; }
        public int luotthich { get; set; }
        public string casi { get; set; }
        public string  loibaihat { get; set; }
        public int luotnghe { get; set; }
        public string theloai_id { get; set; }
        public string chude_id { get; set; }
        public string danhsachphattheloai_id { get; set; }
        public string quangcao { get; set; }
        public string thoiluongbaihat { get; set; }
        public string link { get; set; }
        public string linkhinhanh { get; set; }
        public int daxoa { get; set; }
        public DateTime thoigianxoa { get; set; }
        // THÊM TRƯỜNG QUẢNG CÁO

    }


}