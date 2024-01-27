using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;
using Java.Net.Model.Raw.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net.Code.Instruction;

public interface IInstruction : IRaw, IEquatable<IInstruction>
{
    ushort Position { get; set; }

    IOpCode OpCode { get; }
    object[] IndexOperand { get; }
    object[] Operand { get; }

    [InstanceOfTag]
    public static async ValueTask<IInstruction> InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader, [TagType(TagTypeAttribute.Tag.Handle)] JavaClass handle, CancellationToken cancellationToken)
        => OpCodes.OpCodeList[await reader.ReadByteAsync(cancellationToken)].Instruction(handle);
}
public abstract class IInstruction<I> : BaseRaw<I>, IInstruction where I : IInstruction<I>
{
    [JavaRaw(Index: 0, IsReaded: false, Type: JavaType.Custom)] public OpCode<I> OpCode { get; set; }
    IOpCode IInstruction.OpCode => OpCode;

    public abstract object[] Operand { get; }
    public abstract object[] IndexOperand { get; }
    public ushort Position { get; set; }

    public override ValueTask<JavaByteCodeWriter> WritePropertyAsync(JavaByteCodeWriter writer, PropertyData data, object? value, CancellationToken cancellationToken)
        => data.Index == 0 ? writer.WriteByteAsync(OpCode.Index, cancellationToken) : base.WritePropertyAsync(writer, data, value, cancellationToken);

    public override bool Equals(object? obj) => obj is IInstruction op && Equals(op);
    public virtual bool Equals(I obj) => OpCode == obj.OpCode && IndexOperand.SequenceEqual(obj.IndexOperand);
    public virtual bool Equals(IInstruction? obj) => obj is I t && Equals(t);
    public override int GetHashCode() => HashCode.Combine(OpCode, IndexOperand);
    public static bool operator ==(IInstruction<I> a, IInstruction b) => a is IInstruction op && op.Equals(b);
    public static bool operator !=(IInstruction<I> a, IInstruction b) => !(a == b);
    private string InstructionType
    {
        get
        {
            string name = GetType().Name;
            return name.EndsWith("Instruction") ? name[..^11] : name;
        }
    }

    public override string ToString() => $"{Position.ToString("X2").PadLeft(4, '0')} {OpCode} [{string.Join(",", Operand)}] - {InstructionType}";

    public static I Create(JavaClass handle, OpCode<I> opCode, Action<I>? apply = null)
    {
        I dat = Activator.CreateInstance<I>();
        dat.OpCode = opCode;
        dat.SetHandle(handle);
        apply?.Invoke(dat);
        dat.SetHandle(handle);
        return dat;
    }
}
