using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class yeuthichtop20Model
    {
        [Key]
        public string id { get; set; }
        public string  nguoidung_id { get; set; }
        public string top20_id { get; set; }
        public DateTime thoigian { get; set; }
    }


}