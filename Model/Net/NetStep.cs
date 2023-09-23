using System;
using System.Collections.Generic;
using System.Linq;

namespace Java.Net.Model.Net;

public class NetStep<T> : IEnumerator<T>
{
    private readonly List<IEnumerator<T>> list;
    internal NetStep(List<IEnumerator<T>> list) => this.list = list;
    public bool MoveNext() => list.RepeatStep();

    public T Current => throw new NotSupportedException();
    object System.Collections.IEnumerator.Current => throw new NotSupportedException();
    public void Dispose() => throw new NotSupportedException();
    public void Reset() => throw new NotSupportedException();

    public static NetStep<T> Of(params IEnumerator<T>[] list) => new NetStep<T>(new List<IEnumerator<T>>(list));
    public static NetStep<T> Of(IEnumerable<IEnumerator<T>> list) => new NetStep<T>(list.ToList());
}
