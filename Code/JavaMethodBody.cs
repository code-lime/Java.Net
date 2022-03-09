using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Java.Net.Code;
using Java.Net.Model;
using Java.Net.Model.Attribute;
using Mono.Cecil;

namespace Java.Net.Code
{
    public class JavaMethodBody : IHandle
    {
        public JavaMethod Method { get; }
        public JavaClass Handle => Method.Handle;
        public bool HasCode => Method.Attributes.Any(v => v is CodeAttribute);

        private byte[] Code
        {
            get => GetCodeAttribute(false)?.Code;
            set
            {
                if (value == null)
                {
                    Method.Attributes.RemoveAll(v => v is CodeAttribute);
                    return;
                }
                GetCodeAttribute(true).Code = value;
            }
        }
        public IEnumerable<IInstruction> Instructions
        {
            get => ReadInstructions();
            set
            {
                if (value == null)
                {
                    Code = null;
                    return;
                }
                using MemoryStream stream = new MemoryStream();
                JavaByteCodeWriter writer = new JavaByteCodeWriter(stream);
                foreach (var inst in value) inst.Write(writer);
                Code = stream.ToArray();
            }
        }
        private List<IInstruction> ReadInstructions()
        {
            byte[] code = Code;
            if (code == null) return null;
            List<IInstruction> list = new List<IInstruction>();
            using MemoryStream stream = new MemoryStream(code);
            JavaByteCodeReader reader = new JavaByteCodeReader(stream);
            long start_index = reader.Position;
            while (reader.CanReadNext)
            {
                long offset = reader.Position - start_index;
                IInstruction instruction = reader.ReadInstrustion(Handle);
                instruction.Position = (ushort)offset;
                list.Add(instruction);
            }
            return list;
        }
        private CodeAttribute GetCodeAttribute(bool create)
        {
            CodeAttribute attribute = (CodeAttribute)Method.Attributes.FirstOrDefault(v => v is CodeAttribute);
            if (attribute == null && create) Method.Attributes.Add(attribute = CodeAttribute.Create(Handle, dat =>
            {
                dat.MaxStack = ushort.MaxValue;
                dat.MaxLocals = ushort.MaxValue;
                dat.ExceptionTable = new List<CodeAttribute.Exception>();
                dat.Attributes = new List<JavaAttribute>();
            }));
            return attribute;
        }

        public JavaMethodBody(JavaMethod method)
        {
            this.Method = method;
        }
    }
}
