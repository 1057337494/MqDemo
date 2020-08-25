using CommandBase;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqDemo
{
    public class TopicPublishBLL
    {
        public TopicPublishBLL()
        {
        }

        public void PublishOneInfo(string info)
        {

            using (var model = RabbitMqHelper.Conn.CreateModel())
            {
                model.ExchangeDeclare(RabbitMqHelper.TopicExName, ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);

                model.QueueDeclare(RabbitMqHelper.TopicNoticeQueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                model.QueueBind(RabbitMqHelper.TopicNoticeQueName, RabbitMqHelper.TopicExName, routingKey: "*.*notice");

                model.QueueDeclare(RabbitMqHelper.TopicPaymentQueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                model.QueueBind(RabbitMqHelper.TopicPaymentQueName, RabbitMqHelper.TopicExName, routingKey: "order.*");

                model.QueueDeclare(RabbitMqHelper.TopicStackQueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                model.QueueBind(RabbitMqHelper.TopicStackQueName, RabbitMqHelper.TopicExName, routingKey: "*.stack");

                var properties = model.CreateBasicProperties();
                properties.Persistent = true;
                var msg = info;
                //消息转换为二进制
                var msgBody = Encoding.UTF8.GetBytes(msg);

                var route = "order.notice_stack";

                //消息发出到队列
                model.BasicPublish(RabbitMqHelper.TopicExName, route, null, msgBody);
                Console.WriteLine($"发布{msg}");
            }

        }
    }
}
