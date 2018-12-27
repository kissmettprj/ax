using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Net;
using System.IO;
using System.Collections;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// https://www.cnblogs.com/shenbing/p/5784868.html base
// https://www.crifan.com/csharp_add_proxy_for_httpwebrequest/ proxy

namespace axfunc
{
    public class AXSpider
    {
        //useragent
        //static string c_UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";
        //static string c_UserAgent = @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
        
        //proxy
        private WebProxy gProxy = null;

        //cookies
        System.Net.CookieContainer cc = new System.Net.CookieContainer();
        //合同信息列表,信息条目格式:轻松投,持有中,项目编号,合同编号,url
        ArrayList m_ContractInfoList = new ArrayList();
        //所有合同链接
        ArrayList m_AllContractUrls = new ArrayList();

        //用户名
        string m_username = "";

        
        public bool Login_cunguan(string username, string password)
        {
            //http://cunguan.anxin.com/ajax/ajax.ashx
            string url = "http://cunguan.anxin.com/ajax/ajax.ashx";

            /*
            Action: post
            Cmd: Login4
            sUserName:  
            sPassword:  
            Keep: 1
             */


            HttpWebRequest request = getRequest(url); 

            string data = "Action=post&Cmd=Login4&sUserName=" + username + "&sPassword=" + password + "&Keep=1";
            byte[] requestBuffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(data);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestBuffer.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestBuffer, 0, requestBuffer.Length);
                requestStream.Close();
            }
            string str = "";
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                str = reader.ReadToEnd();

