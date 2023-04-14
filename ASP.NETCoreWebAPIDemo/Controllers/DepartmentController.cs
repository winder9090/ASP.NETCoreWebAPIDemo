using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Models.IService;

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
    }
}
