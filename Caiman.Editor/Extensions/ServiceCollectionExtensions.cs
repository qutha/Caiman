using Caiman.Core.Analysis;
using Caiman.Core.DiscreteSelection;
using Caiman.Core.Matrices;
using Caiman.Core.Optimization;
using Caiman.Editor.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Caiman.Editor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueueManagedServices(this IServiceCollection services)
    {
        IEnumerable<ServiceDescriptor> queueManagedServices = services
            .Where(descriptor => descriptor.ServiceType != typeof(QueueManager) &&
                typeof(IQueueManaged).IsAssignableFrom(descriptor.ServiceType)).ToList();

        foreach (var descriptor in queueManagedServices)
        {
            services.AddSingleton<IQueueManaged>(provider =>
                (IQueueManaged)provider.GetRequiredService(descriptor.ServiceType));
        }

        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ConstructionAnalyzer>()
            .AddSingleton<ConstructionOptimizer>()
            .AddSingleton<SectionSearcher>()
            .AddSingleton<IMatrixBuilder, MatrixBuilder2D>()
            .AddSingleton<GradientFinder>()
            .AddSingleton<DerivativeFinder>();
        return services;
    }
}
