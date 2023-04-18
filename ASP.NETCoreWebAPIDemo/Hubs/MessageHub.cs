using ASP.NETCoreWebAPIDemo.Extension;
using ASP.NETCoreWebAPIDemo.Framework;
using Infrastructure.App;
using Microsoft.AspNetCore.SignalR;
using Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NETCoreWebAPIDemo.Hubs
{
    public class MessageHub : Hub
    {
        //创建用户集合，用于存储所有链接的用户数据
        private static readonly List<OnlineUsers> clientUsers = new();
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        #region 客户端连接

        /// <summary>
        /// 客户端连接的时候调用
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var name = Context.User.Identity.Name;
            var ip = HttpContextExtension.GetClientUserIp(App.HttpContext);

            LoginUser loginUser = JwtUtil.GetLoginUser(App.HttpContext);
            var user = clientUsers.Any(u => u.ConnnectionId == Context.ConnectionId);
            //判断用户是否存在，否则添加集合
            if (!user && Context.User.Identity.IsAuthenticated)
            {
                OnlineUsers users = new(Context.ConnectionId, name, loginUser?.id, ip)
                {
                    Location = "City"
                };
                clientUsers.Add(users);
                Console.WriteLine($"{DateTime.Now}：{name},{Context.ConnectionId}连接服务端success，当前已连接{clientUsers.Count}个");
                Clients.All.SendAsync("welcome", $"欢迎您：{name},当前时间：{DateTime.Now}");
                logger.Info($"欢迎您：{name},当前时间：{DateTime.Now}");
            }

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 连接终止时调用。
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = clientUsers.Where(p => p.ConnnectionId == Context.ConnectionId).FirstOrDefault();
            //判断用户是否存在，否则添加集合
            if (user != null)
            {
                clientUsers.Remove(user);
                Console.WriteLine($"用户{user?.Name}离开了，当前已连接{clientUsers.Count}个");
                logger.Info($"用户{user?.Name}离开了，当前已连接{clientUsers.Count}个");
            }
            return base.OnDisconnectedAsync(exception);
        }

        #endregion

        /// <summary>
        /// 注册信息
        /// </summary>
        /// <param name="connectId"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HubMethodName("SendMessage")]
        public async Task SendMessage(string connectId, string userName, string message)
        {
            Console.WriteLine($"{connectId},message={message}");
            logger.Info($"{connectId},message={message}");

            await Clients.Client(connectId).SendAsync("receiveChat", new { userName, message });
        }
    }
}
