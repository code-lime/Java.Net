using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Java.Net.Binary;
using Java.Net.Code.Instruction;
using Java.Net.Model;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Base;
using Mono.Cecil;

namespace Java.Net.Code
{
    public class JavaMethodBody : IHandle
    {
        public JavaMethod Method { get; }
        public JavaClass Handle => Method.Handle;
        public bool HasCode => Method.Annotations.Any(v => v is CodeAnnotation);

        private byte[] Code
        {
            get => GetCodeAttribute(false)?.Code;
            set
            {
                if (value == null)
                {
                    Method.Annotations.RemoveAll(v => v is CodeAnnotation);
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
        private CodeAnnotation GetCodeAttribute(bool create)
        {
            CodeAnnotation attribute = (CodeAnnotation)Method.Annotations.FirstOrDefault(v => v is CodeAnnotation);
            if (attribute == null && create) Method.Annotations.Add(attribute = CodeAnnotation.Create(Handle, dat =>
            {
                dat.MaxStack = ushort.MaxValue;
                dat.MaxLocals = ushort.MaxValue;
                dat.ExceptionTable = new List<CodeAnnotation.Exception>();
                dat.Attributes = new List<IAnnotation>();
            }));
            return attribute;
        }

        public JavaMethodBody(JavaMethod method)
        {
            this.Method = method;
        }
    }
}
