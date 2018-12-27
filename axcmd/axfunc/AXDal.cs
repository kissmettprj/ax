using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using System.Security.Cryptography;

using Newtonsoft.Json;

using com.kissmett.Common;

namespace axfunc
{
    public class AXDal
    {
        //------------------------------------------------------------------------ validcode

        public static string getUserInfoByValidCode(SqlConnection conn, string validcode,string info)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select * from [ax_user] where validcode ='" + validcode + "' ";
            string res = "";
            DataSet dt = cdal.GetDS(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                res = dt.Tables[0].Rows[0][info].ToString(); 
            }
            
            return res;

        }
        public static string getUserValidCode(SqlConnection conn, string username)
        {
            string res = "";
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select * from [ax_user] where ax_username ='" + username + "' ";
            string validid = ""; string salt = "";
            DataSet dt = cdal.GetDS(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                validid = dt.Tables[0].Rows[0]["validid"].ToString();
                salt = dt.Tables[0].Rows[0]["salt"].ToString();
                //validid + salt md5             
                res = Common.GetMD5(validid + salt);
            }

            return res;

        }
        public static string getUserInfo(SqlConnection conn, string username,string info)
        {
            string res = "";
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select * from [ax_user] where ax_username ='" + username + "' ";
            string validid = ""; string salt = "";
            DataSet dt = cdal.GetDS(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                res = dt.Tables[0].Rows[0][info].ToString();
                
            }

            return res;

        }

