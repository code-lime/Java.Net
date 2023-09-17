using Java.Net.Data.Signature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Java.Net.Model
{
    public interface IConstant : IJava
    {
        [InstanceOfTag] public static IConstant InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader)
        {
            ConstantTag value = (ConstantTag)reader.ReadByte();
            return value switch
            {
                ConstantTag.Utf8 => new Utf8Constant(),
                ConstantTag.Integer => new IntegerConstant(),
                ConstantTag.Float => new FloatConstant(),
                ConstantTag.Long => new LongConstant(),
                ConstantTag.Double => new DoubleConstant(),
                ConstantTag.Class => new ClassConstant(),
                ConstantTag.String => new StringConstant(),
                ConstantTag.Fieldref => new FieldrefConstant(),
                ConstantTag.Methodref => new MethodrefConstant(),
                ConstantTag.InterfaceMethodref => new InterfaceMethodrefConstant(),
                ConstantTag.NameAndType => new NameAndTypeConstant(),
                ConstantTag.MethodHandle => new MethodHandleConstant(),
                ConstantTag.MethodType => new MethodTypeConstant(),
                ConstantTag.InvokeDynamic => new InvokeDynamicConstant(),
                _ => throw new Exception($"ConstantTag:{value}"),
            };
        }

        ConstantTag Tag { get; }

        IConstant DeepClone(JavaClass handle);
    }
    public abstract class IConstant<I> : IJava<I>, IConstant, IEquatable<I> where I : IConstant<I>
    {
        public abstract ConstantTag Tag { get; }
        public override JavaByteCodeWriter Write(JavaByteCodeWriter writer) => base.Write(writer.WriteByte((byte)Tag));
        public override string ToString() => $"{Tag}: ";

        public abstract bool Equals(I other);
        public override bool Equals(object obj) => obj is I _obj && _obj.Tag == Tag && Equals(_obj);
        public override int GetHashCode() => (int)Tag;

        public abstract I DeepClone(JavaClass handle);
        IConstant IConstant.DeepClone(JavaClass handle) => DeepClone(handle);
    }
    /*
    public abstract class IConstant<I> : IConstant, IEquatable<I> where I : IConstant<I>
    {
        public abstract bool Equals(I other);
        public override bool Equals(object obj) => obj is I _obj && _obj.Tag == Tag && Equals(_obj);
        public override int GetHashCode() => (int)Tag;

        public I Init(JavaClass handle, Action<I> action)
        {
            SetHandle(handle);
            I item = (I)this;
            action.Invoke(item);
            return item;
        }

        public static IConstant<I> Create(JavaClass handle, Action<I> action) => Activator.CreateInstance<I>().Init(handle, action);
    }*/

    public sealed class Utf8Constant : IConstant<Utf8Constant>
    {
        public override ConstantTag Tag => ConstantTag.Utf8;
        [IJava] public string Value { get; set; }
        public override bool Equals(Utf8Constant other) => other?.Value == Value;
        public override string ToString() => $"{base.ToString()} {Value}";
        
        public override Utf8Constant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
    }
    public sealed class IntegerConstant : IConstant<IntegerConstant>
    {
        public override ConstantTag Tag => ConstantTag.Integer;
        [IJava] public int Value { get; set; }
        public override bool Equals(IntegerConstant other) => other?.Value == Value;
        public override string ToString() => $"{base.ToString()} {Value}";

        public override IntegerConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
    }
    public sealed class FloatConstant : IConstant<FloatConstant>
    {
        public override ConstantTag Tag => ConstantTag.Float;
        [IJava] public float Value { get; set; }
        public override bool Equals(FloatConstant other) => other?.Value == Value;
        public override string ToString() => $"{base.ToString()} {Value}";

        public override FloatConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
    }
    public sealed class LongConstant : IConstant<LongConstant>
    {
        public override ConstantTag Tag => ConstantTag.Long;
        [IJava] public long Value { get; set; }
        public override bool Equals(LongConstant other) => other?.Value == Value;
        public override string ToString() => $"{base.ToString()} {Value}";

        public override LongConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
    }
    public sealed class DoubleConstant : IConstant<DoubleConstant>
    {
        public override ConstantTag Tag => ConstantTag.Double;
        [IJava] public double Value { get; set; }
        public override bool Equals(DoubleConstant other) => other?.Value == Value;
        public override string ToString() => $"{base.ToString()} {Value}";

        public override DoubleConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
    }
    public sealed class ClassConstant : IConstant<ClassConstant>
    {
        public override ConstantTag Tag => ConstantTag.Class;
        [IJava] public ushort Index { get; set; }
        public string Name
        {
            get => (Handle.Constants[Index] as Utf8Constant).Value;
            set => Index = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        public override bool Equals(ClassConstant other) => other?.Index == Index;
        public override string ToString() => $"{base.ToString()} [{Index}] {Name}";

        public ITypeSignature FieldType { get => ISignature.TryParse<ITypeSignature>(Name); set => Name = value.ToString(); }

        public override ClassConstant DeepClone(JavaClass handle) => Create(handle, v => v.Name = Name);
    }
    public sealed class StringConstant : IConstant<StringConstant>
    {
        public override ConstantTag Tag => ConstantTag.String;
        [IJava] public ushort StringIndex { get; set; }
        public string String
        {
            get => (Handle.Constants[StringIndex] as Utf8Constant).Value;
            set => StringIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        public override bool Equals(StringConstant other) => other?.StringIndex == StringIndex;
        public override string ToString() => $"{base.ToString()} {String}";

        public override StringConstant DeepClone(JavaClass handle) => Create(handle, v => v.String = String);
    }

    public sealed class FieldrefConstant : IRefConstant<FieldrefConstant>
    {
        public override ConstantTag Tag => ConstantTag.Fieldref;
    }
    public sealed class MethodrefConstant : IRefConstant<MethodrefConstant>, IMethodRefConstant
    {
        public override ConstantTag Tag => ConstantTag.Methodref;
        IRefConstant IMethodRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
    }
    public sealed class InterfaceMethodrefConstant : IRefConstant<InterfaceMethodrefConstant>, IMethodRefConstant
    {
        public override ConstantTag Tag => ConstantTag.InterfaceMethodref;
        IRefConstant IMethodRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
    }
    public interface IRefConstant : IConstant
    {
        ushort ClassIndex { get; set; }
        ushort NameAndTypeIndex { get; set; }
        ClassConstant Class { get; set; }
        NameAndTypeConstant NameAndType { get; set; }

        new IRefConstant DeepClone(JavaClass handle);
    }
    public interface IMethodRefConstant : IRefConstant
    {
        new IRefConstant DeepClone(JavaClass handle);
    }
    public abstract class IRefConstant<I> : IConstant<I>, IRefConstant where I : IRefConstant<I>
    {
        [IJava] public ushort ClassIndex { get; set; }
        [IJava] public ushort NameAndTypeIndex { get; set; }
        public ClassConstant Class
        {
            get => Handle.Constants[ClassIndex] as ClassConstant;
            set => ClassIndex = Handle.OfConstant(value);
        }
        public NameAndTypeConstant NameAndType
        {
            get => Handle.Constants[NameAndTypeIndex] as NameAndTypeConstant;
            set => NameAndTypeIndex = Handle.OfConstant(value);
        }
        public override bool Equals(I other) => other?.ClassIndex == ClassIndex && other?.NameAndTypeIndex == NameAndTypeIndex;
        public override string ToString() => $"{base.ToString()} ({Class}, {NameAndType})";

        public override I DeepClone(JavaClass handle) => Create(handle, v =>
        {
            v.Class = Class.DeepClone(handle);
            v.NameAndType = NameAndType.DeepClone(handle);
        });
        IRefConstant IRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
    }

    public sealed class NameAndTypeConstant : IConstant<NameAndTypeConstant>
    {
        public override ConstantTag Tag => ConstantTag.NameAndType;
        [IJava] public ushort NameIndex { get; set; }
        [IJava] public ushort DescriptorIndex { get; set; }
        public string Name
        {
            get => (Handle.Constants[NameIndex] as Utf8Constant).Value;
            set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        public string Descriptor
        {
            get => (Handle.Constants[DescriptorIndex] as Utf8Constant).Value;
            set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        public override bool Equals(NameAndTypeConstant other) => other?.NameIndex == NameIndex && other?.DescriptorIndex == DescriptorIndex;
        public override string ToString() => $"{base.ToString()} {Name} {Descriptor}";

        public ITypeSignature FieldSignature { get => ISignature.TryParse<ITypeSignature>(Descriptor); set => Descriptor = value.ToString(); }
        public MethodTypeSignature MethodSignature { get => ISignature.TryParse<MethodTypeSignature>(Descriptor); set => Descriptor = value.ToString(); }

        public override NameAndTypeConstant DeepClone(JavaClass handle) => Create(handle, v =>
        {
            v.Name = Name;
            v.Descriptor = Descriptor;
        });
    }
    public sealed class MethodHandleConstant : IConstant<MethodHandleConstant>
    {
        public override ConstantTag Tag => ConstantTag.MethodHandle;
        [IJava(IJavaType.Byte)] public ReferenceKind ReferenceKind { get; set; }
        [IJava] public ushort ReferenceIndex { get; set; }
        public IRefConstant Reference
        {
            get => Handle.Constants[ReferenceIndex] as IRefConstant;
            set => ReferenceIndex = Handle.OfConstant(value);
        }
        public override bool Equals(MethodHandleConstant other) => other?.ReferenceKind == ReferenceKind && other?.ReferenceIndex == ReferenceIndex;
        public override string ToString() => $"{base.ToString()} {ReferenceKind} {Reference}";

        public override MethodHandleConstant DeepClone(JavaClass handle) => Create(handle, v =>
        {
            v.ReferenceKind = ReferenceKind;
            v.Reference = Reference.DeepClone(handle);
        });
    }
    public sealed class MethodTypeConstant : IConstant<MethodTypeConstant>
    {
        public override ConstantTag Tag => ConstantTag.MethodType;
        [IJava] public ushort DescriptorIndex { get; set; }
        public string Descriptor
        {
            get => (Handle.Constants[DescriptorIndex] as Utf8Constant).Value;
            set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        public override bool Equals(MethodTypeConstant other) => other?.DescriptorIndex == DescriptorIndex;
        public override string ToString() => $"{base.ToString()} {Descriptor}";

        public MethodTypeSignature MethodDescriptor { get => ISignature.TryParse<MethodTypeSignature>(Descriptor); set => Descriptor = value.ToString(); }

        public override MethodTypeConstant DeepClone(JavaClass handle) => Create(handle, v => v.Descriptor = Descriptor);
    }
    public sealed class InvokeDynamicConstant : IConstant<InvokeDynamicConstant>
    {
        public override ConstantTag Tag => ConstantTag.InvokeDynamic;
        [IJava] public ushort BootstrapMethodAttributeIndex { get; set; }
        public Attribute.BootstrapMethodsAttribute.Bootstrap BootstrapMethod
        {
            get => (Handle.Attributes.FirstOrDefault(v => v is Attribute.BootstrapMethodsAttribute) as Attribute.BootstrapMethodsAttribute).Methods[BootstrapMethodAttributeIndex];
            set => (Handle.Attributes.FirstOrDefault(v => v is Attribute.BootstrapMethodsAttribute) as Attribute.BootstrapMethodsAttribute).Methods[BootstrapMethodAttributeIndex] = value;
        }
        [IJava] public ushort NameAndTypeIndex { get; set; }
        public NameAndTypeConstant NameAndType
        {
            get => Handle.Constants[NameAndTypeIndex] as NameAndTypeConstant;
            set => NameAndTypeIndex = Handle.OfConstant(value);
        }
        public override bool Equals(InvokeDynamicConstant other) => other?.BootstrapMethodAttributeIndex == BootstrapMethodAttributeIndex && other?.NameAndTypeIndex == NameAndTypeIndex;
        public override string ToString() => $"{base.ToString()} {NameAndType} {BootstrapMethod}";

        public override InvokeDynamicConstant DeepClone(JavaClass handle) => Create(handle, v =>
        {
            v.BootstrapMethod = BootstrapMethod.DeepClone(handle);
            v.NameAndType = NameAndType.DeepClone(handle);
        });
    }

    public enum ConstantTag : byte
    {
        None = 0,

        Utf8 = 1,

        Integer = 3,
        Float = 4,
        Long = 5,
        Double = 6,
        Class = 7,
        String = 8,
        Fieldref = 9,
        Methodref = 10,
        InterfaceMethodref = 11,
        NameAndType = 12,

        MethodHandle = 15,
        MethodType = 16,

        InvokeDynamic = 18,
    }
    public enum ReferenceKind : byte
    {
        GetField = 1,
        GetStaticField = 2,
        SetField = 3,
        SetStaticField = 4,
        InvokeVirtual = 5,
        InvokeStatic = 6,
        InvokeSpecial = 7,
        NewInvokeSpecial = 8,
        InvokeInterface = 9,
    }
}
