using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class binhluanModel
    {
        [Key]
        public string id { get; set; }
        public string nguoidung_id { get; set; }
        public string baihat_id { get; set; }

        public string noidung { get; set; }

    }


}