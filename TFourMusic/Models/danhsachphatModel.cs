using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class danhsachphatModel
    {
        
        public string id { get; set; }
        public string tendanhsachphat { get; set; }
        public string mota { get; set; }
        public string linkhinhanh { get; set; }
        public DateTime thoigian { get; set; }

    }


}