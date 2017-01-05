using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Newtonsoft.Json;

namespace Zhixing.Tashanzhishi.Web.Filter
{
    /// <summary>
    /// JSON反序列化过滤器
    /// </summary>
    public class JsonFilterAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        protected Type DataType { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        protected string ModelName { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="type">参数类型,如果是数组请输入数组元素类型</param>
        /// <param name="modelName">参数名称</param>
        public JsonFilterAttribute(Type type, string modelName)
        {
            DataType = type;
            ModelName = modelName;
        }

        /// <summary>
        /// 参数转换处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string json = filterContext.HttpContext.Request.Form[ModelName];
            object obj = null;

            try
            {
                obj = JsonConvert.DeserializeObject(json, DataType);
            }
            catch (Exception ex)
            {
                obj = null;
            }

            filterContext.ActionParameters[ModelName] = obj;
        }
    }

}
