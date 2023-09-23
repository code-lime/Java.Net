using System.Collections.Generic;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP, @"\/", ClassObject.PROP)]
public sealed class PackageSpecifier : IDescriptor<PackageSpecifier>, IDescriptor
{
    public Identifier Identifier { get; set; } = new Identifier();
    public PackageSpecifier? NextPackageSpecifier { get; set; }

    [IgnoreProperty]
    public IEnumerable<string> FullPackageArgs
    {
        get
        {
            yield return Identifier.Value;
            if (NextPackageSpecifier == null)
                yield break;
            foreach (string text in NextPackageSpecifier.FullPackageArgs)
                yield return text;
        }
    }
    [IgnoreProperty] public string FullPackage => string.Join("/", this.FullPackageArgs) + "/";
    public override string ToString() => $"{Identifier}/{NextPackageSpecifier}";
}
