﻿using ASP.NETCoreWebAPIDemo.Extension;
using Infrastructure.Attribute;
using Infrastructure.CustomException;
using Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NETCoreWebAPIDemo.Filters
{
    /// <summary>
    /// 全局操作筛选器
    /// </summary>
    public class GlobalActionMonitor : ActionFilterAttribute
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Action请求前调用,用于验证参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ApiResult response = new();
            response.Code = (int)ResultCode.PARAM_ERROR;

            var values = context.ModelState.Values;     // 
            foreach (var item in values)
            {
                foreach (var err in item.Errors)
                {
                    if (err.ErrorMessage.Contains("JSON"))
                    {
                        return next();
                    }
                    if (!string.IsNullOrEmpty(response.Msg))
                    {
                        response.Msg += " | ";
                    }

                    response.Msg += err.ErrorMessage;
                }
            }
            if (!string.IsNullOrEmpty(response.Msg))
            {
                logger.Info($"请求参数错误,{response.Msg}");
                context.Result = new JsonResult(response);
            }
            return base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// OnResultExecuted是在Action中的代码执行之后运行的方法,用于记录操作日志
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

            //获得注解信息
            LogAttribute logAttribute = GetLogAttribute(controllerActionDescriptor);
            if (logAttribute == null) return;

            try
            {
                string method = context.HttpContext.Request.Method.ToUpper();
                // 获取当前的用户
                string userName = context.HttpContext.GetName();
                string jsonResult = string.Empty;
                if (context.Result is ContentResult result && result.ContentType == "application/json")
                {
                    jsonResult = result.Content.Replace("\r\n", "").Trim();
                }
                if (context.Result is JsonResult result2)
                {
                    jsonResult = result2.Value?.ToString();
                }
                //获取当前执行方法的类名
                //string className =  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
                //获取当前成员的名称
                //string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string controller = context.RouteData.Values["Controller"].ToString();
                string action = context.RouteData.Values["Action"].ToString();

                string ip = HttpContextExtension.GetClientUserIp(context.HttpContext);
                //var ip_info = IpTool.Search(ip);

                //SysOperLog sysOperLog = new()
                //{
                //    Status = 0,
                //    OperName = userName,
                //    OperIp = ip,
                //    OperUrl = HttpContextExtension.GetRequestUrl(context.HttpContext),
                //    RequestMethod = method,
                //    JsonResult = jsonResult,
                //    OperLocation = ip_info.Province + " " + ip_info.City,
                //    Method = controller + "." + action + "()",
                //    //Elapsed = _stopwatch.ElapsedMilliseconds,
                //    OperTime = DateTime.Now
                //};
                //HttpContextExtension.GetRequestValue(context.HttpContext, sysOperLog);

                //if (logAttribute != null)
                //{
                //    sysOperLog.Title = logAttribute?.Title;
                //    sysOperLog.BusinessType = (int)logAttribute?.BusinessType;
                //    sysOperLog.OperParam = logAttribute.IsSaveRequestData ? sysOperLog.OperParam : "";
                //    sysOperLog.JsonResult = logAttribute.IsSaveResponseData ? sysOperLog.JsonResult : "";
                //}

                LogEventInfo ei = new(LogLevel.Info, "GlobalActionMonitor", "");

                ei.Properties["jsonResult"] = !HttpMethods.IsGet(method) ? jsonResult : "";
                //ei.Properties["requestParam"] = sysOperLog.OperParam;
                ei.Properties["user"] = userName;
                logger.Log(ei);

                //OperLogService.InsertOperlog(sysOperLog);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"记录操作日志出错了#{ex.Message}");
            }
        }

        /// <summary>
        /// 获得注解信息
        /// </summary>
        /// <param name="controllerActionDescriptor"></param>
        /// <returns></returns>
        private LogAttribute GetLogAttribute(ControllerActionDescriptor controllerActionDescriptor)
        {
            var attribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                .FirstOrDefault(a => a.GetType().Equals(typeof(LogAttribute)));

            return (LogAttribute)attribute;
        }
    }
}
