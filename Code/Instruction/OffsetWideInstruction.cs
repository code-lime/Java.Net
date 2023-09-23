using Java.Net.Data.Attribute;
using Java.Net.Model;

namespace Java.Net.Code.Instruction;

public sealed class OffsetWideInstruction : IInstruction<OffsetWideInstruction>
{
    [JavaRaw] public int Offset { get; set; }
    public override object[] IndexOperand => new object[] { Offset };
    public override object[] Operand => new object[] { Offset };
}
