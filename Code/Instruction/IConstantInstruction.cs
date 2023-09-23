using Java.Net.Model.Raw.Constant;

namespace Java.Net.Code.Instruction;

public interface IConstantInstruction : IInstruction
{
    IConstant Constant { get; set; }
}
