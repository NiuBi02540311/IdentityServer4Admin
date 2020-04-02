using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4Admin.Models;
using IdentityServer4.Validation;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServer4Admin.Controllers
{
    public class LoginController: Controller
    {
        [HttpGet]
        public IActionResult login() {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult login(User user)
        {
            //https://www.cnblogs.com/libingql/p/11384141.html
            if (ModelState.IsValid)
            {
                //return RedirectToAction(nameof(login));
                try
                {
                    if (user.UserName == "admin" && user.UserPassWord == "123") {
                        var claim = new Claim[]{
                            new Claim("UserName",  user.UserName),
                            new Claim("UserPass",  user.UserPassWord)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EF1DA5B7-C4FA-4240-B997-7D1701BF9BE2"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            issuer: "Issuer",
                            audience: "Audience",
                            claims: claim,
                            notBefore: DateTime.Now,
                            expires: DateTime.Now.AddSeconds(30),
                            signingCredentials: creds);

                        return Ok(
                            new {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            returnUrl="www.baidu.com"
                        });
                    }
                    ViewBag.Message = "密码错误";

                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    //Console.WriteLine(ex.Message);
                }
            }
           

            return View();
        }


        [NonAction]
        private Dictionary<string, object> CustomResponse()
        {

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
    }
}
