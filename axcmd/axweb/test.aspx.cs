using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Newtonsoft.Json;

namespace axweb
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(JsonConvert.SerializeObject(Data));
            //Data: JsonConvert.DeserializeObject<Dictionary<string, object>>(HttpContext.Current.Session[_user].ToString());
            /*
             * https://open.weixin.qq.com/cgi-bin/showdocument?action=dir_list&t=resource/res_list&verify=1&id=open1419316518&lang=zh_CN
             * 
            {
            "openid":"OPENID",
            "nickname":"NICKNAME",
            "sex":1,
            "province":"PROVINCE",
            "city":"CITY",
            "country":"COUNTRY",
            "headimgurl": "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0",
            "privilege":[
            "PRIVILEGE1",
            "PRIVILEGE2"
            ],
            "unionid": " o6_bmasdasdsad6_2sgVt7hMZOPfL"

            }
             */
            string testData = @"
            {
            ""openid"":""OPENID"",
            ""nickname"":""NICKNAME"",
            ""sex"":1,
            ""province"":""PROVINCE"",
            ""city"":""CITY"",
            ""country"":""COUNTRY"",
            ""headimgurl"": """",
            ""privilege"":[
            ""PRIVILEGE1"",""PRIVILEGE2""
            ],
            ""unionid"": ""o6_bmasdasdsad6_2sgVt7hMZOPfL""
            }            
            ";
            //Dictionary<string, object> sData = JsonConvert.DeserializeObject<Dictionary<string, object>>(testData);

            //Response.Write(testData);

            Dictionary<string, object> sData = JsonConvert.DeserializeObject<Dictionary<string, object>>(testData);
            Response.Write(sData["openid"].ToString());
        }
    }
}