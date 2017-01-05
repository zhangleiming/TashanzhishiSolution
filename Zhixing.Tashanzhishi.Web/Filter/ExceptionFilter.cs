using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Zhixing.Tashanzhishi.Web.Filter
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 异常事件处理
        /// </summary>
        public EventHandler<ExceptionFilterEventArgs> ExceptionEventHandler;

        /// <summary>
        /// 抛出异常时执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            //验证异常，添加到模型状态
            if (filterContext.Exception is ValidationException)
            {
                var controller = filterContext.Controller as Controller;

                //获取验证异常
                ValidationException ex = filterContext.Exception as ValidationException;
                foreach(string key in ex.Data.Keys)
                {
                    controller.ModelState.AddModelError(key, ex.Data[key].ToString());
                }

                //设置为异常已处理，否则将继续抛出异常
                filterContext.ExceptionHandled = true;

                //设置操作结果
                var actionName = filterContext.RouteData.GetRequiredString("action");
                filterContext.Result = new ViewResult()
                {
                    ViewName = actionName,
                    ViewData = filterContext.Controller.ViewData
                };
            }

            //其它异常
            if (ExceptionEventHandler != null)
            {
                ExceptionFilterEventArgs eventArgs = new ExceptionFilterEventArgs()
                {
                    FilterContext = filterContext
                };
                ExceptionEventHandler(this, eventArgs);
            }

            //string logInfo = string.Format("controller:{0},action:{1},exception message:{2}",
            //                                filterContext.RouteData.Values["controller"].ToString()
            //                                , filterContext.RouteData.Values["action"].ToString()
            //                                , filterContext.Exception.Message);

        }
    }

    /// <summary>
    /// 异常过滤器事件参数
    /// </summary>
    public class ExceptionFilterEventArgs : EventArgs
    {
        /// <summary>
        /// 过滤器上下文
        /// </summary>
        public ExceptionContext FilterContext { get; set; }
    }
}