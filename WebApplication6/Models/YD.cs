using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class YD
    {
        [DisplayName("紀錄順序")]
        public int Id { get; set; }

        [DisplayName("站名")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Gas { get; set; }


        [DisplayName("紀錄")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Content { get; set; }

        [DisplayName("付款紀錄")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Pay { get; set; }



    }
}