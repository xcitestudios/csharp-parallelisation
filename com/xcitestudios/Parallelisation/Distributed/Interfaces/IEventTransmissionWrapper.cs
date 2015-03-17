namespace com.xcitestudios.Parallelisation.Distributed.Interfaces
{
    using global::com.xcitestudios.Parallelisation.Interfaces;
    using System;

    /// <summary>
    /// Useful for storing an event alongside when the event got "wrapped". Convenience
    /// interface for things such as storing a local copy of an event and when it got, for example
    /// pushed in to a queue to be handled remotely.
    /// </summary>
    /// <typeparam name="T"><see cref="IEvent{U,V}"/></typeparam>
    /// <typeparam name="U"><see cref="IEventInput"/></typeparam>
    /// <typeparam name="V"><see cref="IEventOutput"/></typeparam>
    public interface IEventTransmissionWrapper<T, U, V>
        where T: IEvent<U, V>
        where U: IEventInput
        where V: IEventOutput
    {
        /// <summary>
        /// Event this information is related to.
        /// </summary>
        T Event { get; set; }

        /// <summary>
        /// Date/Time the event was wrapped/transmitted/pushed.
        /// </summary>
        DateTime Datetime { get; set; }

        /// <summary>
        /// Total number of milliseconds between now and <see cref="Datetime"/>.
        /// </summary>
        long TotalMilliseconds { get; }
    }
}
