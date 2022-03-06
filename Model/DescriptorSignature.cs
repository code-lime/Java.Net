using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Java.Net.Model
{
    public static class TypeDescriptor
    {
        internal static GenericParameter FindGeneric<IGenericDefinition>(IGenericDefinition generic, string name)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
        {
            GenericParameter gen = generic.GenericParameters.FirstOrDefault(v => v.Name == name);
            if (gen != null) return gen;
            if (generic.DeclaringType == null) return null;
            gen = FindGeneric(generic.DeclaringType, name);
            //if (gen == null) throw new KeyNotFoundException($"Generic type '{name}' not founded!");
            return gen;
        }

        public static (StringBuilder builder, Func<TypeReference> func) Parse<IGenericDefinition>(string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => Parse(ref descriptor, generic, assembly);
        public static (StringBuilder builder, Func<TypeReference> func) Parse<IGenericDefinition>(ref string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
        {
            char ch = descriptor[0];
            descriptor = descriptor[1..];
            StringBuilder builder = new StringBuilder().Append(ch);
            switch (ch)
            {
                case 'B': return (builder, () => assembly.MainModule.TypeSystem.Byte);
                case 'C': return (builder, () => assembly.MainModule.TypeSystem.Char);
                case 'D': return (builder, () => assembly.MainModule.TypeSystem.Double);
                case 'F': return (builder, () => assembly.MainModule.TypeSystem.Single);
                case 'I': return (builder, () => assembly.MainModule.TypeSystem.Int32);
                case 'J': return (builder, () => assembly.MainModule.TypeSystem.Int64);
                case '*': return (builder, () => assembly.MainModule.TypeSystem.Object);
                case 'L':
                    {
                        int end = descriptor.IndexOf(';');
                        int? gen = descriptor.IndexOf('<');
                        if (gen < 0 || gen > end) gen = null;
                        int next = Math.Min(end, gen ?? int.MaxValue);

                        string[] full_name = descriptor[..next].Split('/');
                        descriptor = descriptor[(next + 1)..];

                        string _namespace = string.Join('.', full_name[..^1]);
                        string name = full_name[^1].Replace('$', '/');
                        if (name.Contains('.'))
                            ;
                        string full_cs = _namespace + "." + name;
                        builder = builder.Append(full_name);

                        List<Func<TypeReference, TypeReference>> list = new List<Func<TypeReference, TypeReference>>();
                        list.Add(v => {
                            TypeReference reference = assembly.MainModule.GetAllTypes().Where(type => type.FullName == full_cs).FirstOrDefault();
                            if (reference == null)
                            {
                                Console.WriteLine($"Type '{full_cs}' not founded!");
                                reference = new TypeReference(_namespace, name, assembly.MainModule, assembly.MainModule);
                            }
                            return reference;
                        });

                        if (gen != null)
                        {
                            list.Add(reference => new GenericInstanceType(reference));
                            List<TypeReference> generic_list = new List<TypeReference>();

                            while (descriptor[0] != '>')
                            {
                                (StringBuilder _builder, Func<TypeReference> _func) = Parse(ref descriptor, generic, assembly);
                                builder = builder.Append(_builder);
                                list.Add(genericInstance =>
                                {
                                    ((GenericInstanceType)genericInstance).GenericArguments.Add(_func.Invoke());
                                    return genericInstance;
                                });
                            }
                            descriptor = descriptor[2..];
                            builder = builder.Append('>');
                        }

                        return (builder, () =>
                        {
                            TypeReference reference = null;
                            foreach (var func in list) reference = func.Invoke(reference);
                            return reference;
                        });
                    }
                case 'T':
                    {
                        int next = descriptor.IndexOf(';');
                        string t_name = descriptor[..next];
                        descriptor = descriptor[(next + 1)..];
                        return (builder.Append(t_name).Append(';'), () => FindGeneric(generic, t_name) ?? generic.Module.TypeSystem.Object);
                    }
                case 'S': return (builder, () => assembly.MainModule.TypeSystem.Int16);
                case 'Z': return (builder, () => assembly.MainModule.TypeSystem.Boolean);
                case 'V': return (builder, () => assembly.MainModule.TypeSystem.Void);
                case ':': return Parse(ref descriptor, generic, assembly);
                case '[':
                    {
                        (StringBuilder _builder, Func<TypeReference> _func) = Parse(ref descriptor, generic, assembly);
                        return (builder.Append(_builder), () => new ArrayType(_func.Invoke()));
                    }
            }
            throw new ArgumentException($"Error parse: '{ch}{descriptor}'", "descriptor");
        }
        public static StringBuilder Read(string descriptor)
            => Read(ref descriptor);
        public static StringBuilder Read(ref string descriptor)
            => Parse<TypeDefinition>(ref descriptor, null, null).builder;
        public static TypeReference Read<IGenericDefinition>(string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => Read(ref descriptor, generic, assembly);
        public static TypeReference Read<IGenericDefinition>(ref string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => Parse(ref descriptor, generic, assembly).func.Invoke();

        public static string ToJavaText(ref string descriptor, Func<string, string> clazz_converter)
        {
            char ch = descriptor[0];
            descriptor = descriptor[1..];
            switch (ch)
            {
                case 'B': return "byte";
                case 'C': return "char";
                case 'D': return "double";
                case 'F': return "float";
                case 'I': return "int";
                case 'J': return "long";
                case '*': return "?";
                case 'L':
                    {
                        int end = descriptor.IndexOf(';');
                        int? gen = descriptor.IndexOf('<');
                        if (gen < 0 || gen > end) gen = null;
                        int next = Math.Min(end, gen ?? int.MaxValue);

                        string full_java = clazz_converter.Invoke(descriptor[..next]);
                        descriptor = descriptor[(next + 1)..];

                        if (gen != null)
                        {
                            full_java += "<";
                            while (descriptor[0] != '>') ToJavaText(ref descriptor, clazz_converter);
                            full_java += ">";
                            descriptor = descriptor[2..];
                        }

                        return full_java;
                    }
                case 'T':
                    {
                        int next = descriptor.IndexOf(';');
                        string t_name = descriptor[..next];
                        descriptor = descriptor[(next + 1)..];
                        return t_name;
                    }
                case 'S': return "short";
                case 'Z': return "boolean";
                case 'V': return "void";
                case ':': return ToJavaText(ref descriptor, clazz_converter);
                case '[': return ToJavaText(ref descriptor, clazz_converter) + "[]";
            }
            throw new ArgumentException($"Error parse: '{ch}{descriptor}'", "descriptor");
        }
    }
    public static class MethodDescriptor
    {
        public static (TypeReference[] args, TypeReference ret) Parse<IGenericDefinition>(string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => Parse(ref descriptor, generic, assembly);
        public static (TypeReference[] args, TypeReference ret) Parse<IGenericDefinition>(ref string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
        {
            char ch = descriptor[0];
            descriptor = descriptor[1..];
            switch (ch)
            {
                case '(':
                    {
                        List<TypeReference> args = new List<TypeReference>();
                        while (descriptor[0] != ')') args.Add(TypeDescriptor.Read(ref descriptor, generic, assembly));
                        int next = descriptor.IndexOf(')');
                        descriptor = descriptor[(next + 1)..];
                        return (args.ToArray(), TypeDescriptor.Read(ref descriptor, generic, assembly));
                    }
            }
            throw new ArgumentException($"Error parse: '{ch}{descriptor}'", "descriptor");
        }

        public static MethodReference Parse<IGenericDefinition>(IMethodRefConstant methodref, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => Parse(methodref.Class.Name, methodref.NameAndType.Name, methodref.NameAndType.Descriptor, generic, assembly);
        public static MethodReference Parse<IGenericDefinition>(InvokeDynamicConstant invokeDynamic, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => Parse((IMethodRefConstant)invokeDynamic.BootstrapMethod.MethodRef.Reference, generic, assembly);
        public static MethodReference Parse<IGenericDefinition>(string type, string name, string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
        {
            name = name switch
            {
                "<init>" => ".ctor",
                "<clinit>" => ".cctor",
                _ => name
            };
            (TypeReference[] args, TypeReference ret) = Parse(descriptor, generic, assembly);
            MethodReference method = new MethodReference(name, ret, TypeDescriptor.Read($"L{type};", generic, assembly));
            foreach (var arg in args) method.Parameters.Add(new ParameterDefinition(arg));
            return method;
        }
    
        public static string ToJavaText(string name, ref string descriptor, Func<string, string> clazz_converter)
        {
            char ch = descriptor[0];
            descriptor = descriptor[1..];
            switch (ch)
            {
                case '(':
                    {
                        List<string> args = new List<string>();
                        while (descriptor[0] != ')') args.Add(TypeDescriptor.ToJavaText(ref descriptor, clazz_converter));
                        int next = descriptor.IndexOf(')');
                        descriptor = descriptor[(next + 1)..];
                        string ret = TypeDescriptor.ToJavaText(ref descriptor, clazz_converter);
                        return $"{ret} {name}({string.Join(",", args)})";
                    }
            }
            throw new ArgumentException($"Error parse: '{ch}{descriptor}'", "descriptor");
        }
    }
    public static class FieldDescriptor
    {
        public static FieldReference Parse<IGenericDefinition>(FieldrefConstant fieldref, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => Parse(fieldref.Class.Name, fieldref.NameAndType.Name, fieldref.NameAndType.Descriptor, generic, assembly);
        public static FieldReference Parse<IGenericDefinition>(string type, string name, string descriptor, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
            => new FieldReference(name, TypeDescriptor.Read(descriptor, generic, assembly), TypeDescriptor.Read($"L{type};", generic, assembly));
    }
    public static class TypeSignature
    {
        private class Ref<T> { public T value; public Ref(T value) => this.value = value; public override string ToString() => value?.ToString() ?? "NULL"; }
        private static Ref<T> OfRef<T>(T value) => new Ref<T>(value);
        private static IEnumerable<GenericParameter> ReadGeneric<IGenericDefinition>(Ref<string> rsignature, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
        {
            int next = rsignature.value.IndexOf(':');
            string name = rsignature.value[..next];
            rsignature.value = rsignature.value[(next + 1)..];
            GenericParameter parameter = new GenericParameter(name, generic);
            generic.GenericParameters.Add(parameter);
            (StringBuilder builder, Func<TypeReference> func) = TypeDescriptor.Parse(ref rsignature.value, generic, assembly);
            yield return parameter;
            parameter.Constraints.Add(new GenericParameterConstraint(func.Invoke()));
        }
        public static void ApplyGeneric<IGenericDefinition>(ref string signature, IGenericDefinition generic, AssemblyDefinition assembly)
            where IGenericDefinition : IGenericParameterProvider, IMemberDefinition
        {
            if (signature[0] == '<')
            {
                signature = signature[1..];
                Ref<string> rsignature = OfRef(signature);
                Dictionary<GenericParameter, IEnumerator<GenericParameter>> list = new Dictionary<GenericParameter, IEnumerator<GenericParameter>>();
                while (rsignature.value[0] != '>')
                {
                    IEnumerator<GenericParameter> enumerator = ReadGeneric(rsignature, generic, assembly).GetEnumerator();
                    enumerator.MoveNext();
                    list[enumerator.Current] = enumerator;
                }
                signature = rsignature.value[1..];
                INetStep<GenericParameter> step = INetStep<GenericParameter>.Of(list.Values);
                while (step.MoveNext()) ;
            }
            if (generic is TypeDefinition type)
            {
                type.BaseType = TypeDescriptor.Read(ref signature, generic, assembly);
                if (signature.Length > 0)
                    type.Interfaces.Add(new InterfaceImplementation(TypeDescriptor.Read(ref signature, generic, assembly)));
            }
        }
    }
    public static class Annotation
    {
        public static void Apply<IGenericAttributeDefinition>(List<JavaAttribute> attributes, IGenericAttributeDefinition generic, AssemblyDefinition assembly)
            where IGenericAttributeDefinition : IGenericParameterProvider, IMemberDefinition, ICustomAttributeProvider
        {
            List<Attribute.Runtime.Annotation> annotations = new List<Attribute.Runtime.Annotation>();
            foreach (JavaAttribute attribute in attributes)
                if (attribute is Attribute.Runtime.IRuntimeAnnotations runtime)
                    annotations.AddRange(runtime.Annotations);
            if (annotations.Count > 0)
                foreach (var annotation in annotations)
                    generic.CustomAttributes.Add(new CustomAttribute(MethodDescriptor.Parse(annotation.Type.Value, ".ctor", "()V", generic, assembly)));
        }
    }
}
