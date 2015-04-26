namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP.Interfaces
{
    using global::com.xcitestudios.Parallelisation.Interfaces;

    /// <summary>
    /// AMQP dispatcher for events with an RPC style response via AMQP.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface IRPCDispatcher<T, U, V> : IEventHandler<T, U, V>
        where T : IEvent<U, V>
        where U : IEventInput
        where V : IEventOutput
    {

        /// <summary>
        /// Take the event and push it off to AMQP, storing a reference locally.
        /// <seealso cref="IEventHandler{T,U,V}"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="exchangeName">Exchange name used to push to.</param>
        /// <param name="routingKey">Routing key to use for dispatch.</param>
        void Handle(T e, string exchangeName = null, string routingKey = null);

        /// <summary>
        /// Start the dispatcher.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the dispatcher.
        /// </summary>
        void Stop();
    }
}
