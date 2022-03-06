using Java.Net.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Java.Net.Code
{
    public interface IInstruction : IJava, IEquatable<IInstruction>
    {
        ushort Position { get; set; }

        OpCode OpCode { get; }
        object[] Operand { get; }

        [InstanceOfTag] public static IInstruction InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader, [TagType(TagTypeAttribute.Tag.Handle)] JavaClass handle)
            => OpCodes.OpCodeList[reader.ReadByte()].Instruction(handle);
    }
    public abstract class IInstruction<I> : IJava<I>, IInstruction where I : IInstruction<I>
    {
        [IJava(Index: 0, IsReaded: false, Type: IJavaType.Custom)] public OpCode<I> OpCode { get; set; }
        OpCode IInstruction.OpCode => OpCode;

        public abstract object[] Operand { get; }
        public ushort Position { get; set; }

        public override JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, IJava.PropertyData data, object value)
            => data.Index == 0 ? writer.WriteByte(OpCode.Index) : base.WriteProperty(writer, data, value);
        
        public override bool Equals(object obj) => obj is IInstruction op && Equals(op);
        public virtual bool Equals(I obj) => OpCode == obj.OpCode && Enumerable.SequenceEqual(Operand, obj.Operand);
        public virtual bool Equals(IInstruction obj) => obj is I t && Equals(t);
        public override int GetHashCode() => HashCode.Combine(OpCode, Operand);
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

        public static I Create(JavaClass handle, OpCode<I> opCode, Action<I> apply = null)
        {
            I dat = Activator.CreateInstance<I>();
            dat.OpCode = opCode;
            dat.SetHandle(handle);
            apply?.Invoke(dat);
            dat.SetHandle(handle);
            return dat;
        }
    }
    public sealed class SingleInstruction : IInstruction<SingleInstruction>
    {
        public override object[] Operand => new object[0];
    }
    public sealed class BiPushInstruction : IInstruction<BiPushInstruction>
    {
        [IJava] public byte Value { get; set; }
        public override object[] Operand => new object[] { Value };
    }
    public sealed class SiPushInstruction : IInstruction<SiPushInstruction>
    {
        [IJava] public ushort Value { get; set; }
        public override object[] Operand => new object[] { Value };
    }
    public sealed class IIncInstruction : IInstruction<IIncInstruction>
    {
        [IJava] public byte FrameIndex { get; set; }
        [IJava] public byte Value { get; set; }
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
        public override object[] Operand => new object[] { MethodIndex, Count, 0 };
    }
    public sealed class InvokeDynamicInstruction : IInstruction<InvokeDynamicInstruction>, IInvokeInterfaceInstruction
    {
        public IRefConstant Method { get => Handle.Constants[MethodIndex] as IRefConstant; set => MethodIndex = Handle.OfConstant(value); }
        [IJava] public ushort MethodIndex { get; set; }
        public byte Count { get => 0; set { } }
        public override object[] Operand => new object[] { MethodIndex, Count, 0 };
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
        public override object[] Operand => new object[] { Default, Low, High, Offsets.ToArray() };
    }
    public sealed class MultiANewArrayInstruction : IInstruction<MultiANewArrayInstruction>
    {
        public ClassConstant Class { get => Handle.Constants[ClassIndex] as ClassConstant; set => ClassIndex = Handle.OfConstant(value); }
        [IJava] public ushort ClassIndex { get; set; }
        [IJava] public byte Dimensions { get; set; }

        public override object[] Operand => new object[] { ClassIndex, Dimensions }; 
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
        public override object[] Operand => new object[] { ArrayType };
    }
    public sealed class OffsetWideInstruction : IInstruction<OffsetWideInstruction>
    {
        [IJava] public int Offset { get; set; }
        public override object[] Operand => new object[] { Offset };
    }
    public sealed class OffsetInstruction : IInstruction<OffsetInstruction>
    {
        [IJava] public short Offset { get; set; }
        public override object[] Operand => new object[] { Offset };
    }
    public sealed class FrameIndexInstruction : IInstruction<FrameIndexInstruction>
    {
        [IJava] public byte FrameIndex { get; set; }
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
        public override object[] Operand => new object[] { ConstantIndex };
    }
    public sealed class ConstantWideInstruction : IInstruction<ConstantWideInstruction>, IConstantInstruction
    {
        public IConstant Constant { get => Handle.Constants[ConstantIndex]; set => ConstantIndex = Handle.OfConstant(value); }
        [IJava] public ushort ConstantIndex { get; set; }
        public override object[] Operand => new object[] { ConstantIndex };
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

        public override object[] Operand => new object[] { OpCodeValue, Index, Value };
    }
}
