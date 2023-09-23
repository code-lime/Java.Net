using Java.Net.Data.Attribute;
using Java.Net.Model;

namespace Java.Net.Code.Instruction;

public sealed class IIncInstruction : IInstruction<IIncInstruction>
{
    [JavaRaw] public byte FrameIndex { get; set; }
    [JavaRaw] public byte Value { get; set; }
    public override object[] IndexOperand => new object[] { FrameIndex, Value };
    public override object[] Operand => new object[] { FrameIndex, Value };
}
