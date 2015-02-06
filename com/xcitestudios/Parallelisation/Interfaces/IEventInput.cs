namespace com.xcitestudios.Parallelisation.Interfaces
{
    /// <summary>
    /// Generic input for any event
    /// </summary>
    public interface IEventInput
    {
        /// <summary>
        /// Convert a JSON representation of this event input in to an actual IEventInput object. Either
        /// a generic "event input" type of a specific instance type.
        /// </summary>
        /// <param name="jsonString">Representation of this input</param>
        void Deserialize(string jsonString);

        /// <summary>
        /// Convert this event input into JSON so it can be handled by anything that supports JSON.
        /// </summary>
        /// <returns>A representation of this input.</returns>
        string Serialize();
    }
}
