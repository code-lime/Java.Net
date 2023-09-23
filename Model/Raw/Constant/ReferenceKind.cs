namespace Java.Net.Model.Raw.Constant;

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
