﻿using Kysect.CommonLib.Reflection.TypeCache;
using System.Reflection;

namespace Kysect.CommonLib.Reflection;

public static class ReflectionTraverser
{
    public static IReadOnlyCollection<Type> GetAllClasses(IReadOnlyCollection<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass)
            .ToList();
    }

    public static IReadOnlyCollection<Type> GetAllImplementationOf<T>(IReadOnlyCollection<Assembly> assemblies)
    {
        return GetAllImplementationOf(assemblies, TypeInstanceCache<T>.Instance);
    }

    public static IReadOnlyCollection<Type> GetAllImplementationOf(IReadOnlyCollection<Assembly> assemblies, Type searchingType)
    {
        if (searchingType.IsGenericType && searchingType.IsGenericTypeDefinition)
        {
            return GetAllClasses(assemblies)
                .Where(t => FindInterfaceImplementationByGenericTypeDefinition(t, searchingType) is not null)
                .ToList();
        }

        var types = GetAllClasses(assemblies)
            .Where(t => searchingType.IsAssignableFrom(t))
            .ToList();

        return types;
    }

    public static Type? FindInterfaceImplementationByGenericTypeDefinition(Type sourceType, Type interfaceGenericTypeDefinition)
    {
        return sourceType
            .GetInterfaces()
            .Where(i => i.IsGenericType)
            .SingleOrDefault(i => i.GetGenericTypeDefinition() == interfaceGenericTypeDefinition);
    }
}