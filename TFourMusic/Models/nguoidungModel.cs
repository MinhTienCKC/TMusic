using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TFourMusic.Models
{ 
    public class nguoidungModel
    {
        [Key]
        public string id { get; set; }
        public int  daxacthuc { get; set; }
        public string matkhau { get; set; }
        public string email { get; set; }
        public string hoten { get; set; }
       
        public string mota { get; set; }
        public string ngaysinh { get; set; }
      
        public string hinhdaidien { get; set; }
    
        public string gioitinh { get; set; }
        public DateTime thoigian { get; set; }
        public int online { get; set; }

        public int vip { get; set; }

        public DateTime hansudungvip { get; set; }
        public string uid { get; set; }
        public int daxoa { get; set; }
        public int vohieuhoa { get; set; }

    }


}