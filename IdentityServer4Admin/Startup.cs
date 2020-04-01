using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer4Admin
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //https://mp.weixin.qq.com/s/3CCyvn2FUHtchIx8xhJHrw
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region 内存方式
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryApiResources(OAuthMemoryData.GetApiResources())
            //    .AddInMemoryClients(OAuthMemoryData.GetClients())
            //    .AddTestUsers(OAuthMemoryData.GetTestUsers());

            #endregion

            #region 数据库存储方式 1 
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryApiResources(OAuthMemoryData.GetApiResources())
            //    .AddInMemoryClients(OAuthMemoryData.GetClients())
            //    //.AddTestUsers(OAuthMemoryData.GetTestUsers());
            //    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            #endregion

            #region 数据库存储方式 2
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(OAuthMemoryData.GetApiResources())
                //.AddInMemoryClients(OAuthMemoryData.GetClients())
                .AddClientStore<ClientStore>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
