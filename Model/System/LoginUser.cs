namespace Model.System
{
    /// <summary>
    /// 登录用户信息存储
    /// </summary>
    public class LoginUser
    {
        public long UserId { get; set; }
        public long DeptId { get; set; }
        public string UserName { get; set; }

        public LoginUser()
        {
        }
    }
}
