using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class bangtinnguoidungModel
    {
        [Key]
        public string id { get; set; }
        public string  nguoidung_id { get; set; }
        public string linkhinhanh { get; set; }
        public string noidung { get; set; }
        public int luotthich { get; set; }
        public DateTime thoigian { get; set; }

    }


}