using ASP.NETCoreWebAPIDemo.Extension;
using ASP.NETCoreWebAPIDemo.Filters;
using ASP.NETCoreWebAPIDemo.Framework;
using ASP.NETCoreWebAPIDemo.Middleware;
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
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalActionMonitor));//ȫ��ע��
            });

            // ���ÿ���
            services.AddCors(options =>
            {
                // corsΪ�������ƣ�������web api����������ӵĿ����������Ҫ���һ��
                options.AddPolicy("cors", builder =>
                {
                    builder
                    .WithOrigins(new string[] { "http://10.10.10.82:7201", "http://localhost:3814",
                        "http://localhost:41911", "http://localhost:8081" })  // ����ָ���������
                    .AllowAnyHeader()       // �����κ���Ϣͷ
                    .AllowAnyMethod()       // �����κ� HTTP ����
                    .AllowCredentials();    // �����Դƾ��
                    // AllowAnyOrigin��ʾ�����κ���
                });
            });

            // ��ͨ��֤��
            services.AddHeiCaptcha();

            // ����ڴ滺��
            services.AddMemoryCache();

            services.AddHttpContextAccessor();

            // ��ʾlogo
            services.AddLogo();

            //����������Model��
            services.Configure<OptionsSetting>(Configuration);

            //jwt ��֤
            services.AddAuthentication(options =>
            {
                // ����Bearer��֤
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddJwtBearer(o =>
            {
                // ���JwtBearer����
                o.TokenValidationParameters = JwtUtil.ValidParameters();
            });

            // ע��Services����
            InjectServices(services, Configuration);

            // Swagger
            services.AddSwaggerConfig();
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

            //ʹ���Զ�ζ�ȥbody����
            app.Use((context, next) =>
            {
                context.Request.EnableBuffering();
                // ������Url�����access_token=[token]��ֱ����������з���
                if (context.Request.Query.TryGetValue("access_token", out var token))
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {token}");
                }
                return next();
            });

            //�������ʾ�̬�ļ�/wwwrootĿ¼�ļ���Ҫ����UseRoutingǰ��
            app.UseStaticFiles();

            //����·�ɷ���
            app.UseRouting();

            // ����CORS�м������ӵ�app.UseRouting()��app.UseEndpoints()֮��,coreΪ��������
            app.UseCors("cors");

            //app.UseAuthentication������Authentication�м�������м������ݵ�ǰHttp�����е�Cookie��Ϣ������HttpContext.User���ԣ�������õ�����
            //����ֻ����app.UseAuthentication����֮��ע����м�����ܹ���HttpContext.User�ж�ȡ��ֵ��
            //��Ҳ��Ϊʲô����ǿ��app.UseAuthentication����һ��Ҫ���������app.UseMvc����ǰ�棬��Ϊֻ������ASP.NET Core��MVC�м���в��ܶ�ȡ��HttpContext.User��ֵ��
            //1.�ȿ�����֤
            app.UseAuthentication();
            //2.�ٿ�����Ȩ
            app.UseAuthorization();

            // ������Ӧ�����м��
            // ʹ��CORS�м��ʱ��������UseResponseCaching֮ǰ����UseCors��
            app.UseResponseCaching();

            //ʹ��ȫ���쳣�м��
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // HTTPS �ض����м�� (UseHttpsRedirection) �� HTTP �����ض��� HTTPS��
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// ע��Services����
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppService();
            services.AddSingleton(new AppSettings(configuration));

            //��ʼ��db
            DbExtension.AddDb(configuration);
        }
    }
}
