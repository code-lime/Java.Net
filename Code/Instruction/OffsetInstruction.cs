using Java.Net.Data.Attribute;
using Java.Net.Model;

namespace Java.Net.Code.Instruction;

public sealed class OffsetInstruction : IInstruction<OffsetInstruction>
{
    [JavaRaw] public short Offset { get; set; }
    public override object[] IndexOperand => new object[] { Offset };
    public override object[] Operand => new object[] { Offset };
}
