using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApiService.Services
{
    public class ApiAuthorizeAttribute: ActionFilterAttribute
    {
        public async override void OnActionExecuting(ActionExecutingContext context)
        {
            //拦截全局里是否带了token
            //if (string.IsNullOrEmpty(context.HttpContext.Request.Query["token"]))
            //{
            //context.Result = new JsonResult(
            //    //该类是KeeSoft框架里自带的一个返回结果集
            //    new KeeSoft.Core.ResponseMessage()
            //    {
            //        Status = KeeSoft.Core.ResponseStatus.ERROR.ToString(),
            //        Text = "token缺失"
            //    }
            //);
            //return;
            // }
            var Token = (context.HttpContext.Request.Headers).Where(x=>x.Key== "Authorization").FirstOrDefault().Value ;
            if (string.IsNullOrEmpty(Token))
            {

                // await context.HttpContext.Response.WriteAsync("获取Token失败！");
                //context.Result = new RedirectResult("/Sign/Index");
                context.Result = new JsonResult(new { msg = "获取Token失败！" }); // 返回给客户端一个匿名类
                //return;
            }
            else {

                context.Result = new JsonResult(new { msg = Token[0] }); // 返回给客户端一个匿名类
                // var keyByteArray = Encoding.ASCII.GetString(symmetricKeyAsBase64);
                //byte[] keyByteArray = System.Text.Encoding.Default.GetBytes(Token[0]);
               // var signingKey = new SymmetricSecurityKey(keyByteArray);
                //var SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.Sha256);

                return;
            }

          
            //foreach (var v in headers) {

            //    await context.HttpContext.Response.WriteAsync($"{v.Key}={v.Value}");
            //}

            ////假设有这么类可以将token解析成用户基本信息
            //KeeSoft.Mini.Utils.User user =
            //    new KeeSoft.Mini.Utils.User(context.HttpContext.Request.Query["token"]);

            ////接着我们将这个user实例注册到控制器的方法里,之后你在控制器里命名带有user的参数,
            ////将自动转成 KeeSoft.Mini.Utils.User类型的实例
            //context.ActionArguments["user"] = user;

            // 判断是否加上了不需要拦截
            var noNeedCheck = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                noNeedCheck = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(NoSignAttribute)));
            }
            if (noNeedCheck) return;

           await context.HttpContext.Response.WriteAsync("在控制器执行之前调用");

            base.OnActionExecuting(context);//httpss://blog.csdn.net/Wagsn8/article/details/84984545?depth_1-utm_source=distribute.pc_relevant.none-task&utm_source=distribute.pc_relevant.none-task
        }

    }
    /// <summary>
    /// 不需要登陆的地方加个这个空的拦截器
    /// https://www.cnblogs.com/chenxi001/p/11667947.html
    /// </summary>
    public class NoSignAttribute : ActionFilterAttribute { }

}
