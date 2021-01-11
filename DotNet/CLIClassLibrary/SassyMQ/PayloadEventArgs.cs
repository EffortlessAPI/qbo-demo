using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YP.SassyMQ.Lib.RabbitMQ
{
    public class PayloadEventArgs : EventArgs
    {
        public PayloadEventArgs()
            : this(default(StandardPayload), default(BasicDeliverEventArgs ))
        {}

        public PayloadEventArgs(BasicDeliverEventArgs bdea)
            : this(default(StandardPayload), bdea)
        {}

        public PayloadEventArgs(StandardPayload payload, BasicDeliverEventArgs  bdea)
        {
            this.Payload = payload;
            this.BasicDeliverEventArgs  = bdea;
        }

        public StandardPayload Payload { get; private set; }
        public BasicDeliverEventArgs BasicDeliverEventArgs  { get; }
    }
}
