using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4Admin
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            // http://localhost:5000/connect/token
            {
                var userName = context.UserName;
                var password = context.Password;

                //验证用户,这么可以到数据库里面验证用户名和密码是否正确
                var claimList = await ValidateUserAsync(userName, password);

                // 验证账号
                context.Result = new GrantValidationResult
                (
                    subject: userName,
                    authenticationMethod: "custom",
                    claims: claimList.ToArray(),
                    //identityProvider:"Jack",
                    customResponse: CustomResponse()
                 );
            }
            catch (Exception ex)
            {
                //验证异常结果
                context.Result = new GrantValidationResult()
                {
                    IsError = true,
                    Error = ex.Message
                };
            }
        }

        #region Private Method

        private Dictionary<string, object> CustomResponse() {

            //Dictionary<string, object> dic = new Dictionary<string, object>();

            //dic.Add("returnUrl", "www.baidu.com");


            //载荷（payload）
            var payload = new Dictionary<string, object>
            {
                { "ReturnUrl","www.baidu.com"},//发行人
                { "iss","流月无双"},//发行人
                { "exp", DateTimeOffset.UtcNow.AddSeconds(10).ToUnixTimeSeconds() },//到期时间
                { "sub", "testJWT" }, //主题
                { "aud", "USER" }, //用户
                { "iat", DateTime.Now.ToString() }, //发布时间 
                { "data" ,new { name="111",age=11,address="hubei"} }
            };
            return payload;
        }
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<List<Claim>> ValidateUserAsync(string loginName, string password)
        {
            //TODO 这里可以通过用户名和密码到数据库中去验证是否存在，
            // 以及角色相关信息，我这里还是使用内存中已经存在的用户和密码
            var user = OAuthMemoryData.GetTestUsers().Where(x=>x.Username==loginName && x.Password==password).FirstOrDefault();

            if (user == null)
                // throw new Exception("登录失败，用户名和密码不正确");
                return null;

            //var clamis = new List<Claim>();
            //clamis.Add(new Claim(ClaimTypes.Name, "Alun"));
            //clamis.Add(new Claim(ClaimTypes.Role, "Users"));
            //var identity = new ClaimsIdentity(clamis, "MyLogin");

            //ClaimsPrincipal principal = new ClaimsPrincipal(identity);


            return new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, $"{loginName}")
                    //new Claim("returnUrl", $"wwww.baidu.com"),
                };
        }
        #endregion
    }
}
