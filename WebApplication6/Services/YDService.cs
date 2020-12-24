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
        public List<YD> GetDataList()
        {
            List<YD> DataList = new List<YD>();
            string SQL = @" SELECT * FROM GasList;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    YD Data = new YD();
                    Data.Id = Convert.ToInt32(dr["Id"]);
                    Data.Gas = dr["Gas"].ToString();
                    Data.Content = dr["Content"].ToString();
                    Data.Pay = dr["Pay"].ToString();
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
        //create
        public void GasInsert(YD newData)
        {
            string SQL = $@"INSERT INTO GasList(Gas,Content) VALUES ('{newData.Gas}','{newData.Content}') ; ";
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