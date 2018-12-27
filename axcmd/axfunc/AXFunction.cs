using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;
using System.Collections;


namespace axfunc
{




    public enum 轻松投类型
    {
        进行中 = 0,
        退出中 = 1,
        已退出 = 2
    };
    //0:持有中;1:竞标中;2:已退出;
    public enum 自由投类型
    {
        持有中 = 0,
        竞标中 = 1,
        已退出 = 2
    };
    public class AXFunction
    {

        public static string coverInfo(string v){
            string res = "";
            if (v.Length <= 4) return v;
            res = v.Substring(0, 2) + "******" + v.Substring(v.Length - 2, 2);
            return res;
        }

        public static double getDaiShou(string content)
        {
            //string content = getUserCenter2016();
            if (content.Trim().Equals("")) return 0;
            Regex reg; Match match;
            reg = new Regex("<p class=\"black-font f14\" data-amount=\"(.+)\" id=\"countUp1\">");
            match = reg.Match(content);
            string s = match.Groups[1].Value.Trim();
            return Convert.ToDouble(s);

        }



        public static string getJiekuanrenCSVHeader() {
            string xlsheader;
            xlsheader = "借款协议编号" + ","
                + "姓名" + ","
                + "身份证号" + ","
                + "平台用户名" + ","
                + "借款用途" + ","
                + "借款本金数额" + ","
                + "年化利率" + ","
                + "起息日" + ","
                + "借款期限" + ","
                + "起" + ","
                + "止" + ","
                + "还款方式" + ","
                + "我的本金" + ","
                + "每期利息" + ","
                + "\n";
            return xlsheader;
        }
        public static string parseJiekuanrenCSVItemsfromContent(string content)
        {
            if (content.Trim().Equals("")) return "";
            Regex reg; Match match;

            reg = new Regex("借款协议\n 编号：(.+)\n");
            match = reg.Match(content);
            string cnum = match.Groups[1].Value.Trim();
            if (cnum.Equals(""))
            {
                Console.WriteLine("非借款协议,跳过");
                return "";
            }

            reg = new Regex("乙方（借款人）：\n姓名：(.+)\n身份证号：(.+)\n安心贷平台用户名：(.+)\n");
            match = reg.Match(content);
            string name = match.Groups[1].Value.Trim();
            string idn = match.Groups[2].Value.Trim();
            string username = match.Groups[3].Value.Trim();


            //reg = new regex("\n借款用途 (.+)\n借款本金数额 (.+) 各出借人借款本金数额详见本协议附件一\n"
            //    + "年化利率 (.+)\n起息日 (.+)\n借款期限 (.+)，(.+)起，至 (.+)止\n还款方式 (.+)\n");             
            reg = new Regex("\n借款用途(.*)\n借款本金数额(.+)各出借人借款本金数额详见本协议附件一\n"
                + "年化利率(.+)\n起息日(.+)\n借款期限(.+)，(.+)起，至(.+)止\n还款方式(.+)\n");
            match = reg.Match(content);
            string usage = match.Groups[1].Value.Trim();//借款用途
            string benjin = match.Groups[2].Value.Trim();//借款本金数额
            string rate = match.Groups[3].Value.Trim();//年化利率
            string qixiri = match.Groups[4].Value.Trim();//起息日 
            string qixian = match.Groups[5].Value.Trim();//借款期限
            string fromt = match.Groups[6].Value.Trim();//起
            string tot = match.Groups[7].Value.Trim();//止
            string fangshi = match.Groups[8].Value.Trim();//还款方式 


            //取关于自己的本金利息:取到最后若干行;再取得没有*的一行;
            reg = new Regex(@"\n平台用户名 姓名 身份证号 出借金额 每期应收本\n息([\s\S]+)");

            match = reg.Match(content);
            string lines = match.Groups[1].Value.Trim();//所有借款人行

            /*
             xup********** 徐鹏飞 4110**********4016 123.00 5.72\nxih******** 张新 4112**********551X 2331.00 108.63\n
             bsl***** 史卫芳 6103**********2562 1912.00 89.10\njsz****** 张建明 3304**********0539 2269.00 105.74\n
             wsy******* 樊军 4104**********0031 2098.00 97.78\nxup********** 徐鹏飞 4110**********4016 892.00 41.56\n
             ddx****** 易世碧 5122**********1423 2185.00 101.82\n492****** 吕幼霞 3306**********6642 13861.00 646.02\n
             800 金凯 3303314 10000.00 466.07\nben***** 李斌 4407**********421X 17000.00 792.32\n
             186******** 张志强 1201**********2010 993.00 46.27
             */
         
            string[] arrLines = lines.Split("\n".ToCharArray());

            //有例子: //Axaa0cc28018051\n5\n雷继忠 320103236 2735.00 920.80\n
            //ken**** ZHAO\nKANG\nGA29**** 6223.00 549.99
            //在分页时有这种情况: sui******** 尹志强 1403**********1510 157.00 26.78187******** 李艳坤 2323**********0060 154.00 26.27\n
            //总结上面两个例子,可以进行"拼行"预处理:若不含空格就拼到一起,若含有空格就前面加空格拼到一起;
            ArrayList newLines = new ArrayList();
            string realline = "";
            foreach (string line in arrLines){
                string l = line.Trim(); 
                if (l.Split(" ".ToCharArray()).Length == 5)
                {
                    newLines.Add(l);
                    realline = "";
                    continue;
                }
                //如果超过5个
                if (l.Split(" ".ToCharArray()).Length > 5)
                {
                    string leftpart = procContractPDFContentChujierenItem_MoreThan5(ref newLines, l);
                    realline = leftpart;
                    continue;
                }                

                //若不能分成5份就要拼起来;
                if (l.IndexOf(" ") < 0)//若不含空格,则直接拼到前面;
                {
                    realline +=l;
                }
                else //若包含空格,则前面加空格后拼到前面
                {
                    realline += " " + l;
                }
                if (realline.Trim().Split(" ".ToCharArray()).Length == 5)
                {
                    newLines.Add(realline.Trim());
                    realline = "";
                }
            }

            //筛选出自己那行
            string ownline = "";
            foreach (string line in newLines)
            {
                if (line.IndexOf("*") < 0)
                {
                    ownline = line;
                    break; 
                }
            }

            //Axaa0cc28018051\n5\n雷继忠 320103236 2735.00 920.80\n 对于这种情况需要特殊处理;
            //按照上面的逻辑ownline将会是 雷继忠 320103236 2735.00 920.80;缺少一个元素;
            string[]  arrOwnLine = ownline.Split(" ".ToCharArray());
           
            string username2 = arrOwnLine[0];//可能跟登录名不一致,有
            string realname = arrOwnLine[1];
            string id = arrOwnLine[2];
            string ownbenjin = arrOwnLine[3];
            string lixi = Common.GetNumberFromString(arrOwnLine[4]);



            Console.WriteLine("借款协议编号:" + cnum
                + ";姓名:" + name
                + ";身份证号:" + idn
                + ";平台用户名:" + username
                + ";借款用途:" + usage
                + ";借款本金数额:" + benjin
                + ";年化利率:" + rate
                + ";起息日:" + qixiri
                + ";借款期限:" + qixian
                + ";起:" + fromt
                + ";止:" + tot
                + ";还款方式:" + fangshi
                + ";我的本金:" + ownbenjin
                + ";我的利息:" + lixi
                + ";");
            string res = cnum + ","
                + name + ","
                + idn + ","
                + username + ","
                + usage + ","
                + benjin + ","
                + rate + ","
                + qixiri + ","
                + qixian + ","
                + fromt + ","
                + tot + ","
                + fangshi + ","
                + ownbenjin + ","
                + lixi + ","
                ;
            return res;
        }

