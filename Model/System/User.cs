using SqlSugar;

namespace Model.System
{

    [SugarTable("user")]
    [Tenant("0")]
    public class User
    {
        public long id { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string username { get; set; }

        public User()
        {
        }
    }
}
