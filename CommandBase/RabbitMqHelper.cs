using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandBase
{
    public class RabbitMqHelper
    {
        private static ConnectionFactory _fac = new ConnectionFactory()
        {
            HostName = "192.168.0.134",
            Port = 5672,
            // VirtualHost = "vir1",
            UserName = "guest",
            Password = "guest"
        };

        private static IConnection _con = _fac.CreateConnection();

        public static IConnection Conn
        {
            get
            {
                return _con;
            }
        }

        public const string ExName = "TestExt";

        public const string QueName = "TestQue";


        public const string TopicExName = "TopicEx";

        public const string TopicPaymentQueName = "Topic_Payment";
        public const string TopicStackQueName = "Topic_Stack";
        public const string TopicNoticeQueName = "Topic_Notice";

    }
}
