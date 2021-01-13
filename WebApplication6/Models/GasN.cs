using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class GasN
    {
        [DisplayName("編號")]
        public int Gid { get; set; }
        
        [DisplayName("站名(請勿重複)")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Name { get; set; }
    }
}