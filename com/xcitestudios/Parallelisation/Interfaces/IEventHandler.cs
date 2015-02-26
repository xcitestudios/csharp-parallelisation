namespace com.xcitestudios.Parallelisation.Interfaces
{
    /// <summary>
    /// Handler for an event instance. This should either be generic for the event type
    /// or each type of event should have its own handler and implement this interface 
    /// only for the type of event it can handle.
    /// </summary>
    public interface IEventHandler<T, U>
        where T: IEventInput
        where U: IEventOutput
    {
        /// <summary>
        /// Take the event and either check the type to handle it appropriately or strongly
        /// type the event and read the input to create output.
        /// 
        /// It is recommended output on the event should be presumed null and set here; however
        /// if the event is to be handled by multiple objects then it could have output set in those cases.
        /// </summary>
        /// <param name="e">The IEvent instance to handle.</param>
        void Handle(IEvent<T, U> e);
    }
}
