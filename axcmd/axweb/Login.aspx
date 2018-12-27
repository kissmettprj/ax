<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="axweb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
 <TR>
	    <TD colspan=2 align="left"><hr><font color="#ff0000"><b>登 录</b></font><hr></TD>
	     
    </TR> 
     
       
    <TR>
	    <TD>安心用户名:</TD>
	    <TD>
        <asp:TextBox ID="txtUserName" runat="server" ></asp:TextBox>
        <asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" ControlToValidate="txtUserName" 
                runat="server" ErrorMessage="用户名不可空"></asp:RequiredFieldValidator></TD>
    </TR>
    <TR>
        
	    <TD>密码:</TD>
	    <TD>
        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" ></asp:TextBox>
        <asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" ControlToValidate="txtPassword" 
                runat="server" ErrorMessage="密码不可空"></asp:RequiredFieldValidator></TD>
    </TR>
    <TR>
        
	    <TD>验证码:</TD>
	    <TD>
        <asp:TextBox ID="txtValiCode"  runat="server"></asp:TextBox>
            
            <img id="validatepic" src="getcode.aspx" border="0" align="absmiddle"
                                            title="点此刷新" style="cursor:pointer;" 
                                            onclick='javascript:this.src="getcode.aspx?rnd="+Math.random();' />
        <asp:RequiredFieldValidator
                ID="RequiredFieldValidator3" ControlToValidate="txtValiCode" 
                runat="server" ErrorMessage="验证码不可空"></asp:RequiredFieldValidator></TD>
    </TR>  
     <TR>
        
	    <TD colspan=2><font color="#ff0000"><asp:Literal ID="lMsg" runat="server" ></asp:Literal></font></TD>
    </TR>
    <TR>
	    <TD colspan=2 align="left">
            
            <asp:Button ID="btnOK" runat="server" Text=" 登 录 " onclick="btnOK_Click"  Width="200" Height="60" /> [<span title="
1.验证你是真实安友;
2.处理专属'你'的数据;
                "
                >为何要登录?</span>]


	    </TD>
	     
    </TR> 
    <TR>
        
	    <TD colspan=2>
			<font color="#ff0000">
				<b>提醒:</b><br>
                <b>
                    本功能需要用您的axd的用户名密码登录;<br />
                    会记录您的用户名作为用户标识;<br>
                    会读取并记录您的待收金额用以统计;<br>
                    不会记录您的密码,不会因此泄密;<br>
                    本站已工信部icp实名备案,备案号 京ICP备18059295号 , <br> 
                    可在工信部域名备案系统查询 http://www.miitbeian.gov.cn ;<br>
                </b><br>
                若您对此站点不能充分信任,可以:<br>
				1.不使用该功能;<br>
				2.或者在使用本功能前后修改axd密码;<br>
                3.据有些安友反映:在2018-11-1?之前,axd修改了密码错误锁定策略,<br>
                从以前"5次错误锁定",修改为"1次错误锁定",请大家确认密码输入正确后登录;

			</font>


	    </TD>
    </TR> 
  

    </TABLE>


    </form>
</body>
</html>
