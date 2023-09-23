using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class MethodrefConstant : IRefConstant<MethodrefConstant>, IMethodRefConstant
{
    public override ConstantTag Tag => ConstantTag.Methodref;
    IRefConstant IMethodRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
}
