using System;
using System.Threading;
using System.Threading.Tasks;
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public interface IConstant : IRaw
{
    [InstanceOfTag]
    public static async ValueTask<IConstant> InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader, CancellationToken cancellationToken)
    {
        ConstantTag value = (ConstantTag)await reader.ReadByteAsync(cancellationToken);
        return value switch
        {
            ConstantTag.Utf8 => new Utf8Constant(),
            ConstantTag.Integer => new IntegerConstant(),
            ConstantTag.Float => new FloatConstant(),
            ConstantTag.Long => new LongConstant(),
            ConstantTag.Double => new DoubleConstant(),
            ConstantTag.Class => new ClassConstant(),
            ConstantTag.String => new StringConstant(),
            ConstantTag.FieldRef => new FieldRefConstant(),
            ConstantTag.MethodRef => new MethodRefConstant(),
            ConstantTag.InterfaceMethodRef => new InterfaceMethodRefConstant(),
            ConstantTag.NameAndType => new NameAndTypeConstant(),
            ConstantTag.MethodHandle => new MethodHandleConstant(),
            ConstantTag.MethodType => new MethodTypeConstant(),
            ConstantTag.InvokeDynamic => new InvokeDynamicConstant(),
            ConstantTag.ModuleInfo => new ModuleInfoConstant(),
            ConstantTag.PackageInfo => new PackageInfoConstant(),
            _ => throw new Exception($"ConstantTag:{value}"),
        };
    }

    ConstantTag Tag { get; }
    JObject JsonData { get; }

    IConstant DeepClone(JavaClass handle);
}
public abstract class IConstant<I> : BaseRaw<I>, IConstant, IEquatable<I> where I : IConstant<I>
{
    public abstract ConstantTag Tag { get; }
    public abstract JObject JsonData { get; }

    public override async ValueTask<JavaByteCodeWriter> WriteAsync(JavaByteCodeWriter writer, CancellationToken cancellationToken) => await base.WriteAsync(await writer.WriteByteAsync((byte)Tag, cancellationToken), cancellationToken);
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