        public static void setUserDaishou(SqlConnection conn, string username, double daishou)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " "
            + " if not exists (select * from [ax_user] where ax_username ='" + username + "' ) "
            + " begin insert into [ax_user] (ax_username,daishou) values('" + username + "'," + daishou + ") "
            + " end else begin update [ax_user] set daishou=" + daishou + " where ax_username ='" + username + "' end ";
            cdal.ExecSQL(sql);
        }

        public static int updateUserValidID(SqlConnection conn, string username, string validid,string salt)
        {

            string validcode = Common.GetMD5(validid + salt);

            CommonDAL cdal = new CommonDAL(conn);
            string sql = " "
            + " if not exists (select * from [ax_user] where ax_username ='" + username + "' ) "
            + " begin insert into [ax_user] (ax_username,validid,salt,validcode) values('" + username + "','" + validid + "','" + salt + "','" + validcode + "' ) "
            + " end else begin update [ax_user] set validid='" + validid + "' , salt='" + salt + "',validcode='" + validcode + "'  where ax_username ='" + username + "' end ";
            
            return cdal.ExecSQL(sql);
        }

        //------------------------------------------------------------------------ contract download

        public static int getContractDownloadTaskStatus(SqlConnection conn, string username)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select isnull(status,0) as status from [ax_contractdownload_task] where ax_username ='" + username + "' ";
            int res = -1; //不存在;
            DataSet dt = cdal.GetDS(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                res = Functions.CleanDBInt(dt.Tables[0].Rows[0]["status"]);
            }
            return res;
        }
        public static void setContractDownloadTaskStatus(SqlConnection conn, string username,int status)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " "
            + " if not exists (select * from [ax_contractdownload_task] where ax_username ='" + username + "' ) "
            + " begin insert into [ax_contractdownload_task] (ax_username,status) values('" + username + "',"+status+") "
            + " end else begin update [ax_contractdownload_task] set status="+status+" where ax_username ='" + username + "' end ";
            cdal.ExecSQL(sql);
        }


        //---------------------------------------- wx
        public static void setUserWXInfo(SqlConnection conn, string ax_username, string wx_openid, string wx_unionid)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " "
            + " update [ax_user] set wx_openid='" + wx_openid + "', wx_unionid='" + wx_unionid + "' where ax_username ='" + ax_username + "'  ";
            cdal.ExecSQL(sql);
        }

        public static void updateWXUser(SqlConnection conn, string wx_userinfo)
        {
            CommonDAL cdal = new CommonDAL(conn);
            Dictionary<string, object> o = JsonConvert.DeserializeObject<Dictionary<string, object>>(wx_userinfo);
            string wx_openid = o["openid"].ToString().Replace("\"", "");
            string wx_unionid = o["unionid"].ToString().Replace("\"", "");
            string sql = " "
            + " if not exists (select * from [wx_user] where wx_openid ='" + wx_openid + "' ) "
            + " begin insert into [wx_user] (wx_openid,wx_unionid,wx_userinfo) values('" + wx_openid + "','" + wx_unionid + "','" + wx_userinfo + "') "
            + " end else begin update [wx_user] set wx_unionid='" + wx_unionid + "', wx_userinfo='" + wx_userinfo + "' where wx_openid ='" + wx_openid + "' end ";
            cdal.ExecSQL(sql);
        }
        public static DataTable getAXUsersByWXOpenID(SqlConnection conn, string wx_openid)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select *  from [ax_user] where wx_openid ='" + wx_openid + "' ";
            DataSet dt = cdal.GetDS(sql);
            return dt.Tables[0];
        }

        //---------------------------------------- qq
        public static void setUserQQInfo(SqlConnection conn, string ax_username, string qq_openid)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " "
            + " update [ax_user] set qq_openid='" + qq_openid + "'  where ax_username ='" + ax_username + "'  ";
            cdal.ExecSQL(sql);
        }

        public static void updateQQUser(SqlConnection conn, string qq_userinfo)
        {
            CommonDAL cdal = new CommonDAL(conn);
            Dictionary<string, object> o = JsonConvert.DeserializeObject<Dictionary<string, object>>(qq_userinfo);
            string qq_openid = o["openid"].ToString().Replace("\"", "");
            string sql = " "
            + " if not exists (select * from [qq_user] where qq_openid ='" + qq_openid + "' ) "
            + " begin insert into [qq_user] (qq_openid,qq_userinfo) values('" + qq_openid + "', '" + qq_userinfo + "') "
            + " end else begin update [qq_user] set qq_userinfo='" + qq_userinfo + "' where qq_openid ='" + qq_openid + "' end ";
            cdal.ExecSQL(sql);
        }
        public static DataTable getAXUsersByQQOpenID(SqlConnection conn, string qq_openid)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select *  from [ax_user] where qq_openid ='" + qq_openid + "' ";
            DataSet dt = cdal.GetDS(sql);
            return dt.Tables[0];
        }



        //----------------------------------------- vote
        public static int vote(SqlConnection conn, string username, string candidates)
        {           

            CommonDAL cdal = new CommonDAL(conn);
            string sql = " delete from [ax_vote] where ax_username ='" + username + "' ";
            foreach (string candidate in candidates.Split(",".ToCharArray())) {
                
                if (!candidate.Trim().Equals(""))
                {
                    sql += "   insert into [ax_vote] (ax_username,candidate,votetime) values('" + username + "','" + candidate.Trim() + "',getdate() ) ";
                }
            }            

            return cdal.ExecSQL(sql);
        }

        public static string getMyCandidates(SqlConnection conn, string username)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select *  from [ax_vote] where ax_username ='" + username + "' ";
            string res = ""; //不存在;
            DataSet dt = cdal.GetDS(sql);
            for (int i = 0; i < dt.Tables[0].Rows.Count;i++ )
            {
                res += Functions.CleanDBString(dt.Tables[0].Rows[i]["candidate"])+",";
            }
            return res;
        }

        public static string[] getCandidatesVoteRes(SqlConnection conn)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select candidate ,count(*) as cnt  from [ax_vote] group by candidate ";
            string[] res = new string[2];
            res[0] = ""; res[1] = "";

            DataSet dt = cdal.GetDS(sql);
            for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
            {
                res[0] += "\"" + Functions.CleanDBString(dt.Tables[0].Rows[i]["candidate"]) + "\",";
                res[1] += "" + Functions.CleanDBInt(dt.Tables[0].Rows[i]["cnt"]) + ",";
            }

            if (res[0].Length > 0) res[0] = res[0].Substring(0, res[0].Length - 1);
            res[0] = "[" + res[0] + "]";
            if (res[1].Length > 0) res[1] = res[1].Substring(0, res[1].Length - 1);
            res[1] = "[" + res[1] + "]";
            return res;
        }
        public static void getVoteTotalSum(SqlConnection conn, out int VoterCount, out double TotalFee)
        {
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select count(*) as cnt , sum(daishou) s   from [ax_user] where ax_username in (select ax_username from ax_vote ) ";
            VoterCount = 0; TotalFee = 0;

            DataSet dt = cdal.GetDS(sql);
            if( dt.Tables[0].Rows.Count >0)
            {
                VoterCount = Functions.CleanDBInt(dt.Tables[0].Rows[0]["cnt"]);
                TotalFee = Functions.CleanDBDouble(dt.Tables[0].Rows[0]["s"]);
            }

            
        }

        //not used
        public static bool hasUserVoted(SqlConnection conn, string username)
        {
            bool res = false;
            CommonDAL cdal = new CommonDAL(conn);
            string sql = " select * from [ax_user] where ax_username ='" + username + "' ";           
            DataSet dt = cdal.GetDS(sql);
            if (dt.Tables[0].Rows.Count > 0)
            {
                res = true;

            }
            return res;
        }




    }
}
