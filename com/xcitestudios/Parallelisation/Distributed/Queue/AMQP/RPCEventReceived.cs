namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP
{
    using global::com.xcitestudios.Parallelisation.Interfaces;
    using System;

    /// <summary>
    /// Raised by <see cref="RPCDispatcher{T,U,V}"/> when a response comes back.
    /// </summary>
    /// <typeparam name="T"><see cref="IEvent{U,V}"/>. The routable event.</typeparam>
    /// <typeparam name="U"><see cref="IEventInput"/>. Type of request.</typeparam>
    /// <typeparam name="V"><see cref="IEventOutput"/>. Type of response.</typeparam>
    public class RPCEventReceived<T, U, V> : EventArgs
        where T : IEvent<U, V>
        where U : IEventInput
        where V : IEventOutput
    {
        /// <summary>
        /// The <see cref="IEvent{U,V}"/>.
        /// </summary>
        public T Event { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="e"></param>
        public RPCEventReceived(T e)
        {
            Event = e;
        }
    }
}
