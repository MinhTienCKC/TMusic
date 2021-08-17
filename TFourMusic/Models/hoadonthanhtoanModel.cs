using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class hoadonthanhtoanModel
    {
            [Key]
       
            public string id { get; set; }
            public string hoadonthanhtoan_id { get; set; }         
            public string mota { get; set; }
            public int giatien { get; set; }
            public string nguoidung_id { get; set; }
            public DateTime thoigian { get; set; }
            public int trangthai { get; set; }
            public string loaigoivip_id { get; set; }
            public string phuongthucthanhtoan { get; set; }

    }


}