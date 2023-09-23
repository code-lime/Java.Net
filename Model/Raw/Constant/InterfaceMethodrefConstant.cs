using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class InterfaceMethodrefConstant : IRefConstant<InterfaceMethodrefConstant>, IMethodRefConstant
{
    public override ConstantTag Tag => ConstantTag.InterfaceMethodref;
    IRefConstant IMethodRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
}
