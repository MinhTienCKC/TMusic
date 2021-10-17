using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class taikhoanquantriModel
    {
        [Key]
        public string id { get; set; }
        public string  taikhoan { get; set; }
        public string matkhau { get; set; }
        public int phanquyen { get; set; } // 0 là nhân viên 1 là admin
        public int vohieuhoa { get; set; }
        public DateTime thoigian { get; set; }

    }


}