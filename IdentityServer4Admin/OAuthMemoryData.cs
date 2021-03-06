﻿
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Admin
{
    public class OAuthMemoryData
    {
        /// <summary>
        /// Api资源 静态方式定义
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
                       {
                            new ApiResource(OAuthConfig.UserApi.ApiName,OAuthConfig.UserApi.ApiName),
                       };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
       {
           new Client()
           {
               ClientId =OAuthConfig.UserApi.ClientId,
               AllowedGrantTypes = new List<string>()
               {
                   GrantTypes.ResourceOwnerPassword.FirstOrDefault(),//Resource Owner Password模式 密码模式
               },
               ClientSecrets = {new Secret(OAuthConfig.UserApi.Secret.Sha256()) },
               AllowedScopes= {OAuthConfig.UserApi.ApiName}, // 允许访问的资源， 这里定义的是 user_api
               AccessTokenLifetime = OAuthConfig.ExpireIn,
           },
              new Client()
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,  //设置模式，客户端模式
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },
                new Client()
                {
                    ClientId="pwdClient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword, //密码模式
                    ClientSecrets= {new Secret("secret".Sha256()) },
                    AllowedScopes= { "api1" }
                }
      };
        }

        /// <summary>
        /// 测试的账号和密码
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
    {
        new TestUser()
        {
             SubjectId = "1",
             Username = "test",
             Password = "123456"
        }
    };
        }
    }

    public class OAuthConfig
    {
        /// <summary>
        /// 过期秒数
        /// </summary>
        public const int ExpireIn = 36000;

        /// <summary>
        /// 用户Api相关
        /// </summary>
        public static class UserApi
        {
            public static string ApiName = "user_api";

            public static string ClientId = "user_clientid";

            public static string Secret = "user_secret"; // 这个是秘钥
        }
    }
}
