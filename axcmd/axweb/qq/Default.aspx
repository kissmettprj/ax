<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="qq.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>QQ绑定</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <img id="headimg" runat="server" height="50" /> <asp:Literal ID="nickname" runat="server"></asp:Literal><br />
        <font color="#ff0000" size="16"><b>已绑定安心贷帐户列表</b></font> <button onclick="window.location.href='../default.aspx';return false;">回主页</button> <br>
         
        <font color="#ff0000"><b> <asp:Literal ID="lMsg" runat="server"></asp:Literal></b></font><br>

        <asp:Button ID="btnAdd" runat="server" Text="为该QQ帐户绑定新安心贷帐户" OnClick="btnAdd_Click" Height="65px" />
        <asp:GridView ID="gridAXUsers" runat="server" AutoGenerateColumns="False" OnRowCommand="gridAXUsers_RowCommand" >
            <Columns>   
            <asp:BoundField HeaderText="安心用户名" DataField="ax_username"  HtmlEncode="False">   
            </asp:BoundField>     
            <asp:BoundField HeaderText="待收"  DataField="daishou" DataFormatString="{0:C}" HtmlEncode="False">   
            </asp:BoundField>   
                <asp:TemplateField HeaderText="切换至" ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbSwitchAXUser" runat="server" CausesValidation="false" CommandName="SwitchAXUser" Text='<%# Eval("ax_username") %>' CommandArgument='<%# Eval("ax_username") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>   

        </asp:GridView>
        
    </div>
    </form>
</body>
</html>
