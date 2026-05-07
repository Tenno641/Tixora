using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Presentation.Common;

namespace Tixora.Shared.Presentation;

public static class MapEndpoints
{
    public static void RegisterEndpoints(
        this IServiceCollection services,
        Assembly assembly)
    {
        IEnumerable<Type> endpoints = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IEndpoint)) && !type.IsAbstract)
            .ToList();
        
        foreach (Type endpoint in endpoints)
            services.AddSingleton(typeof(IEndpoint), endpoint);
    }
}