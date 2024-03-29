using ASP.NETCoreWebAPIDemo.Extension;
using ASP.NETCoreWebAPIDemo.Filters;
using ASP.NETCoreWebAPIDemo.Framework;
using ASP.NETCoreWebAPIDemo.Hubs;
using ASP.NETCoreWebAPIDemo.Middleware;
using Common.Cache;
using Hei.Captcha;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using Yitter.IdGenerator;

namespace ASP.NETCoreWebAPIDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalActionMonitor));//全局注册
            });

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

            //注入SignalR实时通讯，默认用json传输
            services.AddSignalR(options =>
            {
                //客户端发保持连接请求到服务端最长间隔，默认30秒，改成4分钟，网页需跟着设置connection.keepAliveIntervalInMilliseconds = 12e4;即2分钟
                //options.ClientTimeoutInterval = TimeSpan.FromMinutes(4);
                //服务端发保持连接请求到客户端间隔，默认15秒，改成2分钟，网页需跟着设置connection.serverTimeoutInMilliseconds = 24e4;即4分钟
                //options.KeepAliveInterval = TimeSpan.FromMinutes(2);
            });

            // 普通验证码
            services.AddHeiCaptcha();

            // 添加内存缓存
            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            // 显示logo
            services.AddLogo();

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

            // 注入Services服务
            InjectServices(services, Configuration);

            // Swagger
            services.AddSwaggerConfig();

            // 配置MiniProfiler服务
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
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

            //开启访问静态文件/wwwroot目录文件，要放在UseRouting前面
            app.UseStaticFiles();

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

            // 开启响应缓存中间件
            // 使用CORS中间件时，必须在UseResponseCaching之前调用UseCors。
            app.UseResponseCaching();

            //恢复/启动任务
            app.UseAddTaskSchedulers();

            //使用全局异常中间件
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // HTTPS 重定向中间件 (UseHttpsRedirection) 将 HTTP 请求重定向到 HTTPS。
            app.UseHttpsRedirection();

            // 激活MiniProfiler中间件，启用MiniProfiler服务，放在UseEndpoints方法之前。
            app.UseMiniProfiler();

            app.UseEndpoints(endpoints =>
            {
                //设置socket连接
                endpoints.MapHub<MessageHub>("/msgHub");

                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 注入Services服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppService();
            services.AddSingleton(new AppSettings(configuration));
            services.AddHostedService<SyncServer>();     // 注册后台定时任务类

            //开启计划任务
            services.AddTaskSchedulers();

            //初始化db
            DbExtension.AddDb(configuration);

            //注册REDIS 服务
            var openRedis = configuration["RedisServer:open"];
            if (openRedis == "1")
            {
                RedisServer.Initalize();
            }

            // 创建 IdGeneratorOptions 对象，可在构造函数中输入 WorkerId：
            var WorkerId = configuration["SnowId:WorkerId"];
            var options = new IdGeneratorOptions(Convert.ToUInt16(WorkerId));
            // options.WorkerIdBitLength = 10; // 默认值6，限定 WorkerId 最大值为2^6-1，即默认最多支持64个节点。
            options.SeqBitLength = 10; // 默认值6，限制每毫秒生成的ID个数。若生成速度超过5万个/秒，建议加大 SeqBitLength 到 10。
            // options.BaseTime = Your_Base_Time; // 如果要兼容老系统的雪花算法，此处应设置为老系统的BaseTime。
            // ...... 其它参数参考 IdGeneratorOptions 定义。

            // 保存参数（务必调用，否则参数设置不生效）：
            YitIdHelper.SetIdGenerator(options);
        }
    }
}
