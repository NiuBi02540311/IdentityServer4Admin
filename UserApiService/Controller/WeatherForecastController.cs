using IdentityServer4Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApiService.Services;
namespace UserApiService.Controller
{
    [Authorize]
    [ApiController]
    //[Route("[controller]")]
    [Route("api/WeatherForecast/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ActionName("test1")]
        public IEnumerable<WeatherForecast> test1()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [ActionName("test2")]
        public IEnumerable<WeatherForecast> test2()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 局部的使用拦截器
        /// </summary>
        /// <returns></returns>
        
        [ApiAuthorizeAttribute]
        [ActionName("test3")]
        public IActionResult test3()
        {
            return Ok();
        }

        /// <summary>
        /// 不需要登录,使用这个[NoSign] 拦截器
        /// </summary>
        /// <returns></returns>
        [NoSign]
        [ActionName("test4")]
        public IActionResult test4()
        {
            //httpss://www.cnblogs.com/chenxi001/p/11667947.html
            return Ok();
        }
        [HttpGet]
        [ActionName("test4")]
        public IActionResult GetToken()
        {
            var authorizationHeader = Request.Headers["Authorization"].First();
            var key = authorizationHeader.Split(' ')[1];
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(key)).Split(':');

            //var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ServerSecret"]));
            var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(OAuthConfig.UserApi.Secret));
            //OAuthConfig
            //验证是否是信任用户的用户名和密码,可在数据库中查询
            if (credentials[0] == "awesome-username" && credentials[1] == "awesome-password")
            {
                var result = new
                {
                    //token = GenerateToken(serverSecret)
                };

                return Ok(result);
            }

            return BadRequest();
        }
        //private string GenerateToken(SecurityKey key)
        //{
        //    var now = DateTime.UtcNow;
        //    //发行人
        //    var issuer = _configuration["JWT:Issuer"];
        //    //接收者??
        //    var audience = _configuration["JWT:Audience"];
        //    var identity = new ClaimsIdentity();
        //    //可以放一些claim进去授权时候使用
        //    Claim claim = new Claim(ClaimTypes.Name, "Leo");
        //    identity.AddClaim(claim);
        //    //登录凭证
        //    var signingCredentials = new SigningCredentials(key,
        //        SecurityAlgorithms.HmacSha256);
        //    var handler = new JwtSecurityTokenHandler();
        //    //生成token,设置1小时的过期时间
        //    var token = handler.CreateJwtSecurityToken(issuer, audience, identity,
        //        now, now.Add(TimeSpan.FromHours(1)), now, signingCredentials);
        //    var encodedJwt = handler.WriteToken(token);
        //    return encodedJwt;
        //}
    }
}
