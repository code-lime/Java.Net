using Java.Net.Data.Attribute;
using Java.Net.Model;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Code.Instruction;

public sealed class ConstantInstruction : IInstruction<ConstantInstruction>, IConstantInstruction
{
    public IConstant Constant { get => Handle.Constants[ConstantIndex]; set => ConstantIndex = (byte)Handle.OfConstant(value); }
    [JavaRaw] public byte ConstantIndex { get; set; }
    public override object[] IndexOperand => new object[] { ConstantIndex };
    public override object[] Operand => new object[] { Constant };
}
