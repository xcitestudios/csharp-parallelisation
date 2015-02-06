namespace com.xcitestudios.Parallelisation.Interfaces
{
    /// <summary>
    /// An event which determines the type of event and the input and output data storage for that event.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Convert a JSON representation of this event in to an actual IEvent object. Either
        /// a generic "event" type of a specific instance type.
        /// </summary>
        /// <param name="jsonString">Valid JSON representing this event, minimally: {"type": "", "input": {}, "output": {}}</param>
        void Deserialize(string jsonString);

        /// <summary>
        /// Convert this event into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A representation of this event with minimally the type, input and output present, e.g.: {"type": "", "input": {}, "output": {}}</returns>
        string Serialize();

        /// <summary>
        /// Return the type of this event, this is an identifier to determine how to react to it.
        /// 
        /// For example you could use a function name, e.g. CalculateFibonacci. 
        /// 
        /// It is recommended to namespace your types so they will not conflict with others, e.g. "MyCompany.Math.CalculateFibonacci"
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The input for this event that can be passed along with the event to handle it correctly.
        /// </summary>
        IEventInput Input { get; set; }

        /// <summary>
        /// The output for this event after it has been handled. This should remain null until an output has been decided.
        /// </summary>
        IEventOutput Output { get; set; }
    }
}
