using ASP.NETCoreWebAPIDemo.Extension;
using ASP.NETCoreWebAPIDemo.Framework;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddSingleton(new AppSettings(Configuration));
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

            //app.UseAuthentication������Authentication�м�������м������ݵ�ǰHttp�����е�Cookie��Ϣ������HttpContext.User���ԣ�������õ�����
            //����ֻ����app.UseAuthentication����֮��ע����м�����ܹ���HttpContext.User�ж�ȡ��ֵ��
            //��Ҳ��Ϊʲô����ǿ��app.UseAuthentication����һ��Ҫ���������app.UseMvc����ǰ�棬��Ϊֻ������ASP.NET Core��MVC�м���в��ܶ�ȡ��HttpContext.User��ֵ��
            //1.�ȿ�����֤
            app.UseAuthentication();
            //2.�ٿ�����Ȩ
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
