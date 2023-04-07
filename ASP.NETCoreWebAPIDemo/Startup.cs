using ASP.NETCoreWebAPIDemo.Extension;
using ASP.NETCoreWebAPIDemo.Framework;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NETCoreWebAPIDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();

            // Swagger
            services.AddSwaggerConfig();

            //绑定整个对象到Model上
            services.Configure<OptionsSetting>(Configuration);

            //jwt 认证
            services.AddAuthentication(options =>
            {
                // 开启Bearer认证
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(o =>
            {
                // 添加JwtBearer服务
                o.TokenValidationParameters = JwtUtil.ValidParameters();
            });

            // 注册Services服务
            services.AddSingleton(new AppSettings(Configuration));

            // 设置跨域
            services.AddCors(options =>
            {
                // cors为策略名称，后面在web api控制器中添加的跨域策略名称要与此一致
                options.AddPolicy("cors", builder =>
                {
                    builder
                    .WithOrigins(new string[] { "http://10.10.10.82:7201", "http://localhost:3814",
                        "http://localhost:41911", "http://localhost:8081" })  // 允许指定的域访问
                    .AllowAnyHeader()       // 允许任何消息头
                    .AllowAnyMethod()       // 允许任何 HTTP 方法
                    .AllowCredentials();    // 允许跨源凭据
                    // AllowAnyOrigin表示允许任何域；
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger
            app.UseSwagger();

            //使可以多次多去body内容
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                // 允许在Url中添加access_token=[token]，直接在浏览器中访问
                if (context.Request.Query.TryGetValue("access_token", out var token))
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
                return next();
            });

            //开启路由访问
            app.UseRouting();

            // 配置CORS中间件，添加到app.UseRouting()和app.UseEndpoints()之间,core为策略名称
            app.UseCors("cors");

            //app.UseAuthentication会启用Authentication中间件，该中间件会根据当前Http请求中的Cookie信息来设置HttpContext.User属性（后面会用到），
            //所以只有在app.UseAuthentication方法之后注册的中间件才能够从HttpContext.User中读取到值，
            //这也是为什么上面强调app.UseAuthentication方法一定要放在下面的app.UseMvc方法前面，因为只有这样ASP.NET Core的MVC中间件中才能读取到HttpContext.User的值。
            //1.先开启认证
            app.UseAuthentication();
            //2.再开启授权
            app.UseAuthorization();

            // HTTPS 重定向中间件 (UseHttpsRedirection) 将 HTTP 请求重定向到 HTTPS。
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
