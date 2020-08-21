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
            _capBus.Publish($"[{DateTime.Now}]CapSendWithNotTran", DateTime.Now);
        }

    }
}
