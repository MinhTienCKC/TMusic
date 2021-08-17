using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class theodoiModel
    {
        [Key]
        public string id { get; set; }
        public string  nguoidung_id { get; set; }
        public string nguoidung_theodoi_id { get; set; }
        public DateTime thoigian { get; set; }
    }


}