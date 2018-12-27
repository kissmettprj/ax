<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="importborrowercsv.aspx.cs" Inherits="axweb.tools.importborrowercsv" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>上传借款人CSV</title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>

                <td >
                                  <hr />

                    <font color="#ff0000"><b>上传借款人CSV[<asp:Literal ID="txtUserName" runat="server"></asp:Literal>]</b></font>

                    <button onclick="window.location.href='../default.aspx';return false;">回主页
                    </button>
                        <hr />


                </td>
            </tr>
            <tr>

                <td  >选择导出的借款人CSV文件:<asp:FileUpload ID="FileUpload1" runat="server" Width="475px" />

                    <asp:Button ID="btUpload" runat="server" Text="上传" OnClick="btUpload_Click" />


                </td>
            </tr>
            <tr>

                <td colspan="2"></td>
            </tr>
            <tr>
                <td style="width: 100px; height: 21px;">
                    <asp:Label ID="lb_info" runat="server" ForeColor="Red" Width="183px"></asp:Label></td>
                 
            </tr>


        </table>


    </form>
</body>
</html>
