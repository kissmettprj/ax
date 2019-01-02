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

           protected string[] getVoteProgress_Hour()
           {
               
               string[] res = new string[2];
               using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
               {
                   //string username = Functions.ParseStr(Session["username"]);
                   res = AXDal.getVoteProgress_Hour(conn);
                   
               }
               return res;
           }


           protected string[] getVoterDistribute()
           {
                
               string[] res = new string[2];
               using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
               {
                   //string username = Functions.ParseStr(Session["username"]);
                   res = AXDal.getVoterDistribute(conn);
                   
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
    <script type="text/javascript"> 
        window.onload=function(){  
            setInterval(function(){   
                var date=new Date();   
                var year=date.getFullYear(); //获取当前年份   
                var mon=date.getMonth()+1; //获取当前月份   
                var da=date.getDate(); //获取当前日   
                var day=date.getDay(); //获取当前星期几   
                var h=date.getHours(); //获取小时   
                var m=date.getMinutes(); //获取分钟   
                var s=date.getSeconds(); //获取秒   
                var d=document.getElementById('Date');    
                if(h < 10) h='0'+h;
                if(m < 10) m='0'+m;
                if(s < 10) s='0'+s;
                //d.innerHTML=''+year+'年'+mon+'月'+da+'日'+ '星期'+day+' '+h+':'+m+':'+s;  
                d.innerHTML=''+year+'年'+mon+'月'+da+'日'+' '+h+':'+m+':'+s;  
            }
            ,1000)  
        }
</script>
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

            <font color="#ff0000"><b>当前时间: <span id="Date"> </span>  </b></font>   <br>
            <font color="#ff0000"><b>投票总人数: <asp:Literal ID="lVoterCount" runat="server"></asp:Literal > 人</b></font>   <br>
            <font color="#ff0000"><b>投票人待收总金额: <asp:Literal ID="lTotalFee" runat="server"></asp:Literal > 元</b></font>   <br>
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
                label: {
                    normal: {
                        show: true,
                        position: 'top',
                        textStyle: {
                            color: 'red'
                        }
                    }
                },
                //data: [5, 20, 36, 10, 10, 20,15]
                data: <%=getCandidatesRes()[1] %>
            }]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);


        myChart.on('click', function (params) {
            // 控制台打印数据的名称
            console.log(params.name);
        });
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
            <a href="../dataimg/绑定QQ.htm" target="_blank">点此处看使用方法</a>.

	    </TD>
    </TR>


    <TR>        
	    <TD colspan=2>
            
          <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="myChart_voteprogress" style="width: 1000px;height:450px;"></div>
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart_voteprogress = echarts.init(document.getElementById('myChart_voteprogress'));

        // 指定图表的配置项和数据
        var option_voteprogress = {
            title: {
                text: '投票进程'
            },
            tooltip: {},
            legend: {
                data:['投票人数']
            },
            xAxis: {
                //data: ["周华健", "周星驰", "周润发", "周杰伦", "周芷若", "张无忌","张三丰"]
                data: <%=getVoteProgress_Hour()[0] %> ,
                axisLabel: {
                    interval:1,
                    rotate:45,
                    fontSize:10
                }
            },
           
            yAxis: {},
            series: [{
                name: '投票人数',
                type: 'bar',
                label: {
                    normal: {
                        show: true,
                        position: 'top',
                        textStyle: {
                            color: 'red',
                            
                        }
                    }
                },
                //data: [5, 20, 36, 10, 10, 20,15]
                data: <%=getVoteProgress_Hour()[1] %>
                }]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart_voteprogress.setOption(option_voteprogress);


        
    </script>           


	    </TD>
    </TR>



    <TR>        
	    <TD colspan=2>
            
          <!-- 为ECharts准备一个具备大小（宽高）的Dom -->
    <div id="myChart_voterdistribute" style="width: 600px;height:450px;"></div>
    <script type="text/javascript">
        // 基于准备好的dom，初始化echarts实例
        var myChart_voterdistribute = echarts.init(document.getElementById('myChart_voterdistribute'));

        // 指定图表的配置项和数据
        var option_voterdistribute = {
            title: {
                text: '待收分布'
            },
            tooltip: {},
            legend: {
                data:['人数']
            },
            xAxis: {
                //data: ["周华健", "周星驰", "周润发", "周杰伦", "周芷若", "张无忌","张三丰"]
                data: <%=getVoterDistribute()[0] %> ,
                axisLabel: {
                    interval:0,
                    rotate:45,
                    fontSize:10
                }
            },
           
            yAxis: {},
            series: [{
                name: '人数',
                type: 'bar',
                label: {
                    normal: {
                        show: true,
                        position: 'top',
                        textStyle: {
                            color: 'red',
                            
                        }
                    }
                },
                //data: [5, 20, 36, 10, 10, 20,15]
                data: <%=getVoterDistribute()[1] %>
                }]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart_voterdistribute.setOption(option_voterdistribute);


        
    </script>           


	    </TD>
    </TR>





</TABLE>



    </form>
</body>
</html>
