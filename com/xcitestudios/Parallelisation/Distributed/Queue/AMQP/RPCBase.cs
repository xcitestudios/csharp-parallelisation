namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP
{
    using global::com.xcitestudios.Parallelisation.Interfaces;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Base class or AMQP RPC classes.
    /// </summary>
    abstract public class RPCBase
    {
        /// <summary>
        /// Connection to AMQP server.
        /// </summary>
        protected volatile IConnection Connection;

        /// <summary>
        /// Queue used for replies.
        /// </summary>
        protected string QueueName;

        /// <summary>
        /// Thread used to receive responses.
        /// </summary>
        protected Thread RunThread;

        /// <summary>
        /// Dispatchers active state flag.
        /// </summary>
        protected bool Running { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="queueName"></param>
        public RPCBase(IConnection connection, string queueName)
        {
            Connection = connection;
            QueueName = queueName;
        }

        /// <summary>
        /// Start the worker/dispatcher.
        /// </summary>
        public void Start()
        {
            Running = true;
            RunThread.Start();
        }

        /// <summary>
        /// Stop the worker/dispatcher.
        /// </summary>
        public void Stop()
        {
            Running = false;

            if (RunThread != null && RunThread.IsAlive)
            {
                RunThread.Join();
            }
        }
    }
}
