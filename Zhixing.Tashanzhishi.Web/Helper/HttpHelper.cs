using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zhixing.Tashanzhishi.Web.Helper
{
    public static class HttpHelper
    {
        /// <summary>
        /// Get访问URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            string responseContent = "";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Timeout = 5 * 1000;
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
            httpWebRequest.Headers.Add("Accept-Charset", "gb2312,utf-8;q=0.7,*;q=0.7");
            httpWebRequest.Headers.Set("Pragma", "no-cache");

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    responseContent = responseReader.ReadToEnd();
                }
            }

            return responseContent;

        }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string Post(string url, string postData)
        {
            string responseContent = "";

            byte[] postBytes = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Timeout = 5 * 60 * 1000;// 90000;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = postBytes.Length;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
            httpWebRequest.Headers.Add("Accept-Charset", "gb2312,utf-8;q=0.7,*;q=0.7");
            httpWebRequest.Headers.Set("Pragma", "no-cache");
            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        responseContent = responseReader.ReadToEnd();
                    }
                }
            }

            return responseContent;
        }
    }
}
