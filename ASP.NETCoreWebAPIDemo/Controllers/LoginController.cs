using ASP.NETCoreWebAPIDemo.Framework;
using Hei.Captcha;
using Infrastructure;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model.System;
using System;

namespace ASP.NETCoreWebAPIDemo.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly OptionsSetting jwtSettings;
        private readonly SecurityCodeHelper SecurityCodeHelper;

        public LoginController(ILogger<LoginController> logger, IOptions<OptionsSetting> jwtSettings, SecurityCodeHelper captcha)
        {
            _logger = logger;
            this.jwtSettings = jwtSettings.Value;
            SecurityCodeHelper = captcha;
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

        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("captchaImage")]
        public ApiResult CaptchaImage()
        {
            string uuid = Guid.NewGuid().ToString().Replace("-", "");

            var captchaOff = "0";

            var code = SecurityCodeHelper.GetRandomEnDigitalText(4);
            byte[] imgByte = GenerateCaptcha(captchaOff, code);
            string base64Str = Convert.ToBase64String(imgByte);

            var obj = new { captchaOff, uuid, img = base64Str };// File(stream, "image/png")

            return ToJson(1, obj);
        }

        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="captchaOff"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private byte[] GenerateCaptcha(string captchaOff, string code)
        {
            byte[] imgByte;
            if (captchaOff == "1")
            {
                imgByte = SecurityCodeHelper.GetGifEnDigitalCodeByte(code);//动态gif数字字母
            }
            else if (captchaOff == "2")
            {
                imgByte = SecurityCodeHelper.GetGifBubbleCodeByte(code);//动态gif泡泡
            }
            else if (captchaOff == "3")
            {
                imgByte = SecurityCodeHelper.GetBubbleCodeByte(code);//泡泡
            }
            else
            {
                imgByte = SecurityCodeHelper.GetEnDigitalCodeByte(code);//英文字母加数字
            }

            return imgByte;
        }
    }
}
