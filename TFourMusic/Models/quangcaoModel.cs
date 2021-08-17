using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class quangcaoModel
    {
        [Key]
        public string id { get; set; }
        public string tenquangcao { get; set; }
        public string noidung { get; set; }
        public DateTime thoigian { get; set; }
        public string linkhinhanh { get; set; }
        public string baihat_id { get; set; }



    }


}