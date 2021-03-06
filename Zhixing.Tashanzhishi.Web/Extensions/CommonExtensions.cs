﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace Zhixing.Tashanzhishi.Web
{
    /// <summary>
    /// 通用扩展类
    /// </summary>
    public static class CommonExtensions
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
            }
            return decodeString;
        }

        /// <summary>
        /// 移除首字符串
        /// (注意避免空引用)
        /// </summary>
        public static string TrimStartString(this string str, string trimString, bool isTrimStartFirst = true)
        {
            if (isTrimStartFirst)
            {
                str = str.TrimStart();
            }
            if (str.StartsWith(trimString))
            {
                return str.Remove(0, trimString.Length);
            }
            return str;
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
            catch (Exception ex)
            {

            }

            return dbcString;
        }

        /// <summary>
        /// 格式化为 yyyy-mm-dd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DateFormat(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString("yyyy-MM-dd") : "--";
        }

        /// <summary>
        /// 获取字典的值，不存在时使用缺省值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="defaultValueFormat"></param>
        /// <returns></returns>
        public static string StateValueWithDefault<T>(this Dictionary<T,string> dic,T key,string defaultValueFormat="{0}")
        {
            string value;
            if(!dic.TryGetValue(key,out value))
            {
                value = string.Format(defaultValueFormat, key);
            }
            return value;
        }
    }
}
