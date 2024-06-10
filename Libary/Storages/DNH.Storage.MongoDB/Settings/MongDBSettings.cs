using System;
using System.Collections.Generic;
using System.Text;

namespace DNH.Storage.MongoDB.Settings
{
    public class MongDBSettings
    {
        public string MongoConnectionString { get; set; }

        public string TablePrefix { get; set; }
    }
}
