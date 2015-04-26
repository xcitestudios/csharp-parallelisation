using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.xcitestudios.Parallelisation.Interfaces;
using com.xcitestudios.Parallelisation.Distributed.Utilities.Data.Conversion.CSVToJson;

namespace com.xcitestudios.Parallelisation.com.xcitestudios.Parallelisation.Distributed.Utilities.Data.Conversion.CSVToJson.AMQP
{
    /// <summary>
    /// The worker class for converting CSV to JSON.
    /// </summary>
    public class CSVToJsonWorker : IEventHandler<Event, EventInput, EventOutput>
    {
        public void Handle(Event e)
        {
            throw new NotImplementedException();
        }
    }
}
