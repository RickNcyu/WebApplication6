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
        [DisplayName("款項編號")]
        public int Id { get; set; }

        [DisplayName("電子發票號碼")]
        public string Number { get; set; }

        [DisplayName("站名")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Gas { get; set; }


        [DisplayName("款項")]
        public string Content { get; set; }

        [DisplayName("付款")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Pay { get; set; }

        [DisplayName("月份")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Date { get; set; }



    }
}