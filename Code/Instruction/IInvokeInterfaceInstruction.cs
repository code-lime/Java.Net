using Java.Net.Model.Raw.Constant;

namespace Java.Net.Code.Instruction;

public interface IInvokeInterfaceInstruction : IInstruction
{
    IRefConstant Method { get; set; }
    ushort MethodIndex { get; set; }
    byte Count { get; set; }
}
