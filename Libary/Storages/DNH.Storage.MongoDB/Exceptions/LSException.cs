using System;
using System.Collections.Generic;
using System.Text;

namespace DNH.Storage.MongoDB.Exceptions
{
    public class LSException : BaseException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The injected Exception info</param>
        public LSException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
