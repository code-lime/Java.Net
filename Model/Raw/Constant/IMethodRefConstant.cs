using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public interface IMethodRefConstant : IRefConstant
{
    new IRefConstant DeepClone(JavaClass handle);
}
