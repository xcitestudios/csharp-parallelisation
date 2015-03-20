namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP.Interfaces
{
    using com.xcitestudios.Parallelisation.Interfaces;

    /// <summary>
    /// RPC worker implementation for AMQP.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    public interface IRPCWorker<T, U, V> : IEventHandler<T, U, V>
        where T : IEvent<U, V>
        where U : IEventInput
        where V : IEventOutput
    {
        /// <summary>
        /// Start the worker.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the worker.
        /// </summary>
        void Stop();
    }
}
