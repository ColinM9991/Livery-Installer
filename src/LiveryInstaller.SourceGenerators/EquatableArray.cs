using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LiveryInstaller.SourceGenerators;

internal readonly struct EquatableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    public static readonly EquatableArray<T> Empty = new(ImmutableArray<T>.Empty);

    public bool IsEmpty => Array.IsDefaultOrEmpty;
    
    private ImmutableArray<T> Array { get; } = array;

    public bool Equals(EquatableArray<T> other) =>
        Array.AsSpan().SequenceEqual(other.Array.AsSpan());

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Array).GetEnumerator();

    public override bool Equals(object? obj) =>
        obj is EquatableArray<T> other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return Enumerable.Aggregate(Array, 17, (current, item) => current * 31 + (item?.GetHashCode() ?? 0));
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}