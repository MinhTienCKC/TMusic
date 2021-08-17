using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{
    public class goivipModel
    {
        [Key]

        public string id { get; set; }
        public string tengoivip { get; set; }
        public int giatiengoc { get; set; }
        public int giatiengiamgia { get; set; }
        public int sothang { get; set; }
        public DateTime thoigian { get; set; }
        public int trangthai { get; set; }
        public string linkhinhanh { get; set; }
    }


}