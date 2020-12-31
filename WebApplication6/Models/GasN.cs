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

        [DisplayName("站名")]
        public string Name { get; set; }
    }
}