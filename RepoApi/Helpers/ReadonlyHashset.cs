using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RepositoryApi.Helpers
{
    public static class ReadonlyHashset
    {
        public static ReadonlyHashset<T> ToReadonlyHashset<T>(this IEnumerable<T> enumerable) => new ReadonlyHashset<T>(new HashSet<T>(enumerable));
    }
    public class ReadonlyHashset<T> : ISet<T>
    {
        private readonly HashSet<T> set;

        public ReadonlyHashset(HashSet<T> set) => this.set = set;

        public int Count => set.Count;

        public bool IsReadOnly => true;

        public bool Add(T item) => throw new InvalidOperationException("Tried to modify read only set");

        public void Clear() => throw new InvalidOperationException("Tried to modify read only set");

        public bool Contains(T item) => set.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => set.CopyTo(array, arrayIndex);

        public void ExceptWith(IEnumerable<T> other) => throw new InvalidOperationException("Tried to modify read only set");

        public IEnumerator<T> GetEnumerator() => set.GetEnumerator();

        public void IntersectWith(IEnumerable<T> other) => throw new InvalidOperationException("Tried to modify read only set");

        public bool IsProperSubsetOf(IEnumerable<T> other) => set.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other) => set.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<T> other) => set.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other) => set.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<T> other) => set.Overlaps(other);

        public bool Remove(T item) => throw new InvalidOperationException("Tried to modify read only set");

        public bool SetEquals(IEnumerable<T> other) => set.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<T> other) => throw new InvalidOperationException("Tried to modify read only set");

        public void UnionWith(IEnumerable<T> other) => throw new InvalidOperationException("Tried to modify read only set");

        void ICollection<T>.Add(T item) => throw new InvalidOperationException("Tried to modify read only set");

        IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();
    }
}
