using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace wx
{
    /// <summary>
    ///     微信公众号配置文件
    /// </summary>
    public class Config : IRequiresSessionState
    {
        /// <summary>
        ///     开发者ID
        /// </summary>
        public static string AppId { get; set; }

        /// <summary>
        ///     开发者密码
        /// </summary>
        public static string AppSecret { get; set; }

        /// <summary>
        ///     APP access_token 7200秒更新一次
        /// </summary>
        public static string access_token
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Session["$access_token"] == null)
                    {
                        using (var wc = new WebClient())
                        {
                            wc.Encoding = Encoding.UTF8;
                            var ret =
                                wc.DownloadString(
                                    string.Format(
                                        "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}",
                                        AppId, AppSecret));
                            var o = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);
                            if (o.ContainsKey("errmsg")) return o["errmsg"].ToString();
                            HttpContext.Current.Session.Add("$access_token", o["access_token"].ToString());
                            HttpContext.Current.Session.Timeout = 7200;
                            return o["access_token"].ToString();
                        }
                    }
                    return HttpContext.Current.Session["$access_token"].ToString();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}