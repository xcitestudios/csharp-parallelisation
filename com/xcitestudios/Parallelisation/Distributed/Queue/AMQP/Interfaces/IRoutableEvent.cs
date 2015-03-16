namespace com.xcitestudios.Parallelisation.Distributed.Queue.AMQP.Interfaces
{
    using global::com.xcitestudios.Parallelisation.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extends <see cref="IEvent{U,V}"/> to add in an exchange name and routing key.
    /// </summary>
    /// <typeparam name="U">IEventInput</typeparam>
    /// <typeparam name="V">IEventOutput</typeparam>
    public interface IRoutableEvent<U, V> : IEvent<U, V>
        where U: IEventInput
        where V: IEventOutput
    {
        /// <summary>
        /// 
        /// Exchange to publish to, characters are restricted to: a-zA-Z0-9-_.:
        /// </summary>
        string ExchangeName { get; set; }

        /// <summary>
        /// Key used for routing.
        /// </summary>
        string RoutingKey { get; set; }
    }
}
