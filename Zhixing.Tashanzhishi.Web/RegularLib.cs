using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhixing.Tashanzhishi.Web
{
    /// <summary>
    /// 正则表达式库
    /// </summary>
    public static class RegularLib
    {
        /// <summary>
        /// 电子邮件
        /// </summary>
        public static readonly string Email = @"(\w + ([-+.]\w +) *@\w+([-.]\w+)*\.\w+([-.]\w+)*)";

        /// <summary>
        /// 手机号
        /// </summary>
        public static readonly string PhoneNo = @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";

        /// <summary>
        /// 电子邮件或手机号
        /// </summary>
        public static readonly string EmailOrPhoneNo = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(13[0-9]{9})|(\d{5,13})";

        /// <summary>
        /// QQ号码
        /// </summary>
        public static readonly string QQ = @"^[1-9]\d[0-9]{4,9}$";

        /// <summary>
        /// 身份证号
        /// </summary>
        public static readonly string IDNumber = @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";

    }
}
