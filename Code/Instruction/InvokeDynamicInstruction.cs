﻿using Java.Net.Data.Attribute;
using Java.Net.Model;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Code.Instruction;

public sealed class InvokeDynamicInstruction : IInstruction<InvokeDynamicInstruction>, IInvokeInterfaceInstruction
{
    public IRefConstant Method { get => (IRefConstant)Handle.Constants[MethodIndex]; set => MethodIndex = Handle.OfConstant(value); }
    [JavaRaw] public ushort MethodIndex { get; set; }
    public byte Count { get => 0; set { } }
    public override object[] IndexOperand => new object[] { MethodIndex, Count, 0 };
    public override object[] Operand => new object[] { Method, Count, 0 };
}
