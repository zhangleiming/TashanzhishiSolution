using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhixing.Tashanzhishi.Web.Wechat
{
    /// <summary>
    /// 接收到微信消息模型
    /// </summary>
    public class ReceiveMsgInfo
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        #region click事件参数

        /// <summary>
        /// 点击菜单拉取消息时的事件推送
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 点击菜单拉取消息时的事件推送
        /// </summary>
        public string EventKey { get; set; }

        #endregion


        #region 文本消息事件参数

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        #endregion


        #region 图片事件参数

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 图片消息媒体ID
        /// </summary>
        public string MediaId { get; set; }

        #endregion


        /// <summary>
        /// 消息ID
        /// </summary>
        public long MsgId { get; set; }
    }
}
