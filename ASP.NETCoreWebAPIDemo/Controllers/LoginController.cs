using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASP.NETCoreWebAPIDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Login")] // 路由地址规则
        public IActionResult Login(string name, string pass)
        {
            return SUCCESS(new
            {
                token = ""
            });
        }
    }
}
