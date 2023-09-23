using Java.Net.Code.Instruction;
using Java.Net.Model.Raw.Base;
using System;

namespace Java.Net.Code
{
    public interface IOpCode : IEquatable<IOpCode>
    {
        string Name { get; }
        OpCodes.Names Key { get; }
        byte Index { get; }

        IInstruction Instruction(JavaClass handle, Action<IInstruction>? apply = null);
    }
    public readonly struct OpCode<I> : IOpCode where I : IInstruction<I>
    {
        /*
[+] byte: bipush
[+] const: iinc
[+] count: invokeinterface
[+] npairs1234: lookupswitch
[+] match-offset pairs...: lookupswitch
[+] dimensions: multianewarray
[+] atype: newarray
[+] byte12: sipush
[+] lowbyte1234: tableswitch
[+] highbyte1234: tableswitch
[+] jump offsets...: tableswitch
[+] branchbyte1234: goto_w jsr_w
[+] branchbyte12: goto goto_w if_acmpeq if_acmpne if_icmpeq if_icmpne if_icmplt if_icmpge if_icmpgt if_icmple ifeq ifne iflt ifge ifgt ifle ifnonnull ifnull jsr jsr_w
[+] index: aload astore dload dstore fload fstore iinc iload istore ldc lload lstore ret
[+] indexbyte12: anewarray checkcast getfield getstatic instanceof invokedynamic invokeinterface invokespecial invokestatic invokevirtual ldc_w ldc2_w multianewarray new putfield putstatic
[+] ???wide???
        */
        public string Name => key.ToString();
        public OpCodes.Names Key => key;
        public byte Index => (byte)key;

        private readonly OpCodes.Names key;

        internal OpCode(OpCodes.Names key, string? info = null) => this.key = key;

        public override bool Equals(object? obj) => obj is IOpCode op && Equals(op);
        public bool Equals(IOpCode? obj) => obj?.Key == key;
        public override int GetHashCode() => (byte)key;
        public static bool operator ==(OpCode<I> a, IOpCode b) => a is IOpCode op && op.Equals(b);
        public static bool operator !=(OpCode<I> a, IOpCode b) => !(a == b);
        public override string ToString() => $"{Name} (0x{Index:X2})";

        IInstruction IOpCode.Instruction(JavaClass handle, Action<IInstruction>? apply) => Instruction(handle, apply);
        public I Instruction(JavaClass handle, Action<I>? apply = null) => IInstruction<I>.Create(handle, this, apply);

        public static implicit operator OpCodes.Names(OpCode<I> code) => code.key;

        /*public object[] ReadOperand(JavaByteCodeReader reader, IHandle handle)
        {
            switch (key)
            {
                case OpCodes.Names.BIPUSH: return new object[] { reader.ReadByte() };
                case OpCodes.Names.IINC: return new object[] { reader.ReadByte(), unchecked(reader.ReadByte()) };

                case OpCodes.Names.INVOKEINTERFACE: return new object[] { handle.Handle.Constants[reader.ReadUShort()], reader.ReadByte(), 0 };
                case OpCodes.Names.INVOKEDYNAMIC: return new object[] { handle.Handle.Constants[reader.ReadUShort()], 0, 0 };

                case OpCodes.Names.LOOKUPSWITCH:
                    {
                        long count = (4 - (reader.Position % 4)) % 4;
                        for (int i = 0; i < count; i++)
                                Console.WriteLine("LI:" + reader.ReadByte());
                        int _default = reader.ReadInt();
                        int npairs = reader.ReadInt();
                        (int match, int offset)[] pairs = new (int match, int offset)[npairs];
                        for (int i = 0; i < npairs; i++) pairs[i] = (reader.ReadInt(), reader.ReadInt());
                        return new object[] { _default, npairs, pairs };
                    }
                case OpCodes.Names.TABLESWITCH:
                    {
                        long count = (4 - (reader.Position % 4)) % 4;
                        for (int i = 0; i < count; i++)
                                Console.WriteLine("TI:" + reader.ReadByte());
                        int _default = reader.ReadInt();
                        int low = reader.ReadInt();
                        int high = reader.ReadInt();


                        int noffsets = high - low + 1;
                        int[] offsets = new int[noffsets];
                        for (int i = 0; i < noffsets; i++) offsets[i] = reader.ReadInt();
                        return new object[] { _default, low, high, offsets };
                    }
                case OpCodes.Names.MULTIANEWARRAY: return new object[] { handle.Handle.Constants[reader.ReadUShort()], reader.ReadByte() };
                case OpCodes.Names.NEWARRAY: return new object[] { reader.ReadByte() };
                case OpCodes.Names.SIPUSH: return new object[] { reader.ReadUShort() };
                case OpCodes.Names.GOTO_W:
                case OpCodes.Names.JSR_W: return new object[] { reader.ReadInt() };

                case OpCodes.Names.GOTO:
                case OpCodes.Names.IF_ACMPEQ:
                case OpCodes.Names.IF_ACMPNE:
                case OpCodes.Names.IF_ICMPEQ:
                case OpCodes.Names.IF_ICMPNE:
                case OpCodes.Names.IF_ICMPLT:
                case OpCodes.Names.IF_ICMPGE:
                case OpCodes.Names.IF_ICMPGT:
                case OpCodes.Names.IF_ICMPLE:
                case OpCodes.Names.IFEQ:
                case OpCodes.Names.IFNE:
                case OpCodes.Names.IFLT:
                case OpCodes.Names.IFGE:
                case OpCodes.Names.IFGT:
                case OpCodes.Names.IFLE:
                case OpCodes.Names.IFNONNULL:
                case OpCodes.Names.IFNULL:
                case OpCodes.Names.JSR: return new object[] { reader.ReadShort() };

                case OpCodes.Names.ALOAD:
                case OpCodes.Names.ASTORE:
                case OpCodes.Names.DLOAD:
                case OpCodes.Names.DSTORE:
                case OpCodes.Names.FLOAD:
                case OpCodes.Names.FSTORE:
                case OpCodes.Names.ILOAD:
                case OpCodes.Names.ISTORE:
                case OpCodes.Names.LLOAD:
                case OpCodes.Names.LSTORE:
                case OpCodes.Names.RET: return new object[] { reader.ReadByte() };

                case OpCodes.Names.LDC: return new object[] { handle.Handle.Constants[reader.ReadByte()] };

                case OpCodes.Names.ANEWARRAY:
                case OpCodes.Names.CHECKCAST:
                case OpCodes.Names.GETFIELD:
                case OpCodes.Names.GETSTATIC:
                case OpCodes.Names.INSTANCEOF:
                case OpCodes.Names.INVOKESPECIAL:
                case OpCodes.Names.INVOKESTATIC:
                case OpCodes.Names.INVOKEVIRTUAL:
                case OpCodes.Names.LDC_W:
                case OpCodes.Names.LDC2_W:
                case OpCodes.Names.NEW:
                case OpCodes.Names.PUTFIELD:
                case OpCodes.Names.PUTSTATIC: return new object[] { handle.Handle.Constants[reader.ReadUShort()] };

                case OpCodes.Names.WIDE:
                    {
                        OpCode opCode = reader.ReadOpCode();
                        short index = reader.ReadShort();
                        if (opCode == OpCodes.Names.IINC) return new object[] { opCode, index, reader.ReadShort() };
                        return new object[] { opCode, index };
                    }
            }
            return new object[0];
        }
        public JavaByteCodeWriter WriteOperand(JavaByteCodeWriter writer, object[] values, IHandle handle)
        {
            switch (key)
            {
                case OpCodes.Names.BIPUSH: return writer.WriteByte((byte)values[0]);
                case OpCodes.Names.IINC: return writer.WriteByte((byte)values[0]).WriteByte((byte)unchecked(values[1]));

                case OpCodes.Names.INVOKEINTERFACE: return writer.WriteUShort(handle.Handle.OfConstant((IConstant)values[0])).WriteByte((byte)values[1]);
                case OpCodes.Names.INVOKEDYNAMIC: return new object[] { handle.Handle.Constants[reader.ReadUShort()], 0, 0 };

                case OpCodes.Names.LOOKUPSWITCH:
                    {
                        long count = (4 - (reader.Position % 4)) % 4;
                        for (int i = 0; i < count; i++)
                            Console.WriteLine("LI:" + reader.ReadByte());
                        int _default = reader.ReadInt();
                        int npairs = reader.ReadInt();
                        (int match, int offset)[] pairs = new (int match, int offset)[npairs];
                        for (int i = 0; i < npairs; i++) pairs[i] = (reader.ReadInt(), reader.ReadInt());
                        return new object[] { _default, npairs, pairs };
                    }
                case OpCodes.Names.TABLESWITCH:
                    {
                        long count = (4 - (reader.Position % 4)) % 4;
                        for (int i = 0; i < count; i++)
                            Console.WriteLine("TI:" + reader.ReadByte());
                        int _default = reader.ReadInt();
                        int low = reader.ReadInt();
                        int high = reader.ReadInt();


                        int noffsets = high - low + 1;
                        int[] offsets = new int[noffsets];
                        for (int i = 0; i < noffsets; i++) offsets[i] = reader.ReadInt();
                        return new object[] { _default, low, high, offsets };
                    }
                case OpCodes.Names.MULTIANEWARRAY: return new object[] { handle.Handle.Constants[reader.ReadUShort()], reader.ReadByte() };
                case OpCodes.Names.NEWARRAY: return new object[] { reader.ReadByte() };
                case OpCodes.Names.SIPUSH: return new object[] { reader.ReadUShort() };
                case OpCodes.Names.GOTO_W:
                case OpCodes.Names.JSR_W: return new object[] { reader.ReadInt() };

                case OpCodes.Names.GOTO:
                case OpCodes.Names.IF_ACMPEQ:
                case OpCodes.Names.IF_ACMPNE:
                case OpCodes.Names.IF_ICMPEQ:
                case OpCodes.Names.IF_ICMPNE:
                case OpCodes.Names.IF_ICMPLT:
                case OpCodes.Names.IF_ICMPGE:
                case OpCodes.Names.IF_ICMPGT:
                case OpCodes.Names.IF_ICMPLE:
                case OpCodes.Names.IFEQ:
                case OpCodes.Names.IFNE:
                case OpCodes.Names.IFLT:
                case OpCodes.Names.IFGE:
                case OpCodes.Names.IFGT:
                case OpCodes.Names.IFLE:
                case OpCodes.Names.IFNONNULL:
                case OpCodes.Names.IFNULL:
                case OpCodes.Names.JSR: return new object[] { reader.ReadShort() };

                case OpCodes.Names.ALOAD:
                case OpCodes.Names.ASTORE:
                case OpCodes.Names.DLOAD:
                case OpCodes.Names.DSTORE:
                case OpCodes.Names.FLOAD:
                case OpCodes.Names.FSTORE:
                case OpCodes.Names.ILOAD:
                case OpCodes.Names.ISTORE:
                case OpCodes.Names.LLOAD:
                case OpCodes.Names.LSTORE:
                case OpCodes.Names.RET: return new object[] { reader.ReadByte() };

                case OpCodes.Names.LDC: return new object[] { handle.Handle.Constants[reader.ReadByte()] };

                case OpCodes.Names.ANEWARRAY:
                case OpCodes.Names.CHECKCAST:
                case OpCodes.Names.GETFIELD:
                case OpCodes.Names.GETSTATIC:
                case OpCodes.Names.INSTANCEOF:
                case OpCodes.Names.INVOKESPECIAL:
                case OpCodes.Names.INVOKESTATIC:
                case OpCodes.Names.INVOKEVIRTUAL:
                case OpCodes.Names.LDC_W:
                case OpCodes.Names.LDC2_W:
                case OpCodes.Names.NEW:
                case OpCodes.Names.PUTFIELD:
                case OpCodes.Names.PUTSTATIC: return new object[] { handle.Handle.Constants[reader.ReadUShort()] };

                case OpCodes.Names.WIDE:
                    {
                        OpCode opCode = reader.ReadOpCode();
                        short index = reader.ReadShort();
                        if (opCode == OpCodes.Names.IINC) return new object[] { opCode, index, reader.ReadShort() };
                        return new object[] { opCode, index };
                    }
            }
            return new object[0];
        }*/
    }
}
