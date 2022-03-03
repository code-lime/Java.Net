using Java.Net.Flags;
using Java.Net.Net;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Java.Net.Model
{
    public class JavaMethod : IJava<JavaMethod>, INet<Mono.Cecil.MethodDefinition, Mono.Cecil.TypeDefinition>, IFlag<AccessMethod>, IAttributes
    {
        public static JavaMethod Create(JavaClass handle, string name, string descriptor, Action<JavaMethod> apply = null)
        {
            JavaMethod dat = new JavaMethod
            {
                Handle = handle,
                Name = name,
                Descriptor = descriptor,
                Attributes = new List<JavaAttribute>()
            };
            apply.Invoke(dat);
            return dat;
        }

        [IJava(IJavaType.UShort)] public AccessMethod Flags { get; set; }
        [IJava] public ushort NameIndex { get; set; }
        public string Name
        {
            get => (Handle.Constants[NameIndex] as Utf8Constant).Value;
            set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        [IJava] public ushort DescriptorIndex { get; set; }
        public string Descriptor
        {
            get => (Handle.Constants[DescriptorIndex] as Utf8Constant).Value;
            set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
        }
        [IJava] [IJavaArray] public List<JavaAttribute> Attributes { get; set; }

        public IEnumerable<MethodDefinition> ToNet(TypeDefinition parent)
        {
            string descriptor = Descriptor;
            MethodAttributes methodAttributes = MethodAttributes.HideBySig;
            if (Flags.HasFlag(AccessMethod.ABSTRACT)) methodAttributes |= MethodAttributes.Abstract;
            if (Flags.HasFlag(AccessMethod.FINAL)) methodAttributes |= MethodAttributes.Final;
            if (Flags.HasFlag(AccessMethod.PRIVATE)) methodAttributes |= MethodAttributes.Private;
            if (Flags.HasFlag(AccessMethod.PROTECTED)) methodAttributes |= MethodAttributes.Family;
            if (Flags.HasFlag(AccessMethod.PUBLIC)) methodAttributes |= MethodAttributes.Public;
            if (Flags.HasFlag(AccessMethod.STATIC)) methodAttributes |= MethodAttributes.Static;

            string name = Name;
            switch (name)
            {
                case "<init>":
                    methodAttributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
                    name = ".ctor";
                    break;
                case "<clinit>":
                    methodAttributes |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
                    name = ".cctor";
                    break;
            }
            MethodDefinition method = new MethodDefinition(name, methodAttributes, parent.Module.TypeSystem.Object);
            (TypeReference[] args, TypeReference ret) = MethodDescriptor.Parse(ref descriptor, method, parent.Module.Assembly);
            method.ReturnType = ret;
            Attribute.CodeAttribute code = (Attribute.CodeAttribute)Attributes.FirstOrDefault(v => v is Attribute.CodeAttribute);
            Attribute.LocalVariableTableAttribute local = (Attribute.LocalVariableTableAttribute)code?.Attributes?.FirstOrDefault(v => v is Attribute.LocalVariableTableAttribute);
            int length = args.Length;
            int offset = Flags.HasFlag(AccessMethod.STATIC) ? 0 : 1;

            int offset_local = length - (local?.LocalVariableTable?.Count ?? 0);

            for (int i = 0; i < length; i++)
            {
                TypeReference arg = args[i];
                string arg_name = local == null ? string.Empty : (local.LocalVariableTable.FirstOrDefault(v => v.Index == (i + offset))?.Name?.Value ?? string.Empty);
                method.Parameters.Add(new ParameterDefinition(arg_name, ParameterAttributes.None, arg));
            }
            parent.Methods.Add(method);
            if (Flags.HasFlag(AccessMethod.SYNCHRONIZED))
            {
                CustomAttributeArgument attributeArgument = new CustomAttributeArgument(
                    parent.Module.ImportReference(typeof(MethodImplOptions)),
                    MethodImplOptions.Synchronized
                );
                CustomAttribute attribute = new CustomAttribute(
                    parent.Module.ImportReference(typeof(MethodImplAttribute).GetConstructor(new Type[] { typeof(MethodImplOptions) }))
                );
                attribute.ConstructorArguments.Add(attributeArgument);
                method.CustomAttributes.Add(attribute);
            }
            if (Flags.HasFlag(AccessMethod.VARARGS))
            {
                CustomAttribute attribute = new CustomAttribute(
                    parent.Module.ImportReference(typeof(ParamArrayAttribute).GetConstructor(new Type[] { }))
                );
                method.CustomAttributes.Add(attribute);
            }

            yield return method;
            Annotation.Apply(Attributes, method, parent.Module.Assembly);
            yield return method;
            //if (code != null) method.Body = new JavaMethodBody(code).ConvertToNet(method);
            /*JavaMethodBody body = new JavaMethodBody(code);
            foreach (var instruction in body.GetInstructions())
            {
                switch (instruction.OpCode.Key)
                {
                    case Code.OpCodes.Names.INVOKEDYNAMIC:
                    case Code.OpCodes.Names.INVOKEINTERFACE:
                    case Code.OpCodes.Names.INVOKESPECIAL:
                    case Code.OpCodes.Names.INVOKESTATIC:
                    case Code.OpCodes.Names.INVOKEVIRTUAL:
                        break;
                }
            }*/
            //Attribute.CodeAttribute codeAttribute = (Attribute.CodeAttribute)Attributes.First(v => v is Attribute.CodeAttribute);
        }

        public override string ToString() => Name + Descriptor;
    }
}
