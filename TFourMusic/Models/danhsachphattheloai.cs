using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{
    public class danhsachphattheloaiModel
    {
        [Key]
        public string id { get; set; }
        public string tendanhsachphattheloai { get; set; }
        public string theloai_id { get; set; } 
        public string mota { get; set; }
        //public string theloai_id { get; set; }
        public string linkhinhanh { get; set; }
       
    }


}