using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.xcitestudios.Parallelisation.Interfaces;
using com.xcitestudios.Parallelisation.Distributed.Queue.AMQP.Interfaces;
using com.xcitestudios.Network.Server.Connection;

namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP
{
    class BlockingRequestResponse<T, U, V> : IRouted<T, U, V>
        where T: IEvent<U, V>
        where U: IEventInput
        where V: IEventOutput
    {
        public BlockingRequestResponse()
        {
            var conn = AMQPConnection.createConnectionUsingRabbitMQ(new Network.Server.Configuration.AMQPServerConfiguration());
            conn.CreateModel();
        }

        public string RoutingKey { get; set; }

        public void Handle(T e)
        {
            throw new NotImplementedException();
        }
    }
}
