using Java.Net.Data.Descriptor;
using Java.Net.Data.Signature;
using Java.Net.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java.Net.Data
{
    public sealed class JavaAnnotationData
    {
        public ClassTypeSignature Type { get; set; }
        public Dictionary<string, ElementValue> Values { get; set; } = new Dictionary<string, ElementValue>();
        
        public abstract class ElementValue
        {
            public abstract Java.Net.Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle);
        }
        public abstract class ConstantValue<T> : ElementValue
        {
            public T Value { get; set; }
            public ConstantValue(T value) => Value = value;
        }

        public sealed class ByteValue : ConstantValue<byte>
        {
            public ByteValue(byte value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator ByteValue(byte value) => new ByteValue(value);
        }
        public sealed class CharValue : ConstantValue<char>
        {
            public CharValue(char value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator CharValue(char value) => new CharValue(value);
        }
        public sealed class DoubleValue : ConstantValue<double>
        {
            public DoubleValue(double value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator DoubleValue(double value) => new DoubleValue(value);
        }
        public sealed class FloatValue : ConstantValue<float>
        {
            public FloatValue(float value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator FloatValue(float value) => new FloatValue(value);
        }
        public sealed class IntValue : ConstantValue<int>
        {
            public IntValue(int value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator IntValue(int value) => new IntValue(value);
        }
        public sealed class LongValue : ConstantValue<long>
        {
            public LongValue(long value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator LongValue(long value) => new LongValue(value);
        }
        public sealed class ShortValue : ConstantValue<short>
        {
            public ShortValue(short value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator ShortValue(short value) => new ShortValue(value);
        }
        public sealed class BoolValue : ConstantValue<bool>
        { 
            public BoolValue(bool value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator BoolValue(bool value) => new BoolValue(value);
        }
        public sealed class StringValue : ConstantValue<string>
        {
            public StringValue(string value) : base(value) { }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ConstantValue.Of(handle, Value);
            public static implicit operator StringValue(string value) => new StringValue(value);
        }

        public sealed class EnumValue : ElementValue
        {
            public ObjectType Type { get; set; }
            public string Value { get; set; }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.EnumValue.Create(handle, v =>
            {
                v.ConstantName = Utf8Constant.Create(handle, _v => _v.Value = Value);
                v.TypeName = Utf8Constant.Create(handle, _v => _v.Value = Type.DescriptorFormat);
            });
        }
        public sealed class ClassValue : ElementValue
        {
            public IFieldType Type { get; set; }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.ClassValue.Create(handle, v =>
            {
                v.ClassInfo = Utf8Constant.Create(handle, _v => _v.Value = Type.DescriptorFormat);
            });
        }
        public sealed class AnnotationValue : ElementValue
        {
            public JavaAnnotationData Annotation { get; set; }

            public override Model.Attribute.Runtime.ElementValue ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.AnnotationValue.Create(handle, v =>
            {
                v.Annotation = Annotation.ToJava(handle);
            });
        }
        public sealed class ArrayValue : ElementValue
        {
            public List<ElementValue> Values { get; set; } = new List<ElementValue>();

            public override Model.Attribute.Runtime.ElementValue ToJava(JavaClass handle) => Model.Attribute.Runtime.ArrayValue.Create(handle, v =>
            {
                v.Values = Values.Select(_v => _v.ToJava(handle)).ToList();
            });
        }

        public Model.Attribute.Runtime.Annotation ToJava(Model.JavaClass handle) => Model.Attribute.Runtime.Annotation.Create(handle, _v =>
        {
            _v.Type = Utf8Constant.Create(handle, _v => _v.Value = Type.ToDescriptor(TypeVariableReader.EMPTY).DescriptorFormat);
            _v.ElementValuePairs = Values.Select(_v => Model.Attribute.Runtime.ElementValuePair.Create(handle, __v =>
            {
                __v.ElementValue = _v.Value.ToJava(handle);
                __v.Name = Utf8Constant.Create(handle, ___v => ___v.Value = _v.Key);
            })).ToList();
        });
    }
}
