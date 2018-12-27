using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

using com.kissmett.Common;

using axfunc;

namespace axweb.tools
{
    public partial class importborrowercsv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            { //未登录
                Session["backurl"] = "tools/importborrowercsv.aspx";
                Response.Redirect("../Login.aspx");
                return;
            }
            if (!this.IsPostBack)
            {
                this.txtUserName.Text = Session["username"].ToString();
                //ViewState["username"] = Session["username"].ToString();
                //ViewState["password"] = Session["password"].ToString();
                ViewState["wsbase"] = Functions.GetAppConfigString("wsbase", "");


                //string username = ViewState["username"].ToString();
            }
        }

        protected void btUpload_Click(object sender, EventArgs e)
        {
            string wsbase = ViewState["wsbase"].ToString();
            string username = Session["username"].ToString();


            string wsborrowercsv = wsbase + @"\borrowercsv\";

            if (!Directory.Exists(wsborrowercsv)) Directory.CreateDirectory(wsborrowercsv);

            try
           {
             if (FileUpload1.PostedFile.FileName == "")
             {
                 this.lb_info.Text = "请选择文件！";
             }
             else
             {
                 string filepath = FileUpload1.PostedFile.FileName;

                 string localfilename = filepath.Substring(filepath.LastIndexOf("\\") + 1);
                 if (!localfilename.Substring(localfilename.LastIndexOf(".") + 1).ToLower().Equals("csv")) {
                     this.lb_info.Text = "请选择借款人csv文件！";
                     return;                    
                 }

                 string filename = "借款人信息-"+username+".csv";
                 string serverpath = wsborrowercsv + filename;
                 FileUpload1.PostedFile.SaveAs(serverpath);
                 this.lb_info.Text = "上传成功！";
             }
         }
         catch (Exception ex)
         {
             this.lb_info.Text = "上传发生错误！原因是：" + ex.ToString();
         }
        }



    }
}