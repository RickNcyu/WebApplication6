using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public class YDService
    {
        private readonly static string cnstr = ConfigurationManager.ConnectionStrings["TEST"].ConnectionString;
        private readonly SqlConnection conn = new SqlConnection(cnstr);
        public List<YD> GetDataList(string Search,string Searchradio)
        {
            
            
            List<YD> DataList = new List<YD>();
            string SQL = string.Empty;
            //||!string.IsNullOrWhiteSpace(Searchradio) OR  Pay LIKE '%{Searchradio}%'
            //有字有按鈕
            if (!string.IsNullOrWhiteSpace(Search)  &&  !string.IsNullOrWhiteSpace(Searchradio) )
            {
               
                SQL = $@"SELECT * FROM GasList WHERE Pay LIKE '%{Searchradio}%' and (Gas LIKE '%{Search}%' OR Date LIKE '%{Search}%');";
            }
            //有字沒按鈕
            else if(!string.IsNullOrWhiteSpace(Search) && string.IsNullOrWhiteSpace(Searchradio))
            {
                SQL = $@"SELECT * FROM GasList WHERE Gas LIKE '%{Search}%' OR Date LIKE '%{Search}%';";
            }
            //有按鈕沒字
            else if(string.IsNullOrWhiteSpace(Search) && !string.IsNullOrWhiteSpace(Searchradio))
            {
                SQL = $@"SELECT * FROM GasList WHERE Pay LIKE '%{Searchradio}%';";
            }
            else
            {
                DateTime date = DateTime.Today;
                var TaiwanCalendar = new System.Globalization.TaiwanCalendar();
                var Year = TaiwanCalendar.GetYear(date);
                var Month = TaiwanCalendar.GetMonth(date);
                if (Month % 2 == 1)
                {
                    /*if (Month == 1)
                    {
                        Year--;
                        Month = 12;
                    }
                    else
                    {
                        Month--;
                    }*/
                    Month++;
                    
                }
                string datetime = Year.ToString() + Month.ToString("00");
                SQL = $@" SELECT * FROM GasList WHERE Date = '{datetime}';";
            }
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    YD Data = new YD();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    Data.Number = dr["Number"].ToString();
                    Data.Gas = dr["Gas"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.Pay = dr["Pay"].ToString();
                    Data.Cost = Convert.ToInt32(dr["Cost"]);
                    Data.Date = dr["Date"].ToString();
                    //System.Diagnostics.Debug.WriteLine(Data.Gas);

                    DataList.Add(Data);
                }
                
                
                    
            }
            catch(Exception e) 
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        //create 輸入Num(發票號碼)展開50筆
        public void GasInsert(string Num,string Date)
        {

            
            try
            {

                
                string Head=Num.Substring(0,2),temp = Num.Substring(2),Ans="";
                long count = Convert.ToInt64(temp);
                conn.Open();
                for (int i = 0; i < 50; i++)
                {   

                    //temp = Head + count.ToString();
                    Ans = Head+count.ToString();
                    
                    string SQL = $@"INSERT INTO GasList(Number,Date) VALUES ('{Ans}','{Date}') ; ";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.ExecuteNonQuery();
                    count++;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            
        }
        public void DeleteGas(int Id)
        {
            string SQL = $@"DELETE FROM GasList WHERE Id={Id};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public List<GasN> SearchGas()
        {
            List<GasN> DataList = new List<GasN>();
            string SQL= $@"SELECT * FROM GasName order by Gid;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    GasN Data = new GasN();
                    Data.Gid = Convert.ToInt32(dr["Gid"]);
                    Data.Name = dr["Name"].ToString();
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        //取得單一筆資料
        public YD GetDataId(string number)
        {
            YD Data = new YD();
            string SQL = $@"SELECT * FROM GasList WHERE Number={number}; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Id = Convert.ToInt32(dr["Id"]);
                Data.Number = dr["Number"].ToString();
                Data.Gas = dr["Gas"].ToString();
                Data.Content = dr["Content"].ToString();
                Data.Pay = dr["Pay"].ToString();
                Data.Cost = Convert.ToInt32(dr["Cost"]);
                Data.Date = dr["Date"].ToString();
            }
            catch(Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        //直接修改資料庫裡的資料所以為void 不需要再回傳list來顯示到頁面之類的
        public void UpdateData(YD updatedata)
        {
            string SQL = $@"UPDATE GasList SET Gas = N'{updatedata.Gas}',Content= N'{updatedata.Content}',Pay= '{updatedata.Pay}',Cost= '{updatedata.Cost}' WHERE Number= '{updatedata.Number}'; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL,conn);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertName(GasN newData)
        {   
            string SQL = $@"INSERT INTO GasName(Name) VALUES (N'{newData.Name}') ; ";
            string reset = $@"alter table GasName drop column Gid;";
            string change = $@"alter table GasName add Gid int identity (1,1);";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
                /*SqlCommand cmd2 = new SqlCommand(reset, conn);
                cmd2.ExecuteNonQuery();
                SqlCommand cmd3 = new SqlCommand(change, conn);
                cmd3.ExecuteNonQuery();*/
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteGName(string Name)
        {         
            string SQL = $@"DELETE FROM GasName WHERE Name= N'{Name}';";
            
            /*string reset = $@"alter table GasName drop column Gid;";
            string change = $@"alter table GasName add Gid int identity (1,1);";*/
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
                /*SqlCommand cmd2 = new SqlCommand(reset, conn);               
                cmd2.ExecuteNonQuery();
                SqlCommand cmd3 = new SqlCommand(change, conn);
                cmd3.ExecuteNonQuery();*/
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        //檢查重複
        public bool CheckName(string check)
        {
             string SQL = $@"SELECT count(*) FROM GasName where Name=N'{check}';";
             try
             {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL,conn);
                int ans = (int)cmd.ExecuteScalar();
                //System.Diagnostics.Debug.WriteLine(ans);
                if (ans==0) return true;
                else return false;
            }
             catch(Exception e)
             {
                 throw new Exception(e.Message.ToString());
             }
             finally
             {
                 conn.Close();
             }
             
            //System.Diagnostics.Debug.WriteLine(check);
        }

        //高雄修改
        public List<YD> KaoGetDataList(string Search, string Searchradio)
        {


            List<YD> DataList = new List<YD>();
            string SQL = string.Empty;
            //||!string.IsNullOrWhiteSpace(Searchradio) OR  Pay LIKE '%{Searchradio}%'
            //有字有按鈕
            if (!string.IsNullOrWhiteSpace(Search) && !string.IsNullOrWhiteSpace(Searchradio))
            {

                SQL = $@"SELECT * FROM KaohsiungGasList WHERE Pay LIKE '%{Searchradio}%' and (Gas LIKE '%{Search}%' OR Date LIKE '%{Search}%');";
            }
            //有字沒按鈕
            else if (!string.IsNullOrWhiteSpace(Search) && string.IsNullOrWhiteSpace(Searchradio))
            {
                SQL = $@"SELECT * FROM KaohsiungGasList WHERE Gas LIKE '%{Search}%' OR Date LIKE '%{Search}%';";
            }
            //有按鈕沒字
            else if (string.IsNullOrWhiteSpace(Search) && !string.IsNullOrWhiteSpace(Searchradio))
            {
                SQL = $@"SELECT * FROM KaohsiungGasList WHERE Pay LIKE '%{Searchradio}%';";
            }
            else
            {
                DateTime date = DateTime.Today;
                var TaiwanCalendar = new System.Globalization.TaiwanCalendar();
                var Year = TaiwanCalendar.GetYear(date);
                var Month = TaiwanCalendar.GetMonth(date);
                if (Month % 2 == 1)
                {
                    /*if (Month == 1)
                    {
                        Year--;
                        Month = 12;
                    }
                    else
                    {
                        Month--;
                    }*/
                    Month++;

                }
                string datetime = Year.ToString() + Month.ToString("00");
                SQL = $@" SELECT * FROM KaohsiungGasList WHERE Date = '{datetime}';";
            }

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    YD Data = new YD();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    Data.Number = dr["Number"].ToString();
                    Data.Gas = dr["Gas"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.Pay = dr["Pay"].ToString();
                    Data.Cost = Convert.ToInt32(dr["Cost"]);
                    Data.Date = dr["Date"].ToString();
                    //System.Diagnostics.Debug.WriteLine(Data.Gas);

                    DataList.Add(Data);
                }



            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }

        public void KaoGasInsert(string Num, string Date)
        {


            try
            {


                string Head = Num.Substring(0, 2), temp = Num.Substring(2), Ans = "";
                long count = Convert.ToInt64(temp);
                conn.Open();
                for (int i = 0; i < 50; i++)
                {

                    //temp = Head + count.ToString();
                    Ans = Head + count.ToString();

                    string SQL = $@"INSERT INTO KaohsiungGasList(Number,Date) VALUES ('{Ans}','{Date}') ; ";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.ExecuteNonQuery();
                    count++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

        }
        public void KaoDeleteGas(int Id)
        {
            string SQL = $@"DELETE FROM KaohsiungGasList WHERE Id={Id};";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public List<GasN> KaoSearchGas()
        {
            List<GasN> DataList = new List<GasN>();
            string SQL = $@"SELECT * FROM KaohsiungGasName order by Gid;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    GasN Data = new GasN();
                    Data.Gid = Convert.ToInt32(dr["Gid"]);
                    Data.Name = dr["Name"].ToString();
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        public YD KaoGetDataId(string number)
        {
            YD Data = new YD();
            string SQL = $@"SELECT * FROM KaohsiungGasList WHERE Number={number}; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.Id = Convert.ToInt32(dr["Id"]);
                Data.Number = dr["Number"].ToString();
                Data.Gas = dr["Gas"].ToString();
                Data.Content = dr["Content"].ToString();
                Data.Pay = dr["Pay"].ToString();
                Data.Cost = Convert.ToInt32(dr["Cost"]);
                Data.Date = dr["Date"].ToString();
            }
            catch (Exception e)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        public void KaoUpdateData(YD updatedata)
        {
            string SQL = $@"UPDATE KaohsiungGasList SET Gas = N'{updatedata.Gas}',Content= N'{updatedata.Content}',Pay= '{updatedata.Pay}',Cost= '{updatedata.Cost}' WHERE Number= '{updatedata.Number}'; ";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void KaoInsertName(GasN newData)
        {
            string SQL = $@"INSERT INTO KaohsiungGasName(Name) VALUES (N'{newData.Name}') ; ";
            string reset = $@"alter table KaohsiungGasName drop column Gid;";
            string change = $@"alter table KaohsiungGasName add Gid int identity (1,1);";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
                /*SqlCommand cmd2 = new SqlCommand(reset, conn);
                cmd2.ExecuteNonQuery();
                SqlCommand cmd3 = new SqlCommand(change, conn);
                cmd3.ExecuteNonQuery();*/
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public void KaoDeleteGName(string Name)
        {
            string SQL = $@"DELETE FROM KaohsiungGasName WHERE Name= N'{Name}';";

            /*string reset = $@"alter table GasName drop column Gid;";
            string change = $@"alter table GasName add Gid int identity (1,1);";*/
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
                /*SqlCommand cmd2 = new SqlCommand(reset, conn);               
                cmd2.ExecuteNonQuery();
                SqlCommand cmd3 = new SqlCommand(change, conn);
                cmd3.ExecuteNonQuery();*/
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public bool KaoCheckName(string check)
        {
            string SQL = $@"SELECT count(*) FROM KaohsiungGasName where Name=N'{check}';";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                int ans = (int)cmd.ExecuteScalar();
                //System.Diagnostics.Debug.WriteLine(ans);
                if (ans == 0) return true;
                else return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            //System.Diagnostics.Debug.WriteLine(check);
        }

    }
}