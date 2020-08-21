using DotNetCore.CAP;
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
            Console.WriteLine($"接受到消息{header.Values.Count}");
        }

    }
}
