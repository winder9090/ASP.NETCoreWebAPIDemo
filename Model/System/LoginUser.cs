using System.Collections.Generic;

namespace Model.System
{
    /// <summary>
    /// 登录用户信息存储
    /// </summary>
    public class LoginUser
    {
        public long id { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string username { get; set; }

        /// <summary>
        /// 权限集合
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();

        public LoginUser()
        {
        }
    }
}
