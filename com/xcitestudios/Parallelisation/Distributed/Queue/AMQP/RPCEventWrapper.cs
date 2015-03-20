namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP
{
    using global::com.xcitestudios.Parallelisation.Distributed.Interfaces;
    using global::com.xcitestudios.Parallelisation.Interfaces;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Used for storing an RPC event along with local information.
    /// </summary>
    /// <typeparam name="T"><see cref="IEvent{U,V}"/></typeparam>
    /// <typeparam name="U"><see cref="IEventInput"/></typeparam>
    /// <typeparam name="V"><see cref="IEventOutput"/></typeparam>
    public class RPCEventWrapper<T, U, V> : IEventTransmissionWrapper<T, U, V>
        where T : IEvent<U, V>
        where U : IEventInput
        where V : IEventOutput
    {
        /// <summary>
        /// Event pushed in to AMQP.
        /// </summary>
        public T Event { get; set; }

        /// <summary>
        /// Date/Time the event wash pushed in.
        /// </summary>
        public DateTime Datetime { get; set; }

        /// <summary>
        /// Total number of milliseconds since this event was pushed to AMQP.
        /// </summary>
        public long TotalMilliseconds {
            get {
                return (long)DateTime.UtcNow.Subtract(Datetime).TotalMilliseconds;
            }
        }
    }
}
