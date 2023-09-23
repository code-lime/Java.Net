using Java.Net.Data;
using Java.Net.Data.Attribute;
using System.Collections.Generic;

namespace Java.Net.Code.Instruction;

public sealed class LookupSwitchInstruction : IInstruction<LookupSwitchInstruction>
{
    public sealed class Pair : BaseRaw<Pair>
    {
        [JavaRaw] public int Match { get; set; }
        [JavaRaw] public int Offset { get; set; }
    }
    [JavaRaw] public int Default { get; set; }
    [JavaRaw][JavaArray(JavaType.Int)] public List<Pair> Pairs { get; set; }
    public override object[] IndexOperand => new object[] { Default, Pairs.Count, Pairs.ToArray() };
    public override object[] Operand => new object[] { Default, Pairs.Count, Pairs.ToArray() };
}
