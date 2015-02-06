namespace com.xcitestudios.Parallelisation.Interfaces
{
    /// <summary>
    /// Generic output for any event
    /// </summary>
    public interface IEventOutput
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

        /// <summary>
        /// Convert a JSON representation of this event output in to an actual IEventOutput object. Either
        /// a generic "event output" type of a specific instance type.
        /// </summary>
        /// <param name="jsonString">Representation of this output</param>
        void Deserialize(string jsonString);

        /// <summary>
        /// Convert this event output into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A representation of this output with minimally success and responseMessage e.g.: {"success": true, "responseMessage": ""}.</returns>
        string Serialize();
    }
}
