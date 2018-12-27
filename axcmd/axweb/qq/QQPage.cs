using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using Newtonsoft.Json;

using System.Collections.Specialized;

using com.kissmett.Common;


namespace qq
{
    // ref: http://wiki.connect.qq.com/api%E5%88%97%E8%A1%A8
    /// <summary>
    ///     WeiXin 的摘要说明
    /// </summary>
    public abstract class QQPage : Page, IRequiresSessionState
    {
        /// <summary>
        ///     是否访客身份 true:是 false:否
        /// </summary>
        public static bool isGuest
        {
            get { return HttpContext.Current.Session[_user] == null ? true : false; }
        }

        private static string _user
        {
            get { return "$qquser"; }
        }

        private static string _code
        {
            get { return "$qqcode"; }
        }

        private static string _AbsoluteUri
        {
            //get { return HttpContext.Current.Request.Url.AbsoluteUri; }
            get { return "http://www.axtools.net/ax/qq"; }
        }

        /// <summary>
        ///     该路径剔除微信Query的URL用于页面重定向获取最新的用户信息
        /// </summary>
        private static string _RawUrl
        {
            get
            {
                var url = HttpContext.Current.Request.Url.AbsoluteUri;
                url = url.Replace(url.Substring(url.IndexOf("code"), url.IndexOf("STATE") + 5 - url.IndexOf("code")), "");
                return url;
            }
        }

        /// <summary>
        ///     微信用户信息
        /// </summary>
        public Dictionary<string, object> Data
        {
            get { return getQQUser(); }
        }

        protected sealed override void OnPreInit(EventArgs e)
        {
            if (isGuest)
            {
                getQQUser();
            }
            base.OnPreInit(e);
        }

        /// <summary>
        ///     获取微信用户信息
        /// </summary>
        private static Dictionary<string, object> getQQUser()
        {
            string code = HttpContext.Current.Request.QueryString.Get("code") ?? "",
                url = "",
                ret = "";
            try
            {
                var user = HttpContext.Current.Session[_user];
                if (code.Length == 0 && user == null)
                {
                    JumpUrl();
                    return null;
                }
                if (user != null)
                    return
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(
                            HttpContext.Current.Session[_user].ToString());

                #region /*如果session中不存在user信息则对接微信接口获取用户信息*/

                /* 根据code获取access_token */
                url =
                    string.Format(
                        "https://graph.qq.com/oauth2.0/token?client_id={0}&client_secret={1}&code={2}&grant_type=authorization_code&redirect_uri={3}"
                        , Config.AppId
                        , Config.AppSecret
                        , code
                        , _AbsoluteUri
                        );
                ret = HttpService.Get(url);//"access_token=EEA79EE4AB99C99E2F35AC409B59E808&expires_in=7776000&refresh_token=FB70C3A5446CF3AC0072C84D3D779DCB"
                //var o = JsonConvert.DeserializeObject<Dictionary<string, string>>(ret);
                var o = MyURL.GetQueryStringNVC(ret);

                //#region 注意处理微错误{"errcode":40163, "errmsg":"code been used"} 以及{"errcode":40029, "errmsg":"invalid code"}
                //if (o.ContainsKey("errmsg"))
                //{
                //    if (o["errcode"] == "40163" || o["errcode"] == "40029")
                //    {
                //        HttpContext.Current.Response.Redirect(_RawUrl, false);
                //        return null;
                //    }
                //    throw new Exception(o["errmsg"]);
                //}

                //#endregion

                /* 根据access_token d获取用户openid */
                url =
                    string.Format(
                        "https://graph.qq.com/oauth2.0/me?access_token={0}"
                        , o["access_token"]);
                ret = HttpService.Get(url);//"callback( {\"client_id\":\"101529425\",\"openid\":\"6CEC75A0B5A0DF04D537562E38E9C25E\"} );\n"
                ret = ret.Substring(10, ret.IndexOf("}") - 10 + 1);
                var o_me = JsonConvert.DeserializeObject<Dictionary<string, string>>(ret);


                /* 根据access_token和openid获取用户信息 */
                //https://connect.qq.com/sdk/webtools/index.html#

                url = string.Format("https://graph.qq.com/user/get_user_info?access_token={0}&openid={1}&appid={2}&lang=zh_CN"
                    , o["access_token"]
                    , o_me["openid"]
                    , Config.AppId
                    );
                ret = HttpService.Get(url);
                /*"{\n    
                    \"ret\": 0,\n    \"msg\": \"\",\n    \"is_lost\":0,\n    
                    \"nickname\": \"飞～甜\",\n    \"gender\": \"男\",\n    \"province\": \"北京\",\n    \"city\": \"东城\",\n   
                    \"year\": \"1980\",\n    \"constellation\": \"\",\n    
                    \"figureurl\": \"http:\\/\\/qzapp.qlogo.cn\\/qzapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/30\",\n    
                    \"figureurl_1\": \"http:\\/\\/qzapp.qlogo.cn\\/qzapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/50\",\n    
                    \"figureurl_2\": \"http:\\/\\/qzapp.qlogo.cn\\/qzapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/100\",\n    
                    \"figureurl_qq_1\": \"http:\\/\\/thirdqq.qlogo.cn\\/qqapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/40\",\n    
                    \"figureurl_qq_2\": \"http:\\/\\/thirdqq.qlogo.cn\\/qqapp\\/101529425\\/6CEC75A0B5A0DF04D537562E38E9C25E\\/100\",\n    
                    \"is_yellow_vip\": \"0\",\n    \"vip\": \"0\",\n    \"yellow_vip_level\": \"0\",\n    \"level\": \"0\",\n    
                    \"is_yellow_year_vip\": \"0\"\n
                  }\n"
                
                 * */
               

                #endregion

                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);
                data["openid"] = o_me["openid"];

                ret = JsonConvert.SerializeObject(data);
                /*保存用户到Session*/
                HttpContext.Current.Session[_user] = ret;//json

                return
                    data;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     跳转至qq授权登录页 
        ///     https://graph.qq.com/oauth2.0/authorize
        /// </summary>
        private static void JumpUrl()
        {
            var url =
                string.Format(
                    "https://graph.qq.com/oauth2.0/authorize?client_id={0}&redirect_uri={1}&response_type=code&scope=get_user_info&state=STATE#wechat_redirect"
                    , Config.AppId
                    , _AbsoluteUri);
            HttpContext.Current.Response.Redirect(url, false);

        }
    }
}