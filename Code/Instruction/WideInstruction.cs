using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;

namespace Java.Net.Code.Instruction;

public sealed class WideInstruction : IInstruction<WideInstruction>
{
    [JavaRaw(Index: 1, IsReaded: false)] public IOpCode OpCodeValue { get; set; }
    [JavaRaw] public short Index { get; set; }
    [JavaRaw(Index: 2)] public ushort Value { get; set; }
    public override JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, PropertyData data, object value)
    {
        switch (data.Index)
        {
            case 1:
                return writer.WriteByte(OpCodeValue.Index);
            case 2:
                if (OpCodeValue.Key != OpCodes.Names.IINC) return writer;
                break;
        }
        return base.WriteProperty(writer, data, value);
    }
    public override void ReadProperty(JavaByteCodeReader reader, PropertyData data, object value)
    {
        switch (data.Index)
        {
            case 2:
                if (OpCodeValue.Key != OpCodes.Names.IINC) return;
                break;
        }
        base.ReadProperty(reader, data, value);
    }

    public override object[] IndexOperand => new object[] { OpCodeValue, Index, Value };
    public override object[] Operand => new object[] { OpCodeValue, Index, Value };
}
