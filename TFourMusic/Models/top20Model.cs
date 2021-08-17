using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{
    public class top20Model
    {
        [Key]
        public string id { get; set; }
        public string tentop20 { get; set; }
        public string theloai_id { get; set; } 
        public string mota { get; set; }        
        public string danhsachphattheloai_id { get; set; }
     
        public string linkhinhanh { get; set; }
       
        public int daxoa { get; set; }

    }


}