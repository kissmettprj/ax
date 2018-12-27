using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using com.kissmett.Common;

using axfunc;

namespace axweb.vote
{
    public partial class vote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ( Session["username"] == null )
            { //未登录
                //Session["backurl"] = "vote/vote.aspx";
                Response.Redirect("../Login.aspx?backurl=vote/vote.aspx");
                return;
            }
            if (!this.IsPostBack)
            {
                string username = Functions.ParseStr(Session["username"]);
                string daishou = Functions.ParseStr(Session["daishou"]);
                this.txtUserName.Text = username + " 待收: " + daishou + "元";
                initCandidates();
                bindCandidates();
                bindSign();
            }

        }
        void initCandidates() {
            cblCandidates.Items.Clear();
            //cblCandidates.Items.Add(new ListItem("章嘉(内蒙古鄂尔多斯)", "章嘉(内蒙古鄂尔多斯)"));
            //cblCandidates.Items.Add(new ListItem("梦久旅人(江苏连云港)", "梦久旅人(江苏连云港)"));
            //cblCandidates.Items.Add(new ListItem("solomon(北京)", "solomon(北京)"));
            //cblCandidates.Items.Add(new ListItem("刘平之(北京)", "刘平之(北京)"));
            //cblCandidates.Items.Add(new ListItem("赵律师(河北保定)", "赵律师(河北保定)"));
            //cblCandidates.Items.Add(new ListItem("bana(北京)", "bana(北京)"));
            //cblCandidates.Items.Add(new ListItem("春暖花开(陕西汉中)", "春暖花开(陕西汉中)"));
            //cblCandidates.Items.Add(new ListItem("流水(辽宁沈阳)", "流水(辽宁沈阳)"));
            //cblCandidates.Items.Add(new ListItem("刘XH(黑龙江大庆)", "刘XH(黑龙江大庆)"));
            //cblCandidates.Items.Add(new ListItem("微微(山东济南)", "微微(山东济南)"));
            //cblCandidates.Items.Add(new ListItem("景敏(山东菏泽)", "景敏(山东菏泽)"));
            //cblCandidates.Items.Add(new ListItem("和煦阳光(黑龙江绥棱)", "和煦阳光(黑龙江绥棱)"));
            //cblCandidates.Items.Add(new ListItem("洪拳(上海)", "洪拳(上海)"));
            //cblCandidates.Items.Add(new ListItem("芒果(上海)", "芒果(上海)"));
            //cblCandidates.Items.Add(new ListItem("上海Tina(上海)", "上海Tina(上海)"));
            //cblCandidates.Items.Add(new ListItem("泡菜(上海)", "泡菜(上海)"));
            //cblCandidates.Items.Add(new ListItem("小陈儿(河南郑州)", "小陈儿(河南郑州)"));
            //cblCandidates.Items.Add(new ListItem("浙江王先生(浙江舟山)", "浙江王先生(浙江舟山)"));
            //cblCandidates.Items.Add(new ListItem("沈民强131smj(浙江)", "沈民强131smj(浙江)"));
            //cblCandidates.Items.Add(new ListItem("阿辉(甘肃兰州)", "阿辉(甘肃兰州)"));
            //cblCandidates.Items.Add(new ListItem("开心超人(湖北武汉)", "开心超人(湖北武汉)"));
            //cblCandidates.Items.Add(new ListItem("斑纹猫(湖北武汉)", "斑纹猫(湖北武汉)"));
            //cblCandidates.Items.Add(new ListItem("瘦瘦(湖北武汉)", "瘦瘦(湖北武汉)"));
            //cblCandidates.Items.Add(new ListItem("七彩阳光(湖北武汉)", "七彩阳光(湖北武汉)"));
            //cblCandidates.Items.Add(new ListItem("清者自清(河北沧州)", "清者自清(河北沧州)"));
            //cblCandidates.Items.Add(new ListItem("青藤(河北邯郸)", "青藤(河北邯郸)"));
            //cblCandidates.Items.Add(new ListItem("梓彤(内蒙古包头)", "梓彤(内蒙古包头)"));
            //cblCandidates.Items.Add(new ListItem("海豹(甘肃兰州)", "海豹(甘肃兰州)"));
            //cblCandidates.Items.Add(new ListItem("宗华(北京)", "宗华(北京)"));
            //cblCandidates.Items.Add(new ListItem("fengt(福建)", "fengt(福建)"));
            //cblCandidates.Items.Add(new ListItem("果粒妈妈(安徽马鞍山)", "果粒妈妈(安徽马鞍山)"));
            //cblCandidates.Items.Add(new ListItem("最阳光的笑脸(湖北武汉)", "最阳光的笑脸(湖北武汉)"));
            //cblCandidates.Items.Add(new ListItem("超(安徽合肥)", "超(安徽合肥)"));
            //cblCandidates.Items.Add(new ListItem("蟹老板(安徽马鞍山)", "蟹老板(安徽马鞍山)"));
            //cblCandidates.Items.Add(new ListItem("罗定才", "罗定才"));
            //cblCandidates.Items.Add(new ListItem("阳光人生", "阳光人生"));
            //cblCandidates.Items.Add(new ListItem("盛伟东(浙江绍兴)", "盛伟东(浙江绍兴)"));
            //cblCandidates.Items.Add(new ListItem("廖小略(广东深圳)", "廖小略(广东深圳)"));
            //cblCandidates.Items.Add(new ListItem("安爵(广东深圳)", "安爵(广东深圳)"));
            //cblCandidates.Items.Add(new ListItem("张变香(江苏无锡)", "张变香(江苏无锡)"));
            //cblCandidates.Items.Add(new ListItem("无锡张女士(江苏无锡)", "无锡张女士(江苏无锡)"));
            //cblCandidates.Items.Add(new ListItem("lq(山东济南)", "lq(山东济南)"));
            //cblCandidates.Items.Add(new ListItem("祥子(山东菏泽)", "祥子(山东菏泽)"));
            //cblCandidates.Items.Add(new ListItem("曹先生(湖南长沙)", "曹先生(湖南长沙)"));
            //cblCandidates.Items.Add(new ListItem("唐th521qqy(湖南长沙)", "唐th521qqy(湖南长沙)"));
            //cblCandidates.Items.Add(new ListItem("王先生waxihua(湖南永州)", "王先生waxihua(湖南永州)"));

            cblCandidates.Items.Add(new ListItem("江苏梦久旅人", "江苏梦久旅人"));
            cblCandidates.Items.Add(new ListItem("内蒙章嘉", "内蒙章嘉"));
            cblCandidates.Items.Add(new ListItem("上海tina", "上海tina"));
            cblCandidates.Items.Add(new ListItem("陕西春暖花开", "陕西春暖花开"));
            cblCandidates.Items.Add(new ListItem("浙江王先生", "浙江王先生"));
            cblCandidates.Items.Add(new ListItem("河北赵律师", "河北赵律师"));
            cblCandidates.Items.Add(new ListItem("山东景敏", "山东景敏"));
            cblCandidates.Items.Add(new ListItem("广东安爵", "广东安爵"));
            cblCandidates.Items.Add(new ListItem("上海洪拳", "上海洪拳"));
            cblCandidates.Items.Add(new ListItem("山西阳光人生", "山西阳光人生"));

            cblCandidates.Items.Add(new ListItem("北京bana", "北京bana"));
            cblCandidates.Items.Add(new ListItem("北京Soloman", "北京Soloman"));
            cblCandidates.Items.Add(new ListItem("北京刘平之", "北京刘平之"));
            cblCandidates.Items.Add(new ListItem("北京战功累累", "北京战功累累"));
            cblCandidates.Items.Add(new ListItem("北京马姐", "北京马姐"));
            cblCandidates.Items.Add(new ListItem("北京花开花落", "北京花开花落"));
            cblCandidates.Items.Add(new ListItem("北京沛然(BTC-BCH)", "北京沛然(BTC-BCH)"));
            cblCandidates.Items.Add(new ListItem("北京小龙", "北京小龙"));
            cblCandidates.Items.Add(new ListItem("北京面包包(包发发)", "北京面包包(包发发)"));
            cblCandidates.Items.Add(new ListItem("北京tina", "北京tina"));



        }
        void bindCandidates() { 
            //get candidates from db

            using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
            {
                string username = Functions.ParseStr(Session["username"]);
                string candidates = AXDal.getMyCandidates(conn, username);
                WebControlHelper.SetCheckListChecked(cblCandidates,candidates,",");
            }
            
        }
        void bindSign()
        {
            string username = Functions.ParseStr(Session["username"]);
            string wsbase = Functions.GetAppConfigString("wsbase", "");
            string votedataws = wsbase + @"\votedata\";
            string signfile = votedataws + "sign_" + username + ".txt";
            string imgdata = MyFile.fileToString(signfile);

            this.txtSignImgData.Text = imgdata;
        }

        protected void btnVote_Click(object sender, EventArgs e)
        {

            // //check selected count;
            int cnt = WebControlHelper.GetCheckListCheckedCount(this.cblCandidates);
            if (cnt > 9)
            {
                lMsg.Text = "投票不能超过9人,请检查您的投票;";
                return;
            }

            btnVote.Enabled = false;

            string username = Functions.ParseStr(Session["username"]);


            string wsbase = Functions.GetAppConfigString("wsbase", "");
            string votedataws = wsbase + @"\votedata\";
            string imgdata = this.txtSignImgData.Text;
            string signfile = votedataws + "sign_"+username + ".txt";
            MyFile.stringToFile(imgdata, signfile);
            
            using (SqlConnection conn = new SqlConnection(Functions.GetAppConfigString("ConnectionString", "")))
            {


                string candidates = WebControlHelper.GetCheckListChecked(this.cblCandidates, ",");
               

                AXDal.vote(conn, username, candidates);
            }
            Response.Redirect("index.aspx");

            //Response.Write(selectedValues);
        }
    }
}