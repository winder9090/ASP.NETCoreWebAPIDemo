using Model.System;

namespace Service.System.IService
{
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginBody"></param>
        /// <param name="logininfor"></param>
        /// <returns></returns>
        public User Login(string name, string pass);
    }
}
