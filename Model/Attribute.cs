using System;
using System.Collections.Generic;
using System.Text;

namespace Java.Net.Model
{
    using Attribute;
    using System.Linq;

    public interface JavaAttribute : IJava
    {
        ushort NameIndex { get; set; }
        string Name { get; set; }
        AttributeType Type { get; }

        [InstanceOfTag] public static JavaAttribute InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader, [TagType(TagTypeAttribute.Tag.Handle)] JavaClass handle, [TagType(TagTypeAttribute.Tag.LastOfArray)] IJava lastOfArray)
        {
            ushort name_index = reader.ReadUShort();
            string name = (handle.Constants[name_index] as Utf8Constant).Value;
            uint size = reader.ReadUInt();
            AttributeType type;
            JavaAttribute attribute = (type = (Enum.TryParse(name, out type) ? type : AttributeType.Unknown)) switch
            {
                AttributeType.ConstantValue => new ConstantValueAttribute(),
                AttributeType.Code => new CodeAttribute(),
                AttributeType.StackMapTable => new StackMapTableAttribute(),
                AttributeType.Exceptions => new ExceptionsAttribute(),
                AttributeType.InnerClasses => new InnerClassesAttribute(),
                AttributeType.EnclosingMethod => new EnclosingMethodAttribute(),
                AttributeType.Synthetic => new SyntheticAttribute(),
                AttributeType.Signature => new SignatureAttribute(),
                AttributeType.SourceFile => new SourceFileAttribute(),
                AttributeType.SourceDebugExtension => new SourceDebugExtensionAttribute(reader.ReadCount(size)),
                AttributeType.LineNumberTable => new LineNumberTableAttribute(),
                AttributeType.LocalVariableTable => new LocalVariableTableAttribute(),
                AttributeType.LocalVariableTypeTable => new LocalVariableTypeTableAttribute(),
                AttributeType.Deprecated => new DeprecatedAttribute(),
                AttributeType.RuntimeVisibleAnnotations => new RuntimeVisibleAnnotationsAttribute(),
                AttributeType.RuntimeInvisibleAnnotations => new RuntimeInvisibleAnnotationsAttribute(),
                AttributeType.RuntimeVisibleParameterAnnotations => new RuntimeVisibleParameterAnnotationsAttribute(),
                AttributeType.RuntimeInvisibleParameterAnnotations => new RuntimeInvisibleParameterAnnotationsAttribute(),
                AttributeType.AnnotationDefault => new AnnotationDefaultAttribute(),
                AttributeType.BootstrapMethods => new BootstrapMethodsAttribute(),
                _ => new UnknownAttribute(reader.ReadCount(size))
            };
            attribute.NameIndex = name_index;
            return attribute;
        }
    }
    public abstract class JavaAttribute<I> : IJava<I>, JavaAttribute where I : JavaAttribute<I>
    {
        public ushort NameIndex { get; set; }
        public string Name
        {
            get => (Handle.Constants[NameIndex] as Utf8Constant).Value;
            set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        public abstract AttributeType Type { get; }
        public override IJava SetHandle(JavaClass handle)
        {
            IJava dat = base.SetHandle(handle);
            if (NameIndex == 0 && Type != AttributeType.Unknown) Name = Type.ToString();
            return dat;
        }
        public sealed override JavaByteCodeWriter Write(JavaByteCodeWriter writer)
        {
            using System.IO.MemoryStream stream = new System.IO.MemoryStream();
            JavaByteCodeWriter out_writer = base.Write(new JavaByteCodeWriter(stream));

            return writer
                .WriteUShort(NameIndex)
                .WriteUInt((uint)stream.Length)
                .Append(out_writer);
        }
    }
    namespace Attribute
    {
        public enum AttributeType
        {
            Unknown,

            ConstantValue,
            Code,
            StackMapTable,
            Exceptions,
            InnerClasses,
            EnclosingMethod,
            Synthetic,
            Signature,
            SourceFile,
            SourceDebugExtension,
            LineNumberTable,
            LocalVariableTable,
            LocalVariableTypeTable,
            Deprecated,
            RuntimeVisibleAnnotations,
            RuntimeInvisibleAnnotations,
            RuntimeVisibleParameterAnnotations,
            RuntimeInvisibleParameterAnnotations,
            AnnotationDefault,
            BootstrapMethods
        }

        public abstract class BytesAttribute<I> : JavaAttribute<I> where I : JavaAttribute<I>
        {
            public BytesAttribute(byte[] info) => Info = info;
            [IJava(Index: 1, IsReaded: false)] public byte[] Info { get; set; }
            public override JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, IJava.PropertyData data, object value) => data.Index switch
            {
                1 => writer.WriteCount(Info),
                _ => base.WriteProperty(writer, data, value)
            };
        }

        public sealed class UnknownAttribute : BytesAttribute<UnknownAttribute>
        {
            public override AttributeType Type => AttributeType.Unknown;
            public UnknownAttribute(byte[] info) : base(info) { }
        }
        public sealed class ConstantValueAttribute : JavaAttribute<ConstantValueAttribute>
        {
            public override AttributeType Type => AttributeType.ConstantValue;
            [IJava] public ushort ValueIndex { get; set; }
            public IConstant Value
            {
                get => Handle.Constants[ValueIndex] as IConstant;
                set => ValueIndex = Handle.OfConstant(value);
            }
        }
        public sealed class CodeAttribute : JavaAttribute<CodeAttribute>
        {
            public sealed class Exception : IJava<Exception>
            {
                [IJava] public ushort StartPC { get; set; }
                [IJava] public ushort EndPC { get; set; }
                [IJava] public ushort HandlerPC { get; set; }
                [IJava] public ushort CatchTypeIndex { get; set; }
                public ClassConstant CatchType
                {
                    get => Handle.Constants[CatchTypeIndex] as ClassConstant;
                    set => CatchTypeIndex = Handle.OfConstant(value);
                }
            }

            public override AttributeType Type => AttributeType.Code;
            [IJava] public ushort MaxStack { get; set; }
            [IJava] public ushort MaxLocals { get; set; }
            [IJava] [IJavaArray(IJavaType.UInt)] public byte[] Code { get; set; }
            [IJava] [IJavaArray(IJavaType.UShort)] public List<Exception> ExceptionTable { get; set; }
            [IJava] [IJavaArray(IJavaType.UShort)] public List<JavaAttribute> Attributes { get; set; }
        }
        public sealed class StackMapTableAttribute : JavaAttribute<StackMapTableAttribute>
        {
            private static bool IsRange(byte value, byte min_max) => value == min_max;
            private static bool IsRange(byte value, byte min, byte max) => value >= min && value <= max;

            public enum VerificationType : byte
            {
                Top = 0,
                Integer = 1,
                Float = 2,
                Long = 3,
                Double = 4,
                Null = 5,
                UninitializedThis = 6,
                Object = 7,
                Uninitialized = 8
            }
            public interface VerificationTypeInfo : IJava
            {
                VerificationType Tag { get; }
                [InstanceOfTag] public static VerificationTypeInfo InstanceOfTag(
                    [TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader,
                    [TagType(TagTypeAttribute.Tag.LastOfArray)] IJava lastOfArray,
                    [TagType(TagTypeAttribute.Tag.IndexOfArray)] ref int indexOfArray)
                {
                    VerificationType type;
                    return (type = (VerificationType)reader.ReadByte()) switch
                    {
                        VerificationType.Top => new TopVariableInfo(),
                        VerificationType.Integer => new IntegerVariableInfo(),
                        VerificationType.Float => new FloatVariableInfo(),
                        VerificationType.Null => new NullVariableInfo(),
                        VerificationType.UninitializedThis => new UninitializedThisVariableInfo(),
                        VerificationType.Object => new ObjectVariableInfo(),
                        VerificationType.Uninitialized => new UninitializedVariableInfo(),

                        VerificationType.Long => new LongVariableInfo(),
                        VerificationType.Double => new DoubleVariableInfo(),

                        _ => throw new ArgumentException($"VerificationType '{type}' not supported!")
                    };
                }
            }
            public abstract class VerificationTypeInfo<I> : IJava<I>, VerificationTypeInfo where I : VerificationTypeInfo<I>
            {
                [IJava(IJavaType.Byte, IsReaded: false)] public abstract VerificationType Tag { get; }
            }
            public class TopVariableInfo : VerificationTypeInfo<TopVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Top;
            }
            public class IntegerVariableInfo : VerificationTypeInfo<IntegerVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Integer;
            }
            public class FloatVariableInfo : VerificationTypeInfo<FloatVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Float;
            }
            public class LongVariableInfo : VerificationTypeInfo<LongVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Long;
            }
            public class DoubleVariableInfo : VerificationTypeInfo<DoubleVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Double;
            }
            public class NullVariableInfo : VerificationTypeInfo<NullVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Null;
            }
            public class UninitializedThisVariableInfo : VerificationTypeInfo<UninitializedThisVariableInfo>
            {
                public override VerificationType Tag => VerificationType.UninitializedThis;
            }
            public class ObjectVariableInfo : VerificationTypeInfo<ObjectVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Object;
                [IJava] public ushort CPoolIndex { get; set; }
                public ClassConstant CPool
                {
                    get => Handle.Constants[CPoolIndex] as ClassConstant;
                    set => CPoolIndex = Handle.OfConstant(value);
                }
            }
            public class UninitializedVariableInfo : VerificationTypeInfo<UninitializedVariableInfo>
            {
                public override VerificationType Tag => VerificationType.Uninitialized;
                [IJava] public ushort Offset { get; set; }
            }
            public interface IFrame : IJava
            {
                byte FrameType { get; set; }
                [InstanceOfTag] public static IFrame InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader)
                {
                    byte frame_type = reader.ReadByte();
                    IFrame frame =
                        IsRange(frame_type, 0, 63) ? new SameFrame() :
                        IsRange(frame_type, 64, 127) ? new SameLocals1StackItemFrame() :
                        IsRange(frame_type, 247) ? new SameLocals1StackItemFrameExtended() :
                        IsRange(frame_type, 248, 250) ? new ChopFrame() :
                        IsRange(frame_type, 251) ? new SameFrameExtended() :
                        IsRange(frame_type, 252, 254) ? new AppendFrame() :
                        IsRange(frame_type, 255) ? (IFrame)new FullFrame() :
                        throw new IndexOutOfRangeException($"Frame type '{frame_type}' not supported!")
                        ;
                    frame.FrameType = frame_type;
                    return frame;
                }
            }
            public abstract class IFrame<I> : IJava<I>, IFrame where I : IFrame<I>
            {
                [IJava(IsReaded: false)] public byte FrameType { get; set; }
            }
            public class SameFrame : IFrame<SameFrame> { }
            public class SameLocals1StackItemFrame : IFrame<SameLocals1StackItemFrame>
            {
                [IJava] public VerificationTypeInfo Stack { get; set; }
            }
            public class SameLocals1StackItemFrameExtended : IFrame<SameLocals1StackItemFrameExtended>
            {
                [IJava] public ushort OffsetDelta { get; set; }
                [IJava] public VerificationTypeInfo Stack { get; set; }
            }
            public class ChopFrame : IFrame<ChopFrame>
            {
                [IJava] public ushort OffsetDelta { get; set; }
            }
            public class SameFrameExtended : IFrame<SameFrameExtended>
            {
                [IJava] public ushort OffsetDelta { get; set; }
            }
            public class AppendFrame : IFrame<AppendFrame>
            {
                [IJava] public ushort OffsetDelta { get; set; }
                [IJava(Index: 2)] public List<VerificationTypeInfo> Locals { get; set; }
                public override void ReadProperty(JavaByteCodeReader reader, IJava.PropertyData data, object value)
                {
                    if (data.Index == 2) value = Locals = new VerificationTypeInfo[FrameType - 251].ToList();
                    base.ReadProperty(reader, data, value);
                }
            }
            public class FullFrame : IFrame<FullFrame>
            {
                [IJava] public ushort OffsetDelta { get; set; }
                [IJava] [IJavaArray(IJavaType.UShort)] public List<VerificationTypeInfo> Locals { get; set; }
                [IJava] [IJavaArray(IJavaType.UShort)] public List<VerificationTypeInfo> Stack { get; set; }
            }

            public override AttributeType Type => AttributeType.StackMapTable;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<IFrame> Entries { get; set; }
        }
        public sealed class ExceptionsAttribute : JavaAttribute<ExceptionsAttribute>
        {
            public override AttributeType Type => AttributeType.Exceptions;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<ushort> ExceptionIndexTable { get; set; }
        }
        public sealed class InnerClassesAttribute : JavaAttribute<InnerClassesAttribute>
        {
            public override AttributeType Type => AttributeType.InnerClasses;
            public sealed class ClassInfo : IJava<ClassInfo>
            {
                [IJava] public ushort InnerClassInfoIndex { get; set; }
                public ClassConstant InnerClassInfo
                {
                    get => Handle.Constants[InnerClassInfoIndex] as ClassConstant;
                    set => InnerClassInfoIndex = Handle.OfConstant(value);
                }
                [IJava] public ushort OuterClassInfoIndex { get; set; }
                public ClassConstant OuterClassInfo
                {
                    get => Handle.Constants[OuterClassInfoIndex] as ClassConstant;
                    set => OuterClassInfoIndex = Handle.OfConstant(value);
                }
                [IJava] public ushort InnerNameIndex { get; set; }
                public Utf8Constant InnerName
                {
                    get => Handle.Constants[InnerNameIndex] as Utf8Constant;
                    set => InnerNameIndex = Handle.OfConstant(value);
                }
                [IJava(IJavaType.UShort)] public Flags.AccessClass InnerClassAccessFlags { get; set; }
            }
            [IJava] [IJavaArray(IJavaType.UShort)] public List<ClassInfo> Classes { get; set; }
        }
        public sealed class EnclosingMethodAttribute : JavaAttribute<EnclosingMethodAttribute>
        {
            public override AttributeType Type => AttributeType.EnclosingMethod;
            [IJava] public ushort ClassIndex { get; set; }
            public ClassConstant Class
            {
                get => Handle.Constants[ClassIndex] as ClassConstant;
                set => ClassIndex = Handle.OfConstant(value);
            }
            [IJava] public ushort MethodIndex { get; set; }
            public NameAndTypeConstant Method
            {
                get => Handle.Constants[MethodIndex] as NameAndTypeConstant;
                set => MethodIndex = Handle.OfConstant(value);
            }
        }
        public sealed class SyntheticAttribute : JavaAttribute<SyntheticAttribute>
        {
            public override AttributeType Type => AttributeType.Synthetic;
        }
        public sealed class SignatureAttribute : JavaAttribute<SignatureAttribute>
        {
            public override AttributeType Type => AttributeType.Signature;
            [IJava] public ushort SignatureIndex { get; set; }
            public Utf8Constant Signature
            {
                get => Handle.Constants[SignatureIndex] as Utf8Constant;
                set => SignatureIndex = Handle.OfConstant(value);
            }
        }
        public sealed class SourceFileAttribute : JavaAttribute<SourceFileAttribute>
        {
            public override AttributeType Type => AttributeType.SourceFile;
            [IJava] public ushort SourceFileIndex { get; set; }
            public Utf8Constant SourceFile
            {
                get => Handle.Constants[SourceFileIndex] as Utf8Constant;
                set => SourceFileIndex = Handle.OfConstant(value);
            }
        }
        public sealed class SourceDebugExtensionAttribute : BytesAttribute<SourceDebugExtensionAttribute>
        {
            public override AttributeType Type => AttributeType.SourceFile;
            public SourceDebugExtensionAttribute(byte[] info) : base(info) { }
        }
        public sealed class LineNumberTableAttribute : JavaAttribute<LineNumberTableAttribute>
        {
            public sealed class Line : IJava<Line>
            {
                [IJava] public ushort StartPC { get; set; }
                [IJava] public ushort LineNumber { get; set; }
            }
            public override AttributeType Type => AttributeType.LineNumberTable;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<Line> LineNumberTable { get; set; }
        }
        public sealed class LocalVariableTableAttribute : JavaAttribute<LocalVariableTableAttribute>
        {
            public sealed class Variable : IJava<Variable>
            {
                [IJava] public ushort StartPC { get; set; }
                [IJava] public ushort Length { get; set; }
                [IJava] public ushort NameIndex { get; set; }
                public Utf8Constant Name
                {
                    get => Handle.Constants[NameIndex] as Utf8Constant;
                    set => NameIndex = Handle.OfConstant(value);
                }
                [IJava] public ushort DescriptorIndex { get; set; }
                public Utf8Constant Descriptor
                {
                    get => Handle.Constants[DescriptorIndex] as Utf8Constant;
                    set => DescriptorIndex = Handle.OfConstant(value);
                }
                [IJava] public ushort Index { get; set; }
            }
            public override AttributeType Type => AttributeType.LocalVariableTable;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<Variable> LocalVariableTable { get; set; }
        }
        public sealed class LocalVariableTypeTableAttribute : JavaAttribute<LocalVariableTypeTableAttribute>
        {
            public class VariableType : IJava<VariableType>
            {
                [IJava] public ushort StartPC { get; set; }
                [IJava] public ushort Length { get; set; }
                [IJava] public ushort NameIndex { get; set; }
                public Utf8Constant Name
                {
                    get => Handle.Constants[NameIndex] as Utf8Constant;
                    set => NameIndex = Handle.OfConstant(value);
                }
                [IJava] public ushort SignatureIndex { get; set; }
                public Utf8Constant Signature
                {
                    get => Handle.Constants[SignatureIndex] as Utf8Constant;
                    set => SignatureIndex = Handle.OfConstant(value);
                }
                [IJava] public ushort Index { get; set; }
            }
            public override AttributeType Type => AttributeType.LocalVariableTypeTable;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<VariableType> LocalVariableTypeTable { get; set; }
        }
        public sealed class DeprecatedAttribute : JavaAttribute<DeprecatedAttribute>
        {
            public override AttributeType Type => AttributeType.Deprecated;
        }

        namespace Runtime
        {
            public enum ElementTag : byte
            {
                Byte = (byte)'B',
                Char = (byte)'C',
                Double = (byte)'D',
                Float = (byte)'F',
                Integer = (byte)'I',
                Long = (byte)'J',
                Short = (byte)'S',
                Boolean = (byte)'Z',

                String = (byte)'s',

                Enum = (byte)'e',
                Class = (byte)'c',
                Annotation = (byte)'@',
                Array = (byte)'['
            }
            public interface ElementValue : IJava
            {
                ElementTag Tag { get; set; }
                [InstanceOfTag] public static ElementValue InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader)
                {
                    ElementTag tag;
                    ElementValue value = (tag = (ElementTag)reader.ReadByte()) switch
                    {
                        ElementTag.Byte => new ConstantValue(),
                        ElementTag.Char => new ConstantValue(),
                        ElementTag.Double => new ConstantValue(),
                        ElementTag.Float => new ConstantValue(),
                        ElementTag.Integer => new ConstantValue(),
                        ElementTag.Long => new ConstantValue(),
                        ElementTag.Short => new ConstantValue(),
                        ElementTag.Boolean => new ConstantValue(),
                        ElementTag.String => new ConstantValue(),

                        ElementTag.Enum => new EnumValue(),
                        ElementTag.Class => new ClassValue(),
                        ElementTag.Annotation => new AnnotationValue(),
                        ElementTag.Array => new ArrayValue(),

                        _ => throw new ArgumentException($"ElementTag '{tag}' not supported!")
                    };
                    value.Tag = tag;
                    return value;
                }
            }
            public abstract class ElementValue<I> : IJava<I>, ElementValue where I : ElementValue<I>
            {
                [IJava(IJavaType.Byte, IsReaded: false)] public ElementTag Tag { get; set; }
            }
            public sealed class ConstantValue : ElementValue<ConstantValue>
            {
                [IJava] public ushort ConstantIndex { get; set; }
                public IConstant Constant
                {
                    get => Handle.Constants[ConstantIndex] as IConstant;
                    set => ConstantIndex = Handle.OfConstant(value);
                }

                private static ConstantValue CreateOf(JavaClass handle, ElementTag tag, IConstant constant) => ConstantValue.Create(handle, c =>
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
            public sealed class EnumValue : ElementValue<EnumValue>
            {
                [IJava] public ushort TypeNameIndex { get; set; }
                public Utf8Constant TypeName
                {
                    get => Handle.Constants[TypeNameIndex] as Utf8Constant;
                    set => TypeNameIndex = Handle.OfConstant(value);
                }
                [IJava] public ushort ConstantNameIndex { get; set; }
                public Utf8Constant ConstantName
                {
                    get => Handle.Constants[ConstantNameIndex] as Utf8Constant;
                    set => ConstantNameIndex = Handle.OfConstant(value);
                }
            }
            public sealed class ClassValue : ElementValue<ClassValue>
            {
                [IJava] public ushort ClassInfoIndex { get; set; }
                public Utf8Constant ClassInfo
                {
                    get => Handle.Constants[ClassInfoIndex] as Utf8Constant;
                    set => ClassInfoIndex = Handle.OfConstant(value);
                }
            }
            public sealed class AnnotationValue : ElementValue<AnnotationValue>
            {
                [IJava] public Annotation Annotation { get; set; }
            }
            public sealed class ArrayValue : ElementValue<ArrayValue>
            {
                [IJava] [IJavaArray(IJavaType.UShort)] public List<ElementValue> Values { get; set; }
            }
            public sealed class ElementValuePair : IJava<ElementValuePair>
            {
                [IJava] public ushort NameIndex { get; set; }
                public Utf8Constant Name
                {
                    get => Handle.Constants[NameIndex] as Utf8Constant;
                    set => NameIndex = Handle.OfConstant(value);
                }
                [IJava] public ElementValue ElementValue { get; set; }
            }
            public sealed class Annotation : IJava<Annotation>
            {
                [IJava] public ushort TypeIndex { get; set; }
                public Utf8Constant Type
                {
                    get => Handle.Constants[TypeIndex] as Utf8Constant;
                    set => TypeIndex = Handle.OfConstant(value);
                }
                [IJava] [IJavaArray(IJavaType.UShort)] public List<ElementValuePair> ElementValuePairs { get; set; }
            }
            public sealed class ParameterAnnotation : IJava<ParameterAnnotation>
            {
                [IJava] [IJavaArray(IJavaType.UShort)] public List<Annotation> Annotations { get; set; }
            }

            public interface IRuntimeAnnotations
            {
                List<Annotation> Annotations { get; }
            }
            public interface IRuntimeParameterAnnotations
            {
                List<ParameterAnnotation> ParameterAnnotations { get; }
            }
        }

        public sealed class RuntimeVisibleAnnotationsAttribute : JavaAttribute<RuntimeVisibleAnnotationsAttribute>, Runtime.IRuntimeAnnotations
        {
            public override AttributeType Type => AttributeType.RuntimeVisibleAnnotations;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<Runtime.Annotation> Annotations { get; set; }
        }
        public sealed class RuntimeInvisibleAnnotationsAttribute : JavaAttribute<RuntimeInvisibleAnnotationsAttribute>, Runtime.IRuntimeAnnotations
        {
            public override AttributeType Type => AttributeType.RuntimeInvisibleAnnotations;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<Runtime.Annotation> Annotations { get; set; }
        }

        public sealed class RuntimeVisibleParameterAnnotationsAttribute : JavaAttribute<RuntimeVisibleParameterAnnotationsAttribute>, Runtime.IRuntimeParameterAnnotations
        {
            public override AttributeType Type => AttributeType.RuntimeVisibleParameterAnnotations;
            [IJava] [IJavaArray(IJavaType.Byte)] public List<Runtime.ParameterAnnotation> ParameterAnnotations { get; set; }
        }
        public sealed class RuntimeInvisibleParameterAnnotationsAttribute : JavaAttribute<RuntimeInvisibleParameterAnnotationsAttribute>, Runtime.IRuntimeParameterAnnotations
        {
            public override AttributeType Type => AttributeType.RuntimeInvisibleParameterAnnotations;
            [IJava] [IJavaArray(IJavaType.Byte)] public List<Runtime.ParameterAnnotation> ParameterAnnotations { get; set; }
        }

        public sealed class AnnotationDefaultAttribute : JavaAttribute<AnnotationDefaultAttribute>
        {
            public override AttributeType Type => AttributeType.AnnotationDefault;
            [IJava] public Runtime.ElementValue DefaultValue { get; set; }
        }
        public sealed class BootstrapMethodsAttribute : JavaAttribute<BootstrapMethodsAttribute>
        {
            public sealed class Bootstrap : IJava<Bootstrap>
            {
                [IJava] public ushort MethodRefIndex { get; set; }
                public MethodHandleConstant MethodRef
                {
                    get => Handle.Constants[MethodRefIndex] as MethodHandleConstant;
                    set => MethodRefIndex = Handle.OfConstant(value);
                }
                [IJava] [IJavaArray(IJavaType.UShort)] public List<ushort> ArgumentsIndexes { get; set; }
                public IConstant[] Arguments
                {
                    get => ArgumentsIndexes.Select(v => Handle.Constants[v]).ToArray();
                    set => ArgumentsIndexes = value.Select(v => Handle.OfConstant(v)).ToList();
                }
            }
            public override AttributeType Type => AttributeType.BootstrapMethods;
            [IJava] [IJavaArray(IJavaType.UShort)] public List<Bootstrap> Methods { get; set; }
        }
    }
}
