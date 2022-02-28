using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using WebApplication6.Models;
using WebApplication6.Services;
using WebApplication6.ViewModels;
using Microsoft.AspNet.Identity;
namespace WebApplication6.Controllers
{
    [Authorize]
    public class GasController : Controller
    {
        private readonly YDService GasSer = new YDService();
        // GET: Gas
        //[Authorize(Roles= "fzp.fzp@msa.hinet.net")]

        //注意 SERACH 和SEARCHRADIO 名稱必須與view裡一樣



        
        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
        public ActionResult Index(string Search,string Searchradio)
        {
            System.Diagnostics.Debug.WriteLine("start");
            //System.Diagnostics.Debug.WriteLine(s);
            System.Diagnostics.Debug.WriteLine(Search);
            System.Diagnostics.Debug.WriteLine(Searchradio);
            System.Diagnostics.Debug.WriteLine("end");
            GasViewModel DATA = new GasViewModel();

            DATA.Search = Search;
            DATA.Searchradio = Searchradio;
            //System.Diagnostics.Debug.WriteLine(User.Identity.GetUserName());
           

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

        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
        public ActionResult Create()
        {

            return PartialView();
        }

        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
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

        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
        public ActionResult Delete(int Id)
        {
            GasSer.DeleteGas(Id);

            return RedirectToAction("Index");
        }

        //多載處理GET 另一種為HTTPPOST
        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
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

        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
        [HttpPost]
        public ActionResult Edit(YD updatedata)
        {
            // updatedata.Id=
            /*System.Diagnostics.Debug.WriteLine(Number);
            System.Diagnostics.Debug.WriteLine(updatedata.Number);*/

            GasSer.UpdateData(updatedata);
            return RedirectToAction("Index");
        }

        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
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
        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
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

        [Authorize(Users = "fzp.fzp@msa.hinet.net,arst8325729@gmail.com")]
        public ActionResult GNameDelete(string Name)
        {
            
            GasSer.DeleteGName(Name);

            return RedirectToAction("GName");
        }


        //高雄
        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        public ActionResult KaoIndex(string Search, string Searchradio)
        {

        
            GasViewModel DATA = new GasViewModel();

            DATA.Search = Search;
            DATA.Searchradio = Searchradio;
            //System.Diagnostics.Debug.WriteLine(Search);
            //System.Diagnostics.Debug.WriteLine(User.Identity.GetUserName());
            

            //var categories = GasSer.SearchGas();
            //前面的Name為資料表中選單顯示的欄位 :Name=>中華 顯示的欄位
            //後面的Name為傳入的值 Name=>中華 傳入的欄位
            //SelectList selectList = new SelectList(categories,"Name","Name");

            //ViewBag.Test = selectList;
            DATA.DataList = GasSer.KaoGetDataList(DATA.Search, DATA.Searchradio);

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

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        public ActionResult KaoCreate()
        {

            return PartialView();
        }

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult KaoCreate(GasViewModel Data)
        {

            if (!ModelState.IsValid)
            {
                return PartialView();
            }
            GasSer.KaoGasInsert(Data.Num, Data.Date);

            /*System.Diagnostics.Debug.WriteLine(Data.Num[1]);
            string temp=Data.Num.Substring(2);

            System.Diagnostics.Debug.WriteLine(temp);*/


            //ViewBag.Message = "gg";
            return RedirectToAction("KaoIndex");

        }

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        public ActionResult KaoDelete(int Id)
        {
            GasSer.KaoDeleteGas(Id);

            return RedirectToAction("KaoIndex");
        }

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        public ActionResult KaoEdit(string Number, string Gas, string Content, string Pay, int Cost, string Date)
        {
            System.Diagnostics.Debug.WriteLine(Gas);
            ViewBag.Number = Number;
            ViewBag.Date = Date;
            ViewBag.Content = Content;
            ViewBag.Cost = Cost;
            var categories = GasSer.KaoSearchGas();
            SelectList selectList = new SelectList(categories, "Name", "Name");
            //如果已經有站名修改時有預設值
            if (!string.IsNullOrEmpty(Gas))
            {
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

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        [HttpPost]
        public ActionResult KaoEdit(YD updatedata)
        {
            // updatedata.Id=
            /*System.Diagnostics.Debug.WriteLine(Number);
            System.Diagnostics.Debug.WriteLine(updatedata.Number);*/

            GasSer.KaoUpdateData(updatedata);
            return RedirectToAction("KaoIndex");
        }

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        public ActionResult KaoGName()
        {
            var List = GasSer.KaoSearchGas();
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

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KaoGName(GasN Data)
        {

            if (!ModelState.IsValid)
            {
                //System.Diagnostics.Debug.WriteLine("Yes");
                return RedirectToAction("KaoGName");

            }
            /*if (string.IsNullOrWhiteSpace(Data.Name))
            {
                
                return RedirectToAction("GName");

            }*/
            /*string check;
            check = Data.Name;
            System.Diagnostics.Debug.WriteLine("1"+check);*/
            if (!GasSer.KaoCheckName(Data.Name))
            {
                //System.Diagnostics.Debug.WriteLine("Wrong"); 
                TempData["Message"] = "重複新增";
                return RedirectToAction("KaoGName");
            }
            GasSer.KaoInsertName(Data);

            return RedirectToAction("KaoGName");
        }

        [Authorize(Users = "wttssh@yahoo.com.tw,arst8325729@gmail.com")]
        public ActionResult KaoGNameDelete(string Name)
        {

            GasSer.KaoDeleteGName(Name);

            return RedirectToAction("KaoGName");
        }
    }
}