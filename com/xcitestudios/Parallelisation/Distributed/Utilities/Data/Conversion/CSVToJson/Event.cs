namespace com.xcitestudios.Parallelisation.Distributed.Utilities.Data.Conversion.CSVToJson
{
    using global::com.xcitestudios.Parallelisation.Interfaces;

    /// <summary>
    /// Event used for converting CSV to JSON.
    /// </summary>
    public class Event : IEvent<EventInput, EventOutput>
    {
        /// <summary>
        /// Return the type of this event, this is an identifier to determine how to react to it.
        /// 
        /// For example you could use a function name, e.g. CalculateFibonacci. 
        /// 
        /// It is recommended to namespace your types so they will not conflict with others, e.g. "MyCompany.Math.CalculateFibonacci"
        /// </summary>
        public string Type
        {
            get
            {
                return "com\\xcitestudios\\Parallelisation\\Distributed\\Utilities\\Data\\Conversion\\CSVToJson";
            }
        }

        /// <summary>
        /// The input for this event that can be passed along with the event to handle it correctly.
        /// </summary>
        public EventInput Input { get; set; }

        /// <summary>
        /// The output for this event after it has been handled. This should remain null until an output has been decided.
        /// </summary>
        public EventOutput Output { get; set; }

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
