using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;

using com.kissmett.Common;

using axfunc;

namespace wx
{
    public partial class bindaxuser : System.Web.UI.Page // WeiXinPage // 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null) //need username
            {
                Session["backurl"] = "wx/bindaxuser.aspx";
                Response.Redirect("../Login.aspx");
                return;
            }

            string testData = @"
            {
            ""openid"":""OPENID"",
            ""nickname"":""NICKNAME"",
            ""sex"":1,
            ""province"":""PROVINCE"",
            ""city"":""CITY"",
            ""country"":""COUNTRY"",
            ""headimgurl"": ""http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0"",
            ""privilege"":[
            ""PRIVILEGE1"",""PRIVILEGE2""
            ],
            ""unionid"": ""o6_bmasdasdsad6_2sgVt7hMZOPfL""
            }            
            "; 
            Dictionary<string, object> o = JsonConvert.DeserializeObject<Dictionary<string, object>>(testData); //test
            //Dictionary<string, object> o = Data;
            Response.Write(o["nickname"].ToString().Replace("\"", ""));

            string s = JsonConvert.SerializeObject(o);
            //Response.Write(s);

            //若Data==null则,需要用户授权;若Data!=null,则说明已经授权和获取userinfo;
            if (o != null)
            {
                //ax_user
                string ax_username = Session["username"].ToString();
                string wx_openid = o["openid"].ToString().Replace("\"", "");
                string wx_unionid = o["unionid"].ToString().Replace("\"", "");
                using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
                {
                    AXDal.setUserWXInfo(conn, ax_username, wx_openid, wx_unionid);
                }
            }
            string url = "default.aspx";
            Response.Redirect(url);

        }
    }
}