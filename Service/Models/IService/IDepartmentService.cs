using Model;
using Model.Models;
using System.Collections.Generic;

namespace Service.Models.IService
{
    public interface IDepartmentService : IBaseService<department>
    {
        public PagedInfo<department> GetDepartmentList(PagerInfo pager);
        public List<department> GetDepartmentList();
    }
}
