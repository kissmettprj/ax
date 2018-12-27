using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO.Compression;

using System.IO;
using System.Text;

using RabbitMQ.Client;

using com.kissmett.Common;

namespace axweb.tools
{
    public partial class mycontracts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null || Session["password"] == null) //need username password
            { //未登录
                Session["backurl"] = "tools/mycontracts.aspx";
                Response.Redirect("../Login.aspx");
                return;
            }
            if (!this.IsPostBack)
            {
                this.txtUserName.Text = Session["username"].ToString();
                //ViewState["username"] = Session["username"].ToString();
                //ViewState["password"] = Session["password"].ToString();
                ViewState["wsbase"] = Functions.GetAppConfigString("wsbase", "");


                string username = Session["username"].ToString();

                int totalcount = getUserContractCount(username);
                int dcount = getDownloadedCount(username);

                float dminutes = (totalcount - dcount) * 66 / 60;
                lDownloadedCount.Text = Convert.ToString(dcount);
                lContractCount.Text = Convert.ToString(totalcount);
                lDownloadMinutes.Text = Convert.ToString(dminutes);

                //用户目录存在,说明任务已经开始过;不允许再次提价;
                if (isUserFolderExist(username))
                {
                    btnOK.Enabled = false;

                }
                else
                {
                    btnOK.Enabled = true;

                }

                if (isUserContractCSVExist(username))
                {
                    btnDownloadContractCSV.Enabled = true;
                    btnClean.Enabled = true;
                }
                else
                {
                    btnDownloadContractCSV.Enabled = false;
                    btnClean.Enabled = false;
                }

                if (isUserJKRCSVExist(username))
                {
                    btnDownloadJKRCSV.Enabled = true;
                }
                else
                {
                    btnDownloadJKRCSV.Enabled = false;
                }

                if (isUserJKRCSVExist(username))
                {
                    btnDownloadAll.Enabled = true;
                }
                else
                {
                    btnDownloadAll.Enabled = false;
                }



            }
            

        }
            

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //session
            string username = Session["username"].ToString();
            string password = Session["password"].ToString();


            //add mq
            string msg = @"{""type"":""download"",""data"":{""username"":""" + username + @""",""password"":""" + password + @"""} }";
            uint msgcnt = PublishMsg(msg);
            lQueueMsgCnt.Text = Convert.ToString(msgcnt) ;
            btnOK.Enabled = false;

        }
        uint PublishMsg(string msg)
        {
            uint msgcnt = 0;
            //config
            string wsbase = Functions.GetAppConfigString("wsbase", "");
            string mqhost = Functions.GetAppConfigString("mqhost", "127.0.0.1");
            int mqport = Functions.GetAppConfigInt("mqport", 5672);
            string mqusername = Functions.GetAppConfigString("mqusername", "guest");
            string mqpassword = Functions.GetAppConfigString("mqpassword", "guest");
            string queuename = Functions.GetAppConfigString("queuename", "task_queue");

            var factory = new ConnectionFactory() { HostName = mqhost, Port = mqport, UserName = mqusername, Password = mqpassword };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                QueueDeclareOk q = channel.QueueDeclare(queue: queuename,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                msgcnt = q.MessageCount;
                var message = msg;
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.SetPersistent(true);

                channel.BasicPublish(exchange: "",
                                     routingKey: queuename,
                                     basicProperties: properties,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
 
            return msgcnt;
        }



        bool isUserFolderExist(string username)
        {
            //config
            string wsbase = ViewState["wsbase"].ToString();
            string ws = wsbase + @"\" + username;
            return Directory.Exists(ws);
        }

        int getDownloadedCount(string username)
        {
            if (isUserFolderExist(username))
            {
                string wsbase = ViewState["wsbase"].ToString();
                string ws = wsbase + @"\" + username;
                string[] files = Directory.GetFiles(ws, "*.pdf");
                return files.Length;
            }
            return 0;
            
        }

        bool isUserContractCSVExist(string username)
        {
            //config
            string wsbase = ViewState["wsbase"].ToString();
            string csvfile = wsbase + @"\" + username + @"\合同信息-" + username + ".csv";
            return File.Exists(csvfile);
        }
        int getUserContractCount(string username)
        {
            if (isUserContractCSVExist(username))
            {
                //读取文件行数-1
                //config
                string wsbase = ViewState["wsbase"].ToString();
                string csvfile = wsbase + @"\" + username + @"\合同信息-" + username + ".csv";
                int lc = MyFile.getTxtFileLineCount(csvfile);
                return (lc - 1);
            }
            else
            {
                return 0;
            }
        }

        bool isUserJKRCSVExist(string username)
        {
            //config
            string wsbase = ViewState["wsbase"].ToString();
            string csvfile = wsbase + @"\" + username + @"\借款人信息-" + username + ".csv";
            return File.Exists(csvfile);
        }
        //bool isUserContractsZipExist(string username)
        //{
        //    //config
        //    string wsbase = ViewState["wsbase"].ToString();
        //    string zipfile = wsbase + @"\" + username + ".zip"; 
        //    return File.Exists(zipfile);
        //}

        protected void btnDownloadCSV_Click(object sender, EventArgs e)
        {
            string username = this.txtUserName.Text;
            if (isUserContractCSVExist(username))
            {
                string wsbase = ViewState["wsbase"].ToString();
                string csvfile = wsbase + @"\" + username + @"\合同信息-" + username + ".csv";

                //Response.ContentType = "application/x-zip-compressed";
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename= 合同信息-"+ username + ".csv");
                string filename = csvfile;
                Response.TransmitFile(filename);
            }
        }

        protected void btnDownloadAll_Click(object sender, EventArgs e)
        {
            string username = this.txtUserName.Text;
            //借款人是最后生成的.
            if (isUserJKRCSVExist(username))
            {
                string wsbase = ViewState["wsbase"].ToString();
                string ws = wsbase + @"\" + username + @"\";
                string zipfile = wsbase + @"\" + username + @".zip";
                //gene zip
                if (File.Exists(zipfile)) File.Delete(zipfile);

                ZipFile.CreateFromDirectory(ws, zipfile);

              


                Response.ContentType = "application/x-zip-compressed";
                //Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename= " + username + ".zip");
                string filename = zipfile;
                Response.TransmitFile(filename);

                //delete zip and pdfs  //在此次会影响下载,需开发一定时任务清除;也可让用户自行选择;
                //MyFile.DeleteFileByExt(ws, ".pdf");
                //File.Delete(zipfile);
                

            }
        }

        protected void btnDownloadJKRCSV_Click(object sender, EventArgs e)
        {          

            string username = this.txtUserName.Text;
            if (isUserJKRCSVExist(username))
            {
                string wsbase = ViewState["wsbase"].ToString();
                string csvfile = wsbase + @"\" + username + @"\借款人信息-" + username + ".csv";

                //Response.ContentType = "application/x-zip-compressed";
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename= 借款人信息-" + username + ".csv");
                string filename = csvfile;
                Response.TransmitFile(filename);

               
            }
        }

        //清除zip和pdfs
        protected void btnClean_Click(object sender, EventArgs e)
        {  
            string username = this.txtUserName.Text;
            //借款人是最后生成的.
            if (isUserJKRCSVExist(username))
            {
                string wsbase = ViewState["wsbase"].ToString();
                string ws = wsbase + @"\" + username + @"\";
                string zipfile = wsbase + @"\" + username + @".zip";
                MyFile.DeleteFileByExt(ws, ".pdf");
                if (File.Exists(zipfile)) File.Delete(zipfile);
                lMsg.Text = "您在本服务器上缓存的合同已清空.";
            }
        }
    }
}