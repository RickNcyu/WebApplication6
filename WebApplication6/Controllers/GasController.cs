using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;
using WebApplication6.Services;
using WebApplication6.ViewModels;

namespace WebApplication6.Controllers
{
    [Authorize]
    public class GasController : Controller
    {
        private readonly YDService GasSer = new YDService();
        // GET: Gas
        
        public ActionResult Index()
        {

            GasViewModel DATA = new GasViewModel();
            //System.Diagnostics.Debug.WriteLine("asd");
            DATA.DataList = GasSer.GetDataList();
            
                foreach (var item in DATA.DataList)
                {
                    System.Diagnostics.Debug.WriteLine(DATA.DataList[0].Gas);
                }
                System.Diagnostics.Debug.WriteLine("yes");
           
                return View(DATA);
        }
        public  ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "Gas,Content")]YD Data)
        {
            GasSer.GasInsert(Data);
            return RedirectToAction("Index");
        }
    }
}