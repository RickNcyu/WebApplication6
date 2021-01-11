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
        public List<YD> GetDataList(string Search)
        {
            List<YD> DataList = new List<YD>();
            string SQL = string.Empty;
            if (!string.IsNullOrWhiteSpace(Search))
            {
                SQL = $@"SELECT * FROM GasList WHERE Gas LIKE '%{Search}%' OR Date LIKE '%{Search}%';";
            }
            else
            {
                SQL = @" SELECT * FROM GasList;";
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
                for (int i = 0; i <= 50; i++)
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
            string SQL= $@"SELECT * FROM GasName;";
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
            string SQL = $@"UPDATE GasList SET Gas = '{updatedata.Gas}',Content='{updatedata.Content}',Pay='{updatedata.Pay}',Cost='{updatedata.Cost}' WHERE Number='{updatedata.Number}'; ";
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
        
    }
}