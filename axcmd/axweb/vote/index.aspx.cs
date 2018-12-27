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

namespace axweb.vote
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
                {
                    int VoterCount = 0;
                    double TotalFee = 0;
                    AXDal.getVoteTotalSum(conn, out VoterCount, out TotalFee);

                    lTotalFee.Text = TotalFee.ToString();
                    lVoterCount.Text = VoterCount.ToString();

                 }
            }
        }
    }
}