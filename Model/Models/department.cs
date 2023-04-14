using SqlSugar;

namespace Model.Models
{
    [SugarTable("department")]
    [Tenant("0")]
    public class department
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string departmentName { get; set; }

        public department()
        {
        }
    }
}
