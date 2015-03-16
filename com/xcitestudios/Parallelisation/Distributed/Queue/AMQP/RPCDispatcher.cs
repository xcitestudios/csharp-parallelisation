namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP
{
    using global::com.xcitestudios.Parallelisation.Distributed.Queue.AMQP.Interfaces;
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
    /// AMQP dispatcher for events with an RPC style response via AMQP.
    /// </summary>
    /// <typeparam name="T"><see cref="IRoutableEvent{U,V}"/></typeparam>
    /// <typeparam name="U"><see cref="IEventInput"/></typeparam>
    /// <typeparam name="V"><see cref="IEventOutput"/></typeparam>
    public class RPCDispatcher<T, U, V> : RPCBase, IEventHandler<T, U, V>
        where T : IRoutableEvent<U, V>
        where U : IEventInput
        where V : IEventOutput
    {
        /// <summary>
        /// Raised when a response is received for a known <see cref="IRoutableEvent{U,V}"/>.
        /// </summary>
        public event EventHandler EventHandled;

        /// <summary>
        /// The channel used to send jobs out.
        /// </summary>
        protected volatile IModel OutgoingChannel;

        /// <summary>
        /// Stores events so they can be married up when a response comes back.
        /// </summary>
        protected volatile Dictionary<string, RPCEventWrapper<T, U, V>> Events = new Dictionary<string, RPCEventWrapper<T, U, V>>();

        /// <summary>
        /// Maximum time any job is allowed to run.
        /// </summary>
        protected long MaximumExecutionTime = 0;

        /// <summary>
        /// Properties used to inform AMQP what the packet contains.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected IBasicProperties GetProperties(T e)
        {
            var properties = OutgoingChannel.CreateBasicProperties();

            properties.ContentType = "application/json";
            properties.DeliveryMode = 2;
            properties.CorrelationId = Guid.NewGuid().ToString();
            properties.ReplyTo = QueueName;

            return properties;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="returnQueue"></param>
        /// <param name="maximumExecutionMilliseconds">Maximum time any job can take before it is marked failed (0 for never)</param>
        public RPCDispatcher(IConnection connection, string returnQueue = null, long maximumExecutionMilliseconds = 0) 
            : base(connection, returnQueue)
        {
            MaximumExecutionTime = maximumExecutionMilliseconds;
        }

        /// <summary>
        /// Take the event and push it off to AMQP, storing a reference locally.
        /// <seealso cref="IEventHandler{T,U,V}"/>.
        /// </summary>
        /// <param name="e"></param>
        public void Handle(T e)
        {
            if (!Running)
            {
                throw new Exception("RPC is not started");
            }

            var body = Encoding.UTF8.GetBytes(e.SerializeJSON());
            var properties = GetProperties(e);

            var wrapper = new RPCEventWrapper<T, U, V>()
            {
                Event = e,
                PushedDatetime = DateTime.UtcNow
            };

            lock (Events)
            {
                Events[properties.CorrelationId] = wrapper;
                OutgoingChannel.BasicPublish(e.ExchangeName, e.RoutingKey, properties, body);
            }
        }

        /// <summary>
        /// Determine if any job is long running based on <see cref="MaximumExecutionTime"/>.
        /// </summary>
        protected void CheckLongRunning()
        {
            if (MaximumExecutionTime <= 0)
            {
                return;
            }

            lock (Events)
            {
                var keys = Events.Keys.ToArray();

                foreach (var k in keys)
                {
                    if (Events[k].TotalMilliseconds > MaximumExecutionTime)
                    {
                        var e = Events[k].Event;
                        Events.Remove(k);

                        if (e.Output == null)
                        {
                            e.Output = (V)Activator.CreateInstance(typeof(V));
                        }

                        e.Output.WasSuccessful = false;
                        e.Output.ResponseMessage = "No response received from RPC system";

                        EventHandled(this, new RPCEventReceived<T, U, V>(e));
                    }
                }
            }
        }   

        /// <summary>
        /// Thread where the incoming events are consumed an handled.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="consumer"></param>
        protected void RunningThread(IModel channel, QueueingBasicConsumer consumer)
        {
            BasicDeliverEventArgs result;
            T e;

            while (Running)
            {
                if (!consumer.Queue.Dequeue(500, out result))
                {
                    continue;
                }

                lock(Events)
                {
                    if (result.BasicProperties.CorrelationId == null || !Events.ContainsKey(result.BasicProperties.CorrelationId))
                    {
                        continue;
                    }
                }

                e = HandleEventResponse(result);

                EventHandled(this, new RPCEventReceived<T, U, V>(e));
            }

            channel.Close();
        }
        
        /// <summary>
        /// Handle a response and extract the event from it.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Type {T}</returns>
        protected T HandleEventResponse(BasicDeliverEventArgs result)
        {
            var body = Encoding.UTF8.GetString(result.Body);

            var responseObj = (T)Activator.CreateInstance(typeof(T));
            responseObj.DeserializeJSON(body);

            T e;

            lock (Events)
            {
                e = Events[result.BasicProperties.CorrelationId].Event;
                e.Output = (V)responseObj.Output;

                Events.Remove(result.BasicProperties.CorrelationId);
            }

            return e;
        }

        /// <summary>
        /// Start the dispatcher.
        /// </summary>
        public new void Start()
        {
            IModel channel;
            QueueingBasicConsumer consumer;

            SetupChannelsAndConsumer(out channel, out consumer);

            RunThread = new Thread(() => RunningThread(channel, consumer));

            base.Start();
        }

        /// <summary>
        /// Set up the outgoing channel as well as the incoming channel along with its consumer.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="consumer"></param>
        protected void SetupChannelsAndConsumer(out IModel channel, out QueueingBasicConsumer consumer)
        {
            OutgoingChannel = Connection.CreateModel();
            channel = Connection.CreateModel();

            if (QueueName == null)
            {
                QueueName = channel.QueueDeclare("", false, false, true, null).QueueName;
            }

            consumer = new QueueingBasicConsumer(channel);

            try
            {
                channel.BasicConsume(QueueName, true, consumer);
            }
            catch (OperationInterruptedException ex)
            {
                if (ex.Message.Contains("ACCESS_REFUSED"))
                {
                    throw new ArgumentException(String.Format("Reply queue '{0}' could not be consumed (access denied or does not exist).", QueueName), ex);
                }
            }
        }
    }
}
