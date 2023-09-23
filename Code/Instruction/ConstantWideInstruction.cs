using Java.Net.Data.Attribute;
using Java.Net.Model;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Code.Instruction;

public sealed class ConstantWideInstruction : IInstruction<ConstantWideInstruction>, IConstantInstruction
{
    public IConstant Constant { get => Handle.Constants[ConstantIndex]; set => ConstantIndex = Handle.OfConstant(value); }
    [JavaRaw] public ushort ConstantIndex { get; set; }

    public override object[] IndexOperand => new object[] { ConstantIndex };
    public override object[] Operand => new object[] { Constant };
}
