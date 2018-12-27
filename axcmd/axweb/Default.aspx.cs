using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



using axfunc;

namespace axweb
{
    public partial class _Default : System.Web.UI.Page
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
            if (!spider.Login(username, password))
            {
                lMsg.Text = "验证失败,请重新输入;";
                return;
            }

            //session
            Session["username"] = username;
            Session["password"] = password;

            Response.Redirect("tools/mycontracts.aspx");



        }
      

      

    }
}