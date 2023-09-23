﻿namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(@"\^", ClassObject.PROP)]
public sealed class ClassThrowsSignature : IDescriptor<ClassThrowsSignature>, IThrowsSignature
{
    public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

    public override string ToString() => $"^{ClassTypeSignature}";
}
