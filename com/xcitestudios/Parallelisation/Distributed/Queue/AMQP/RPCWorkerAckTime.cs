namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP
{
    using System;

    /// <summary>
    /// When to send ACK when a worker.
    /// </summary>
    [Serializable]
    public enum RPCWorkerAckTime
    {
        /// <summary>
        /// Send ACK before working on the job.
        /// </summary>
        ACK_BEFORE = 1,

        /// <summary>
        /// Send ACK after working on the job.
        /// </summary>
        ACK_AFTER = 2
    }
}
