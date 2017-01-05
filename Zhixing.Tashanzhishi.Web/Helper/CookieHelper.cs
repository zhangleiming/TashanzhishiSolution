using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Zhixing.Tashanzhishi.Web.Helper
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        public static void Add(string cookieName, string value)
        {
            Add(cookieName, value, DateTime.MinValue);
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expiredAt">过期时间</param>
        /// <param name="domain">Cookie域名</param>
        public static void Add(string cookieName, string value, DateTime expiredAt, string domain = "")
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            cookie = cookie ?? new HttpCookie(cookieName);
            cookie.Value = value;
            if (expiredAt > DateTime.Now)
            {
                cookie.Expires = expiredAt;
            }
            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }
            cookie.Path = "/";

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
                return string.Empty;
            return cookie.Value;
        }

        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key, string domain = "")
        {
            HttpCookie cookie = new HttpCookie(key);
            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
    }
}
