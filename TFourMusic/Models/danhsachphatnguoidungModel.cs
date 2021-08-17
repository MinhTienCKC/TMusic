using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class danhsachphatnguoidungModel
    {
        
        public string id { get; set; }
        public string  nguoidung_id { get; set; }
        public string tendanhsachphat { get; set; }
        public int chedo { get; set; }
        public DateTime thoigian { get; set; }

        public string linkhinhanh { get; set; }

    }


}