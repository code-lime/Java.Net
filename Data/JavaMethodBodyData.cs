using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Net.Data.Signature;
using Java.Net.Flags;
using Java.Net.Model;
using Java.Net.Model.Attribute;
using Mono.Cecil.Cil;

namespace Java.Net.Data
{
    public class JavaMethodBodyData
    {
        public ushort MaxStack { get; set; }
        public ushort MaxLocals { get; set; }
        public byte[] Code { get; set; }
        public List<Exception> ExceptionTable { get; set; }
        public List<JavaAttribute> Attributes { get; set; }

        public sealed class Exception : IJava<Exception>
        {
            public ushort StartPC { get; set; }
            public ushort EndPC { get; set; }
            public ushort HandlerPC { get; set; }
            public SimpleClassTypeSignature CatchType { get; set; }
        }

        public CodeAttribute ToJava()
        {
            CodeAttribute attribute = new CodeAttribute()
            {
                MaxLocals = MaxLocals,
                MaxStack = MaxStack
            };
            return attribute;
            //MethodBody
        }
    }
}
