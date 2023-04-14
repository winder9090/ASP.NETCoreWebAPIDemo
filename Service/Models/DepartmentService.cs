using Infrastructure.Attribute;
using Model;
using Model.Models;
using Repository;
using Service.Models.IService;
using System.Collections.Generic;

namespace Service.Models
{
    [AppService(ServiceType = typeof(IDepartmentService), ServiceLifetime = LifeTime.Transient)]
    public class DepartmentService : BaseService<department>, IDepartmentService
    {
        public PagedInfo<department> GetDepartmentList(PagerInfo pager)
        {
            int totalCount = 0;
            var lDepartment = Queryable().OrderBy(it => it.id);

            if (lDepartment == null)
            {
                pager.TotalNum = 0;
                return null;
            }
            pager.TotalNum = totalCount;


            return lDepartment.ToPage(pager);
        }


        public List<department> GetDepartmentList()
        {
            var lDepartment = Queryable().OrderBy(it => it.id).ToList();
            return lDepartment;
        }
    }
}
