using Java.Net.Binary;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net.Code.Instruction;

public sealed class TableSwitchInstruction : IInstruction<TableSwitchInstruction>
{
    [JavaRaw] public int Default { get; set; }
    [JavaRaw] public int Low { get; set; }
    [JavaRaw] public int High { get; set; }
    [JavaRaw(Index: 1)] public List<int> Offsets { get; set; } = null!;
    public override ValueTask ReadPropertyAsync(JavaByteCodeReader reader, PropertyData data, object? value, CancellationToken cancellationToken)
    {
        if (data.Index == 2) value = Offsets = new int[High - Low + 1].ToList();
        return base.ReadPropertyAsync(reader, data, value, cancellationToken);
    }
    public override object[] IndexOperand => new object[] { Default, Low, High, Offsets.ToArray() };
    public override object[] Operand => new object[] { Default, Low, High, Offsets.ToArray() };
}
