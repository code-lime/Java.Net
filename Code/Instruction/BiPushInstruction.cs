﻿using Java.Net.Data.Attribute;

namespace Java.Net.Code.Instruction;

public sealed class BiPushInstruction : IInstruction<BiPushInstruction>
{
    [JavaRaw] public byte Value { get; set; }
    public override object[] IndexOperand => new object[] { Value };
    public override object[] Operand => new object[] { Value };
}
