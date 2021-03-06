﻿namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP
{
    using com.xcitestudios.Parallelisation.Interfaces;
    using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// AMQP dispatcher for events with an RPC style response via AMQP.
    /// Will stop code execution until a response is received.
    /// </summary>
    /// <typeparam name="T"><see cref="IEvent{U,V}"/></typeparam>
    /// <typeparam name="U"><see cref="IEventInput"/></typeparam>
    /// <typeparam name="V"><see cref="IEventOutput"/></typeparam>
    public class BlockingRPCDispatcher<T, U, V> : RPCDispatcher<T, U, V>
        where T : IEvent<U, V>
        where U : IEventInput
        where V : IEventOutput
    {
        /// <summary>
        /// Holds the event that is waiting for a response.
        /// </summary>
        protected T Event { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="returnQueue"></param>
        /// <param name="maximumExecutionMilliseconds">Maximum time any job can take before it is marked failed (0 for never)</param>
        public BlockingRPCDispatcher(IConnection connection, string returnQueue = null, long maximumExecutionMilliseconds = 0)
            : base(connection, returnQueue, maximumExecutionMilliseconds)
        {
            Event = default(T);
        }

        /// <summary>
        /// Take the event and push it off to AMQP, storing a reference locally.
        /// <seealso cref="IEventHandler{T,U,V}"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="exchangeName">Exchange name used to push to, falls back to <see cref="P:RPCDispatcher.DefaultExchange"/></param>
        /// <param name="routingKey">Routing key to use for dispatch, falls back to <see cref="P:RPCDispatcher.DefaultRoutingKey"/></param>
        public new void Handle(T e, string exchangeName = null, string routingKey = null)
        {
            Event = e;
            this.EventHandled += EventIncoming;
            base.Handle(Event);

            while (Event.Output == null)
            {
                Thread.Sleep(50);
            }

            e.Output = Event.Output;

            this.EventHandled -= EventIncoming;

            Event = default(T);
        }

        /// <summary>
        /// Triggered by the RPC call when the event comes back.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EventIncoming(object sender, EventArgs e)
        {
            if (!(e is RPCEventReceived<T, U, V>))
            {
                return;
            }

            var ev = (RPCEventReceived<T, U, V>)e;

            Event.Output = ev.Event.Output;
        }
    }
}
