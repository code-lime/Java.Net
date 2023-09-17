using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java.Net.Data
{
    /*public interface IInstruction : IEquatable<IInstruction>
    {
        Java.Net.Code.OpCode OpCode { get; }
        object[] Operand { get; }
    }
    public abstract class Instruction : IInstruction
    {
        public Java.Net.Code.OpCode OpCode { get; }
        public abstract object[] Operand { get; }

        public override bool Equals(object obj) => obj is IInstruction op && Equals(op);
        public virtual bool Equals(IInstruction obj) => obj == null ? false : (OpCode == obj.OpCode && Enumerable.SequenceEqual(Operand, obj.Operand));
        public override int GetHashCode() => HashCode.Combine(OpCode, Operand);
        public static bool operator ==(Instruction a, IInstruction b) => a is IInstruction op && op.Equals(b);
        public static bool operator !=(Instruction a, IInstruction b) => !(a == b);
        private string InstructionType
        {
            get
            {
                string name = GetType().Name;
                return name.EndsWith("Instruction") ? name[..^11] : name;
            }
        }

        public override string ToString() => $"{OpCode} [{string.Join(",", Operand)}] - {InstructionType}";
    }
    public sealed class SingleInstruction : Instruction
    {
        public override object[] Operand => new object[0];
    }
    public abstract class ValuePushInstruction<T> : Instruction
    {
        public T Value { get; set; }
        public override object[] Operand => new object[] { Value };

        public sealed class Byte : ValuePushInstruction<byte> { }
        public sealed class UShort : ValuePushInstruction<ushort> { }
        public sealed class Integer : ValuePushInstruction<int> { }
        public sealed class Single : ValuePushInstruction<float> { }
        public sealed class String : ValuePushInstruction<string> { }
        public sealed class Class : ValuePushInstruction<Signature.ITypeSignature> { }
        public sealed class MethodType : ValuePushInstruction<Descriptor.MethodDescriptor> { }
        public sealed class MethodHandle : ValuePushInstruction<MethodDescriptor> { }
    }
    public class 
    public sealed class IIncInstruction : IInstruction<IIncInstruction>
    {
        [IJava] public byte FrameIndex { get; set; }
        [IJava] public byte Value { get; set; }
        public override object[] IndexOperand => new object[] { FrameIndex, Value };
        public override object[] Operand => new object[] { FrameIndex, Value };
    }
    public interface IInvokeInterfaceInstruction : IInstruction
    {
        IRefConstant Method { get; set; }
        ushort MethodIndex { get; set; }
        byte Count { get; set; }
    }
    public sealed class InvokeInterfaceInstruction : IInstruction<InvokeInterfaceInstruction>, IInvokeInterfaceInstruction
    {
        public IRefConstant Method { get => Handle.Constants[MethodIndex] as IRefConstant; set => MethodIndex = Handle.OfConstant(value); }
        [IJava] public ushort MethodIndex { get; set; }
        [IJava] public byte Count { get; set; }
        public override object[] IndexOperand => new object[] { MethodIndex, Count, 0 };
        public override object[] Operand => new object[] { Method, Count, 0 };
    }
    public sealed class InvokeDynamicInstruction : IInstruction<InvokeDynamicInstruction>, IInvokeInterfaceInstruction
    {
        public IRefConstant Method { get => Handle.Constants[MethodIndex] as IRefConstant; set => MethodIndex = Handle.OfConstant(value); }
        [IJava] public ushort MethodIndex { get; set; }
        public byte Count { get => 0; set { } }
        public override object[] IndexOperand => new object[] { MethodIndex, Count, 0 };
        public override object[] Operand => new object[] { Method, Count, 0 };
    }
    public sealed class LookupSwitchInstruction : IInstruction<LookupSwitchInstruction>
    {
        public sealed class Pair : IJava<Pair>
        {
            [IJava] public int Match { get; set; }
            [IJava] public int Offset { get; set; }
        }
        [IJava] public int Default { get; set; }
        [IJava] [IJavaArray(IJavaType.Int)] public List<Pair> Pairs { get; set; }
        public override object[] IndexOperand => new object[] { Default, Pairs.Count, Pairs.ToArray() };
        public override object[] Operand => new object[] { Default, Pairs.Count, Pairs.ToArray() };
    }
    public sealed class TableSwitchInstruction : IInstruction<TableSwitchInstruction>
    {
        [IJava] public int Default { get; set; }
        [IJava] public int Low { get; set; }
        [IJava] public int High { get; set; }
        [IJava(Index: 1)] public List<int> Offsets { get; set; }
        public override void ReadProperty(JavaByteCodeReader reader, IJava.PropertyData data, object value)
        {
            if (data.Index == 2) value = Offsets = new int[High - Low + 1].ToList();
            base.ReadProperty(reader, data, value);
        }
        public override object[] IndexOperand => new object[] { Default, Low, High, Offsets.ToArray() };
        public override object[] Operand => new object[] { Default, Low, High, Offsets.ToArray() };
    }
    public sealed class MultiANewArrayInstruction : IInstruction<MultiANewArrayInstruction>
    {
        public ClassConstant Class { get => Handle.Constants[ClassIndex] as ClassConstant; set => ClassIndex = Handle.OfConstant(value); }
        [IJava] public ushort ClassIndex { get; set; }
        [IJava] public byte Dimensions { get; set; }

        public override object[] IndexOperand => new object[] { ClassIndex, Dimensions };
        public override object[] Operand => new object[] { Class, Dimensions };
    }
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
        [IJava(IJavaType.Byte)] public Type ArrayType { get; set; }
        public override object[] IndexOperand => new object[] { ArrayType };
        public override object[] Operand => new object[] { ArrayType };
    }
    public sealed class OffsetWideInstruction : IInstruction<OffsetWideInstruction>
    {
        [IJava] public int Offset { get; set; }
        public override object[] IndexOperand => new object[] { Offset };
        public override object[] Operand => new object[] { Offset };
    }
    public sealed class OffsetInstruction : IInstruction<OffsetInstruction>
    {
        [IJava] public short Offset { get; set; }
        public override object[] IndexOperand => new object[] { Offset };
        public override object[] Operand => new object[] { Offset };
    }
    public sealed class FrameIndexInstruction : IInstruction<FrameIndexInstruction>
    {
        [IJava] public byte FrameIndex { get; set; }
        public override object[] IndexOperand => new object[] { FrameIndex };
        public override object[] Operand => new object[] { FrameIndex };
    }
    public interface IConstantInstruction : IInstruction
    {
        IConstant Constant { get; set; }
    }
    public sealed class ConstantInstruction : IInstruction<ConstantInstruction>, IConstantInstruction
    {
        public IConstant Constant { get => Handle.Constants[ConstantIndex]; set => ConstantIndex = (byte)Handle.OfConstant(value); }
        [IJava] public byte ConstantIndex { get; set; }
        public override object[] IndexOperand => new object[] { ConstantIndex };
        public override object[] Operand => new object[] { Constant };
    }
    public sealed class ConstantWideInstruction : IInstruction<ConstantWideInstruction>, IConstantInstruction
    {
        public IConstant Constant { get => Handle.Constants[ConstantIndex]; set => ConstantIndex = Handle.OfConstant(value); }
        [IJava] public ushort ConstantIndex { get; set; }

        public override object[] IndexOperand => new object[] { ConstantIndex };
        public override object[] Operand => new object[] { Constant };
    }
    public sealed class WideInstruction : IInstruction<WideInstruction>
    {
        [IJava(Index: 1, IsReaded: false)] public OpCode OpCodeValue { get; set; }
        [IJava] public short Index { get; set; }
        [IJava(Index: 2)] public ushort Value { get; set; }
        public override JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, IJava.PropertyData data, object value)
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
        public override void ReadProperty(JavaByteCodeReader reader, IJava.PropertyData data, object value)
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
    }*/
}
