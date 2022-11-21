using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace SharedLibrary
{
    public static class AutoServiceRegister
    {
        public static IServiceCollection AddDatAccessServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            var scopedServiceType = typeof(ScopedServiceAttribute);
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetExportedTypes().Where(x => x.IsDefined(scopedServiceType));

                foreach (var type in types)
                {
                    if (type.IsInterface)
                    {
                        var implementationType = type.GetCustomAttribute<ScopedServiceAttribute>()?.ImplementationType ?? types.FirstOrDefault(x => x != null && type != x && type.IsAssignableFrom(x));
                        services.AddScoped(type, implementationType);
                    }
                    else
                        services.AddScoped(type);
                }
            }
            return services;
        }
    }
}
