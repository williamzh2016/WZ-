using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Dreamsmart.Models
{
    public class Tweet
    {

        private string strHashTag;
        private string strId;
        private string strTweetText;
        private string strCreatedBy;
        private DateTime dtCreatedDate;
        private long lngRetweetCount;

        public String HashTag
        {
            get
            {
                return strHashTag;
            }
            set
            {
                strHashTag = value;
            }
        }

        public String Id
        {
            get
            {
                return strId;
            }
            set
            {
                strId = value;
            }
        }

        public string TweetText
        {
            get
            {
                return strTweetText;
            }
            set
            {
                strTweetText = value;
            }
        }

        public string CreatedBy
        {
            get
            {
                return strCreatedBy;
            }
            set
            {
                strCreatedBy = value;
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return dtCreatedDate;
            }
            set
            {
                dtCreatedDate = value;
            }
        }

        public long RetweetCount
        {
            get
            {
                return lngRetweetCount;
            }
            set
            {
                lngRetweetCount = value;
            }
        }

        public Boolean Save()
        {
            SqlConnection objConn = new SqlConnection();
            try
            {
                objConn.ConnectionString = ConfigurationManager.AppSettings["DBConnection"].ToString();
                SqlCommand objCommand = new SqlCommand();
                objCommand.Connection = objConn;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "InsertTweet";

                SqlParameter objSqlParameter1 = new SqlParameter("vHashTag", SqlDbType.NVarChar, 100);
                SqlParameter objSqlParameter2 = new SqlParameter("vId", SqlDbType.NVarChar, 50);
                SqlParameter objSqlParameter3 = new SqlParameter("vTweetText", SqlDbType.NVarChar, 500);
                SqlParameter objSqlParameter4 = new SqlParameter("vCreatedBy", SqlDbType.NVarChar, 50);
                SqlParameter objSqlParameter5 = new SqlParameter("vCreatedDate", SqlDbType.DateTime);
                SqlParameter objSqlParameter6 = new SqlParameter("vRetweetCount", SqlDbType.Int, 100);

                objSqlParameter1.Direction = ParameterDirection.Input;
                objSqlParameter2.Direction = ParameterDirection.Input;
                objSqlParameter3.Direction = ParameterDirection.Input;
                objSqlParameter4.Direction = ParameterDirection.Input;
                objSqlParameter5.Direction = ParameterDirection.Input;
                objSqlParameter6.Direction = ParameterDirection.Input;

                objSqlParameter1.Value = this.HashTag;
                objSqlParameter2.Value = this.Id;
                objSqlParameter3.Value = this.TweetText;
                objSqlParameter4.Value = this.CreatedBy;
                objSqlParameter5.Value = this.CreatedDate;
                objSqlParameter6.Value = this.RetweetCount;

                objCommand.Parameters.Add(objSqlParameter1);
                objCommand.Parameters.Add(objSqlParameter2);
                objCommand.Parameters.Add(objSqlParameter3);
                objCommand.Parameters.Add(objSqlParameter4);
                objCommand.Parameters.Add(objSqlParameter5);
                objCommand.Parameters.Add(objSqlParameter6);

                objConn.Open();
                objCommand.ExecuteNonQuery();
                objConn.Close();
                return true;
            }
            catch (Exception ex)
            {
                objConn.Close();
                return false;
            }
        }

        public DataTable Load(String strHashTag = "")
        {
            SqlConnection objConn = new SqlConnection();
            try
            {
                objConn.ConnectionString = ConfigurationManager.AppSettings["DBConnection"].ToString();
                SqlCommand objCommand = new SqlCommand();
                objCommand.Connection = objConn;
                objCommand.CommandType = CommandType.StoredProcedure;
                objCommand.CommandText = "LoadTweet";

                SqlParameter objSqlParameter1 = new SqlParameter("vHashTag", SqlDbType.NVarChar, 100);
                objSqlParameter1.Direction = ParameterDirection.Input;
                objSqlParameter1.Value = strHashTag;
                objCommand.Parameters.Add(objSqlParameter1);
                objConn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter(objCommand);
                objSqlDataAdapter.Fill(dt);

                objSqlDataAdapter.Dispose();
                objConn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                objConn.Close();
                return null;
            }
        }

    }
}