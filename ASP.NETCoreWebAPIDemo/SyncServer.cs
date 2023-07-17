using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ASP.NETCoreWebAPIDemo
{
    public class SyncServer : IHostedService, IDisposable
    {
        private System.Timers.Timer _timerRx;

        /// <summary>
        /// 释放托管资源，释放时触发
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine(nameof(SyncServer) + "被释放闭");
            _timerRx?.Dispose();
        }

        /// <summary>
        /// 启动任务绑定
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(SyncServer) + "已被启动");
            _timerRx = new System.Timers.Timer(interval: 60000)
            {
                AutoReset = true      //设置是执行一次（false）还是一直执行(true)；
            };

            _timerRx.Elapsed += new ElapsedEventHandler(RXdata);
            _timerRx.Start();
            return Task.FromResult(0);
        }

        /// <summary>
        /// 任务关闭时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(SyncServer) + "已被停止");
            return Task.FromResult(0);
        }


        /// <summary>
        /// 定时执行的操作，绑定到定时器上
        /// </summary>
        /// <param name="stateobject"></param>
        /// <param name="e"></param>
        private void RXdata(object stateobject, ElapsedEventArgs e)
        {
            Console.WriteLine("同步数据");
        }
    }
}
