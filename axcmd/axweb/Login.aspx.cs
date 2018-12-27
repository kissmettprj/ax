using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using com.kissmett.Common;
using axfunc;


namespace axweb
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {

            string username = txtUserName.Text;
            string password = txtPassword.Text;
            string valicode = this.txtValiCode.Text;
            string checkcode = Request.Cookies["CheckCode"].Value;
            if (String.Compare(checkcode, valicode, true) != 0)
            {
                lMsg.Text = "验证码错误，请输入正确的验证码。";
                return;
            }

            AXSpider spider = new AXSpider();
            //spider.SpiderOutputHandler = new SpiderOutputDelegate(Log);

            Console.WriteLine("验证账户...");
            if (!spider.Login_cunguan(username, password))
            {
                lMsg.Text = "验证失败,请重新输入;";
                return;
            }

            //get info
            double daishou = 0;
            try
            {
                string usercenter = spider.getUserCenter2016_cunguan();
                daishou = axfunc.AXFunction.getDaiShou(usercenter);
            }
            catch (Exception exp) {
                lMsg.Text = "待收获取有问题,设置为0";
            }

            //session
            Session["username"] = username;
            Session["password"] = password;            
            Session["daishou"] = daishou;

            //db
             using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
            {
                AXDal.setUserDaishou(conn, username, daishou);
            }

            string backurl = Functions.ParseStr(Session["backurl"], "Default.aspx");
            backurl = Functions.ParseStr(Request["backurl"], backurl);//url参数优先

            //Response.Redirect("tools/mycontracts.aspx");
            Response.Redirect(backurl);

        }
    }
}