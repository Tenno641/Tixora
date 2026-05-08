using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Api.Common;

namespace Tixora.Shared.Api;

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