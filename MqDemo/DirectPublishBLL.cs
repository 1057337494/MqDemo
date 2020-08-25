using CommandBase;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqDemo
{
    public class DirectPublishBLL
    {
        public DirectPublishBLL()
        {
        }

        public void PublishOneInfo(string info)
        {

            using (var model = RabbitMqHelper.Conn.CreateModel())
            {
                model.ExchangeDeclare(RabbitMqHelper.ExName, ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
                model.QueueDeclare(RabbitMqHelper.QueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                model.QueueBind(RabbitMqHelper.QueName, RabbitMqHelper.ExName, routingKey: RabbitMqHelper.QueName);


                //消息持久化，防止丢失
                // model.QueueDeclare(queName, true, false, false, null);
                var properties = model.CreateBasicProperties();
                properties.Persistent = true;
                var msg = info;
                //消息转换为二进制
                var msgBody = Encoding.UTF8.GetBytes(msg);
                //消息发出到队列
                model.BasicPublish(RabbitMqHelper.ExName, RabbitMqHelper.QueName, properties, msgBody);
                Console.WriteLine($"发布{msg}");
            }

        }
    }
}
