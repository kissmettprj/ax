<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="axweb._Default" %>
<%@ Import Namespace="Newtonsoft.Json"  %>
<%@ Import Namespace="System.Collections.Generic" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>安心工具包</title>
    <link rel="shortcut icon" href="favicon.ico" /> 
     
</head>
<body>
    <form id="form1" runat="server">
      

    <TABLE>
        <tr><td colspan="3" align="center">
            <font color="#ff0000" size="16"><b>安心工具包</b></font><br>
            <!-- www.axtools.net 域名备案中 -->
            <hr />
            </td></tr>
     <TR valign="top">
          <!--------- 第一列 --------->
	    <TD  align="left">  

            

    <TABLE>

 

     <TR>

	    <TD colspan=2 align="left"><font color="#ff0000"><b>代表选举</b></font><hr>
            <button onclick="window.location.href='vote/index.aspx';return false;" style="height: 65px; width: 350px" ><font color="blue" size="5"><b>进 行 投 票</b></font><br>
                <font color="#ff0000"><b>安友身份验证后的投票</b></font>
            </button>  <br>   
            
            <hr>


	    </TD>
	     
    </TR> 

     <TR>
	    <TD colspan=2 align="left"><font color="#ff0000"><b>安友验证</b></font><hr>
            <button onclick="window.location.href='validate/MyValidCode.aspx';return false;" style="height: 65px; width: 350px" ><font color="blue" size="5"><b>我的验证码</b></font><br>
                <font color="#ff0000"><b>保护隐私下的安友身份验证码生成</b></font>
            </button><br> <br>   
            
            <button onclick="window.location.href='validate/ValidateCode.aspx';return false;" style="height: 65px; width: 350px" ><font color="blue" size="5"><b>安 友 验 证</b></font><br>
                <font color="#ff0000"><b>保护隐私下的安友身份验证</b></font>
            </button><hr>


	    </TD>
	     
    </TR> 
     <TR>
	    <TD colspan=2 align="left"><font color="#ff0000"><b>一键下载</b></font><hr></TD>
	     
    </TR> 
      <TR>        
	    <TD colspan=2>未下载合同的用户:<br>可以通过axd用户名密码登录,获取所有pdf合同/合同摘要/借款人及借款明细;<br>

           <a href="dataimg/axspiderol_userguide.doc" target="_blank">&lt; 使用说明 &gt;</a><br>
            系统下载合同后,将基于合同生成 合同信息.csv和借款人信息.csv ;<br>
            这两份文件将与所有pdf打一zip包中; <br>
            <font color="#ff0000"><b>声明:<br>系统将保留从您合同中导出的 如上两份csv文件 做统一的分析;<br>
            使用此功能,等同于您认可并授权以上做法;<br></b></font>

	    </TD>
    </TR>
     <TR>
	    <TD colspan=2 align="left"><hr>
            <button onclick="alert('安心贷web宕机,无法使用.');return false;window.location.href='tools/mycontracts.aspx';return false;" style="height: 65px; width: 350px" ><font color="blue" size="5"><b>开 始 使 用</b></font><br>
                <font color="#ff0000"><b>资源有限,排队原则</b></font>
            </button>
            
	    </TD>
	     
    </TR>   
     
    <TR style="display:none;">
	    <TD>安心用户名:</TD>
	    <TD>
        <asp:TextBox ID="txtUserName" runat="server" ></asp:TextBox>
        <asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" ControlToValidate="txtUserName" 
                runat="server" ErrorMessage="用户名不可空"></asp:RequiredFieldValidator></TD>
    </TR>
    <TR style="display:none;">
        
	    <TD >密码:</TD>
	    <TD >
        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" ></asp:TextBox>
        <asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" ControlToValidate="txtPassword" 
                runat="server" ErrorMessage="密码不可空"></asp:RequiredFieldValidator></TD>
    </TR>
    <TR  style="display:none;">
        
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
   <TR style="display:none;">
        
	    <TD valign="top">邀请码:</TD>
	    <TD>
        <asp:TextBox ID="txtInviteCode"  runat="server" Text="天地辽阔相遇难得" ReadOnly="true"></asp:TextBox> 
            暂未开启 <br><img src="dataimg/qrcode.png" width="170px"  /> 向我索要
            
           
        <asp:RequiredFieldValidator
                ID="RequiredFieldValidator4" ControlToValidate="txtInviteCode" 
                runat="server" ErrorMessage="邀请码不可空"></asp:RequiredFieldValidator>


           
	    </TD>
    </TR>    
    <TR style="display:none;">
	    <TD colspan=2 align="left">
            
            <asp:Button ID="btnOK" runat="server" Text=" 登 录 " onclick="btnOK_Click" /> [<span title="
1.验证你是真实安友;
2.下载'你'的数据;
                "
                >为何要登录?</span>]


	    </TD>
	     
    </TR> 
    <TR style="display:none;">
        
	    <TD colspan=2><font color="#ff0000"><asp:Literal ID="lMsg" runat="server" ></asp:Literal></font></TD>
    </TR>



    <TR style="display:none;">

	    <TD colspan=2 align="left">
            <hr />
            <font color="#ff0000"><b>一键下载(单机版)</b></font><hr></TD>
	     
    </TR> 
      <TR style="display:none;">        
	    <TD colspan=2> 暂不提供
	    </TD>
    </TR>
  </TABLE>

	   </TD>
        <!--    ------- 第二列 ------ --->
	    <TD>
 


 <TABLE>
    <TR>
	    <TD colspan=2 align="left"><font color="#ff0000"><b>一键导出</b></font><hr></TD>
	     
    </TR> 

    <TR>
        
	    <TD colspan=2> 已下载合同的用户:<br>可以下载<a href="exe/axpdf.v3.zip" target="_blank">单机工具</a>,自行导出借款人及借款明细;<br>
            <hr />
            请参考使用说明;<br>
            解压axpdf.v3.zip内所有文件到pdf合同目录;<br>
            双击运行axpdf.exe;<br>
            <hr />
            另,<b>一键下载</b>已经包含<b>一键导出</b>功能,其zip中包含此借款人csv;
            <hr />
            该版本扩展支持:我的本金/每期利息;<br>可用于与银行流水的对账;<br>
            <img src="dataimg/jkrcsv.png" width="550px"  /><br>
            


	    </TD>
    </TR>
     <TR>
	    <TD colspan=2 align="left"><hr><font color="#ff0000"><b>导入借款人</b></font><hr></TD>
	     
    </TR>    
    <TR>
        
	    <TD colspan=2> 
            希望你在使用<b>上述版本</b>的axpdf导出借款人信息后,将csv提交上来;<br>
            我汇集大家的借款人进行分析;<br>
            请使用下面的功能进行提交;<br />
            注:<b>一键下载</b>已经包含<b>导入借款人</b>功能,不需要再次导入;<br />
            
      
            

	    </TD>
    </TR>
     <TR>
	    <TD colspan=2 align="left"><hr>
            <button onclick="window.location.href='tools/importborrowercsv.aspx';return false;" style="height: 65px; width: 350px"><font color="blue" size="5"><b>开 始 使 用</b></font><br>

               <font color="#ff0000"><b>动动手指,即可聚沙成塔集腋成裘,勿以善小而不为</b></font>
            </button>
            
	    </TD>
	     
    </TR>   
     
 </TABLE>

	   </TD>
         <!--------- 第三列 --------->
	    <TD>
 


 <TABLE>
    <TR>

	    <TD colspan=2 align="left" valign="top">   
            <!----><font color="#ff0000"><b>绑定微信/QQ登录</b></font>
            <hr>
            
            <button onclick="window.location.href='qq/default.aspx';return false;" style="height: 75px; width: 350px" align="center" >
                <table>
                    <tr>
                        <td>
                            <%if(Session["$qquser"]==null){ %>
                            <img  height=50px" src="data:image/jpeg;base64,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" />
                            <%}else{ 
                                  string qquserjson = Session["$qquser"].ToString();
                                  Dictionary<string, object> o = JsonConvert.DeserializeObject<Dictionary<string, object>>(qquserjson);
                                  string headurl = o["figureurl_1"].ToString() ;
                            %>
                            <img  height=50px" src="<%Response.Write(headurl); %>" />
                            <%} %>
                        </td>
                        <td><font color="blue" size="5"><b>绑定QQ登录</b></font><br>
                <font color="#ff0000"><b>绑定QQ,下次直接QQ登录,免安心登录;</b></font></td>
                    </tr>
                </table>
                
            </button>  
            <br><br>
          
            
            <button onclick="alert('微信开放平台权限申请中...');" style="height: 75px; width: 350px" align="center" >
                <table>
                    <tr>
                        <td>
                            <img  height="50px" src="data:image/jpg;base64,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" />

                        </td>
                        <td><font color="blue" size="5"><b>绑定微信登录</b></font><br>
                <font color="#ff0000"><b>绑定微信,下次直接微信登录,免安心登录;</b></font></td>
                    </tr>
                </table>
                
            </button>   

            <br>
            <!--
            <img onclickk="window.location.href='qq/default.aspx';return false;"  height="65px" src="http://qzonestyle.gtimg.cn/qzone/vas/opensns/res/img/Connect_logo_5.png" />
            <br>
           -->
            


            
            <hr>


	    </TD>
	     
    </TR> 
     <!--
        <TR>
	    <TD colspan=2 align="left"><font color="#ff0000"><b>意见反馈</b></font><hr>
            ax合同几经修改,解析时难免不能完全适配,<br>若遇到问题请反馈至公众号:<br>
            <img src="dataimg/qrcode_for_gh_c75ded8e6ea6_258.jpg"   /><br>
            反馈问题仅限使用及技术问题;<br>
            对问题请图文描述提供出错pdf等,珍惜彼此的时间;<br>
            <font color="#ff0000">本公众号定期发布各程序的更新信息;</font>
	    </TD>
	     
    </TR> 
    -->
        
     <TR>
	    <TD colspan=2 align="left"><hr><font color="#ff0000"><b>声明</b></font><hr>
           <font color="#ff0000">本站不发起任何形式的众筹;<br>
           所有利用本站发起的众筹行为皆欺诈;<br>
            </font>
           维权不违法;<br>
           证据搜集不停息; <br>
           为政府/公安部门提供线索; <br>
	    </TD>
	     
    </TR> 
    
     <TR>
	    <TD colspan=2 align="left"><hr><font color="#ff0000"><b>开源地址</b></font><hr>
            
          本项目源代码开源于github:<br>     
           
             <svg height="32"   viewBox="0 0 16 16" version="1.1" width="32" aria-hidden="true"><path fill-rule="evenodd" d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0 0 16 8c0-4.42-3.58-8-8-8z"></path></svg>
	    <a href="https://github.com/kissmettprj/ax" target="_blank">https://github.com/kissmettprj/ax</a>
            
              </TD>
	     
    </TR> 
         </TABLE>  
	    </TD>
	     
    </TR> 

        <TR>
	    <TD colspan=3 align="center"><hr>
            <font color="#0000ff" size="5"><b>
            让我们认识生活的真相,并依然热爱它
            <!--
             Il n'ya qu'un héroïsme au monde : c'est de voir le monde tel qu'il est et de l'aimer. 
        --Romain Rolland
                -->
	        </b></font> 
            <br />
            <br />
            <a href="http://www.miitbeian.gov.cn/" target="_blank">京ICP备18059295号</a> 
	    </TD>
	     
    </TR>       
  </TABLE>  


    </form>
</body>
</html>
