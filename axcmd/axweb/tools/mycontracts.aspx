<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mycontracts.aspx.cs" Inherits="axweb.tools.mycontracts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>我的合同</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <TABLE>
    <TR>
        
	    <TD colspan=2>
            
                        <hr />
            <font color="#ff0000"><b>我的合同[<asp:Literal ID="txtUserName" runat="server"></asp:Literal>]</b></font>

            <button onclick="window.location.href='../default.aspx';return false;">回主页</button>
                        <hr />



	    </TD>
    </TR>
        
 
         
    <TR>
	    <TD colspan=2>
       <font color="#ff0000"><b>www.anxin.com服务异常,本功能无法使用</b></font><br>
            
<asp:Button ID="btnOK" runat="server" Text="点击此按钮开始下载任务排队" OnClick="btnOK_Click" Enabled="false" /><br>
            按钮灰掉代表任务已经提交,无需再次点击;<br>
            因资源限制,每人下载任务只执行一次;<br>
            可关闭浏览器,后台任务排期不会因浏览器的关闭而停止;<br>  
            任务将在后台排队,请等待[等待时间因前续排队具体任务决定不可估计]...<br>
            <b>下载任务开始后,后台程序将登录axd,先将合同下载到本服务器</b>(为何不直接下到你的机器,因为Web版本做不到)<br>
            不定期回到此页面查看即可;
            <hr>

            排队人数:<asp:Literal ID="lQueueMsgCnt" runat="server" ></asp:Literal>人<br>
            合同总数:<asp:Literal ID="lContractCount" runat="server" ></asp:Literal> <-- 若此处为0,代表前面有排队者,当你的下载任务开始时,会获取到该值;<br>
            已下载数:<asp:Literal ID="lDownloadedCount" runat="server" ></asp:Literal><br>
            下载时间估算:<asp:Literal ID="lDownloadMinutes" runat="server" ></asp:Literal>分钟<br>
            若在2018-11-2之后,没有再次下载过合同,每个合同下载时间平均为66s;<br>
            第二次下载会很快,0.5s;<hr>
            
            下载任务完成后,下面的功能将可用;<br>
            获取[所有合同及CSV] --- 下载打为zip包的pdf合同文件+合同信息.csv+借款人信息.csv;<br>
            清空本站缓存的[所有合同] --- 清空您在本站的所有pdf合同和zip包;注意清空后因资源限制,无法再次使用下载功能;<br>
            获取[合同信息CSV文件] --- 系统保留此CSV,您可永久获取;<br>
            获取[借款人信息CSV文件] --- 系统保留此CSV,您可永久获取;<br>
            <hr>

            下面的按钮可点击时,即可获取相应内容;  
	    </TD>
	     
    </TR> 
     
    <TR>
	    <TD colspan=2>
            <asp:Button ID="btnDownloadAll" runat="server" Text="获取 [所有合同及CSV] " OnClick="btnDownloadAll_Click" BackColor="#99CCFF"   />  
            &nbsp;  
            <asp:Button ID="btnClean" runat="server" Text="清空本站缓存的 [所有合同] " BackColor="#99CCFF" OnClick="btnClean_Click"   /> 
            <font color="red"><asp:Literal ID="lMsg" runat="server" ></asp:Literal></font>
             <br>
            
            示例:<br>
            <img src="../dataimg/contractzip.png" width="160px"  />


	    </TD>
	     
    </TR> 
     
    <TR>
	    <TD colspan=2><asp:Button ID="btnDownloadContractCSV" runat="server" Text="获取 [合同信息CSV文件] " OnClick="btnDownloadCSV_Click" BackColor="#99CCFF"   /><br>
            示例:<br>
            <img src="../dataimg/contractlist.png" width="300px"  />

	    </TD>
	     
    </TR> 
     
    <TR>
	    <TD colspan=2><asp:Button ID="btnDownloadJKRCSV" runat="server" Text=" 获取 [借款人信息CSV文件] " OnClick="btnDownloadJKRCSV_Click" BackColor="#99CCFF"   /><br>
	      示例:<br>
          <img src="../dataimg/jkrcsv.png" width="300px"  />

	    </TD>
    </TR> 
    
    <TR>
        
	    <TD colspan=2>
            <button style="background-color:#99CCFF;" onclick="window.open('http://anxin.com/down.html?Cmd=mhistroy&start=2008-10-25&end=2018-10-26&typeID=0');">一键下载安心流水</button> <br>
           http://anxin.com/down.html?Cmd=mhistroy&start=2008-10-25&end=2018-10-26&typeID=0 <br>
        可以通过这个链接一键下载安心流水;
            

	    </TD>
    </TR>
    </TABLE>
    </div>
    </form>
</body>
</html>
