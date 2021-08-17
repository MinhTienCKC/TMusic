using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class quantriModel
    {
        [Key]
        public string id { get; set; }
        public string taikhoan { get; set; }
        public string matkhau { get; set; }
        

        

    }


}