using CommandBase;
using DotNetCore.CAP;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqDemo
{
    public class CapSubBLL : ICapSubscribe
    {
        private readonly ICapPublisher _capBus;

        public CapSubBLL(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        [CapSubscribe("CapSendWithNotTran")]
        public void CapSendWithNotTran([FromCap]CapHeader header)
        {
            foreach (var item in header.Keys)
            {
                Console.WriteLine($"接受到消息{header[item]}");
            }
        }
    }
}
