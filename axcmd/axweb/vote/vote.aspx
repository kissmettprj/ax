<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vote.aspx.cs" Inherits="axweb.vote.vote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>投票</title>
    <script type="text/javascript"> 
 
    function vote() { 
         
        document.getElementById("btnVote").disabled = true;
        __doPostBack('btnVote', '');
        return true; 
       
    } 
    </script> 


    

  <script type="text/javascript">
  <!--
    window.onload = function () {
        var c = new lineCanvas({
            el: document.getElementById("canvas"),//绘制canvas的父级div
            clearEl: document.getElementById("clearCanvas"),//清除按钮
            saveEl: document.getElementById("saveCanvas"),//保存按钮
            //      linewidth:1,//线条粗细，选填
            //      color:"black",//线条颜色，选填
            //      background:"#ffffff"//线条背景，选填
        });

        dataUrl = document.getElementById("txtSignImgData").value;
        document.getElementById("SignImg").src = dataUrl;
        loadCanvas(c.cxt, dataUrl);
    };

    function loadCanvas(context,dataURL) {
        //var canvas = document.getElementById('myCanvas');
        //var context = canvas.getContext('2d');

        // load image from data url
        var imageObj = new Image();
        imageObj.onload = function () {
            context.drawImage(this, 0, 0);
        };

        imageObj.src = dataURL;
    }

    function lineCanvas(obj) {
        this.linewidth = 1;
        this.color = "#000000";
        this.background = "#ffffff";
        for (var i in obj) {
            this[i] = obj[i];
        };
        this.canvas = document.createElement("canvas");
        this.el.appendChild(this.canvas);
        this.cxt = this.canvas.getContext("2d");
        this.canvas.width = this.el.clientWidth;
        this.canvas.height = this.el.clientHeight;
        this.cxt.fillStyle = this.background;
        this.cxt.fillRect(0, 0, this.canvas.width, this.canvas.width);
        this.cxt.strokeStyle = this.color;
        this.cxt.lineWidth = this.linewidth;
        this.cxt.lineCap = "round";

        this.mouseCanDraw = false;
        this.canvasOffsetX = this.el.offsetLeft;
        this.canvasOffsetY = this.el.offsetTop;


        //开始绘制
        this.canvas.addEventListener("touchstart", function (e) {
            this.cxt.beginPath();
            x = e.changedTouches[0].pageX - this.canvasOffsetX;
            y = e.changedTouches[0].pageY - this.canvasOffsetY;
            this.cxt.moveTo(x, y);
        }.bind(this), false);
        this.canvas.addEventListener("mousedown", function (e) {
            this.cxt.beginPath();
            x = e.clientX - this.canvasOffsetX;
            y = e.clientY - this.canvasOffsetY;
            this.cxt.moveTo(x, y);
            this.mouseCanDraw = true;
        }.bind(this), false);

        //绘制中
        this.canvas.addEventListener("touchmove", function (e) {
            x = e.changedTouches[0].pageX - this.canvasOffsetX;
            y = e.changedTouches[0].pageY - this.canvasOffsetY;
            this.cxt.lineTo(x, y);
            this.cxt.stroke();
        }.bind(this), false);
        this.canvas.addEventListener("mousemove", function (e) {
            if (this.mouseCanDraw) {
                x = e.clientX - this.canvasOffsetX;
                y = e.clientY - this.canvasOffsetY;
                this.cxt.lineTo(x, y);
                this.cxt.stroke();
            }
        }.bind(this), false);
        //结束绘制
        this.canvas.addEventListener("touchend", function () {
            this.cxt.closePath();
        }.bind(this), false);
        this.canvas.addEventListener("mouseup", function () {
            this.mouseCanDraw = false;
            this.cxt.closePath();
        }.bind(this), false);
        //清除画布
        this.clearEl.addEventListener("click", function () {
            this.cxt.clearRect(0, 0, this.canvas.width, this.canvas.height);
            
        }.bind(this), false);
        //保存图片，直接转base64
        this.saveEl.addEventListener("click", function () {
            var imgBase64 = this.canvas.toDataURL();
            console.log(imgBase64);
            document.getElementById("SignImg").src = imgBase64;
            document.getElementById("txtSignImgData").value = imgBase64;
            
            document.getElementById('signBoard').style.display = 'none';
            document.getElementById('displayBoard').style.display = '';
        }.bind(this), false);

        document.getElementById('signBoard').style.display = 'none';

    };

    //-->
  </script>

</head>
<body>
    <form id="form1" runat="server">


<TABLE>
    <TR>
        
	    <TD colspan=2>
            
            <font color="#ff0000" size="12"><b>代表选举</b></font>      <hr />
            <font color="#ff0000"><b>[<asp:Literal ID="txtUserName" runat="server"></asp:Literal>]</b></font>

            <button onclick="window.location.href='../default.aspx';return false;">回主页</button>
                        <hr />


            请选择您支持做代表的安友:  <br>
            截止时间前可以修改自己的选择;最多选9人,否则系统有红色提示限制而提交失败;
            <br>
            
            <asp:CheckBoxList ID="cblCandidates" runat="server">
            </asp:CheckBoxList>

            <br />
            <!--
            签名:(可选)<br />
            <span id="signBoard"  >
            <div id="canvas" style="height:200px;width:400px;border:solid;border-width:1px;"></div>
            <input type="button" id="clearCanvas" value="清除" >
            <input type="button" id="saveCanvas" value="确认" >

            </span> 
            <span id="displayBoard" onclick="beginSign()">
            <input type="button" id="signCanvas" value="编辑签名"  >
            
 <script>
     //var iLineCanvas
     function beginSign() {
         document.getElementById('signBoard').style.display = '';
         document.getElementById('displayBoard').style.display = 'none';
     }
 </script>
            
            <br>
            <img id="SignImg"  style="height:200px;width:400px;border:solid;border-width:1px;" />
                </span>
             -->
            <asp:TextBox ID="txtSignImgData" runat="server" ClientIDMode="Static" style="visibility:hidden"></asp:TextBox>
           

	    </TD>


    </TR>
    <TR>
        
	    <TD colspan=2>
            <font color="#ff0000" size="6"><b><asp:Literal ID="lMsg" runat="server"></asp:Literal></b></font>
            <hr />
            <asp:Button ID="btnVote" runat="server" Text="确 认 投 票" onclick="btnVote_Click" UseSubmitBehavior="false" OnClientClick="return vote();" Width="200" Height="60" />
            <br>
            说明:<br>
            一次投票可勾选多人,最多9人;<br>
            在投票截止前,可修改投票,每一次的修改将覆盖你前面的所有投票;<br>
            一个帐号最终只有一次投票有效(不管修改几次);<br>





	    </TD>
    </TR>
</TABLE>

    </form>
</body>
</html>
