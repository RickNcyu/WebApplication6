using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication6.Models;

namespace WebApplication6.ViewModels
{
    public class GasViewModel
    {
        public List<YD> DataList { get; set; }
        [DisplayName("搜尋:")]
        public string Search { get; set; }
        [DisplayName("電子發票號碼")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Num { get; set; }
        [DisplayName("月份")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Date { get; set; }

    }
}