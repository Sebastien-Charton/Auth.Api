﻿using System.Reflection;

namespace Auth.Api.Domain.Extensions;

public static class TypeUtilities
{
    public static T[] GetAllPublicConstantValues<T>(this Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
            .Select(x => (T)x.GetRawConstantValue()!)
            .ToArray();
    }
}
