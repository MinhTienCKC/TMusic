using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TFourMusic.Models
{
    public class baocaonguoidungModel
    {
        [Key]
        public string id { get; set; }
        public string noidung { get; set; }
        public string nguoidung_id { get; set; }
        public string nguoidung_baocao_id { get; set; }
        public string motavande { get; set; }
        public DateTime thoigian { get; set; }
        public int trangthai { get; set; }
        public DateTime ngayxuly { get; set; }
    }
}
