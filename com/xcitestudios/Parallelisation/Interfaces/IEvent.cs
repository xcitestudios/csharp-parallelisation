﻿namespace com.xcitestudios.Parallelisation.Interfaces
{
    using global::com.xcitestudios.Generic.Data.Manipulation.Interfaces;

    /// <summary>
    /// An event which determines the type of event and the input and output data storage for that event.
    /// </summary>
    public interface IEvent<T, U> : ISerialization
        where T: IEventInput
        where U: IEventOutput
    {
        /// <summary>
        /// Return the type of this event, this is an identifier to determine how to react to it.
        /// 
        /// For example you could use a function name, e.g. CalculateFibonacci. 
        /// 
        /// It is recommended to namespace your types so they will not conflict with others, e.g. "MyCompany.Math.CalculateFibonacci"
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Did the event get handled correctly and can the data be trusted to be correct for the request.
        /// </summary>
        /// <returns>True if the event went smoothly, false if the output is invalid.</returns>
        bool WasSuccessful { get; set; }

        /// <summary>
        /// A general human readable response, useful for providing an error message if WasSuccessful returns false.
        /// </summary>
        string ResponseMessage { get; set; }

        /// <summary>
        /// The input for this event that can be passed along with the event to handle it correctly.
        /// </summary>
        T Input { get; set; }

        /// <summary>
        /// The output for this event after it has been handled. This should remain null until an output has been decided.
        /// </summary>
        U Output { get; set; }
    }
}
