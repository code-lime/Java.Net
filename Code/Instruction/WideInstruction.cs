using Java.Net.Binary;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net.Code.Instruction;

public sealed class WideInstruction : IInstruction<WideInstruction>
{
    [JavaRaw(Index: 1, IsReaded: false)] public IOpCode OpCodeValue { get; set; } = null!;
    [JavaRaw] public short Index { get; set; }
    [JavaRaw(Index: 2)] public ushort Value { get; set; }
    public override async ValueTask<JavaByteCodeWriter> WritePropertyAsync(JavaByteCodeWriter writer, PropertyData data, object? value, CancellationToken cancellationToken)
    {
        switch (data.Index)
        {
            case 1:
                return await writer.WriteByteAsync(OpCodeValue.Index, cancellationToken);
            case 2:
                if (OpCodeValue.Key != OpCodes.Names.IINC) return writer;
                break;
        }
        return await base.WritePropertyAsync(writer, data, value, cancellationToken);
    }
    public override ValueTask ReadPropertyAsync(JavaByteCodeReader reader, PropertyData data, object? value, CancellationToken cancellationToken)
    {
        switch (data.Index)
        {
            case 2:
                if (OpCodeValue.Key != OpCodes.Names.IINC)
                    return ValueTask.CompletedTask;
                break;
        }
        return base.ReadPropertyAsync(reader, data, value, cancellationToken);
    }

    public override object[] IndexOperand => new object[] { OpCodeValue, Index, Value };
    public override object[] Operand => new object[] { OpCodeValue, Index, Value };
}
