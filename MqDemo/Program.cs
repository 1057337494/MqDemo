using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using DotNetCore.CAP;
using DotNetCore.CAP.RabbitMQ;
using DotNetCore.CAP.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace MqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection service = new ServiceCollection();



            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

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
             // x.UsePostgreSql("数据库连接字符串");

             //如果你使用的 MongoDB，你可以添加如下配置：
             //x.UseMongoDB("ConnectionStrings");  //注意，仅支持MongoDB 4.0+集群

             //CAP支持 RabbitMQ、Kafka、AzureServiceBus 等作为MQ，根据使用选择配置：
             x.UseRabbitMQ(s=> {
                 s.HostName = "192.168.0.134";
                 s.Password = "guest";
                 s.UserName = "guest";
                 s.Port = 5672;
             
             });
             //x.UseKafka("ConnectionStrings");
             //x.UseAzureServiceBus("ConnectionStrings");
             x.UseDashboard();

         });


         services.AddScoped<CapSenderBLL>();
         services.AddLogging();

         service = services;
     })
     .RunConsoleAsync();
            var build = service.BuildServiceProvider();



            var bll = build.GetService<CapSenderBLL>();
            while (true)
            {
                Thread.Sleep(3 * 1000);
                bll.CapSendWithNotTran();
            }





            Console.WriteLine("Hello World!");
        }
    }
}
