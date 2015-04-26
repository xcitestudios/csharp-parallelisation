namespace com.xcitestudios.Parallelisation.Distributed.Utilities.Data.Conversion.CSVToJson
{
    using global:: com.xcitestudios.Parallelisation.Interfaces;

    /// <summary>
    /// Event output for converting CSV to JSON.
    /// </summary>
    public class EventOutput : IEventOutput
    {
        /// <summary>
        /// Did the event get handled correctly and can the data be trusted to be correct for the request.
        /// </summary>
        /// <returns>True if the event went smoothly, false if the output is invalid.</returns>
        public bool WasSuccessful { get; set; }

        /// <summary>
        /// A general human readable response, useful for providing an error message if WasSuccessful returns false.
        /// </summary>
        public string ResponseMessage { get; set; }

        public void DeserializeJSON(string jsonString)
        {
            throw new System.NotImplementedException();
        }

        public string SerializeJSON()
        {
            throw new System.NotImplementedException();
        }
    }
}
