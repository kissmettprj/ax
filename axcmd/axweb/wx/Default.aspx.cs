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
    public partial class Default :System.Web.UI.Page // WeiXinPage // 
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(Session["username"]==null) //need username
            //{
            //    Session["backurl"] = "tools/importborrowercsv.aspx";
            //    Response.Redirect("../Login.aspx");
            //    return;
            //}

            //Data: JsonConvert.DeserializeObject<Dictionary<string, object>>(HttpContext.Current.Session[_user].ToString());
            /*
             * https://open.weixin.qq.com/cgi-bin/showdocument?action=dir_list&t=resource/res_list&verify=1&id=open1419316518&lang=zh_CN
             * 
            {
            "openid":"OPENID",
            "nickname":"NICKNAME",
            "sex":1,
            "province":"PROVINCE",
            "city":"CITY",
            "country":"COUNTRY",
            "headimgurl": "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0",
            "privilege":[
            "PRIVILEGE1",
            "PRIVILEGE2"
            ],
            "unionid": " o6_bmasdasdsad6_2sgVt7hMZOPfL"

            }
             */
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
            //Response.Write(o["nickname"].ToString().Replace("\"", ""));
            headimg.Src = o["figureurl_1"].ToString();
            nickname.Text = o["nickname"].ToString().Replace("\"", "");


            string s = JsonConvert.SerializeObject(o);
            //Response.Write(s);

            //若Data==null则,需要用户授权;若Data!=null,则说明已经授权和获取userinfo;
            if (o != null)
            {
                string wx_openid = o["openid"].ToString().Replace("\"", "");
                //string wx_unionid = Data["unionid"].ToString().Replace("\"", "");
                string wx_userinfo = JsonConvert.SerializeObject(o);

                using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
                {
                    //wx_user
                    AXDal.updateWXUser(conn,wx_userinfo);
                    //get ax_user by wx_openid;
                    DataTable dt = AXDal.getAXUsersByWXOpenID(conn, wx_openid);
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

            Session["username"] = null;
            Session["password"] = null;
            Response.Redirect("bindaxuser.aspx");
        

        }
    }
}