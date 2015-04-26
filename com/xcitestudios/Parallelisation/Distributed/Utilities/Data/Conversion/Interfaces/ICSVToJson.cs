namespace com.xcitestudios.Parallelisation.Distributed.Utilities.Data.Conversion.Interfaces
{
    /// <summary>
    /// Interface for converting CSV to JSON.
    /// </summary>
    public interface ICSVToJson
    {
        /// <summary>
        /// Row limit for each event.
        /// </summary>
        uint RowLimitPerWorker { get; set; }

        /// <summary>
        /// Custom headers specified for use.
        /// </summary>
        string[] CustomHeaders { get; set; }

        /// <summary>
        /// Designate the first row of the CSV as containing headers.
        /// </summary>
        bool FirstRowIsHeaders { get; set; }

        /// <summary>
        /// Enable/disable the use of custom headers.
        /// </summary>
        bool UseCustomHeaders { get; set; }

        /// <summary>
        /// Process the CSV file.
        /// </summary>
        void Process();

        /// <summary>
        /// Is processing finished?
        /// </summary>
        /// <returns></returns>
        bool IsFinished();
    }
}
