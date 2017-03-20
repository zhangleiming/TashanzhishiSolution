using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Zhixing.Tashanzhishi.Web.Helper
{
    /// <summary>
    /// 通用助手扩展类
    /// </summary>
    public static class CommonHelperExtension
    {
        /// <summary>
        /// 集合为null或者空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        ///// <summary>
        ///// 反序列化JSON字符串到对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static T DeserializeJSON<T>(this string json)
        //{
        //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        //    T obj = default(T);
        //    try
        //    {
        //        obj = jsSerializer.Deserialize<T>(json);
        //    }
        //    catch
        //    {
        //    }

        //    return obj;
        //}

        ///// <summary>
        ///// 将对象序列化为JSON字符串
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static string SerializeToJSON<T>(this T obj)
        //{
        //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        //    string jsonString = default(string);
        //    try
        //    {
        //        jsonString = jsSerializer.Serialize(obj);
        //    }
        //    catch
        //    {
        //    }

        //    return jsonString;
        //}

        /// <summary>
        /// 字符串进行Base64编码
        /// </summary>
        /// <param name="inputString">输入字符串</param>
        /// <returns></returns>
        public static string ToBase64(this string inputString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            string base64String = "";
            try
            {
                base64String = Convert.ToBase64String(bytes);
            }
            catch
            {

            }

            return base64String;
        }

        /// <summary>
        /// 字符串进行Base64解码
        /// </summary>
        /// <param name="inputString">输入字符串</param>
        /// <returns></returns>
        public static string DecodeBase64(this string inputString)
        {
            string decodeString = "";
            try
            {
                byte[] bytes = Convert.FromBase64String(inputString);
                decodeString = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
            }

            return decodeString;
        }

        /// <summary>
        /// 字节数组进行Base64编码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64Bytes(this byte[] bytes)
        {
            string base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        /// <summary>
        /// 字符串Base64解码为字节数组
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] DecodeBase64Bytes(this string inputString)
        {
            byte[] bytes = null;

            try
            {
                bytes = Convert.FromBase64String(inputString);
            }
            catch
            {

            }

            return bytes;
        }

        /// <summary>
        /// 全角转化为半角
        /// </summary>
        /// <param name="inputString">输入字符串</param>
        /// <returns>半角字符串</returns>
        public static String ToDBC(this String inputString)
        {
            string dbcString = inputString;

            try
            {
                char[] c = inputString.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == 12288)
                    {
                        c[i] = (char)32;
                        continue;
                    }
                    if (c[i] > 65280 && c[i] < 65375)
                        c[i] = (char)(c[i] - 65248);
                }

                dbcString = new string(c);
            }
            catch
            {

            }

            return dbcString;
        }

        private static MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

        /// <summary>
        /// 字符串进行MD5
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MD5Hash(this string content)
        {
            byte[] contentBytes = Encoding.Default.GetBytes(content);
            byte[] hash_byte = md5Provider.ComputeHash(contentBytes);
            string resule = BitConverter.ToString(hash_byte);
            string md5Value = resule.Replace("-", "");

            return md5Value;
        }
        
        /// <summary>
        /// 获取文件的MD值
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static string FileMD5(this Stream inputStream)
        {
            string fileMD5 = "";
            try
            {
                byte[] hash_byte = md5Provider.ComputeHash(inputStream);
                string resule = BitConverter.ToString(hash_byte);
                fileMD5 = resule.Replace("-", "");
            }
            catch
            {
                fileMD5 = "";
            }

            return fileMD5;
        }

        /// <summary>
        /// Base64字符串格式化
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static string Base64Format(this string base64String)
        {
            return base64String.Replace("/", "_a").Replace("+", "_b").Replace("=", "_c");
        }

    }
}
