using ASP.NETCoreWebAPIDemo.Framework;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model.System;

namespace ASP.NETCoreWebAPIDemo.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly OptionsSetting jwtSettings;

        public LoginController(ILogger<LoginController> logger, IOptions<OptionsSetting> jwtSettings)
        {
            _logger = logger;
            this.jwtSettings = jwtSettings.Value;
        }

        [HttpGet]
        [Route("Login")] // 路由地址规则
        public IActionResult Login(string name, string pass)
        {
            LoginUser loginUser = new();
            loginUser.UserId = 1;
            loginUser.UserName = name;

            return SUCCESS(JwtUtil.GenerateJwtToken(JwtUtil.AddClaims(loginUser), jwtSettings.JwtSettings));
        }
    }
}
