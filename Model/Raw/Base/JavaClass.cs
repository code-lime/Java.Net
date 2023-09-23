using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;
using Java.Net.Flags;
using Java.Net.Model.Net;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Constant;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Java.Net.Model.Raw.Base;

public class JavaClass : BaseRaw<JavaClass>, INet<TypeDefinition>, IFlag<AccessClass>, IAnnotations
{
    public static JavaClass Create(string new_clazz, Action<JavaClass> load) => Create(new_clazz, apply: load);
    public static JavaClass Create(string new_clazz, string super_clazz = "java/lang/Object", Action<JavaClass>? apply = null)
    {
        JavaClass dat = new JavaClass
        {
            MagicNum = MagicNumClass,
            JavaVersion = new Version(0, 52),
            Flags = AccessClass.NONE,
            Constants = new List<IConstant>() { null! },
            Interfaces = new List<ushort>(),
            Fields = new List<JavaField>(),
            Methods = new List<JavaMethod>(),
            Annotations = new List<IAnnotation>()
        };
        dat.ThisClassIndex = dat.OfConstant(ClassConstant.Create(dat, v => v.Name = new_clazz));
        dat.SuperClassIndex = dat.OfConstant(ClassConstant.Create(dat, v => v.Name = super_clazz));
        apply?.Invoke(dat);
        return dat;
    }
    public JavaClass() => Handle = this;

    public const uint MagicNumClass = 3405691582;

    [JavaRaw] public uint MagicNum { get; set; }
    [JavaRaw] private ushort Major { get => (ushort)(JavaVersion?.Major ?? 0); set => JavaVersion = new Version(value, Minor); }
    [JavaRaw] private ushort Minor { get => (ushort)(JavaVersion?.Minor ?? 0); set => JavaVersion = new Version(Major, value); }
    public Version JavaVersion { get; set; } = null!;

    [JavaRaw(Index: 3)][JavaArray] public List<IConstant> Constants { get; set; } = null!;

    public override void ReadProperty(JavaByteCodeReader reader, PropertyData data, object value)
    {
        switch (data.Index)
        {
            case 3:
                {
                    int constantCount = reader.ReadUShort();
                    Constants = new IConstant[constantCount].ToList();
                    for (int i = 1; i < constantCount; i++)
                    {
                        IConstant constant = IRaw.Read<IConstant>(new MethodTag() { Parent = this, Reader = reader }, reader);
                        Constants[i] = constant;
                        switch (constant.Tag)
                        {
                            case ConstantTag.Double:
                            case ConstantTag.Long: i++; break;
                        }
                    }
                    return;
                }
        }
        base.ReadProperty(reader, data, value);
    }
    public override JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, PropertyData data, object value)
    {
        switch (data.Index)
        {
            case 3:
                {
                    ushort constantCount = (ushort)Constants.Count;
                    writer = writer.WriteUShort(constantCount);
                    for (int i = 1; i < constantCount; i++)
                    {
                        IConstant old = Constants[i - 1];
                        if (old != null)
                        {
                            switch (old.Tag)
                            {
                                case ConstantTag.Double:
                                case ConstantTag.Long: continue;
                            }
                        }
                        writer = IRaw.Write(Constants[i], writer);
                    }
                    return writer;
                }
        }
        return base.WriteProperty(writer, data, value);
    }

    public override IRaw Read(JavaByteCodeReader reader)
    {
        IRaw _this = base.Read(reader);
        _this.SetHandle(this);
        return _this;
    }

    [JavaRaw(JavaType.UShort)] public AccessClass Flags { get; set; }
    [JavaRaw] public ushort ThisClassIndex { get; set; }
    public ClassConstant ThisClass
    {
        get => (ClassConstant)Constants[ThisClassIndex];
        set => ThisClassIndex = OfConstant(value);
    }
    [JavaRaw] public ushort SuperClassIndex { get; set; }
    public ClassConstant SuperClass
    {
        get => (ClassConstant)Constants[SuperClassIndex];
        set => SuperClassIndex = OfConstant(value);
    }
    [JavaRaw][JavaArray] public List<ushort> Interfaces { get; set; } = null!;
    [JavaRaw][JavaArray] public List<JavaField> Fields { get; set; } = null!;
    [JavaRaw][JavaArray] public List<JavaMethod> Methods { get; set; } = null!;
    [JavaRaw][JavaArray] public List<IAnnotation> Annotations { get; set; } = null!;

    public string ThisClassPath => ThisClass.Name + ".class";
    public ushort OfConstant(IConstant constant)
    {
        constant.SetHandle(this);
        Constants ??= new List<IConstant> { null! };
        ushort length = (ushort)Constants.Count;
        for (ushort i = 1; i < length; i++)
            if (constant.Equals(Constants[i]))
                return i;
        List<IConstant> constants = Constants;
        constants.Add(constant);
        switch (constant.Tag)
        {
            case ConstantTag.Double:
            case ConstantTag.Long:
                constants.Add(null!);
                break;
        }
        Constants = constants;
        return length;
    }
    public T AddConstant<T>(T constant) where T : IConstant => (T)Constants[OfConstant(constant)];

    public override string ToString() => ThisClass.Name;

    public IEnumerable<TypeDefinition> ToNet(AssemblyDefinition parent)
    {
        throw new NotSupportedException();
        /*string[] path = ThisClass.Name.Split('/');
        TypeAttributes typeAttributes = TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit;
        
        if (Flags.HasFlag(AccessClass.INTERFACE)) typeAttributes |= TypeAttributes.Abstract;
        if (Flags.HasFlag(AccessClass.ABSTRACT)) typeAttributes |= TypeAttributes.Abstract;
        if (Flags.HasFlag(AccessClass.FINAL)) typeAttributes |= TypeAttributes.Sealed;
        if (Flags.HasFlag(AccessClass.PUBLIC)) typeAttributes |= TypeAttributes.Public;

        string @namespace = string.Join('.', path[..^1]);
        string name = path[^1];
        string[] name_path = name.Split('$');

        TypeDefinition type = new TypeDefinition(@namespace, name, typeAttributes);
        parent.MainModule.Types.Add(type);
        yield return type;
        TypeDefinition parent_type = null;
        if (name_path.Length > 1)
        {
            string parent_java_path = string.Join('/', path[..^1]) + "/" + string.Join('$', name_path[..^1]);
            foreach (var _type in parent.MainModule.GetAllTypes())
            {
                string java_path = _type.FullName.Replace('/', '$').Replace('.', '/');
                if (parent_java_path != java_path) continue;
                parent_type = _type;
                break;
            }
        }
        yield return type;
        if (parent_type != null)
        {
            parent.MainModule.Types.Remove(type);
            parent_type.NestedTypes.Add(type);
            type.Name = name_path[^1];
            type.Namespace = null;// parent_type.Namespace;
        }
        yield return type;
        string signature = ((Attribute.SignatureAttribute)Attributes.FirstOrDefault(v => v is Attribute.SignatureAttribute))?.Signature?.Value;
        if (signature != null) TypeSignature.ApplyGeneric(ref signature, type, parent);
        yield return type;
        Annotation.Apply(Attributes, type, parent);
        Console.WriteLine($"TYPE: {type.FullName}");
        yield return type;

        INetStep<FieldDefinition> fields = INetExt.Create(Fields, type);
        while (fields.MoveNext()) ;
        yield return type;

        INetStep<MethodDefinition> methods = INetExt.Create(Methods, type);
        while (methods.MoveNext()) ;
        yield return type;*/
    }
    //if (Flags.HasFlag(AccessClass.ANNOTATION))
}
