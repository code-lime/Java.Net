using Java.Net.Model;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Java.Net.Code
{
    public readonly struct Instruction : IEquatable<Instruction>
    {
        public OpCode OpCode => opCode;
        public object[] Operand => operand;

        private readonly OpCode opCode;
        private readonly object[] operand;

        internal Instruction(OpCode opCode, object[] operand)
        {
            this.opCode = opCode;
            this.operand = operand;
        }

        public object this[int index]
        {
            get => operand[index];
            set => operand[index] = value;
        }

        public override bool Equals(object obj) => obj is Instruction op && Equals(op);
        public bool Equals(Instruction obj) => opCode == obj.opCode && Enumerable.SequenceEqual(operand, obj.operand);
        public override int GetHashCode() => HashCode.Combine(opCode, operand);
        public static bool operator ==(Instruction a, Instruction b) => a is Instruction op && op.Equals(b);
        public static bool operator !=(Instruction a, Instruction b) => !(a == b);
        public override string ToString() => $"I: {opCode} [{string.Join(",", operand)}]";
    }
}
