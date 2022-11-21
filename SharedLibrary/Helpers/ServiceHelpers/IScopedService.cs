using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary
{
    /// <summary>
    /// Inherit your service from this interface to auto register it in DI container.
    /// Make sure you call 'IServiceCollection.AddAppServices()' inside 'ConfigureServices(...)' method in StartUp class.
    /// </summary>
    public interface IScopedService
    {
    }
}
