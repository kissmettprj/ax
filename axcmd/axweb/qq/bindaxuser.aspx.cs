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

namespace qq
{
    public partial class bindaxuser : QQPage //  System.Web.UI.Page //
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null) //need username
            {
                Session["backurl"] = "qq/bindaxuser.aspx";
                Response.Redirect("../Login.aspx");
                return;
            }

            //string testData = Functions.ParseStr(Session["$qquser"]);
            //Dictionary<string, object> o = JsonConvert.DeserializeObject<Dictionary<string, object>>(testData); //test
            Dictionary<string, object> o = Data;
            Response.Write(o["nickname"].ToString().Replace("\"", ""));

            string s = JsonConvert.SerializeObject(o);
            //Response.Write(s);

            //若Data==null则,需要用户授权;若Data!=null,则说明已经授权和获取userinfo;
            if (o != null)
            {
                //ax_user
                string ax_username = Session["username"].ToString();
                string qq_openid = o["openid"].ToString().Replace("\"", "");
                using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
                {
                    AXDal.setUserQQInfo(conn, ax_username, qq_openid);
                }
            }
            string url = "default.aspx";
            Response.Redirect(url);

        }
    }
}