        //处理这种情况:
        //在分页时有这种情况: sui******** 尹志强 1403**********1510 157.00 26.78187******** 李艳坤 2323**********0060 154.00 26.27\n
        //将前5项目处理到清洗数组newLines数组,
        //将最后剩下不足4块的部分返回;用于后续的拼接;
        static string procContractPDFContentChujierenItem_MoreThan5(ref ArrayList newLines, string line) {
            string l = line;
            //如果超过5个
            while (l.Split(" ".ToCharArray()).Length >= 5)
            {
                string[] vs = l.Split(" ".ToCharArray());
                string v = vs[0]
                    + " " + vs[1]
                    + " " + vs[2]
                    + " " + vs[3]
                    ; //sui******** 尹志强 1403**********1510 157.00
                //取出前两位小数
                string v4 = vs[4].Substring(0, vs[4].IndexOf(".") + 3);//26.78
                v += " " + v4; //sui******** 尹志强 1403**********1510 157.00 26.78
                newLines.Add(v);

                //26.78187******** 李艳坤 2323**********0060 154.00 26.27
                string leftline = l.Substring(l.IndexOf(vs[4]), l.Length - l.IndexOf(vs[4])  );
                //187******** 李艳坤 2323**********0060 154.00 26.27
                leftline = leftline.Substring(leftline.IndexOf(v4) + v4.Length, leftline.Length - (leftline.IndexOf(v4) + v4.Length)  );

                l = leftline;

            }
            return l;

        }

        public static void parseJiekuanrenCSVfromPathPDF(string path, string csvfile)
        {
            var files = Directory.GetFiles(path, "*.pdf");

            string xlsheader = AXFunction.getJiekuanrenCSVHeader();
            string xlscontent = xlsheader;

            foreach (var file in files)
            {
                Console.WriteLine("Parsing File:" + file);
                string content = "";
                try { 
                    content = Common.ReadPdfFile(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Get PDF Constent Error: " + e.Message);
                }


                string line = "";
                try {
                    line = AXFunction.parseJiekuanrenCSVItemsfromContent(content); 
                }
                catch(Exception e) {
                    Console.WriteLine("Parsing Content Error: "+e.Message);
                }
                
                if (!line.Trim().Equals(""))
                {
                    xlscontent += line + "\n";
                }
            }

            //SavaTxtFile(xlscontent, path+@"\导出借款人.csv");
            File.WriteAllText(csvfile, xlscontent, Encoding.Default);
        }






    }
}
