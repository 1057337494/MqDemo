using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace MqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection service = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddCommandLine(args).Build();
            var dbConnection = config.GetSection("ConnectionStrings:MySql").Value;
            var mqConnection = config.GetSection("AppSettings:RabbitMq").Value;

            new HostBuilder()
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddCap(x =>
                 {
                     x.UseMySql(s =>
                     {
                         s.ConnectionString = dbConnection;
                         s.TableNamePrefix = "testCap";
                     });                    
                     x.UseRabbitMQ(s =>
                     {
                         s.HostName = "192.168.0.134";
                         s.Password = "guest";
                         s.UserName = "guest";
                         s.Port = 5672;
                     });                 
                     x.UseDashboard();
                 });

                 services.AddScoped<CapSenderBLL>();
                 services.AddScoped<CapSubBLL>();
                 services.AddScoped<DirectPublishBLL>();
                 services.AddScoped<TopicPublishBLL>();
                 services.AddScoped<SubBLL>();

                 services.AddLogging();
                 service = services;
             })
             .RunConsoleAsync()
             ;

            var build = service.BuildServiceProvider();
            var type = config.GetSection("type").Value;
            if ("0".Equals(type))
            {
                var bll = build.GetService<DirectPublishBLL>();              
                while (true)
                {

                    Thread.Sleep(1 * 1000);
                    var msg = Guid.NewGuid().ToString();
                    bll.PublishOneInfo(msg);
                }
            }
            else if("1".Equals(type))
            {
                // var bll = build.GetService<CapSubBLL>();
                var bll = build.GetService<SubBLL>();
                bll.Sub();    
            }
            else if ("2".Equals(type))
            {
                
                var bll = build.GetService<TopicPublishBLL>();
                while (true)
                {

                    Thread.Sleep(1 * 1000);
                    var msg = Guid.NewGuid().ToString();
                    bll.PublishOneInfo(msg);
                }
            }
            else if ("3".Equals(type))
            {
                var bll = build.GetService<SubBLL>();
                bll.PaymentSub();
            }
            else if ("4".Equals(type))
            {
                // var bll = build.GetService<CapSubBLL>();
                var bll = build.GetService<SubBLL>();
                bll.StackSub();
            }
            else if ("5".Equals(type))
            {
                // var bll = build.GetService<CapSubBLL>();
                var bll = build.GetService<SubBLL>();
                bll.NoticeSub();
            }

            Console.ReadKey();
        }
    }
}
