using CommandBase;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Text;

namespace MqDemo
{
    public class CapSenderBLL
    {
        private readonly ICapPublisher _capBus;

        public CapSenderBLL(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        public void CapSendWithNotTran()
        {
            _capBus.Publish($"CapSendWithNotTran", DateTime.Now);
            Console.WriteLine("发送消息");
        }


       


    }
}
