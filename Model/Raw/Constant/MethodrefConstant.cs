using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class MethodRefConstant : BaseRefConstant<MethodRefConstant>, IMethodRefConstant
{
    public override ConstantTag Tag => ConstantTag.MethodRef;
    IRefConstant IMethodRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
}
