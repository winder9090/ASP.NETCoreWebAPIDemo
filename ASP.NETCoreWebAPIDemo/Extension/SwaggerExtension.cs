﻿using Infrastructure.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace ASP.NETCoreWebAPIDemo.Extension
{
    internal class PrivateSwagger
    {
        internal Func<Stream> GetSwaggerIndex()
        {
            return () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("ASP.NETCoreWebAPIDemo.index.html");
        }

    }


    public static class SwaggerExtension 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {

            });
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NETCoreWebAPIDemo v1");
                PrivateSwagger p = new PrivateSwagger();
                c.IndexStream = p.GetSwaggerIndex();
                c.RoutePrefix = string.Empty;
            });
        }

        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            IWebHostEnvironment hostEnvironment = App.GetRequiredService<IWebHostEnvironment>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ASP.NET Core Web API Demo",
                    Version = "v1",
                    Description = "",
                });

                try
                {
                    var tempPath = hostEnvironment.ContentRootPath;
                    //添加文档注释
                    c.IncludeXmlComments(Path.Combine(tempPath, "ASP.NETCoreWebAPIDemo.xml"), true);
                    c.IncludeXmlComments(Path.Combine(tempPath, "Model.xml"), true);
                    //c.IncludeXmlComments(Path.Combine(Directory.GetParent(tempPath).FullName, "ZR.Model", "ZRModel.xml"), true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("swagger 文档加载失败" + ex.Message);
                }

                //参考文章：http://www.zyiz.net/tech/detail-134965.html
                //需要安装包Swashbuckle.AspNetCore.Filters
                // 开启权限小锁 需要在对应的Action上添加[Authorize]才能看到
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //在header 中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "请输入Login接口返回的Token，前置Bearer。示例：Bearer {Token}",
                        Name = "Authorization",//jwt默认的参数名称,
                        Type = SecuritySchemeType.ApiKey, //指定ApiKey
                        BearerFormat = "JWT",//标识承载令牌的格式 该信息主要是出于文档目的
                        Scheme = JwtBearerDefaults.AuthenticationScheme//授权中要使用的HTTP授权方案的名称
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
