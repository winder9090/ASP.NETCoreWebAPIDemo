using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Models.IService;

namespace ASP.NETCoreWebAPIDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class RedisController : BaseController
    {
        private readonly IRedidService RedidService;

        public RedisController(IRedidService RedidService)
        {
            this.RedidService = RedidService;
        }

        [Route("GetValue")]
        [HttpGet]

        public IActionResult GetValue([FromQuery] RedisData redisData)
        {
            RedisData newRedisData = RedidService.GetRedisDataByKey(redisData.RedisKey);
            return SUCCESS(newRedisData);
        }

        [Route("SetValue")]
        [HttpGet]
        public IActionResult SetValue([FromQuery] RedisData redisData)
        {
            var newRedisData = RedidService.SetRedisDataByKey(redisData);
            return SUCCESS(newRedisData);
        }
    }
}
