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
    /// RPC worker implementation for AMQP.
    /// </summary>
    public class RPCWorker<T, U, V> : RPCBase, IEventHandler<T, U, V>
        where T : IRoutableEvent<U, V>
        where U : IEventInput
        where V : IEventOutput
    {
        /// <summary>
        /// When to ACK, before or after working on the message.
        /// </summary>
        protected RPCWorkerAckTime AckTime { get; set; }

        /// <summary>
        /// External handler used to work on the event.
        /// </summary>
        protected IEventHandler<T, U, V> Handler { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="queueName"></param>
        /// <param name="handler"></param>
        /// <param name="ackTime"></param>
        public RPCWorker(IConnection connection, string queueName, IEventHandler<T, U, V> handler, RPCWorkerAckTime ackTime = RPCWorkerAckTime.ACK_AFTER)
            : base(connection, queueName)
        {
            AckTime = ackTime;
            Handler = handler;
        }

        /// <summary>
        /// Pick up jobs, pass the out to be handled, then send a response.
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

                if (AckTime == RPCWorkerAckTime.ACK_BEFORE)
                {
                    channel.BasicAck(result.DeliveryTag, false);
                }

                var body = result.Body;
                var props = result.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                e = GetEventObject(result);

                Handle(e);

                var response = Encoding.UTF8.GetBytes(e.SerializeJSON());
                channel.BasicPublish("", props.ReplyTo, replyProps, response);

                if (AckTime == RPCWorkerAckTime.ACK_AFTER)
                {
                    channel.BasicAck(result.DeliveryTag, false);
                }
            }

            channel.Close();
        }

        /// <summary>
        /// Take the event and push it off to AMQP, storing a reference locally.
        /// <seealso cref="IEventHandler{T,U,V}"/>.
        /// </summary>
        /// <param name="e"></param>
        public void Handle(T e)
        {
            Handler.Handle(e);
        }

        /// <summary>
        /// Handle a response and extract the event from it.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Type {T}</returns>
        protected T GetEventObject(BasicDeliverEventArgs result)
        {
            var body = Encoding.UTF8.GetString(result.Body);

            var responseObj = (T)Activator.CreateInstance(typeof(T));
            responseObj.DeserializeJSON(body);

            return responseObj;
        }

        /// <summary>
        /// Start the worker listening for jobs.
        /// </summary>
        public new void Start()
        {
            var channel = Connection.CreateModel();

            channel.BasicQos(0, 1, false);

            var consumer = new QueueingBasicConsumer(channel);

            try
            {
                channel.BasicConsume(QueueName, false, consumer);
            }
            catch (OperationInterruptedException ex)
            {
                if (ex.Message.Contains("ACCESS_REFUSED"))
                {
                    throw new ArgumentException(String.Format("Reply queue '{0}' could not be consumed (access denied or does not exist).", QueueName), ex);
                }
            }

            RunThread = new Thread(() => RunningThread(channel, consumer));

            base.Start();
        }
    }
}
