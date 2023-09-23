using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Code.Instruction;

public sealed class NewArrayInstruction : IInstruction<NewArrayInstruction>
{
    public enum Type : byte
    {
        BOOLEAN = 4,
        CHAR = 5,
        FLOAT = 6,
        DOUBLE = 7,
        BYTE = 8,
        SHORT = 9,
        INT = 10,
        LONG = 11
    }
    [JavaRaw(JavaType.Byte)] public Type ArrayType { get; set; }
    public override object[] IndexOperand => new object[] { ArrayType };
    public override object[] Operand => new object[] { ArrayType };
}
