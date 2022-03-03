using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Collections;
using static Java.Net.Model.IJava;

namespace Java.Net.Model
{
    public class IJavaObject
    {
        private readonly Action<JavaByteCodeReader> reader;
        private readonly Func<JavaByteCodeWriter, JavaByteCodeWriter> writer;

        public IJavaObject(Action<JavaByteCodeReader> reader, Func<JavaByteCodeWriter, JavaByteCodeWriter> writer)
        {
            this.reader = reader;
            this.writer = writer;
        }
        public IJavaObject(Action<JavaByteCodeReader> reader, Action<JavaByteCodeWriter> writer) : this(reader, (a) => { writer.Invoke(a); return a; }) { }

        public static IJavaObject Property<T>(
            Func<JavaByteCodeReader, T> reader,
            Func<JavaByteCodeWriter, T, JavaByteCodeWriter> writer,
            Func<T> getter,
            Action<T> setter
        ) => new IJavaObject((_reader) => setter.Invoke(reader.Invoke(_reader)), (_writer) => writer.Invoke(_writer, getter.Invoke()));

        public void Read(JavaByteCodeReader reader) => this.reader.Invoke(reader);
        public JavaByteCodeWriter Write(JavaByteCodeWriter writer) => this.writer.Invoke(writer);
    }
    public abstract class IJava<I> : IJava where I : IJava<I>
    {
        public JavaClass Handle { get; set; }
        private IJavaData _javaData;
        public IJavaData JavaData => _javaData ??= IJavaData.OfType(GetType());

        public static I Create(JavaClass handle, Action<I> apply = null)
        {
            I dat = Activator.CreateInstance<I>();
            dat.SetHandle(handle);
            apply.Invoke(dat);
            dat.SetHandle(handle);
            return dat;
        }
        public T Create<T>(Action<T> apply = null) where T : IJava<T> => IJava<T>.Create(this.Handle, apply);

        public virtual void ReadProperty(JavaByteCodeReader reader, IJava.PropertyData data, object value) => data.Reader(reader, new InstanceOfTagData.Data() { Parent = this, Reader = reader }, value);
        public virtual IJava Read(JavaByteCodeReader reader)
        {
            foreach (PropertyData data in JavaData.Properties)
                if (data.IsReaded)
                    ReadProperty(reader, data, data.Property.GetValue(this));
            return this;
        }
        public virtual JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, IJava.PropertyData data, object value) => data.Writer(writer, this, value);
        public virtual JavaByteCodeWriter Write(JavaByteCodeWriter writer)
        {
            foreach (PropertyData data in JavaData.Properties)
                if (data.IsWrited)
                    writer = WriteProperty(writer, data, data.Property.GetValue(this));
            return writer;
        }

