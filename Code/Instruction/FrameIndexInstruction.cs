using Java.Net.Data.Attribute;
using Java.Net.Model;

namespace Java.Net.Code.Instruction;

public sealed class FrameIndexInstruction : IInstruction<FrameIndexInstruction>
{
    [JavaRaw] public byte FrameIndex { get; set; }
    public override object[] IndexOperand => new object[] { FrameIndex };
    public override object[] Operand => new object[] { FrameIndex };
}
