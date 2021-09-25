using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class baocaobaihatModel
    {
        [Key]
        public string id { get; set; }
        public string noidung { get; set; }
        public string motavande { get; set; }
        public string nguoidung_id { get; set; }
        public string nguoidung_baocao_id { get; set; }

        public string baihat_id { get; set; }

        public string baihat_baocao_id { get; set; }

        public DateTime thoigian { get; set; }

        public DateTime ngayxuly { get; set; }

        public int daxoa { get; set; }
        public int trangthai { get; set; }
    }


}