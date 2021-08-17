using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class theloaiModel
    {
        [Key]
        public string id { get; set; }
        public string  tentheloai { get; set; }
        public string linkhinhanh { get; set; }

    }


}