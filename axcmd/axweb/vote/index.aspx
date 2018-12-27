<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="axweb.vote.index" %>
<%@ Import Namespace="com.kissmett.Common" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="axfunc" %>


    <script runat="server">  
    
           protected string[] getCandidatesRes() {
               //string candidateNames = "";// " [\"周华健\", \"周星驰\", \"周润发\", \"周杰伦\", \"周芷若\", \"张无忌\",\"张三丰\"]";
               string[] res = new string[2];
               using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
               {
                   //string username = Functions.ParseStr(Session["username"]);
                   res=AXDal.getCandidatesVoteRes(conn);
                   
               }
               return res;
           }
    </script>  
<%

    
   

 
        
%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>投票</title>
    <link rel="shortcut icon" href="../favicon.ico" /> 
     <script src="//cdnjs.cloudflare.com/ajax/libs/echarts/4.1.0/echarts-en.min.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
   

<TABLE>
    <TR>
        
	    <TD colspan=2>
            
            <font color="#ff0000" size="12"><b>代表选举</b></font>     

            <button onclick="window.location.href='../default.aspx';return false;">回主页</button>
            <hr />


            当前选举结果:<br>
            <font color="#ff0000"><b>总投票人数: <asp:Literal ID="lVoterCount" runat="server"></asp:Literal > 人</b></font>   <br>
            <font color="#ff0000"><b>总投票人待收金额: <asp:Literal ID="lTotalFee" runat="server"></asp:Literal > 元</b></font>   <br>
            候选人当前得票:<br>

            <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="main" style="width: 600px;height:450px;"></div>
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('main'));

        // 指定图表的配置项和数据
        var option = {
            title: {
                text: '代表选举'
            },
            tooltip: {},
            legend: {
                data:['得票数']
            },
            xAxis: {
                //data: ["周华健", "周星驰", "周润发", "周杰伦", "周芷若", "张无忌","张三丰"]
                data: <%=getCandidatesRes()[0] %> ,
                axisLabel: {
                    interval:0,
                    rotate:45
                }
                },
           
            yAxis: {},
            series: [{
                name: '得票',
                type: 'bar',
                //data: [5, 20, 36, 10, 10, 20,15]
                data: <%=getCandidatesRes()[1] %>
            }]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
    </script>

             
	    </TD>
    </TR>
    <TR>
        
	    <TD colspan=2>
 <script type="text/javascript">
  <!--
     function govote(){
         if(confirm("提醒:\r本功能需要用您的axd的用户名密码登录;\r会记录您的用户名作为用户标识;\r会读取并记录您的待收金额用以统计;\r不会记录您的密码,不会因此泄密;\r本站已工信部icp实名备案,备案号 京ICP备18059295号 ,\r 可在工信部域名备案系统查询 http://www.miitbeian.gov.cn ;\r\r若您对此站点不能充分信任,可以:\r1.不使用该功能;\r2.或者在使用本功能前后修改axd密码;\r3.据有些安友反映:在2018-11-1?之前,axd修改了密码错误锁定策略,\r从以前'5次错误锁定',修改为'1次错误锁定',请大家确认密码输入正确后登录;\r\r您确认使用该功能否?")){
             window.location.href='vote.aspx';
		}
		
     return false;
	
     }
    //-->
  </script>
            
        <button onclick="govote();return false;" style="height: 65px; width: 350px" ><font color="blue" size="5"><b>我 要 投 票</b></font><br>
                <font color="#ff0000"><b>安友身份验证后的投票,防伪造,防重复</b></font>
            </button>     
             <br>
            说明:<br>
            一次投票可勾选多人;<br>
            在投票截止前,可修改投票,每一次的修改将覆盖你前面的所有投票;<br>
            一个帐号最终只有一次投票有效(不管修改几次);<br>            
            <br>

            2018-12-26: 若干名安友反馈登录时出现问题,大体分两类:<br> 
            1.点击登录后长时间等待(超时);2.点击登录后报用户名密码错误(报错); <br> 
            若是1超时,则请过会再试;若是2报错,则请查准您的密码;
            <br /> <br />
            可选操作:
            <br />
            大家可以使用绑定qq功能,免去次次去axd验证的麻烦(绑定时需要走一次axd认证 以后可以绕开axd认证)<br>
            <a href="../dataimg/绑定QQ.doc" target="_blank">点此处看使用方法</a>.



	    </TD>
    </TR>
</TABLE>



    </form>
</body>
</html>
