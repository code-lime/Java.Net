using System;
using Java.Net.Binary;
using Java.Net.Data;

namespace Java.Net.Data.Data;

public class MethodTag
{
    public IRaw? Parent { get; set; }
    public IRaw? LastOfArray { get; set; }
    public JavaByteCodeReader? Reader { get; set; }
    public int IndexOfArray { get; set; }

    public MethodTag Edit(Action<MethodTag> func)
    {
        func.Invoke(this);
        return this;
    }

    public MethodTag Copy() => new MethodTag()
    {
        Parent = Parent,
        LastOfArray = LastOfArray,
        Reader = Reader
    };

    public static readonly MethodTag NULL = null!;
    public static MethodTag Create(JavaByteCodeReader? reader, IRaw handle) => new MethodTag { Reader = reader, Parent = handle };
}
