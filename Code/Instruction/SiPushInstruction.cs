using Java.Net.Data.Attribute;
using Java.Net.Model;

namespace Java.Net.Code.Instruction;

public sealed class SiPushInstruction : IInstruction<SiPushInstruction>
{
    [JavaRaw] public ushort Value { get; set; }
    public override object[] IndexOperand => new object[] { Value };
    public override object[] Operand => new object[] { Value };
}
