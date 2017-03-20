using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Zhixing.Tashanzhishi.Web
{
    public static class TashanzhishiWebUtility
    {
        /// <summary>
        /// 获取访问的网站域名
        /// </summary>
        /// <returns></returns>
        public static string GetDomain()
        {
            return HttpContext.Current.Request.Url.Host ?? string.Empty;
        }

        /// <summary>
        /// 获取当前用户的UserAgent
        /// </summary>
        /// <returns></returns>
        public static string GetUserAgent()
        {
            var userAgent = HttpContext.Current.Request.UserAgent ?? string.Empty;
            if (userAgent.Length > 1024)
            {
                userAgent = userAgent.Substring(0, 1024);
            }

            return userAgent;
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            HttpRequest Request = HttpContext.Current.Request;
            return !string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                                ? Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim()
                                : Request.UserHostAddress ?? string.Empty;
        }

        /// <summary>
        /// 字符进行UrlEncode编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncode(string s)
        {
            return HttpContext.Current.Server.UrlEncode(s);
        }

        /// <summary>
        /// 对字符串进行UrlEncode编码
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlEncode(string s, Encoding encoding)
        {
            return System.Web.HttpUtility.UrlEncode(s, encoding);
        }
    }
}
