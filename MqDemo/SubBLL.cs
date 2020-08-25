using CommandBase;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqDemo
{
    public class SubBLL
    {
        public SubBLL()
        {
        }

        public  void Sub()
        {
            using (var model = RabbitMqHelper.Conn.CreateModel())
            {
                //消息持久化，防止丢失
                var consumer = new EventingBasicConsumer(model);
                consumer.Received +=
                (md, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine(string.Format("***接收时间:{0}，消息内容：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                };
                model.BasicConsume(RabbitMqHelper.QueName, true, consumer: consumer);

                Console.WriteLine("按任意值，退出程序");
                Console.ReadKey();
            }

        }


        public void PaymentSub()
        {
            using (var model = RabbitMqHelper.Conn.CreateModel())
            {
                model.QueueDeclare(RabbitMqHelper.TopicPaymentQueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                model.QueueBind(RabbitMqHelper.TopicPaymentQueName, RabbitMqHelper.TopicExName, routingKey: "*.payment.*");

                //消息持久化，防止丢失
                var consumer = new EventingBasicConsumer(model);
                consumer.Received +=
                (md, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine(string.Format("***接收时间:{0}，处理付款单业务：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                };
                model.BasicConsume(RabbitMqHelper.TopicPaymentQueName, true, consumer: consumer);

                Console.WriteLine("启动付款单处理程序,按任意值，退出程序");
                Console.ReadKey();
            }
        }

        public void NoticeSub()
        {
            using (var model = RabbitMqHelper.Conn.CreateModel())
            {
                //消息持久化，防止丢失
                var consumer = new EventingBasicConsumer(model);
                consumer.Received +=
                (md, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine(string.Format("***接收时间:{0}，处理消息业务：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                };
                model.BasicConsume(RabbitMqHelper.TopicNoticeQueName, true, consumer: consumer);

                Console.WriteLine("启动消息业务处理程序，按任意值，退出程序");
                Console.ReadKey();
            }
        }

        public void StackSub()
        {
            using (var model = RabbitMqHelper.Conn.CreateModel())
            {
                //消息持久化，防止丢失
                var consumer = new EventingBasicConsumer(model);
                consumer.Received +=
                (md, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine(string.Format("***接收时间:{0}，处理库存业务：{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msgBody));
                };
                model.BasicConsume(RabbitMqHelper.TopicStackQueName, true, consumer: consumer);

                Console.WriteLine("启动库存业务处理程序，按任意值，退出程序");
                Console.ReadKey();
            }
        }


    }
}
