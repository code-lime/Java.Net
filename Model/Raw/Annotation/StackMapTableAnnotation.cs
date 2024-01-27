using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Data.Data;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class StackMapTableAnnotation : IAnnotation<StackMapTableAnnotation>
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
    public interface IVerificationTypeInfo : IRaw
    {
        VerificationType Tag { get; }
        [InstanceOfTag]
        public static async ValueTask<IVerificationTypeInfo> InstanceOfTag(
            [TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader,
            CancellationToken cancellationToken)
        {
            VerificationType type;
            return (type = (VerificationType)await reader.ReadByteAsync(cancellationToken)) switch
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
    public abstract class VerificationTypeInfo<I> : BaseRaw<I>, IVerificationTypeInfo where I : VerificationTypeInfo<I>
    {
        [JavaRaw(JavaType.Byte, IsReaded: false)] public abstract VerificationType Tag { get; }
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
        [JavaRaw] public ushort CPoolIndex { get; set; }
        public ClassConstant CPool
        {
            get => (ClassConstant)Handle.Constants[CPoolIndex];
            set => CPoolIndex = Handle.OfConstant(value);
        }
    }
    public class UninitializedVariableInfo : VerificationTypeInfo<UninitializedVariableInfo>
    {
        public override VerificationType Tag => VerificationType.Uninitialized;
        [JavaRaw] public ushort Offset { get; set; }
    }
    public interface IFrame : IRaw
    {
        byte FrameType { get; set; }
        [InstanceOfTag]
        public static async ValueTask<IFrame> InstanceOfTag([TagType(TagTypeAttribute.Tag.Reader)] JavaByteCodeReader reader, CancellationToken cancellationToken)
        {
            byte frame_type = await reader.ReadByteAsync(cancellationToken);
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
    public abstract class IFrame<I> : BaseRaw<I>, IFrame where I : IFrame<I>
    {
        [JavaRaw(IsReaded: false)] public byte FrameType { get; set; }
    }
    public class SameFrame : IFrame<SameFrame> { }
    public class SameLocals1StackItemFrame : IFrame<SameLocals1StackItemFrame>
    {
        [JavaRaw] public IVerificationTypeInfo Stack { get; set; } = null!;
    }
    public class SameLocals1StackItemFrameExtended : IFrame<SameLocals1StackItemFrameExtended>
    {
        [JavaRaw] public ushort OffsetDelta { get; set; }
        [JavaRaw] public IVerificationTypeInfo Stack { get; set; } = null!;
    }
    public class ChopFrame : IFrame<ChopFrame>
    {
        [JavaRaw] public ushort OffsetDelta { get; set; }
    }
    public class SameFrameExtended : IFrame<SameFrameExtended>
    {
        [JavaRaw] public ushort OffsetDelta { get; set; }
    }
    public class AppendFrame : IFrame<AppendFrame>
    {
        [JavaRaw] public ushort OffsetDelta { get; set; }
        [JavaRaw(Index: 2)] public List<IVerificationTypeInfo> Locals { get; set; } = null!;
        public override ValueTask ReadPropertyAsync(JavaByteCodeReader reader, PropertyData data, object? value, CancellationToken cancellationToken)
        {
            if (data.Index == 2) value = Locals = new IVerificationTypeInfo[FrameType - 251].ToList();
            return base.ReadPropertyAsync(reader, data, value, cancellationToken);
        }
    }
    public class FullFrame : IFrame<FullFrame>
    {
        [JavaRaw] public ushort OffsetDelta { get; set; }
        [JavaRaw][JavaArray(JavaType.UShort)] public List<IVerificationTypeInfo> Locals { get; set; } = null!;
        [JavaRaw][JavaArray(JavaType.UShort)] public List<IVerificationTypeInfo> Stack { get; set; } = null!;
    }

    public override AnnotationType Type => AnnotationType.StackMapTable;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<IFrame> Entries { get; set; } = null!;
}
