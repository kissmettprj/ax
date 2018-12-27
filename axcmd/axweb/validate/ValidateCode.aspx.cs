using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using com.kissmett.Common;

using axfunc;

namespace axweb.validate
{
    public partial class ValidateCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string code =  Functions.ParseStr(Request["code"]);
                if (code.Trim().Equals("")) 
                { 
                    return;
                }
                txtValidCode.Text = code;
                validate();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            validate();
        }

        protected void validate()
        {
            string validcode = txtValidCode.Text;
            using (SqlConnection conn = new SqlConnection(Globals.ConnectionString))
            {
                string username = AXDal.getUserInfoByValidCode(conn, validcode, "ax_username");
                string daishou = AXDal.getUserInfoByValidCode(conn, validcode, "daishou");
                string mycandi = AXDal.getMyCandidates(conn, username);

                string voteres = "该安友已投票!";
                if(mycandi.Equals("")){
                    voteres = "该安友尚未投票!";
                }
                if (username.Equals(""))
                {
                    lMsg.Text = " 未通过识别! ";
                }
                else
                {
                    lMsg.Text = "用户名:" + AXFunction.coverInfo(username) + " ; <br>"
                        + "待收: " + daishou + "元 ,已识别!  <br>"
                        //+ voteres+""
                        ;

                }
            }

        }
    }
}