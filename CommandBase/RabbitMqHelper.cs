using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace CommandBase
{
    public class RabbitMqHelper
    {
        public IConnection Conn
        {
            get
            {
                return new RabbitMQ.Client.ConnectionFactory()
                {
                    HostName = "localhost",
                    Port = 5672
                }.CreateConnection();
            }
        }


        public void Publish()
        {
            var msg = "tstInfo";
            var queName = "testQue";

           var model= Conn.CreateModel();
            //消息持久化，防止丢失
            model.QueueDeclare(queName, true, false, false, null);
            var properties = model.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            //消息转换为二进制
            var msgBody = Encoding.UTF8.GetBytes(msg);
            //消息发出到队列
            model.BasicPublish("", queName, properties, msgBody);
        }



    }
}
