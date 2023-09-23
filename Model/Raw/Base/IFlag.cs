using System;

namespace Java.Net.Model.Raw.Base;

public interface IFlag<T> where T : Enum, IConvertible
{
    T Flags { get; set; }
}
