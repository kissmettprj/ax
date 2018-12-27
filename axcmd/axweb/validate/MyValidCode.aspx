<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyValidCode.aspx.cs" Inherits="axweb.validate.MyValidCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>我的验证码</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <font color="#ff0000"><b>请牢记你的验证码和二维码,请勿随意出示给别人,仅出示给值得信任的群主验证用:</b></font> 
        <button onclick="window.location.href='../default.aspx';return false;">回主页</button>
        <br />
        自定义码: <asp:TextBox ID="txtSalt" runat="server"></asp:TextBox>  <asp:Button ID="btnOK" runat="server" Text="加码生成" OnClick="btnOK_Click" /> <br />
        <asp:Image ID="imgQRCode" runat="server" />
        <br>
        

        <asp:Literal ID="lMyValidID" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
