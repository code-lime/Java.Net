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
        public CodeAttribute code;
        public JavaClass Handle => code.Handle;

        private IEnumerable<IInstruction> instructions = null;
        public IEnumerable<IInstruction> Instructions
        {
            get => new List<IInstruction>(instructions ??= ReadInstructions().ToList());
            set
            {
                using MemoryStream stream = new MemoryStream();
                JavaByteCodeWriter writer = new JavaByteCodeWriter(stream);
                foreach (var inst in value)
                    inst.Write(writer);
                code.Code = stream.ToArray();
            }
        }
        private IEnumerable<IInstruction> ReadInstructions()
        {
            using MemoryStream stream = new MemoryStream(code.Code);
            JavaByteCodeReader reader = new JavaByteCodeReader(stream);
            long start_index = reader.Position;
            while (reader.CanReadNext)
            {
                long offset = reader.Position - start_index;
                IInstruction instruction = reader.ReadInstrustion(Handle);
                instruction.Position = (ushort)offset;
                yield return instruction;
            }
        }

        public JavaMethodBody(CodeAttribute code)
        {
            this.code = code;
            //Console.WriteLine(string.Join(", ", code.Code.Select(v => "0x" + v.ToString("X2"))));
        }
    }
}
