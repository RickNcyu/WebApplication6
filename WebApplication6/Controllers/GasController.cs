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
        public ActionResult Index(string Search)
        {

            


            GasViewModel DATA = new GasViewModel();
            DATA.Search = Search;

            var categories = GasSer.SearchGas();
            //前面的Name為資料表中選單顯示的欄位 :Name=>中華 顯示的欄位
            //後面的Name為傳入的值 Name=>中華 傳入的欄位
            SelectList selectList = new SelectList(categories,"Name","Name");
            
            ViewBag.Test = selectList;
            
            System.Diagnostics.Debug.WriteLine(Search);
            DATA.DataList = GasSer.GetDataList(DATA.Search);

            /*foreach (var item in DATA.DataList)
            {
                System.Diagnostics.Debug.WriteLine(DATA.DataList[0].Gas);
            }*/

            //倒序
            var List = from s in DATA.DataList select s ;
            List = List.OrderByDescending(s=>s.Number);
            DATA.DataList = List.ToList();
            return View(DATA);
        }
        public ActionResult Create()
        {
           
            return PartialView();
        }
        
        [HttpPost]
        public ActionResult Create(GasViewModel Data)
        {
            GasSer.GasInsert(Data.Num,Data.Date);

            /*System.Diagnostics.Debug.WriteLine(Data.Num[1]);
            string temp=Data.Num.Substring(2);

            System.Diagnostics.Debug.WriteLine(temp);*/

            var categories = GasSer.SearchGas();
            SelectList selectList = new SelectList(categories, "Id", "Gas");
            ViewBag.Test = selectList;
            //ViewBag.Message = "gg";
            return RedirectToAction("Index");
            
        }
        public ActionResult Delete(int Id)
        {
            GasSer.DeleteGas(Id);
            return RedirectToAction("Index");
        }
    }
}