                Console.WriteLine("Login Response:" + str);
                reader.Close();
            }
            m_username = username;
            if (str.Equals("True")) return true;
            return false;


        }


        public string getUserCenter2016_cunguan() {
            /*
             * http://cunguan.anxin.com/usercenter2016/
             * 
             */
            string url = "http://cunguan.anxin.com/usercenter2016/";
            string data = "";
            string str = getPostURLContent(url, data);
            return str;

        }


        //登录www.anxin.com
        public bool Login(string username, string password)
        {
            string url = "https://www.anxin.com/ajax/ajax.ashx";
            /*
                Action:post
                Cmd:Login4
                sUserName: aaa
                sPassword: xxx
                Keep:0
             */


            HttpWebRequest request = getRequest(url);

            string data = "Action=post&Cmd=Login4&sUserName=" + username + "&sPassword=" + password + "&Keep=0";
            byte[] requestBuffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(data);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestBuffer.Length;
            
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestBuffer, 0, requestBuffer.Length);
                requestStream.Close();
            }
            string str = "";
            WebResponse response = request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                str = reader.ReadToEnd();

                Console.WriteLine("Login Response:" + str);
                reader.Close();
            }
            m_username = username;
            if (str.Equals("True")) return true;
            return false;


        }


        public void DownLoad(string url, string filename)
        {

            if (File.Exists(filename))
            {
                Console.WriteLine("文件已存在,跳过:" + filename);
                return;
            }

            DateTime dtStart = DateTime.Now;
            HttpWebRequest request = getRequest(url);

            WebResponse response = request.GetResponse();


            Stream responseStream = response.GetResponseStream();


            FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            byte[] bArr = new byte[1024];
            int iTotalSize = 0;
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            Console.Write("开始下载:" + filename);
            while (size > 0)
            {
                iTotalSize += size;
                fs.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
                Console.Write(".");
            }
            fs.Close();

            responseStream.Close();
            DateTime dtEnd = DateTime.Now;
            Console.WriteLine("完成.["+(dtEnd-dtStart).ToString()+"]");

        }


        public string getPostURLContent(string url, string data)
        {

            HttpWebRequest request = getRequest(url);


            byte[] requestBuffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(data);


            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestBuffer.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestBuffer, 0, requestBuffer.Length);
                requestStream.Close();
            }

            WebResponse response = request.GetResponse();
            string str = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                str = reader.ReadToEnd();

                //Console.WriteLine("ParsePage_AnXinPlan Response:" + str);
                reader.Close();
            }

            return str;

        }

        public string GetURLContent(string url)
        {

            HttpWebRequest request = getRequest(url);

            WebResponse response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));

            string content = "";

            //content = reader.ReadToEnd();
            //Console.WriteLine();

            string strLine = null;
            while ((strLine = reader.ReadLine()) != null)
            {
                content += strLine;
            }
            reader.Close();

            return content;
        }

        public void initParserWork()
        {
            m_AllContractUrls.Clear();
            m_ContractInfoList.Clear();
            Console.WriteLine("init Spider Parser;");
        }


        //获取所有标的合同 轻松/散标/自由投(无合适的数据支持) ------------------- 所有标汇总
        public int getAll_ContractURLList()
        {
            //ArrayList a1 = getQST_ContractURLList();//轻松投
            //ArrayList a2 = getSB_ContractURLList();//散标
            //ArrayList a3 = getZYT_ContractURLList();//自由投
            //a1.AddRange(a2);
            //a1.AddRange(a3);
            //return a1;
            getZYT_ContractURLList();//自由投
            getQST_ContractURLList();//轻松投
            getSB_ContractURLList();//散标

            Console.WriteLine("All Contracts[自由投.持有中;轻松投.进行中/退出中;散标.回收中] List Parsed;");

            return m_ContractInfoList.Count;
        }

        //获取轻松投合同url列表 ------------------- 轻松投汇总
        public void getQST_ContractURLList()
        {
            Console.WriteLine("Parsing 轻松投...");
            //ArrayList a1 = getQST_ContractURLListByTypeId(轻松投类型.进行中);//进行中
            //ArrayList a2 = getQST_ContractURLListByTypeId(轻松投类型.退出中);//退出中
            ////ArrayList a3 = getQST_ContractURLListByTypeId(轻松投类型.已退出);//已退出 无pdf链接
            //a1.AddRange(a2);
            ////a1.AddRange(a3);
            getQST_ContractURLListByTypeId(轻松投类型.进行中);
            getQST_ContractURLListByTypeId(轻松投类型.退出中);
            Console.WriteLine("Parse 轻松投 Done.");
            //return a1;

        }


        //通过typeid获取轻松贷合同URL列表;
        public void getQST_ContractURLListByTypeId(轻松投类型 typeId)
        {
            string url = "https://www.anxin.com/ajax2017/ajax_AnXinPlan.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;

            //获取所有 JoanId 对应的合同url立标;
            ArrayList list_AllContractUrls = new ArrayList();
            while (true)
            {
                string data = "cmd=GetUserAnXinPlanList&typeId=" + (int)typeId + "&beginTime=2018-01-13&endTime=&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&bid=0";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;

                JArray DataList = (JArray)jo["DataList"];
                for (int i = 0; i < DataList.Count; i++)
                {
                    //每一行代表一个项目Plan,对应一系列合同;
                    //加入日期,加入金额,退出日期,红包,退出中?字段有可能没有值
                    //CTime?   Amount ClearDate RedPacket?
                    string Id = Common.procStr(DataList[i]["Id"]); //项目编号
                    string CTime = Common.procStr(DataList[i]["CTime"]); //加入日期
                    string Amount = Common.procStr(DataList[i]["Amount"]); //加入金额
                    string ClearDate = Common.procStr(DataList[i]["ClearDate"]); //退出日期
                    string RedPacket = Common.procStr(DataList[i]["RedPacket"]); //红包
                    string JoanId = Common.procStr(DataList[i]["JoanId"]);
                    Console.WriteLine(" i=" + getTotal + " ;JoanId=" + JoanId + " ;Id =" + Id);
                    ArrayList list_ContractUrls = getContractByJoanId_QST(JoanId);//获取列表
                    list_AllContractUrls.AddRange(list_ContractUrls);
                    getTotal++;
                    //将urls放入m_AllContractUrls
                    m_AllContractUrls.AddRange(list_ContractUrls);

                    //处理为所需的合同信息,放入m_ContractInfoList
                    string prefix = "轻松投," + typeId.ToString() + ","
                        + Id + ","
                        + CTime + ","
                        + Amount + ","
                        + ClearDate + ","
                        + RedPacket + ","
                        ;
                    ArrayList list_ContractMoreInfo = getPrefixedContractArryList(list_ContractUrls, prefix);
                    m_ContractInfoList.AddRange(list_ContractMoreInfo);
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }



            //return list_AllContractUrls;

        }

        //通过 JoanId 获得其下所有合同链接
        public ArrayList getContractByJoanId_QST(string JoanId)
        {
            /*
            ====> 通过 "JoanId": 15348,对应下面请求中的pid
            https://www.anxin.com/ajax2017/ajax_AnXinPlan.ashx	
            cmd:GetAnXinPlanLoanByID //此处区别于zyt
            pageIndex:1
            pageSize:8
            pid:15348
             */

            string url = "https://www.anxin.com/ajax2017/ajax_AnXinPlan.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;


            //获取所有 contracturl
            ArrayList list_contracturls = new ArrayList();
            while (true)
            {
                //分页处理
                string data = "cmd=GetAnXinPlanLoanByID&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&pid=" + JoanId + "";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;

                JArray DataList = (JArray)jo["DataList"];
                for (int i = 0; i < DataList.Count; i++)
                {
                    string LoanId = DataList[i]["LoanId"].ToString();//此处区别于zyt
                    if (!LoanId.Trim().Equals(""))
                    {
                        string contracturl = @"https://www.anxin.com/contract/contract.html?loanid=" + LoanId;
                        list_contracturls.Add(contracturl);
                        Console.WriteLine(" i=" + getTotal + @" ;" + contracturl);
                    }
                    getTotal++;
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }

            return list_contracturls;
        }

        //通过 JoanId 获得其下所有合同链接
        public ArrayList getContractByJoanId_ZYT(string JoanId)
        {
            /*
            ====> 通过 "JoanId": 15348,对应下面请求中的pid
            https://www.anxin.com/ajax2017/ajax_AnXinPlan.ashx	
            cmd:GetFreedomLoanByProjectID //此处区别于qst
            pageIndex:1
            pageSize:8
            pid:15348
             */

            string url = "https://www.anxin.com/ajax2017/ajax_AnXinPlan.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;


            //获取所有 contracturl
            ArrayList list_contracturls = new ArrayList();
            while (true)
            {
                //分页处理
                string data = "cmd=GetFreedomLoanByProjectID&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&pid=" + JoanId + "";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;

                JArray DataList = (JArray)jo["DataList"];
                for (int i = 0; i < DataList.Count; i++)
                {
                    string LoanId = DataList[i]["LoanID"].ToString();//此处区别于qst
                    if (!LoanId.Trim().Equals(""))
                    {
                        string contracturl = @"https://www.anxin.com/contract/contract.html?loanid=" + LoanId;
                        list_contracturls.Add(contracturl);
                        Console.WriteLine(" i=" + getTotal + @" ;" + contracturl);
                    }
                    getTotal++;
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }

            return list_contracturls;
        }




        //获取自由投合同url列表 ------------------- 自由投汇总
        public void getZYT_ContractURLList()
        {
            Console.WriteLine("Parsing 自由投...");
            //ArrayList a1 = getZYT_ContractURLListByTypeId(自由投类型.持有中);//持有中
            ////ArrayList a2 = getZYT_ContractURLListByTypeId(自由投类型.竞标中);//竞标中 无数据暂时避掉
            ////ArrayList a3 = getZYT_ContractURLListByTypeId(自由投类型.已退出);//已退出 无pdf链接
            ////a1.AddRange(a2);
            ////a1.AddRange(a3);

            getZYT_ContractURLListByTypeId(自由投类型.持有中);

            Console.WriteLine("Parse 自由投 Done.");
            //return a1;

        }



        //通过typeid获取自由投合同URL列表;
        public void getZYT_ContractURLListByTypeId(自由投类型 typeId)
        {
            /*
    cmd:GetUserProjectLoanList
    typeId:0
    beginTime:2018-02-04 <----暂时写死这个
    endTime:
    pageIndex:1
    pageSize:8
    bid:0
    https://www.anxin.com/ajax2017/ajax_AnXinPlan.ashx
             */
            string url = "https://www.anxin.com/ajax2017/ajax_AnXinPlan.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;

            //获取所有 JoanId 对应的合同url列表;
            ArrayList list_AllContractUrls = new ArrayList();
            while (true)
            {
                string data = "cmd=GetUserProjectLoanList&typeId=" + (int)typeId + "&beginTime=2018-02-04&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&bid=0";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;

                JArray DataList = (JArray)jo["DataList"];
                for (int i = 0; i < DataList.Count; i++)
                {
                    //每一行对应一个项目Project,对应一系列合同;
                    //加入日期,加入金额,退出日期,红包
                    //CTime   Amount null RedPacket

                    string JoanId = DataList[i]["JoanId"].ToString();
                    string ProjectId = Common.procStr(DataList[i]["ProjectId"]);//项目编号
                    string CTime = Common.procStr(DataList[i]["CTime"]);//加入日期
                    string Amount = Common.procStr(DataList[i]["Amount"]);//加入金额
                    //string xxx = Common.procStr(DataList[i]["xxx"]);//退出日期,无
                    string RedPacket = Common.procStr(DataList[i]["RedPacket"]);//红包

                    Console.WriteLine(" i=" + getTotal + " ;JoanId=" + JoanId);
                    ArrayList list_ContractUrls = getContractByJoanId_ZYT(JoanId);//获取列表
                    list_AllContractUrls.AddRange(list_ContractUrls);
                    getTotal++;

                    //将urls放入m_AllContractUrls
                    m_AllContractUrls.AddRange(list_ContractUrls);
                    //处理为所需的合同信息;
                    string prefix = "自由投," + typeId.ToString() + ","
                        + ProjectId + ","
                        + CTime + ","
                        + Amount + ","
                        + "" + ","
                        + RedPacket + ","
                        ;
                    ArrayList list_ContractMoreInfo = getPrefixedContractArryList(list_ContractUrls, prefix);
                    m_ContractInfoList.AddRange(list_ContractMoreInfo);
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }

            //return list_AllContractUrls;

        }




        //获取散标合同url列表 ------------------- 散标汇总
        public void getSB_ContractURLList()
        {
            Console.WriteLine("Parsing 散标 ...");
            //ArrayList a1 = getSB_Huishouzhong_ContractURLList();//散标_回收中
            ////ArrayList a2 = getSB_Jingbiaozhong_ContractURLList();//竞标中 无数据参考
            ////ArrayList a3 = getSB_Yijieqing_ContractURLList();//已结清 无pdf 无法下载下载;
            ////ArrayList a4 = getSB_Kuaisuzhuanrang_ContractURLList(");//快速转让,无数据参考
            ////a1.AddRange(a2);
            ////a1.AddRange(a3);
            ////a1.AddRange(a4);

            getSB_Huishouzhong_ContractURLList();//散标_回收中
            Console.WriteLine("Parse 散标 Done.");
            //return a1;
        }

        //获取散标_回收中 合同列表
        public void getSB_Huishouzhong_ContractURLList()
        {
            /*
            cmd:GetBackingList
            typeId:0
            beginTime:2011-11-15 <---暂时写死这个
            endTime:
            pageIndex:1
            pageSize:8
            bid:0
             */

            string url = "https://www.anxin.com/ajax2016/ajax_Investment.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;


            //获取所有 contracturl
            ArrayList list_contracturls = new ArrayList();
            while (true)
            {
                //分页处理
                string data = "cmd=GetBackingList&typeId=0&beginTime=2011-11-15&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&bid=0";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;
                JArray DataList = (JArray)jo["DataList"];
                for (int i = 0; i < DataList.Count; i++)
                {
                    //每一行代表一笔回收中的项目;对应一个合同;
                    //出借日期, 出借金额, 还款期数,红包
                    //LoanTime Amount PeriodTime RedPacket
                    string LoanId = Common.procStr(DataList[i]["LoanId"]);
                    string BorrowId = Common.procStr(DataList[i]["BorrowId"]);
                    string LoanTime = Common.procStr(DataList[i]["LoanTime"]);
                    string Amount = Common.procStr(DataList[i]["Amount"]);
                    string PeriodTime = Common.procStr(DataList[i]["PeriodTime"]);
                    string RedPacket = Common.procStr(DataList[i]["RedPacket"]);
                    if (!LoanId.Trim().Equals(""))
                    {
                        string contracturl = @"https://www.anxin.com/contract/contract.html?loanid=" + LoanId;
                        list_contracturls.Add(contracturl);
                        Console.WriteLine(" i=" + getTotal + @" ;" + contracturl);

                        //将urls放入m_AllContractUrls
                        m_AllContractUrls.Add(contracturl);
                        //处理为所需的合同信息;放入m_ContractInfoList
                        string v = "散标,回收中," + BorrowId + ","
                            + LoanTime + ","
                            + Amount + ","
                            + PeriodTime + ","
                            + RedPacket + ","
                            + LoanId + ","
                            + contracturl;
                        m_ContractInfoList.Add(v);
                    }
                    getTotal++;
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }


            //return list_contracturls;
        }


        //获取散标_竞标中 合同列表
        //--------------------因为无数据所以此处存疑,应此从调用方注释掉;2018-10-18
        public ArrayList getSB_Jingbiaozhong_ContractURLList()
        {
            /*
            cmd:GetBackingList
            typeId:0
            beginTime:2011-11-15 <---暂时写死这个
            endTime:
            pageIndex:1
            pageSize:8
            bid:0
             */

            string url = "https://www.anxin.com/ajax2016/ajax_Investment.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;


            //获取所有 contracturl
            ArrayList list_contracturls = new ArrayList();
            while (true)
            {
                //分页处理
                string data = "cmd=GetBiddingList&typeId=1&beginTime=2011-11-15&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&bid=0";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;

                JArray DataList = (JArray)jo["DataList"];//--------------------因为无数据所以此处存疑,应此从调用方注释掉;2018-10-18
                for (int i = 0; i < DataList.Count; i++)
                {
                    string LoanId = DataList[i]["LoanId"].ToString();
                    if (!LoanId.Trim().Equals(""))
                    {
                        string contracturl = @"https://www.anxin.com/contract/contract.html?loanid=" + LoanId;
                        list_contracturls.Add(contracturl);
                        Console.WriteLine(" i=" + getTotal + @" ;" + contracturl);
                    }
                    getTotal++;
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }

            return list_contracturls;
        }


        //获取散标_已结清 合同列表 //无合同链接,无法下载;(逻辑不对,不调用!!!)
        public ArrayList getSB_Yijieqing_ContractURLList()
        {
            /*
            cmd:GetBackingList
            typeId:0
            beginTime:2011-11-15 <---暂时写死这个
            endTime:
            pageIndex:1
            pageSize:8
            bid:0
             */

            string url = "https://www.anxin.com/ajax2016/ajax_Investment.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;


            //获取所有 contracturl
            ArrayList list_contracturls = new ArrayList();
            while (true)
            {
                //分页处理
                string data = "cmd=GetInvestmentBacked&typeId=2&beginTime=2011-11-15&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&bid=0";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;

                JArray DataList = (JArray)jo["DataList"];
                for (int i = 0; i < DataList.Count; i++)
                {
                    string LoanId = DataList[i]["LoanId"].ToString(); //此处结构是错误的!!!!!2018-10-18
                    if (!LoanId.Trim().Equals(""))
                    {
                        string contracturl = @"https://www.anxin.com/contract/contract.html?loanid=" + LoanId;
                        list_contracturls.Add(contracturl);
                        Console.WriteLine(" i=" + getTotal + @" ;" + contracturl);
                    }
                    getTotal++;
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }

            return list_contracturls;
        }



        //获取散标_快速转让 合同列表 //无数据暂时注释;且代码需要按实际response数据格式修改;
        public ArrayList getSB_Kuaisuzhuanrang_ContractURLList()
        {
            /*
            cmd:GetBackingList
            typeId:0
            beginTime:2011-11-15 <---暂时写死这个
            endTime:
            pageIndex:1
            pageSize:8
            bid:0
             */

            string url = "https://www.anxin.com/ajax2016/ajax_Investment.ashx";
            HttpWebRequest request = getRequest(url);

            int pageSize = 8;
            int pageIndex = 1;

            int getTotal = 0;


            //获取所有 contracturl
            ArrayList list_contracturls = new ArrayList();
            while (true)
            {
                //分页处理
                string data = "cmd=GetRedeemingList&typeId=3&beginTime=2011-11-15&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&bid=0";
                string str = getPostURLContent(url, data);

                //pages,total            
                JObject jo = (JObject)JsonConvert.DeserializeObject(str);
                int Pages = Convert.ToInt32(jo["Pages"].ToString());
                int Total = Convert.ToInt32(jo["Total"].ToString());

                if (Total == 0) break;

                JArray DataList = (JArray)jo["DataList"];//--------------------因为无数据所以此处存疑,应此从调用方注释掉;
                for (int i = 0; i < DataList.Count; i++)
                {
                    string LoanId = DataList[i]["LoanId"].ToString();
                    if (!LoanId.Trim().Equals(""))
                    {
                        string contracturl = @"https://www.anxin.com/contract/contract.html?loanid=" + LoanId;
                        list_contracturls.Add(contracturl);
                        Console.WriteLine(" i=" + getTotal + @" ;" + contracturl);
                    }
                    getTotal++;
                }

                if (getTotal < Total) { pageIndex++; }
                else break;
            }

            return list_contracturls;
        }


        public int DownloadContracts(string path)
        {
            int cnt = 0;
            for (int i = 0; i < m_AllContractUrls.Count; i++)
            {
                string url = m_AllContractUrls[i].ToString();
                string fname = path + @"\" + getContractNameByURL(url);
                try
                {
                    Console.Write("[" + (i + 1) + "/" + m_AllContractUrls.Count + "]");
                    DownLoad(url, fname);
                    cnt++;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occur: "+e.Message);
                }
            }
            return cnt;

        }

        public void SaveContractsCSV(string filename)
        {
            string xlscontent = "类型,子类型,项目编号,加入日期,加入金额,退出日期,红包,合同编号,在线地址\n";
            foreach (string line in m_ContractInfoList)
            {

                if (!line.Trim().Equals(""))
                {
                    xlscontent += line + "\n";
                }
            }

            //SavaTxtFile(xlscontent, path+@"\导出借款人.csv");
            File.WriteAllText(filename, xlscontent, Encoding.Default);
            Console.WriteLine("导出合同csv完毕[" + filename + "].");

        }

        public int LoadContractsCSV(string filename)
        {
            initParserWork();

            string[] lines = File.ReadAllLines(filename, Encoding.Default);
            for (int i = 1; i < lines.Length;i++ )
            {
                string line = lines[i].Trim();
                if (!line.Trim().Equals(""))
                {
                    m_ContractInfoList.Add(line);
                    string url = line.Split(",".ToCharArray())[8];
                    m_AllContractUrls.Add(url);
                }
            }
            return m_ContractInfoList.Count;


        }

        //将合同列表数组数据加入前缀,相当于csv增加列;
        static ArrayList getPrefixedContractArryList(ArrayList arr, string prefix)
        {
            ArrayList newArr = new ArrayList();
            for (int i = 0; i < arr.Count; i++)
            {
                string v = arr[i].ToString();//url
                string cNo = getContractNoByURL(v);
                v = prefix + cNo + "," + v;
                newArr.Add(v);
            }
            return newArr;

        }

        //从url提取合同编号
        static string getContractNoByURL(string url)
        {
            //https://www.anxin.com/contract/contract.html?loanid=2434983
            int pos = url.IndexOf("=");
            string res = url.Substring(pos + 1, url.Length - pos - 1);
            return res;
        }


        //通过url生成文件名
        static string getContractNameByURL(string url)
        {
            //https://www.anxin.com/contract/contract.html?loanid=2434983            
            string res = getContractNoByURL(url);
            res = "contract_" + res + ".pdf";
            return res;

        }

        /* set proxy
         * Note:
         * 1. current only support http proxy
         * 2. current only support single proxy
         */
        public void setProxy(string proxyIp, int proxyPort)
        {
            gProxy = new WebProxy(proxyIp, proxyPort);
        }
        
        public HttpWebRequest getRequest(string url)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;//SecurityProtocolType.Tls1.2;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.CookieContainer = cc;
            //request.UserAgent = c_UserAgent;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"; 

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version11;

            if (gProxy != null)
            {
                request.Proxy = gProxy;
            }

            return request;
        }


    }
}
