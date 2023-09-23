using System;
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public interface IConstant : IRaw
{
    [InstanceOfTag]
    public static IConstant InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader)
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
public abstract class IConstant<I> : BaseRaw<I>, IConstant, IEquatable<I> where I : IConstant<I>
{
    public abstract ConstantTag Tag { get; }
    public override JavaByteCodeWriter Write(JavaByteCodeWriter writer) => base.Write(writer.WriteByte((byte)Tag));
    public override string ToString() => $"{Tag}: ";

    public abstract bool Equals(I? other);
    public override bool Equals(object? obj) => obj is I _obj && _obj.Tag == Tag && Equals(_obj);
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
