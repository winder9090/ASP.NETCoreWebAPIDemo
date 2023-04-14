using Model;
using Model.Models;

namespace Service.Models.IService
{
    public interface IDepartmentService : IBaseService<department>
    {
        public PagedInfo<department> GetDepartmentList(PagerInfo pager);
    }
}
