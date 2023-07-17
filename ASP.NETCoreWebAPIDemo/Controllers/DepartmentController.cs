using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Models.IService;
using StackExchange.Profiling;

namespace ASP.NETCoreWebAPIDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService DepartmentService;

        public DepartmentController(IDepartmentService DepartmentService)
        {
            this.DepartmentService = DepartmentService;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] PagerInfo pagerInfo)
        {
            var list = DepartmentService.GetDepartmentList(pagerInfo);
            return SUCCESS(list);
        }

        [HttpGet("export")]
        public IActionResult Export()
        {
            var list = DepartmentService.GetDepartmentList();

            string sFileName = ExportExcel(list, "Department", "单位信息");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }

        /// <summary>
        /// 获取html片段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHtml")]
        public IActionResult GetHtml()
        {
            var html = MiniProfiler.Current.RenderIncludes(HttpContext);
            return Ok(html.Value);
        }
    }
}
