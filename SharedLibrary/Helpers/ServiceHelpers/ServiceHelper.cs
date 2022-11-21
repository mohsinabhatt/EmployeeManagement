using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SharedLibrary
{
    public static class ServiceHelper
    {
        private const string errorMessage = "Unable to find the required services. Please add a call to 'IApplicationBuilder.UseServiceHelper' inside the 'Configure(...)' method in application startup code.";
        private static IServiceProvider provider;


        public static void UseServiceHelper(this IApplicationBuilder app)
        {
            provider = app.ApplicationServices;
        }


        public static T GetService<T>()
        {
            if(provider == null)
                throw new InvalidOperationException(errorMessage);
            return provider.GetService<T>();
        }


        public static T GetRequiredService<T>()
        {
            if (provider == null)
                throw new InvalidOperationException(errorMessage);

            return provider.GetRequiredService<T>();
        }
    }
}
