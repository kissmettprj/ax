using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using System.Drawing.Imaging;

using com.kissmett.Common;


// http://localhost:81/control/GetQRCodeImage.aspx?v=http://localhost:81/test.aspx


namespace axweb.control
{
    public partial class GetQRCodeImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string v = Functions.ParseStr(Request["v"], "http://www.kissmett.com");
            Bitmap b = QRCodeHelper.Create_ImgCode(v, 10);
            System.IO.MemoryStream ms = null;
            ms = new System.IO.MemoryStream();
            b.Save(ms, ImageFormat.Bmp);
            Response.ClearContent();
            Response.BinaryWrite(ms.ToArray());
            ms.Close();


        }
    }
}