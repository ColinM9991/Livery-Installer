using System;
using System.Collections.Immutable;
using System.Linq;

namespace LiveryInstaller.SourceGenerators;

internal readonly struct EquatableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableArray<T>>
    where T : IEquatable<T>
{
    public static readonly EquatableArray<T> Empty = new(ImmutableArray<T>.Empty);

    public ImmutableArray<T> Array { get; } = array;

    public bool Equals(EquatableArray<T> other) =>
        Array.AsSpan().SequenceEqual(other.Array.AsSpan());

    public override bool Equals(object? obj) =>
        obj is EquatableArray<T> other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return Enumerable.Aggregate(Array, 17, (current, item) => current * 31 + (item?.GetHashCode() ?? 0));
        }
    }
}