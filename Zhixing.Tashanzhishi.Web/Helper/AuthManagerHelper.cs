using System;
using System.Web;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Zhixing.Tashanzhishi.Web.Helper
{
    public static class AuthManagerHelper
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userID"></param>
        /// <param name="signInAction">登录行为</param>
        public static void SignIn(string userName, string userID, Action signInAction = null)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userID));
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

            signInAction?.Invoke();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="signOutAction">注销行为</param>
        public static void SignOut(Action signOutAction = null)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            signOutAction?.Invoke();
        }

        /// <summary>
        /// 登录用户标识
        /// </summary>
        public static string NameIdentifier
        {
            get
            {
                return GetClaimValue(ClaimTypes.NameIdentifier);
            }
        }

        /// <summary>
        /// 登录用户名称
        /// </summary>
        public static string Name
        {
            get
            {
                return GetClaimValue(ClaimTypes.Name);
            }
        }

        #region 助手方法

        /// <summary>
        /// 根据类型获取对象的值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetClaimValue(string type)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.Claims.Where(s => s.Type == type).Select(s => s.Value).SingleOrDefault();
            return userName;
        }

        /// <summary>
        /// 认证管理器
        /// </summary>
        private static IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        #endregion

    }
}
