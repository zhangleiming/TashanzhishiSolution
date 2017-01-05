using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Zhixing.Tashanzhishi.Web.Helper
{
    public class SecurityHelper
    {
        #region 用户密码创建以及加密 public static string CreateSalt() public static string Encrypt(UserPasswordFormat format, string cleanString, string salt)
        /// <summary>
        /// 创建加密策略
        /// </summary>
        /// <returns>string</returns>
        public static string CreateSalt()
        {
            byte[] bytSalt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytSalt);
            string salt = Convert.ToBase64String(bytSalt).ToLower();
            return salt;
        }

        /// <summary>
        /// 获取密码的密文
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        public static string GetCiphertextPassword(string password, string passwordSalt)
        {
            string ciphertextPassword = Encrypt(UserPasswordFormat.MD5Hash, password, passwordSalt).Replace("-", "");
            return ciphertextPassword;
        }

        /// <summary>
        /// 加密用户密码
        /// </summary>
        /// <param name="format">用户密码加密方案</param>
        /// <param name="cleanString">原始密码</param>
        /// <param name="salt">加密策略</param>
        /// <returns>返回加密字符串</returns>
        public static string Encrypt(UserPasswordFormat format, string cleanString, string salt)
        {
            //Byte[] clearBytes;
            //Byte[] hashedBytes;
            // System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("unicode");
            // clearBytes = encoding.GetBytes(salt.ToLower().Trim() + cleanString.Trim());
            string p = salt.ToLower().Trim() + cleanString.Trim();
            switch (format)
            {
                case UserPasswordFormat.ClearText:
                    return cleanString;
                case UserPasswordFormat.Sha1Hash:
                    return GetMd5Sha1Hash(p, Encoding.UTF8);
                case UserPasswordFormat.MD5:
                    return GetMd5(p, Encoding.UTF8, true);
                case UserPasswordFormat.MD5_16:
                    return GetMd5_16(p, Encoding.UTF8).ToUpper();
                case UserPasswordFormat.MD5Hash:
                case UserPasswordFormat.Encyrpted:
                default:
                    return GetMd5Hash(p, Encoding.UTF8);
            }
        }
        #endregion

        #region 用户密码加密格式
        /// <summary>
        /// 用户密码加密格式
        /// </summary>
        public enum UserPasswordFormat
        {
            /// <summary>
            /// 不加密
            /// </summary>
            ClearText = 0,
            /// <summary>
            /// md5加密
            /// </summary>
            MD5 = 1,
            /// <summary>
            /// md5 16位加密
            /// </summary>
            MD5_16 = 2,
            /// <summary>
            /// MD5加密 hash加密
            /// </summary>
            MD5Hash = 3,
            /// <summary>
            /// SHA1加密
            /// </summary>
            Sha1Hash = 4,
            /// <summary>
            /// MD5加密
            /// </summary>
            Encyrpted = 5,
            /// <summary>
            /// MD5加密
            /// </summary>
            default1 = 3
        }
        #endregion

        #region 设置用户Tikets public static void AuthenticationUser(string userName, bool isLong) public static void AuthenticationUser(string userName, int userNum, bool isLong)
        /// <summary>
        /// 设置用户TIKETS
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="isLong">是否长久</param>
        public static void AuthenticationUser(string userName, bool isLong)
        {
            HttpCookie cookie = new HttpCookie("QstUserName", userName);
            cookie.Expires.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
            FormsAuthentication.RedirectFromLoginPage(userName, isLong);
        }
        /// <summary>
        /// 设置用户Tikets
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userNum">通行证号码</param>
        /// <param name="isLong">是否长久</param>
        public static void AuthenticationUser(string userName, int userNum, bool isLong)
        {
            HttpCookie cookie = new HttpCookie("QstUserNum", userNum.ToString());
            cookie.Expires.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
            AuthenticationUser(userName, isLong);
        }
        #endregion

        #region md5 Hash加密 默认位utf-8 static string GetMd5Hash(string str, Encoding encode)
        /// <summary>
        /// md5 Hash加密 默认位utf-8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string str)
        {
            return GetMd5Hash(str, Encoding.UTF8);
        }
        /// <summary>
        /// md5 Hash加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string str, Encoding encode)
        {
            Byte[] clearBytes;
            Byte[] hashedBytes;
            // System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("unicode");
            clearBytes = encode.GetBytes(str);
            hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            return BitConverter.ToString(hashedBytes);
        }
        #endregion

        #region Md532  static string GetMd5(string str, System.Text.Encoding encod ,bool upper)
        /// <summary>
        /// 获取Md5加密 
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            return GetMd5(str, System.Text.Encoding.UTF8, false);
        }

        /// <summary>
        /// 获取Md5加密 
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string GetMd5(string str, System.Text.Encoding encode)
        {
            return GetMd5(str, encode, false);
        }
        /// <summary>
        /// 获取Md5加密 
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="upper">是否为大写</param>
        /// <returns></returns>
        public static string GetMd5(string str, bool upper)
        {
            return GetMd5(str, System.Text.Encoding.UTF8, upper);
        }
        /// <summary>
        /// 获取Md5加密 
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="encod">编码方案，默认为utf-8</param>
        /// <param name="upper">是否为大写</param>
        /// <returns></returns>
        public static string GetMd5(string str, System.Text.Encoding encod, bool upper)
        {
            Byte[] clearBytes;
            Byte[] hashedBytes;
            // System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("unicode");
            clearBytes = encod.GetBytes(str);
            hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            string md5 = BitConverter.ToString(hashedBytes).Replace("-", "");
            return upper ? md5.ToUpper() : md5.ToLower();
        }
        #endregion

        #region  获取Md5 Sha1Hash加密  static string GetMd5Sha1Hash(string str, System.Text.Encoding encoding)
        /// <summary>
        /// 获取Md5 Sha1Hash加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5Sha1Hash(string str)
        {
            return GetMd5Sha1Hash(str, System.Text.Encoding.GetEncoding("unicode"));
        }
        /// <summary>
        /// 获取Md5 Sha1Hash加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetMd5Sha1Hash(string str, System.Text.Encoding encoding)
        {
            Byte[] clearBytes;
            Byte[] hashedBytes;
            // System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("unicode");
            clearBytes = encoding.GetBytes(str);
            hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(clearBytes);
            return BitConverter.ToString(hashedBytes);

        }
        #endregion

        #region MD5 16位加密 编码默认utf-8  string GetMd5_16(string ConvertString,Encoding encode)
        /// <summary>
        /// MD5 16位加密 编码默认utf-8
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetMd5_16(string ConvertString)
        {
            return GetMd5_16(ConvertString, Encoding.UTF8);
        }
        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <param name="encode">编码 默认为utf-8</param>
        /// <returns></returns>
        public static string GetMd5_16(string ConvertString, Encoding encode)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            string t2 = BitConverter.ToString(md5.ComputeHash(encode.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        #endregion

        #region 推广下线参数加密（Des）

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="sourceString">加密字符串</param>
        /// <param name="key">密钥（8位）</param>
        /// <param name="iv">向量（8位）</param>
        /// <returns></returns>
        public static string DESEncrypt(string sourceString, string key, string iv)
        {
            try
            {
                byte[] btKey = Encoding.UTF8.GetBytes(key);

                byte[] btIV = Encoding.UTF8.GetBytes(iv);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);

                            cs.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                    catch
                    {
                        return sourceString;
                    }
                }
            }
            catch { }

            return "DES加密出错";
        }

        /// <summary>
        /// DES解密   
        /// </summary>
        /// <param name="encryptedString">解密字符串</param>
        /// <param name="key">密钥（8位）</param>
        /// <param name="iv">向量（8位）</param>
        /// <returns></returns>
        public static string DESDecrypt(string encryptedString, string key, string iv)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(key);

            byte[] btIV = Encoding.UTF8.GetBytes(iv);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return encryptedString;
                }
            }
        }
        #endregion


    }
}
