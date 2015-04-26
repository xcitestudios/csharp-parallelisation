namespace com.xcitestudios.Parallelisation.Distributed.Utilities.Data.Conversion
{
    using global::com.xcitestudios.Parallelisation.Distributed.Utilities.Data.Conversion.Interfaces;
    using System;

    /// <summary>
    /// Base class for CSVToJson converters.
    /// </summary>
    public abstract class CSVToJsonAbstract : ICSVToJson
    {
        /// <summary>
        /// Headers used for the CSV (from the CSV).
        /// </summary>
        protected string[] Headers;

        /// <summary>
        /// Enable or disable the use of custom headers.
        /// </summary>
        public bool UseCustomHeaders { get; set; }

        private uint _RowLimitPerWorker = 100;
        /// <summary>
        /// Row limit for each event.
        /// </summary>
        public uint RowLimitPerWorker
        {
            get
            {
                return _RowLimitPerWorker;
            }
            set
            {
                _RowLimitPerWorker = value;
            }
        }

        /// <summary>
        /// Custom headers specified for use.
        /// </summary>
        public string[] CustomHeaders { get; set; }

        /// <summary>
        /// Designate the first row of the CSV as containing headers.
        /// </summary>
        public bool FirstRowIsHeaders { get; set; }

        /// <summary>
        /// Process the CSV file.
        /// </summary>
        public abstract void Process();

        /// <summary>
        /// Is processing finished?
        /// </summary>
        /// <returns></returns>
        public abstract bool IsFinished();

        /// <summary>
        /// Make sure we have headers to use, populate this->headers with this->customHeaders if valid.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if custom headers is true without headers specified or if no headers are internally set.</exception>
        protected void CalculateHeaders()
        {
            if (this.UseCustomHeaders)
            {
                if (this.CustomHeaders == null || this.CustomHeaders.Length == 0)
                {
                    throw new ArgumentException("Use custom headers is true but no custom headers specified");
                }
            }
            else if (this.Headers == null || this.Headers.Length == 0)
            {
                throw new ArgumentException("Cannot process, no headers in CSV and no custom headers specified");
            }
        }
    }
}
