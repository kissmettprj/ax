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
    public partial class Default : QQPage // System.Web.UI.Page //
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(Data);
            //Data 为 QQPage.Data内容为:
            /*"{\n    
            \"ret\": 0,\n    \"msg\": \"\",\n    \"is_lost\":0,\n    
            \"nickname\": \"飞～甜\",\n    \"gender\": \"男\",\n    \"province\": \"北京\",\n    \"city\": \"东城\",\n   
            \"year\": \"1980\",\n    \"constellation\": \"\",\n    
            \"figureurl\": \"http:\\/\\/qzapp.qlogo.cn\\/qzapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/30\",\n    
            \"figureurl_1\": \"http:\\/\\/qzapp.qlogo.cn\\/qzapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/50\",\n    
            \"figureurl_2\": \"http:\\/\\/qzapp.qlogo.cn\\/qzapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/100\",\n    
            \"figureurl_qq_1\": \"http:\\/\\/thirdqq.qlogo.cn\\/qqapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/40\",\n    
            \"figureurl_qq_2\": \"http:\\/\\/thirdqq.qlogo.cn\\/qqapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/100\",\n    
            \"is_yellow_vip\": \"0\",\n    \"vip\": \"0\",\n    \"yellow_vip_level\": \"0\",\n    \"level\": \"0\",\n    
            \"is_yellow_year_vip\": \"0\"\n
             
             "openid":"xxxxxx" ---后补的; 非tencent返回的;
             
            }\n"
                
            */
            if (Data == null) return;
            Dictionary<string, object> o = Data;
            //Response.Write(o["nickname"].ToString().Replace("\"", ""));
            headimg.Src = o["figureurl_1"].ToString();
            nickname.Text = o["nickname"].ToString().Replace("\"", "");

            string s = JsonConvert.SerializeObject(o); 
            //Response.Write(s);

            //若Data==null则,需要用户授权;若Data!=null,则说明已经授权和获取userinfo;
            if (o != null)
            {
                string qq_openid = o["openid"].ToString().Replace("\"", "");
                //string wx_unionid = Data["unionid"].ToString().Replace("\"", "");
                string qq_userinfo = JsonConvert.SerializeObject(o);

                using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
                {

                    //wx_user
                    AXDal.updateQQUser(conn, qq_userinfo); 

                    //若Session.username存在,则将其绑定到当前qq
                    if (Session["username"] != null)
                    {
                        string ax_username = Session["username"].ToString();
                        AXDal.setUserQQInfo(conn, ax_username, qq_openid);
                    }


                    //get ax_user by wx_openid;
                    DataTable dt = AXDal.getAXUsersByQQOpenID(conn, qq_openid);
                    gridAXUsers.DataSource = dt;
                    gridAXUsers.DataBind();
                }


            }
            


            //base.OnLoad(e);
        }

        protected void gridAXUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SwitchAXUser")
            {
                string ax_username = e.CommandArgument.ToString();
                Session["username"] = ax_username;

                //daishou
                string daishou = "0";
                using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
                {
                    daishou = AXDal.getUserInfo(conn, ax_username, "daishou");
                }
                Session["daishou"] = daishou;

                //Response.Write("you click " + e.CommandArgument.ToString());
                lMsg.Text = "已切换至用户[" + ax_username + "]";
                Response.Redirect("../Default.aspx");
            }
        }

        //为微信帐户增加新的安心贷帐户;
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //切换到登录,并跳转到bindnew;
            //Session["backurl"] = "wx/bindaxuser.aspx";
            //Response.Redirect("../Login.aspx");

            //Session.Abandon(); //不可以,这样会清掉qq wx对接后的session
            Session["username"] = null;
            Session["password"] = null;
            Response.Redirect("bindaxuser.aspx");
            
        

        }
    }
}