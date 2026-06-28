using System;
using System.Collections.Generic;

namespace LiveryInstaller.SourceGenerators;

internal static class Extensions
{
    public static EquatableArray<T> ToEquatableArray<T>(this IEnumerable<T> array) where T : IEquatable<T> =>
        new([..array]);
}