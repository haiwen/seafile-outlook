﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeafileClient
{
    /// <summary>
    /// Interface for classes which execute seafile web api requests
    /// </summary>
    public interface ISeafileWebConnection
    {
        /// <summary>
        /// Execute the given Seafile request and return the response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serverUri">uri of the seafile server</param>
        /// <param name="request">The request to send</param>        
        Task<T> SendRequestAsync<T>(Uri serverUri, SeafRequest<T> request, CancellationToken token);

        Task<T> SendRequestAsync<T>(Uri serverUri, SeafRequest<T> request);
    }
}
