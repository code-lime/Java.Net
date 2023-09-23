using Java.Net.Data.Descriptor.Field;
using System.Linq;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct("L", ClassObject.PROP, ClassObject.PROP, ClassObject.ZERO_ARRAY_PROP, ";")]
public sealed class ClassTypeSignature : IDescriptor<ClassTypeSignature>, IFieldTypeSignature
{
    public PackageSpecifier? PackageSpecifier { get; set; } = null;
    public SimpleClassTypeSignature SimpleClassTypeSignature { get; set; } = new SimpleClassTypeSignature();
    public ClassTypeSignatureSuffix[] ClassTypeSignatureSuffix { get; set; } = System.Array.Empty<ClassTypeSignatureSuffix>();

    [IgnoreProperty] public string FullSimplePackage
        => this.PackageSpecifier?.FullPackage + this.SimpleClassTypeSignature.Identifier.Value;
    public IDescriptor ToDescriptor(TypeVariableReader variable)
        => new ObjectType { ClassName = FullSimplePackage };

    public override string ToString() => $"L{PackageSpecifier?.ToString() ?? ""}{SimpleClassTypeSignature}{string.Join<ClassTypeSignatureSuffix>("", ClassTypeSignatureSuffix)};";
}
