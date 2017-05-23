using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeafileClient
{
    public static class SeafileConnectionFactory
    {
        static ISeafileWebConnection defaultConnection = new SeafileHttpConnection();

        /// <summary>
        /// Returns the default implementation for ISeafWebConnection
        /// </summary>        
        public static ISeafileWebConnection GetDefaultConnection()
        {
            return defaultConnection;
        }
    }
}
