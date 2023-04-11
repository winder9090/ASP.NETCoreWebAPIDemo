﻿namespace Model.System
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

        public LoginUser()
        {
        }
    }
}
