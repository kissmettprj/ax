using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using Newtonsoft.Json;

using com.kissmett.Common;


namespace wx
{

    /// <summary>
    ///     WeiXin 的摘要说明
    /// </summary>
    public abstract class WeiXinPage : Page, IRequiresSessionState
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
            get { return "$wxuser"; }
        }

        private static string _code
        {
            get { return "$wxcode"; }
        }

        private static string _AbsoluteUri
        {
            get { return HttpContext.Current.Request.Url.AbsoluteUri; }
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
            get { return getWxUser(); }
        }

        protected sealed override void OnPreInit(EventArgs e)
        {
            if (isGuest)
            {
                getWxUser();
            }
            base.OnPreInit(e);
        }

        /// <summary>
        ///     获取微信用户信息
        /// </summary>
        private static Dictionary<string, object> getWxUser()
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
                        "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code"
                        , Config.AppId
                        , Config.AppSecret
                        , code);
                ret = HttpService.Get(url);
                var o = JsonConvert.DeserializeObject<Dictionary<string, string>>(ret);

                #region 注意处理微错误{"errcode":40163, "errmsg":"code been used"} 以及{"errcode":40029, "errmsg":"invalid code"}

                if (o.ContainsKey("errmsg"))
                {
                    if (o["errcode"] == "40163" || o["errcode"] == "40029")
                    {
                        HttpContext.Current.Response.Redirect(_RawUrl, false);
                        return null;
                    }
                    throw new Exception(o["errmsg"]);
                }

                #endregion

                /* 根据access_token和openid获取用户信息 */
                url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN"
                    , o["access_token"]
                    , o["openid"]);
                ret = HttpService.Get(url);
                /*保存用户到Session*/
                HttpContext.Current.Session[_user] = ret;//json

                #endregion

                return
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(HttpContext.Current.Session[_user].ToString());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     跳转至微信授权登录页 
        /// </summary>
        private static void JumpUrl()
        {
            var url =
                string.Format(
                    "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect"
                    , Config.AppId
                    , _AbsoluteUri);
            HttpContext.Current.Response.Redirect(url, false);
        }
    }
}