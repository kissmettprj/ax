using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Data;
using System.Data.SqlClient;

using System.Drawing;
using System.Drawing.Imaging;

using com.kissmett.Common;
using axfunc;

namespace axweb.validate
{
    public partial class MyValidCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["username"] == null) //只需要username在即可;两个途径会设置username,1.ax登录验证后;2.绑定qq的切换后;这两个路径都是通过验证的;
            { //未登录
                Session["backurl"] = "validate/myvalidcode.aspx";
                Response.Redirect("../Login.aspx");
                return;
            }
            if (!IsPostBack) {
                string username = Session["username"].ToString();

              
                //绑定salt;
                using (SqlConnection conn = new SqlConnection(Globals.ConnectionString)) 
                {
                    string salt = AXDal.getUserInfo(conn, username, "salt");
                    string validid = AXDal.getUserInfo(conn, username, "validid");
                    txtSalt.Text = salt;
                    ViewState["salt"] = salt;
                    ViewState["validid"] = validid;

                }
               
                GeneValidCode();

            }//ispostback

        }//page_load

        protected void GeneQRCode(string validID)
        {

            string username = Session["username"].ToString();

            string v = validID;
            Bitmap b = QRCodeHelper.Create_ImgCode(v, 10);
            System.IO.MemoryStream ms = null;
            string filename = Server.MapPath("../qrcode")+@"\"+username+".png";
            b.Save(filename, ImageFormat.Png);

            imgQRCode.ImageUrl = "../qrcode/" + username + ".png";
            

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            GeneValidCode();
        }

        protected void GeneValidCode()
        {
            lMyValidID.Text = "";
            string validCode = "";

            using (SqlConnection conn = new SqlConnection(Globals.ConnectionString))
            {
                string username = Session["username"].ToString();
                //string password = Session["password"].ToString();//不需要再次登录验证,因username存在即可说明"曾经"通过了验证;

                string salt = txtSalt.Text;               
                string validID = Functions.ParseStr(ViewState["validid"]);

                if (validID.Equals(""))
                {
                    //spider验证后生成一个;
                    AXSpider spider = new AXSpider();
                    //if (spider.Login_cunguan(username, password))
                    //{
                        validID = Guid.NewGuid().ToString();
                        ViewState["validid"] = validID;
                        AXDal.updateUserValidID(conn, username, validID, salt);                        
                    //}
                    //else
                    //{
                    //    //能到此页面,应该已经通过安心用户登录验证;
                    //    lMyValidID.Text = "验证异常,稍后重试";
                    //    return;
                    //}
                }//validID

                string oldSalt = Functions.ParseStr(ViewState["salt"]);
                //validid为空,或者salt发生改变,更新db
                if (!oldSalt.Equals(salt))
                {
                    AXDal.updateUserValidID(conn, username, validID, salt);

                }



                validCode = Common.GetMD5(validID + salt);

                lMyValidID.Text = validCode;

                //生成二维码;

                string valid_url = "http://"+Request.Url.Host + "/ax/validate/ValidateCode.aspx?code=" + validCode;
                GeneQRCode(valid_url);
            }//using conn

        }
    }
}