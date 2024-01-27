namespace Java.Net.Model.Raw.Constant;

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
    FieldRef = 9,
    MethodRef = 10,
    InterfaceMethodRef = 11,
    NameAndType = 12,

    MethodHandle = 15,
    MethodType = 16,

    InvokeDynamic = 18,
    ModuleInfo = 19,
    PackageInfo = 20
}
