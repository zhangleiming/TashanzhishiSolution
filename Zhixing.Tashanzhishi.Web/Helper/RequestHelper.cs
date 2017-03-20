using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zhixing.Tashanzhishi.Web.Models;

namespace Zhixing.Tashanzhishi.Web.Helper
{
    /// <summary>
    /// 请求帮助类
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// 获取http请求参数
        /// </summary>
        /// <returns></returns>
        public static RequestModel GetRequestInfo()
        {
            RequestModel requestInfo = new RequestModel() {
                IP = TashanzhishiWebUtility.GetIP(),
                Domain = TashanzhishiWebUtility.GetDomain(),
                UserAgent = TashanzhishiWebUtility.GetUserAgent()
            };

            return requestInfo;
        }
    }
}
