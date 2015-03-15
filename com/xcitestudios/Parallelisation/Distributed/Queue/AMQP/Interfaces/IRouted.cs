namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using com.xcitestudios.Parallelisation.Interfaces;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">IEvent</typeparam>
    /// <typeparam name="U">IEventInput</typeparam>
    /// <typeparam name="V">IEventOutput</typeparam>
    public interface IRouted<T, U, V> : IEventHandler<T, U, V>
        where T: IEvent<U, V>
        where U: IEventInput
        where V: IEventOutput
    {
        /// <summary>
        /// 
        /// </summary>
        string RoutingKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string ReturnQueue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string CorrelationID { get; set; }
    }
}