        public virtual IJava SetHandle(JavaClass handle)
        {
            Handle = handle;
            foreach (PropertyData data in JavaData.Properties)
                if (data.IsReaded)
                {
                    object value = data.Property.GetValue(this);
                    if (value is IJava java) java.SetHandle(handle);
                    else if (value is Array array)
                        foreach (var item in array)
                        {
                            if (item is IJava _java)
                                _java.SetHandle(handle);
                        }
                }
            return this;
        }
        public virtual IJava Load(JavaClass handle, Action<IJava> action)
        {
            SetHandle(handle);
            action.Invoke(this);
            return this;
        }
    }
    public interface IJava : IHandle
    {
        public class ArrayPropertyData : PropertyData
        {
            public IJavaType LengthType { get; }

            public ArrayPropertyData(PropertyInfo property, IJavaAttribute javaAttribute, IJavaArrayAttribute arrayAttribute) : base(property, javaAttribute)
            {
                LengthType = arrayAttribute.LengthType;
            }

            protected override long? ReadLength(JavaByteCodeReader reader) => ReadLength(reader, LengthType);
            protected override void WriteLength(JavaByteCodeWriter writer, long length) => WriteLength(writer, LengthType, length);
        }
        public class PropertyData
        {
            protected static long ReadLength(JavaByteCodeReader reader, IJavaType type) => type switch
            {
                IJavaType.Byte => reader.ReadByte(),
                IJavaType.UShort => reader.ReadUShort(),
                IJavaType.UInt => reader.ReadUInt(),
                IJavaType.Int => reader.ReadInt(),
                IJavaType.Long => reader.ReadLong(),
                _ => throw new TypeAccessException($"Type of length '{type}' not supported!"),
            };
            protected static JavaByteCodeWriter WriteLength(JavaByteCodeWriter reader, IJavaType type, long value) => type switch
            {
                IJavaType.Byte => reader.WriteByte((byte)value),
                IJavaType.UShort => reader.WriteUShort((ushort)value),
                IJavaType.UInt => reader.WriteUInt((uint)value),
                IJavaType.Int => reader.WriteInt((int)value),
                IJavaType.Long => reader.WriteLong((long)value),
                _ => throw new TypeAccessException($"Type of length '{type}' not supported!"),
            };
            protected static void ResizeArray(ref System.Array oldArray, long newSize)
            {
                long oldSize = oldArray.LongLength;
                System.Type elementType = oldArray.GetType().GetElementType();
                System.Array newArray = System.Array.CreateInstance(elementType, newSize);
                long preserveLength = System.Math.Min(oldSize, newSize);
                if (preserveLength > 0) System.Array.Copy(oldArray, newArray, preserveLength);
                oldArray = newArray;
            }
            protected static object CastType(Type type, object value) => type.IsEnum
                ? Enum.ToObject(type, value)
                : value.GetType().IsEnum
                    ? Convert.ChangeType(Convert.ChangeType(value, ((Enum)value).GetTypeCode()), type)
                    : value
                ;
            protected static T CastType<T>(object value) => (T)CastType(typeof(T), value);

            public PropertyInfo Property { get; }
            public IJavaType PropertyType { get; }
            public Type ElementType { get; }
            public bool IsArray { get; }

            public bool IsReaded { get; }
            public bool IsWrited { get; }

            public string PropertyName { get; }
            public int Index { get; }

            public List<IJavaInvokeAttribute> InvokeData { get; } = new List<IJavaInvokeAttribute>();

            private void InvokeMethod(string method, Type[] types, IJava ijava, object[] args)
            {
                Property.DeclaringType.GetMethod(
                    method,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
                    null,
                    types,
                    null).Invoke(ijava, args);
            }
            private void InvokeDataRead(JavaByteCodeReader reader, IJava ijava, object obj, bool isLast, IJavaInvokeAttribute.TypeInvoke type)
            {
                foreach (var data in InvokeData)
                {
                    if (!data.IsRead) continue;
                    if (data.IsLast != isLast || data.Type != type) continue;
                    InvokeMethod(
                        data.Invoke,
                        new Type[] { typeof(JavaByteCodeReader), typeof(object) },
                        ijava,
                        new object[] { reader, obj }
                    );
                }
            }
            private JavaByteCodeWriter InvokeDataWrite(JavaByteCodeWriter writer, IJava ijava, object obj, bool isLast, IJavaInvokeAttribute.TypeInvoke type)
            {
                foreach (var data in InvokeData)
                {
                    if (data.IsRead) continue;
                    if (data.IsLast != isLast || data.Type != type) continue;
                    InvokeMethod(
                        data.Invoke,
                        new Type[] { typeof(JavaByteCodeWriter), typeof(object) },
                        ijava,
                        new object[] { writer, obj }
                    );
                }
                return writer;
            }

            private void InvokeDataRead(JavaByteCodeReader reader, IJava ijava, object obj, bool isLast, ref int index)
            {
                foreach (var data in InvokeData)
                {
                    if (!data.IsRead) continue;
                    if (data.IsLast != isLast || data.Type != IJavaInvokeAttribute.TypeInvoke.ArrayElement) continue;
                    object[] args = new object[] { reader, obj, index };
                    InvokeMethod(
                        data.Invoke,
                        new Type[] { typeof(JavaByteCodeReader), typeof(object), typeof(int).MakeByRefType() },
                        ijava,
                        args
                    );
                    index = (int)args[2];
                }
            }
            private JavaByteCodeWriter InvokeDataWrite(JavaByteCodeWriter writer, IJava ijava, object obj, bool isLast, ref int index)
            {
                foreach (var data in InvokeData)
                {
                    if (data.IsRead) continue;
                    if (data.IsLast != isLast || data.Type != IJavaInvokeAttribute.TypeInvoke.ArrayElement) continue;
                    object[] args = new object[] { writer, obj, index };
                    InvokeMethod(
                        data.Invoke,
                        new Type[] { typeof(JavaByteCodeWriter), typeof(object), typeof(int).MakeByRefType() },
                        ijava,
                        args
                    );
                    index = (int)args[2];
                }
                return writer;
            }

            public PropertyData(PropertyInfo property, IJavaAttribute javaAttribute)
            {
                Property = property;
                Index = javaAttribute.Index;
                PropertyType = javaAttribute.Type;
                PropertyName = property.Name;
                IsReaded = javaAttribute.IsReaded;
                IsWrited = javaAttribute.IsWrited;
                Type propertyType = property.PropertyType;
                IsArray = propertyType.IsArray || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>));
                ElementType = propertyType.IsArray
                    ? propertyType.GetElementType()
                    : (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
                        ? propertyType.GenericTypeArguments[0]
                        : propertyType;

                InvokeData.AddRange(property.GetCustomAttributes<IJavaInvokeAttribute>());
            }

            protected virtual long? ReadLength(JavaByteCodeReader reader) => null;
            protected virtual void WriteLength(JavaByteCodeWriter writer, long length) { }

            public virtual object Reader(JavaByteCodeReader reader, InstanceOfTagData.Data data, object value)
            {
                if (!IsArray) return ReadElement(reader, value ?? ElementType.GetConstructor(new Type[0])?.Invoke(new object[0]), data.Parent, data);

                long? _length = ReadLength(reader);
                IList arr;
                if (value == null)
                {
                    arr = Array.CreateInstance(ElementType, _length.HasValue ? _length.Value : throw new ArgumentNullException($"Can't create default array of type '{ElementType.FullName}'"));
                    if (!Property.PropertyType.IsArray) arr = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ElementType), arr);
                }
                else
                {
                    arr = (IList)value;
                }
                int length = arr.Count;
                InvokeDataRead(reader, data.Parent, arr, false, IJavaInvokeAttribute.TypeInvoke.Array);
                IJava last = null;
                for (int i = 0; i < length; i++)
                {
                    InvokeDataRead(reader, data.Parent, arr, false, ref i);
                    InstanceOfTagData.Data itemData = data.Copy()
                        .Edit(v => v.LastOfArray = last)
                        .Edit(v => v.IndexOfArray = i);
                    object item = ReadElement(reader, arr[i], null, itemData);
                    if (item is IJava _last) last = _last;
                    arr[i] = item;
                    i = itemData.IndexOfArray;
                    InvokeDataRead(reader, data.Parent, arr, true, ref i);
                }
                InvokeDataRead(reader, data.Parent, arr, true, IJavaInvokeAttribute.TypeInvoke.Array);

                Property.SetValue(data.Parent, arr);
                return arr;
            }
            private object ReadElement(JavaByteCodeReader reader, object obj, IJava ijava, InstanceOfTagData.Data data)
            {
                InvokeDataRead(reader, ijava, obj, false, IJavaInvokeAttribute.TypeInvoke.Element);
                if (PropertyType == IJavaType.Custom) ((IJavaObject)obj).Read(reader);
                else if (Property.SetMethod != null)
                {
                    object value = CastType(ElementType, PropertyType switch
                    {
                        IJavaType.Byte => reader.ReadByte(),
                        IJavaType.UShort => reader.ReadUShort(),
                        IJavaType.UInt => reader.ReadUInt(),
                        IJavaType.Int => reader.ReadInt(),
                        IJavaType.Long => reader.ReadLong(),
                        IJavaType.Float => reader.ReadFloat(),
                        IJavaType.Double => reader.ReadDouble(),
                        IJavaType.UTF8 => reader.ReadUTF8(reader.ReadUShort()),
                        IJavaType.IJava => Read(ElementType, data, reader, (IJava)obj),
                        _ => throw new TypeAccessException($"Type '{Property}' in property '{Property.DeclaringType.FullName}.{Property.Name}' not supported!"),
                    });
                    if (ijava != null) Property.SetValue(ijava, value);
                    obj = value;
                }
                InvokeDataRead(reader, ijava, obj, true, IJavaInvokeAttribute.TypeInvoke.Element);
                return obj;
            }
            public virtual JavaByteCodeWriter Writer(JavaByteCodeWriter writer, IJava ijava, object value)
            {
                if (!IsArray) return WriteElement(writer, ijava, value);
                IList arr = (IList)value;
                int length = arr.Count;
                WriteLength(writer, length);
                writer = InvokeDataWrite(writer, ijava, value, false, IJavaInvokeAttribute.TypeInvoke.Array);
                for (int i = 0; i < length; i++)
                {
                    InvokeDataWrite(writer, ijava, arr, false, ref i);
                    writer = WriteElement(writer, ijava, arr[i]);
                    InvokeDataWrite(writer, ijava, arr, true, ref i);
                }
                writer = InvokeDataWrite(writer, ijava, value, true, IJavaInvokeAttribute.TypeInvoke.Array);
                return writer;
            }
            private JavaByteCodeWriter WriteElement(JavaByteCodeWriter writer, IJava ijava, object value)
            {
                writer = InvokeDataWrite(writer, ijava, value, false, IJavaInvokeAttribute.TypeInvoke.Element);
                return InvokeDataWrite(PropertyType switch
                {
                    IJavaType.Byte => writer.WriteByte(CastType<byte>(value)),
                    IJavaType.UShort => writer.WriteUShort(CastType<ushort>(value)),
                    IJavaType.UInt => writer.WriteUInt(CastType<uint>(value)),
                    IJavaType.Int => writer.WriteInt(CastType<int>(value)),
                    IJavaType.Long => writer.WriteLong(CastType<long>(value)),
                    IJavaType.Float => writer.WriteFloat(CastType<float>(value)),
                    IJavaType.Double => writer.WriteDouble(CastType<double>(value)),
                    IJavaType.UTF8 => writer.WriteUTF8<ushort>((string)value, writer.WriteUShort),
                    IJavaType.IJava => ((IJava)value).Write(writer),
                    IJavaType.Custom => ((IJavaObject)value).Write(writer),
                    _ => throw new TypeAccessException($"Type '{PropertyType}' in property '{Property.DeclaringType.FullName}.{Property.Name}' not supported!"),
                }, ijava, value, true, IJavaInvokeAttribute.TypeInvoke.Element);
            }

            public override string ToString() => PropertyType + (IsArray ? "[]" : "") + " " + PropertyName;

            public static PropertyData Of(PropertyInfo property)
            {
                IJavaType? _type = GetJavaType(property);
                IJavaAttribute attr = property.GetCustomAttribute<IJavaAttribute>();
                if (_type == null) return null;
                IJavaArrayAttribute arr = property.GetCustomAttribute<IJavaArrayAttribute>();
                return arr == null
                    ? new PropertyData(property, new IJavaAttribute(_type.Value, attr.IsReaded, attr.IsWrited, attr.Index))
                    : new ArrayPropertyData(property, new IJavaAttribute(_type.Value, attr.IsReaded, attr.IsWrited, attr.Index), arr);
            }
        }
        public class InstanceOfTagData
        {
            public class Data
            {
                public IJava Parent { get; set; }
                public IJava LastOfArray { get; set; }
                public JavaByteCodeReader Reader { get; set; }
                public int IndexOfArray { get; set; }

                public Data Edit(Action<Data> func)
                {
                    func.Invoke(this);
                    return this;
                }

                public Data Copy() => new Data() {
                    Parent = Parent,
                    LastOfArray = LastOfArray,
                    Reader = Reader
                };
            }

            public MethodInfo Method { get; }
            public (TagTypeAttribute.Tag tag, bool is_ref)[] Arguments { get; }
            public InstanceOfTagData(MethodInfo method, (TagTypeAttribute.Tag tag, bool is_ref)[] arguments)
            {
                Method = method;
                Arguments = arguments;
            }

            public IJava InvokeMethod(Data data)
            {
                int length = Arguments.Length;
                object[] args = new object[length];
                for (int i = 0; i < length; i++)
                {
                    (TagTypeAttribute.Tag tag, bool _) = Arguments[i];
                    args[i] = tag switch
                    {
                        TagTypeAttribute.Tag.Parent => data.Parent,
                        TagTypeAttribute.Tag.Handle => data.Parent?.Handle,
                        TagTypeAttribute.Tag.Reader => data.Reader,
                        TagTypeAttribute.Tag.IndexOfArray => data.IndexOfArray,
                        TagTypeAttribute.Tag.LastOfArray => data.LastOfArray,
                        _ => new ArgumentException($"TagType '{Arguments[i]}' not supported!")
                    };
                }
                IJava java = (IJava)Method.Invoke(null, args);
                for (int i = 0; i < length; i++)
                {
                    (TagTypeAttribute.Tag tag, bool is_ref) = Arguments[i];
                    if (!is_ref) continue;
                    object item = args[i];
                    switch (tag)
                    {
                        case TagTypeAttribute.Tag.Parent: data.Parent = (IJava)item; break;
                        case TagTypeAttribute.Tag.Handle: data.Parent.SetHandle((JavaClass)item); break;
                        case TagTypeAttribute.Tag.Reader: data.Reader = (JavaByteCodeReader)item; break;
                        case TagTypeAttribute.Tag.IndexOfArray: data.IndexOfArray = (int)item; break;
                        case TagTypeAttribute.Tag.LastOfArray: data.LastOfArray = (IJava)item; break;
                    }
                }
                return java;
            }

            public static InstanceOfTagData Of(MethodInfo method)
            {
                if (method.GetCustomAttribute<InstanceOfTagAttribute>() == null) return null;
                if (!method.DeclaringType.IsAssignableFrom(method.ReturnParameter.ParameterType)) return null;

                ParameterInfo[] list = method.GetParameters();
                int length;
                (TagTypeAttribute.Tag tag, bool is_ref)[] arguments = new (TagTypeAttribute.Tag tag, bool is_ref)[length = list.Length];
                for (int i = 0; i < length; i++)
                {
                    TagTypeAttribute attribute = list[i].GetCustomAttribute<TagTypeAttribute>();
                    if (attribute == null) return null;
                    arguments[i] = (attribute.Type, list[i].ParameterType.IsByRef);
                }
                return new InstanceOfTagData(method, arguments);
            }
        }
        public class IJavaData
        {
            public List<PropertyData> Properties { get; } = new List<PropertyData>();
            public InstanceOfTagData InstanceOfTag { get; } = null;
            public Type Type { get; }

            private static IEnumerable<PropertyInfo> GetProperties(Type type)
            {
                if (type == null) yield break;
                foreach (var prop in GetProperties(type.BaseType)) yield return prop;
                foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                    if (prop.DeclaringType == type)
                        yield return prop;
            }
            private static IEnumerable<MethodInfo> GetMethods(Type type)
            {
                if (type == null) yield break;
                foreach (var meth in GetMethods(type.BaseType))
                    yield return meth;
                foreach (var itype in type.GetInterfaces())
                    foreach (var meth in GetMethods(itype))
                        yield return meth;
                foreach (MethodInfo meth in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                    if (meth.DeclaringType == type)
                        yield return meth;
            }

            public IJavaData(Type type)
            {
                this.Type = type;
                foreach (MethodInfo method in GetMethods(type))
                {
                    InstanceOfTagData data = InstanceOfTagData.Of(method);
                    if (data == null) continue;
                    InstanceOfTag = data;
                }

                Dictionary<string, PropertyData> props = new Dictionary<string, PropertyData>();
                foreach (PropertyInfo prop in GetProperties(type))
                {
                    PropertyData data = PropertyData.Of(prop);
                    if (data == null) continue;
                    props[data.PropertyName] = data;
                }
                Properties.AddRange(props.Values);
            }

            private static readonly ConcurrentDictionary<Type, IJavaData> classes = new ConcurrentDictionary<Type, IJavaData>();
            public static IJavaData OfType(Type type)
            {
                lock (classes) return classes.TryGetValue(type, out IJavaData _data) ? _data : classes[type] = new IJavaData(type);
            }

            public static IJava ReadOfType(Type type, InstanceOfTagData.Data data, JavaByteCodeReader reader, IJava def)
            {
                def ??= OfType(type).InstanceOfTag?.InvokeMethod(data) ?? (IJava)Activator.CreateInstance(type);
                return (def is JavaClass j ? def.SetHandle(j) : def.SetHandle(data.Parent?.Handle)).Read(reader);
            }

            public static JavaByteCodeWriter WriteOfType(IJava obj, JavaByteCodeWriter writer) => obj.Write(writer);
        }

        IJavaData JavaData { get; }

        void ReadProperty(JavaByteCodeReader reader, PropertyData data, object value);
        IJava Read(JavaByteCodeReader reader);
        JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, PropertyData data, object value);
        JavaByteCodeWriter Write(JavaByteCodeWriter writer);

        IJava SetHandle(JavaClass handle);
        IJava Load(JavaClass handle, Action<IJava> action);

        private static readonly Dictionary<Type, IJavaType> typeFormats = new Dictionary<Type, IJavaType>()
        {
            [typeof(byte)] = IJavaType.Byte,
            [typeof(ushort)] = IJavaType.UShort,
            [typeof(uint)] = IJavaType.UInt,
            [typeof(int)] = IJavaType.Int,
            [typeof(long)] = IJavaType.Long,
            [typeof(float)] = IJavaType.Float,
            [typeof(double)] = IJavaType.Double,
            [typeof(string)] = IJavaType.UTF8,
            [typeof(IJavaObject)] = IJavaType.Custom
        };

        private static IJavaType? GetJavaType(PropertyInfo property)
        {
            IJavaAttribute attribute = property.GetCustomAttribute<IJavaAttribute>();
            if (attribute?.Type != IJavaType.Auto) return attribute?.Type;
            Type type = property.PropertyType;
            type = type.IsArray
                ? type.GetElementType()
                : (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                    ? type.GenericTypeArguments[0]
                    : type;

            if (typeFormats.TryGetValue(type, out IJavaType _type)) return _type;
            return typeof(IJava).IsAssignableFrom(type) ? IJavaType.IJava : (IJavaType?)null;
        }

        private static IJava Read(Type type, InstanceOfTagData.Data data, JavaByteCodeReader reader, IJava def = null) => IJavaData.ReadOfType(type, data, reader, def);
        public static I Read<I>(InstanceOfTagData.Data data, JavaByteCodeReader reader, I def = null) where I : class, IJava => (I)Read(typeof(I), data, reader, def);
        public static I ReadArray<I>(InstanceOfTagData.Data data, byte[] bytes, I def = null) where I : class, IJava
        {
            using MemoryStream stream = new MemoryStream(bytes);
            return Read(data, new JavaByteCodeReader(stream), def);
        }
        public static IJava ReadArray(Type type, InstanceOfTagData.Data data, byte[] bytes, IJava def = null)
        {
            using MemoryStream stream = new MemoryStream(bytes);
            return Read(type, data, new JavaByteCodeReader(stream), def);
        }

        public static JavaByteCodeWriter Write(IJava obj, JavaByteCodeWriter writer) => IJavaData.WriteOfType(obj, writer);
        public static byte[] WriteArray(IJava java)
        {
            using MemoryStream stream = new MemoryStream();
            java.Write(new JavaByteCodeWriter(stream));
            stream.Position = 0;
            return stream.ToArray();
        }
        public static void SetHandle(IJava java, JavaClass handle) => java.SetHandle(handle);
    }
    public enum IJavaType
    {
        Auto,

        Byte,
        UShort,
        UInt,
        Int,
        Long,
        Float,
        Double,
        UTF8,

        IJava,
        Custom
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] public class IJavaAttribute : System.Attribute
    {
        public int Index { get; }
        public bool IsReaded { get; }
        public bool IsWrited { get; }
        public IJavaType Type { get; }
        public IJavaAttribute(IJavaType Type = IJavaType.Auto, bool IsReaded = true, bool IsWrited = true, int Index = -1)
        {
            this.Index = Index;
            this.Type = Type;
            this.IsReaded = IsReaded;
            this.IsWrited = IsWrited;
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] public class IJavaArrayAttribute : System.Attribute
    {
        public IJavaType LengthType { get; }
        public IJavaArrayAttribute(IJavaType LengthType = IJavaType.UShort) => this.LengthType = LengthType;
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)] public class IJavaInvokeAttribute : System.Attribute
    {
        public enum TypeInvoke
        {
            Element,
            Array,
            ArrayElement
        }

        public string Invoke { get; }
        public bool IsRead { get; }
        public bool IsLast { get; }
        public TypeInvoke Type { get; }
        public IJavaInvokeAttribute(string Invoke, bool IsRead, bool IsLast = true, TypeInvoke Type = TypeInvoke.Element)
        {
            this.Invoke = Invoke;
            this.IsRead = IsRead;
            this.IsLast = IsLast;
            this.Type = Type;
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)] public class InstanceOfTagAttribute : System.Attribute { }
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)] public class TagTypeAttribute : System.Attribute
    {
        public Tag Type { get; }
        public enum Tag
        {
            Parent,
            Handle,
            Reader,
            IndexOfArray,
            LastOfArray
        }
        public TagTypeAttribute(Tag type) { this.Type = type; }
    }
}
