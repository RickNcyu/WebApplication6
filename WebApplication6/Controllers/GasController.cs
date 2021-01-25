using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
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
        public ActionResult Index(string Search,string Searchradio)
        {
            GasViewModel DATA = new GasViewModel();

            DATA.Search = Search;
            DATA.Searchradio = Searchradio;

            
            
            //var categories = GasSer.SearchGas();
            //前面的Name為資料表中選單顯示的欄位 :Name=>中華 顯示的欄位
            //後面的Name為傳入的值 Name=>中華 傳入的欄位
            //SelectList selectList = new SelectList(categories,"Name","Name");

            //ViewBag.Test = selectList;
            DATA.DataList = GasSer.GetDataList(DATA.Search,DATA.Searchradio);

            //TotalCost
            int Cost = 0;
            foreach (var item in DATA.DataList)
            {
                //System.Diagnostics.Debug.WriteLine(item.Cost);
                Cost = Cost + item.Cost;
            }

            ViewBag.Cost = Cost;
            //System.Diagnostics.Debug.WriteLine(Cost);
            /*foreach (var item in DATA.DataList)
            {
                System.Diagnostics.Debug.WriteLine(DATA.DataList[0].Gas);
            }*/

            //倒序
            /*var List = from s in DATA.DataList select s ;
            List = List.OrderByDescending(s=>s.Number);
            DATA.DataList = List.ToList();*/
            return View(DATA);
        }
        public ActionResult Create()
        {

            return PartialView();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(GasViewModel Data)
        {

            if (!ModelState.IsValid)
            {
                return PartialView();
            }
            GasSer.GasInsert(Data.Num, Data.Date);

            /*System.Diagnostics.Debug.WriteLine(Data.Num[1]);
            string temp=Data.Num.Substring(2);

            System.Diagnostics.Debug.WriteLine(temp);*/


            //ViewBag.Message = "gg";
            return RedirectToAction("Index");

        }
        public ActionResult Delete(int Id)
        {
            GasSer.DeleteGas(Id);

            return RedirectToAction("Index");
        }
        //多載處理GET 另一種為HTTPPOST
        public ActionResult Edit(string Number, string Gas, string Content, string Pay, int Cost, string Date)
        {

            ViewBag.Number = Number;
            ViewBag.Date = Date;
            ViewBag.Content = Content;
            ViewBag.Cost = Cost;
            var categories = GasSer.SearchGas();
            SelectList selectList = new SelectList(categories, "Name", "Name");
            //如果已經有站名修改時有預設值
            if (!string.IsNullOrEmpty(Gas)) {
                selectList.Where(q => q.Value == Gas).First().Selected = true;
            }


            ViewBag.Gas = selectList;

            //?????????好像如果把model.pay的值(假設為已付款)傳進來後 HTML中的SELECETLISTITEM若抓取到符合對應的值會自動變為預設
            //!!!!!!!!!!! 懂了 假設NEW SELECETELISTITEM新增的TEXT 與 傳進來的string Pay 相符(相等)則SELECETLISTITEM會自動將預設變更為Pay
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "已付款" });
            list.Add(new SelectListItem { Text = "未付款" });

            //System.Diagnostics.Debug.WriteLine(Pay);



            ViewBag.List = list;



            //System.Diagnostics.Debug.WriteLine(Ifpay);


            //return RedirectToAction("Index");

            return View();
        }
        [HttpPost]
        public ActionResult Edit(YD updatedata)
        {
            // updatedata.Id=
            /*System.Diagnostics.Debug.WriteLine(Number);
            System.Diagnostics.Debug.WriteLine(updatedata.Number);*/

            GasSer.UpdateData(updatedata);
            return RedirectToAction("Index");
        }
        public ActionResult GName()
        {
            var List = GasSer.SearchGas();
            //System.Diagnostics.Debug.WriteLine(List[0].Name);
            ViewBag.Data = List;
            if (TempData["Message"] != null)
            {
                ViewBag.Message = "重複新增";
                TempData.Remove("Message");
            }
            //ViewBag.message = "重複新增";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GName(GasN Data)
        {
            
            if (!ModelState.IsValid)
            {
                //System.Diagnostics.Debug.WriteLine("Yes");
                return RedirectToAction("GName");
                
            }
            /*if (string.IsNullOrWhiteSpace(Data.Name))
            {
                
                return RedirectToAction("GName");

            }*/
            /*string check;
            check = Data.Name;
            System.Diagnostics.Debug.WriteLine("1"+check);*/
            if (!GasSer.CheckName(Data.Name))
            {
                //System.Diagnostics.Debug.WriteLine("Wrong"); 
                TempData["Message"] = "重複新增";
                return RedirectToAction("GName"); 
            }
            GasSer.InsertName(Data);
            
            return RedirectToAction("GName");
        }
        public ActionResult GNameDelete(string Name)
        {
            
            GasSer.DeleteGName(Name);

            return RedirectToAction("GName");
        }
       
    }
}