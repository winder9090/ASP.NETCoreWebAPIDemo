using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ASP.NETCoreWebAPIDemo
{
    public class SyncServer : IHostedService
    {
        private System.Timers.Timer _timerRx;

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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(SyncServer) + "已被停止");
            return Task.FromResult(0);
        }

        private void RXdata(object stateobject, ElapsedEventArgs e)
        {
            Console.WriteLine("同步数据");
        }
    }
}
