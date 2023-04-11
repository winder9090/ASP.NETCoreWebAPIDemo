using Infrastructure.Attribute;
using Model.System;
using Service.System.IService;

namespace Service.System
{
    /// <summary>
    /// 登录
    /// </summary>
    [AppService(ServiceType = typeof(IUserService), ServiceLifetime = LifeTime.Transient)]
    public class UserService : BaseService<User>, IUserService
    {
        public UserService()
        {

        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="loginBody"></param>
        /// <returns></returns>
        public User Login(string name, string pass)
        {
            User user = GetFirst(it => it.account == name && it.password == pass);

            if (user == null || user.id <= 0)
            {
                return null;
            }

            return user;
        }
    }
}
