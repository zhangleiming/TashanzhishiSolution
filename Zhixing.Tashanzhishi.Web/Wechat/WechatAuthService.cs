using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Zhixing.Tashanzhishi.Web.Helper;

namespace Zhixing.Tashanzhishi.Web.Wechat
{
    /// <summary>
    /// 微信授权服务
    /// </summary>
    public class WechatAuthService
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        private string appID = "";

        /// <summary>
        /// 应用密钥
        /// </summary>
        private string appSecret = "";

        private const string SnsApi_Base_Format = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";

        private const string SnsApi_UserInfo_Format = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect";
        public WechatAuthService(string appID,string appSecret)
        {
            this.appID = appID;
            this.appSecret = appSecret;
        }

        /// <summary>
        /// 获取基础信息Url
        /// </summary>
        /// <param name="state"></param>
        /// <param name="directUrl"></param>
        /// <returns></returns>
        public string GetSnsApiBaseUrl(string state,string directUrl)
        {
            directUrl = directUrl = TashanzhishiWebUtility.UrlEncode(directUrl);
            string url = string.Format(SnsApi_Base_Format, appID, directUrl, state);
            return url;
        }

        /// <summary>
        /// 获取用户信息Url
        /// </summary>
        /// <param name="state"></param>
        /// <param name="directUrl"></param>
        /// <returns></returns>
        public string GetSnsApiUserInfoUrl(string state,string directUrl)
        {
            directUrl = TashanzhishiWebUtility.UrlEncode(directUrl);
            string url = string.Format(SnsApi_UserInfo_Format, appID, directUrl, state);
            return url;
        }

        /// <summary>
        /// 通过code获取网页授权access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetAuthCode(string code)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appID, appSecret, code);
            string fullMsg = HttpHelper.Get(url);
            return fullMsg;
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string GetUserInfo(string accessToken,string openid)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accessToken, openid);
            string fullMsg = HttpHelper.Get(url);
            return fullMsg;
        }

        /// <summary>
        /// 刷新Token，使授权过期
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public string RefreshToken(string refreshToken)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1} ",appID, refreshToken);
            string fullMsg = HttpHelper.Get(url);
            return fullMsg;
        }

        /// <summary>
        /// 获取访问令牌
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appID, appSecret);
            string fullMsg = HttpHelper.Get(url);
            return fullMsg;
        }

        /// <summary>
        /// 创建微信公众号菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="menuJSON"></param>
        /// <returns></returns>
        public string CreateMenu(string accessToken,string menuJSON)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", accessToken);
            string fullMsg = HttpHelper.Post(url, menuJSON);

            return fullMsg;
        }

        /// <summary>
        /// 获取微信发送的消息
        /// </summary>
        /// <returns></returns>
        public ReceiveMsgInfo GetWechatMsg()
        {
            string responseMsg = "";

            using (Stream inputStream = HttpContext.Current.Request.InputStream)
            {
                byte[] bytes = new byte[inputStream.Length];
                inputStream.Read(bytes, 0, bytes.Length);
                responseMsg = Encoding.UTF8.GetString(bytes);
            }

            ReceiveMsgInfo receiveMsgInfo = ParseReceiveMsgInfo(responseMsg);

            return receiveMsgInfo;
        }

        /// <summary>
        /// 返回简单文本
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string TransmitText(ReceiveMsgInfo msg, string content)
        {
            string textTpl = @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[{3}]]></Content>
                                </xml>";
            return string.Format(textTpl, msg.FromUserName, msg.ToUserName, ConvertDateTimeInt(DateTime.Now), content);
        }

        #region 助手方法

        private int ConvertDateTimeInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 解析微信消息报文
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        private ReceiveMsgInfo ParseReceiveMsgInfo(string postData)
        {
            XElement xEle = XElement.Parse(postData);

            ReceiveMsgInfo receiveMsgInfo = new ReceiveMsgInfo()
            {
                ToUserName = xEle.Element("ToUserName").Value,
                FromUserName = xEle.Element("FromUserName").Value,
                CreateTime = new DateTime(long.Parse(xEle.Element("CreateTime").Value)),
                MsgType = xEle.Element("MsgType").Value
            };

            switch (receiveMsgInfo.MsgType)
            {
                case "event":
                    receiveMsgInfo.Event = xEle.Element("Event").Value;
                    receiveMsgInfo.EventKey = xEle.Element("EventKey").Value;
                    break;
                case "text":
                    receiveMsgInfo.Content = xEle.Element("Content").Value;
                    receiveMsgInfo.MsgId = long.Parse(xEle.Element("MsgId").Value);
                    break;
                case "image":

                    receiveMsgInfo.PicUrl = xEle.Element("PicUrl").Value;
                    receiveMsgInfo.MediaId = xEle.Element("MediaId").Value;

                    break;
            }

            return receiveMsgInfo;
        }

        #endregion
    }
}
