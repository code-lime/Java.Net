using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class ConstantValue : ElementValue<ConstantValue>
{
    [JavaRaw] public ushort ConstantIndex { get; set; }
    public IConstant Constant
    {
        get => Handle.Constants[ConstantIndex] as IConstant;
        set => ConstantIndex = Handle.OfConstant(value);
    }

    private static ConstantValue CreateOf(JavaClass handle, ElementTag tag, IConstant constant) => Create(handle, c =>
    {
        c.Tag = tag;
        c.ConstantIndex = handle.OfConstant(constant);
    });

    public static ConstantValue Of(JavaClass handle, byte value) => CreateOf(handle, ElementTag.Byte, IntegerConstant.Create(handle, v => v.Value = value));
    public static ConstantValue Of(JavaClass handle, char value) => CreateOf(handle, ElementTag.Char, IntegerConstant.Create(handle, v => v.Value = value));
    public static ConstantValue Of(JavaClass handle, double value) => CreateOf(handle, ElementTag.Double, DoubleConstant.Create(handle, v => v.Value = value));
    public static ConstantValue Of(JavaClass handle, float value) => CreateOf(handle, ElementTag.Float, FloatConstant.Create(handle, v => v.Value = value));
    public static ConstantValue Of(JavaClass handle, int value) => CreateOf(handle, ElementTag.Integer, IntegerConstant.Create(handle, v => v.Value = value));
    public static ConstantValue Of(JavaClass handle, long value) => CreateOf(handle, ElementTag.Long, LongConstant.Create(handle, v => v.Value = value));
    public static ConstantValue Of(JavaClass handle, short value) => CreateOf(handle, ElementTag.Short, IntegerConstant.Create(handle, v => v.Value = value));
    public static ConstantValue Of(JavaClass handle, bool value) => CreateOf(handle, ElementTag.Boolean, IntegerConstant.Create(handle, v => v.Value = value ? 1 : 0));
    public static ConstantValue Of(JavaClass handle, string value) => CreateOf(handle, ElementTag.String, Utf8Constant.Create(handle, v => v.Value = value));
}
