using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class InterfaceMethodRefConstant : BaseRefConstant<InterfaceMethodRefConstant>, IMethodRefConstant
{
    public override ConstantTag Tag => ConstantTag.InterfaceMethodRef;
    IRefConstant IMethodRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
}
