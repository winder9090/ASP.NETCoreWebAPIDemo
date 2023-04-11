using ASP.NETCoreWebAPIDemo.Framework;
using Common.Cache;
using Hei.Captcha;
using Infrastructure;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model.System;
using Service.System.IService;
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
        private readonly IUserService UserService;

        public LoginController(ILogger<LoginController> logger, IOptions<OptionsSetting> jwtSettings, SecurityCodeHelper captcha, IUserService UserService)
        {
            _logger = logger;
            this.jwtSettings = jwtSettings.Value;
            SecurityCodeHelper = captcha;
            this.UserService = UserService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [Route("Login")] // 路由地址规则
        [HttpGet]
        public IActionResult Login(string name, string pass, string Code, string Uuid)
        {
            // 从缓存中读取验证码并验证输入的验证码
            if (CacheHelper.Get(Uuid) is string str && !str.ToLower().Equals(Code.ToLower()))
            {
                return ToResponse(103, "验证码错误");
            }

            User user = UserService.Login(name, pass);


            if (user == null)
            {
                return ToResponse(105, "账号或密码错误");
            }

            LoginUser loginUser = new LoginUser();
            loginUser.account = user.account;
            loginUser.id = user.id;
            loginUser.password = user.password;
            loginUser.username = user.username;


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
            CacheHelper.SetCache(uuid, code);   // 将验证码保存到缓存中
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
