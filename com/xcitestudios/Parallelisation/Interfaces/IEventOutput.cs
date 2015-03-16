namespace com.xcitestudios.Parallelisation.Interfaces
{
    using global::com.xcitestudios.Generic.Data.Manipulation.Interfaces;

    /// <summary>
    /// Generic output for any event
    /// </summary>
    public interface IEventOutput : ISerialization
    {
        /// <summary>
        /// Did the event get handled correctly and can the data be trusted to be correct for the request.
        /// </summary>
        /// <returns>True if the event went smoothly, false if the output is invalid.</returns>
        bool WasSuccessful { get; set; }

        /// <summary>
        /// A general human readable response, useful for providing an error message if WasSuccessful returns false.
        /// </summary>
        string ResponseMessage { get; set; }
    }
}
