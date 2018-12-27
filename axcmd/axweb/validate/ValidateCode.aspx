<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ValidateCode.aspx.cs" Inherits="axweb.validate.ValidateCode" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>安友验证</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
         <font color="#ff0000"><b>安友验证:</b></font> 
        <button onclick="window.location.href='../default.aspx';return false;">回主页</button><br />
        验证码:<asp:TextBox ID="txtValidCode" runat="server" Width="230px"></asp:TextBox> <asp:Button ID="btnOK" runat="server" Text="验证" OnClick="btnOK_Click" />
        
        <br />
           <font color="red" size="100px"><b><asp:Literal ID="lMsg" runat="server"></asp:Literal></b></font>

    </div>
        
 
        
    </form>
</body>
</html>
