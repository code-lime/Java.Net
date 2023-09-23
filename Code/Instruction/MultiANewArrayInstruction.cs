using Java.Net.Data.Attribute;
using Java.Net.Model;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Code.Instruction;

public sealed class MultiANewArrayInstruction : IInstruction<MultiANewArrayInstruction>
{
    public ClassConstant Class { get => Handle.Constants[ClassIndex] as ClassConstant; set => ClassIndex = Handle.OfConstant(value); }
    [JavaRaw] public ushort ClassIndex { get; set; }
    [JavaRaw] public byte Dimensions { get; set; }

    public override object[] IndexOperand => new object[] { ClassIndex, Dimensions };
    public override object[] Operand => new object[] { Class, Dimensions };
}
