using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add Services to the container
        return services;
    }
}
