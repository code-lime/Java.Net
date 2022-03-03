using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Java.Net.Code;
using System.Text;

using JOpCode = Java.Net.Code.OpCode;
using JOpCodes = Java.Net.Code.OpCodes;
using JInstruction = Java.Net.Code.Instruction;
using NOpCode = Mono.Cecil.Cil.OpCode;
using NOpCodes = Mono.Cecil.Cil.OpCodes;
using NInstruction = Mono.Cecil.Cil.Instruction;
using System.Linq;
using Java.Net.Model;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Java.Net.Net
{
    public static class InstructionConverter
    {
        private static readonly Dictionary<JOpCodes.Names, (Func<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> create, Action<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> init)> convert = new Dictionary<JOpCodes.Names, (Func<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> create, Action<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> init)>();

        private interface ICreate
        {
            NInstruction[] Create(JInstruction instruction, Mono.Cecil.MethodDefinition method);
        }
        private interface IInit
        {
            void Init(JInstruction instruction, Mono.Cecil.MethodDefinition method, NInstruction[] instructions);
        }
        private interface IFunc : ICreate, IInit
        {

        }

        /*private abstract class IFunc
        {
            public abstract NInstruction[] Create(JInstruction instruction, Mono.Cecil.MethodDefinition method);
            public abstract void Init(JInstruction instruction, Mono.Cecil.MethodDefinition method, NInstruction[] instructions);
        }
        private class SingleFunc : IFunc
        {
            public NInstruction[] Instructions { get; }
            public SingleFunc(params NOpCode[] instructions) : this(instructions.Select(v => NInstruction.Create(v)).ToArray()) { }
            public SingleFunc(params NInstruction[] instructions) => Instructions = instructions;
            public override NInstruction[] Create(JInstruction instruction, MethodDefinition method) => Instructions;
            public override void Init(JInstruction instruction, Mono.Cecil.MethodDefinition method, NInstruction[] instructions) { }
        }*/

        private static void Of(JOpCodes.Names input, params NOpCode[] output) => Of(input, output.Select(v => NInstruction.Create(v)).ToArray());
        private static void Of(JOpCodes.Names input, params NInstruction[] output) => Of(input, (a, b) => output);
        private static void Of(JOpCodes.Names input, Func<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> func) => convert[input] = (func, (a,b,c) => { });
        private static void Of(JOpCodes.Names input, Func<JInstruction, Mono.Cecil.MethodDefinition, NInstruction> func) => convert[input] = ((a, b) => new NInstruction[] { func.Invoke(a, b) }, (a, b, c) => { });
        private static void Of(JOpCodes.Names input, Func<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> create, Action<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> init) => convert[input] = (create, init);
        private static void Of(JOpCodes.Names input, Func<JInstruction, Mono.Cecil.MethodDefinition, NInstruction> create, Action<JInstruction, Mono.Cecil.MethodDefinition, NInstruction> init) => convert[input] = ((a, b) => new NInstruction[] { create.Invoke(a, b) }, (a, b, c) => init.Invoke(a, b, c[0]));

        //private static NInstruction[] ConvertToNet(this JInstruction javaInstruction, Mono.Cecil.MethodDefinition method) => convert[javaInstruction.OpCode].Invoke(javaInstruction, method);

        private static NInstruction Create(NOpCode opCode, object obj = null)
        {
            return (NInstruction)Activator.CreateInstance(
                typeof(NInstruction),
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new object[] { opCode, obj },
                null
            );
        }

        static InstructionConverter()
        {
            Of(JOpCodes.NOP, NOpCodes.Nop);
            Of(JOpCodes.ACONST_NULL, NOpCodes.Ldnull);
            Of(JOpCodes.ICONST_M1, NOpCodes.Ldc_I4_M1);
            Of(JOpCodes.ICONST_0, NOpCodes.Ldc_I4_0);
            Of(JOpCodes.ICONST_1, NOpCodes.Ldc_I4_1);
            Of(JOpCodes.ICONST_2, NOpCodes.Ldc_I4_2);
            Of(JOpCodes.ICONST_3, NOpCodes.Ldc_I4_3);
            Of(JOpCodes.ICONST_4, NOpCodes.Ldc_I4_4);
            Of(JOpCodes.ICONST_5, NOpCodes.Ldc_I4_5);
            Of(JOpCodes.LCONST_0, NInstruction.Create(NOpCodes.Ldc_I8, 0L));
            Of(JOpCodes.LCONST_1, NInstruction.Create(NOpCodes.Ldc_I8, 1L));
            Of(JOpCodes.FCONST_0, NInstruction.Create(NOpCodes.Ldc_R4, 0F));
            Of(JOpCodes.FCONST_1, NInstruction.Create(NOpCodes.Ldc_R4, 1F));
            Of(JOpCodes.FCONST_2, NInstruction.Create(NOpCodes.Ldc_R4, 2F));
            Of(JOpCodes.DCONST_0, NInstruction.Create(NOpCodes.Ldc_R8, 0D));
            Of(JOpCodes.DCONST_1, NInstruction.Create(NOpCodes.Ldc_R8, 1D));
            
            Of(JOpCodes.BIPUSH, (j,m) => NInstruction.Create(NOpCodes.Ldc_I4, (int)j[0])); //TODO START
            Of(JOpCodes.SIPUSH, (j, m) => NInstruction.Create(NOpCodes.Ldc_I4, (int)j[0]));
            /*
    public class IntegerConstant : IConstant<IntegerConstant>
    public class FloatConstant : IConstant<FloatConstant>
    public class LongConstant : IConstant<LongConstant>
    public class DoubleConstant : IConstant<DoubleConstant>
            */
            Of(JOpCodes.LDC, (j,m) =>
            {
                IConstant constant = (IConstant)j[0];
                if (constant is IntegerConstant c0) return NInstruction.Create(NOpCodes.Ldc_I4, c0.Value);
                else if (constant is DoubleConstant c1) return NInstruction.Create(NOpCodes.Ldc_R8, c1.Value);
                else if (constant is FloatConstant c2) return NInstruction.Create(NOpCodes.Ldc_R4, c2.Value);
                else if (constant is LongConstant c3) return NInstruction.Create(NOpCodes.Ldc_I8, c3.Value);
                else if (constant is StringConstant c4) return NInstruction.Create(NOpCodes.Ldstr, c4.String);
                throw new ArrayTypeMismatchException($"Constant type '{constant.Tag}' not supported!");
            });
            Of(JOpCodes.LDC_W, (j, m) =>
            {
                IConstant constant = (IConstant)j[0];
                if (constant is IntegerConstant c0) return NInstruction.Create(NOpCodes.Ldc_I4, c0.Value);
                else if (constant is DoubleConstant c1) return NInstruction.Create(NOpCodes.Ldc_R8, c1.Value);
                else if (constant is FloatConstant c2) return NInstruction.Create(NOpCodes.Ldc_R4, c2.Value);
                else if (constant is LongConstant c3) return NInstruction.Create(NOpCodes.Ldc_I8, c3.Value);
                else if (constant is StringConstant c4) return NInstruction.Create(NOpCodes.Ldstr, c4.String);
                throw new ArrayTypeMismatchException($"Constant type '{constant.Tag}' not supported!");
            });
            Of(JOpCodes.LDC2_W, (j, m) =>
            {
                IConstant constant = (IConstant)j[0];
                if (constant is IntegerConstant c0) return NInstruction.Create(NOpCodes.Ldc_I4, c0.Value);
                else if (constant is DoubleConstant c1) return NInstruction.Create(NOpCodes.Ldc_R8, c1.Value);
                else if (constant is FloatConstant c2) return NInstruction.Create(NOpCodes.Ldc_R4, c2.Value);
                else if (constant is LongConstant c3) return NInstruction.Create(NOpCodes.Ldc_I8, c3.Value);
                else if (constant is StringConstant c4) return NInstruction.Create(NOpCodes.Ldstr, c4.String);
                throw new ArrayTypeMismatchException($"Constant type '{constant.Tag}' not supported!");
            });
            Of(JOpCodes.ILOAD, (j, m) => NInstruction.Create(NOpCodes.Ldloc, (byte)j[0])); 
            Of(JOpCodes.LLOAD, (j, m) => NInstruction.Create(NOpCodes.Ldloc, (byte)j[0])); 
            Of(JOpCodes.FLOAD, (j, m) => NInstruction.Create(NOpCodes.Ldloc, (byte)j[0])); 
            Of(JOpCodes.DLOAD, (j, m) => NInstruction.Create(NOpCodes.Ldloc, (byte)j[0])); 
            Of(JOpCodes.ALOAD, (j, m) => NInstruction.Create(NOpCodes.Ldloc, (byte)j[0])); //*/?TODO END

            Of(JOpCodes.ILOAD_0, NOpCodes.Ldloc_0);
            Of(JOpCodes.ILOAD_1, NOpCodes.Ldloc_1);
            Of(JOpCodes.ILOAD_2, NOpCodes.Ldloc_2);
            Of(JOpCodes.ILOAD_3, NOpCodes.Ldloc_3);
            Of(JOpCodes.LLOAD_0, NOpCodes.Ldloc_0);
            Of(JOpCodes.LLOAD_1, NOpCodes.Ldloc_1);
            Of(JOpCodes.LLOAD_2, NOpCodes.Ldloc_2);
            Of(JOpCodes.LLOAD_3, NOpCodes.Ldloc_3);
            Of(JOpCodes.FLOAD_0, NOpCodes.Ldloc_0);
            Of(JOpCodes.FLOAD_1, NOpCodes.Ldloc_1);
            Of(JOpCodes.FLOAD_2, NOpCodes.Ldloc_2);
            Of(JOpCodes.FLOAD_3, NOpCodes.Ldloc_3);
            Of(JOpCodes.DLOAD_0, NOpCodes.Ldloc_0);
            Of(JOpCodes.DLOAD_1, NOpCodes.Ldloc_1);
            Of(JOpCodes.DLOAD_2, NOpCodes.Ldloc_2);
            Of(JOpCodes.DLOAD_3, NOpCodes.Ldloc_3);
            Of(JOpCodes.ALOAD_0, NOpCodes.Ldloc_0);
            Of(JOpCodes.ALOAD_1, NOpCodes.Ldloc_1);
            Of(JOpCodes.ALOAD_2, NOpCodes.Ldloc_2);
            Of(JOpCodes.ALOAD_3, NOpCodes.Ldloc_3);

            Of(JOpCodes.IALOAD, NOpCodes.Ldelem_I4);
            Of(JOpCodes.LALOAD, NOpCodes.Ldelem_I8);
            Of(JOpCodes.FALOAD, NOpCodes.Ldelem_R4);
            Of(JOpCodes.DALOAD, NOpCodes.Ldelem_R8);
            Of(JOpCodes.AALOAD, NOpCodes.Ldelem_Ref); //?TODO
            Of(JOpCodes.BALOAD, NOpCodes.Ldelem_I1);
            Of(JOpCodes.CALOAD, (j,m) => NInstruction.Create(NOpCodes.Ldelem_Any, m.Module.ImportReference(typeof(char))));
            Of(JOpCodes.SALOAD, NOpCodes.Ldelem_I2);

            Of(JOpCodes.ISTORE, (j,m) => Create(NOpCodes.Stloc, j[0])); //?TODO START
            Of(JOpCodes.LSTORE, (j, m) => Create(NOpCodes.Stloc, j[0]));
            Of(JOpCodes.FSTORE, (j, m) => Create(NOpCodes.Stloc, j[0]));
            Of(JOpCodes.DSTORE, (j, m) => Create(NOpCodes.Stloc, j[0]));
            Of(JOpCodes.ASTORE, (j, m) => Create(NOpCodes.Stloc, j[0])); //*/?TODO END

            Of(JOpCodes.ISTORE_0, NOpCodes.Stloc_0);
            Of(JOpCodes.ISTORE_1, NOpCodes.Stloc_1);
            Of(JOpCodes.ISTORE_2, NOpCodes.Stloc_2);
            Of(JOpCodes.ISTORE_3, NOpCodes.Stloc_3);
            Of(JOpCodes.LSTORE_0, NOpCodes.Stloc_0);
            Of(JOpCodes.LSTORE_1, NOpCodes.Stloc_1);
            Of(JOpCodes.LSTORE_2, NOpCodes.Stloc_2);
            Of(JOpCodes.LSTORE_3, NOpCodes.Stloc_3);
            Of(JOpCodes.FSTORE_0, NOpCodes.Stloc_0);
            Of(JOpCodes.FSTORE_1, NOpCodes.Stloc_1);
            Of(JOpCodes.FSTORE_2, NOpCodes.Stloc_2);
            Of(JOpCodes.FSTORE_3, NOpCodes.Stloc_3);
            Of(JOpCodes.DSTORE_0, NOpCodes.Stloc_0);
            Of(JOpCodes.DSTORE_1, NOpCodes.Stloc_1);
            Of(JOpCodes.DSTORE_2, NOpCodes.Stloc_2);
            Of(JOpCodes.DSTORE_3, NOpCodes.Stloc_3);
            Of(JOpCodes.ASTORE_0, NOpCodes.Stloc_0);
            Of(JOpCodes.ASTORE_1, NOpCodes.Stloc_1);
            Of(JOpCodes.ASTORE_2, NOpCodes.Stloc_2);
            Of(JOpCodes.ASTORE_3, NOpCodes.Stloc_3);

            Of(JOpCodes.IASTORE, NOpCodes.Stelem_I4);
            Of(JOpCodes.LASTORE, NOpCodes.Stelem_I8);
            Of(JOpCodes.FASTORE, NOpCodes.Stelem_R4);
            Of(JOpCodes.DASTORE, NOpCodes.Stelem_R8);
            Of(JOpCodes.AASTORE, NOpCodes.Stelem_Ref); //?TODO
            Of(JOpCodes.BASTORE, NOpCodes.Stelem_I1);
            Of(JOpCodes.CASTORE, (j, m) => NInstruction.Create(NOpCodes.Stelem_Any, m.Module.ImportReference(typeof(char)))); //?TODO
            Of(JOpCodes.SASTORE, NOpCodes.Stelem_I2);

            Of(JOpCodes.POP, NOpCodes.Pop);
            /*{
                NInstruction goto_pop = NInstruction.Create(NOpCodes.Pop);
                NInstruction goto_last = NInstruction.Create(NOpCodes.Nop);
                Of(JOpCodes.POP2,
                    NInstruction.Create(NOpCodes.Pop),
                    NInstruction.Create(NOpCodes.Dup),
                    NInstruction.Create(NOpCodes.Isinst), //TODO typeof(double)
                    NInstruction.Create(NOpCodes.Brtrue, goto_pop),
                    NInstruction.Create(NOpCodes.Dup),
                    NInstruction.Create(NOpCodes.Isinst), //TODO typeof(long)
                    NInstruction.Create(NOpCodes.Brfalse, goto_last),
                    goto_pop,
                    goto_last
                );
            }*/

            Of(JOpCodes.DUP, NOpCodes.Dup);

            /* //TODO START
            Of(JOpCodes.DUP_X1, NOpCodes.Nop); 
            Of(JOpCodes.DUP_X2, NOpCodes.Nop); 
            Of(JOpCodes.DUP2, NOpCodes.Nop); 
            Of(JOpCodes.DUP2_X1, NOpCodes.Nop); 
            Of(JOpCodes.DUP2_X2, NOpCodes.Nop); 
            Of(JOpCodes.SWAP, NOpCodes.Nop); //*///TODO END
            Of(JOpCodes.IADD, NOpCodes.Add);
            Of(JOpCodes.LADD, NOpCodes.Add);
            Of(JOpCodes.FADD, NOpCodes.Add);
            Of(JOpCodes.DADD, NOpCodes.Add);
            Of(JOpCodes.ISUB, NOpCodes.Sub);
            Of(JOpCodes.LSUB, NOpCodes.Sub);
            Of(JOpCodes.FSUB, NOpCodes.Sub);
            Of(JOpCodes.DSUB, NOpCodes.Sub);
            Of(JOpCodes.IMUL, NOpCodes.Mul);
            Of(JOpCodes.LMUL, NOpCodes.Mul);
            Of(JOpCodes.FMUL, NOpCodes.Mul);
            Of(JOpCodes.DMUL, NOpCodes.Mul);
            Of(JOpCodes.IDIV, NOpCodes.Div);
            Of(JOpCodes.LDIV, NOpCodes.Div);
            Of(JOpCodes.FDIV, NOpCodes.Div);
            Of(JOpCodes.DDIV, NOpCodes.Div);
            Of(JOpCodes.IREM, NOpCodes.Rem);
            Of(JOpCodes.LREM, NOpCodes.Rem);
            Of(JOpCodes.FREM, NOpCodes.Rem);
            Of(JOpCodes.DREM, NOpCodes.Rem);
            Of(JOpCodes.INEG, NOpCodes.Neg);
            Of(JOpCodes.LNEG, NOpCodes.Neg);
            Of(JOpCodes.FNEG, NOpCodes.Neg);
            Of(JOpCodes.DNEG, NOpCodes.Neg);
            Of(JOpCodes.ISHL, NOpCodes.Shl);
            Of(JOpCodes.LSHL, NOpCodes.Shl);
            Of(JOpCodes.ISHR, NOpCodes.Shr);
            Of(JOpCodes.LSHR, NOpCodes.Shr);
            Of(JOpCodes.IUSHR, NOpCodes.Shr_Un);
            Of(JOpCodes.LUSHR, NOpCodes.Shr_Un);
            Of(JOpCodes.IAND, NOpCodes.And);
            Of(JOpCodes.LAND, NOpCodes.And);
            Of(JOpCodes.IOR, NOpCodes.Or);
            Of(JOpCodes.LOR, NOpCodes.Or);
            Of(JOpCodes.IXOR, NOpCodes.Xor);
            Of(JOpCodes.LXOR, NOpCodes.Xor);
            Of(JOpCodes.IINC, (j, m) => {
                return new NInstruction[]
                {
                    NInstruction.Create(NOpCodes.Ldloc, (byte)j[0]),
                    NInstruction.Create(NOpCodes.Ldc_I4, (int)j[1]),
                    NInstruction.Create(NOpCodes.Add),
                    NInstruction.Create(NOpCodes.Stloc, (byte)j[0]),
                };
            }); //?TODO
            Of(JOpCodes.I2L, NOpCodes.Conv_I8);
            Of(JOpCodes.I2F, NOpCodes.Conv_R4);
            Of(JOpCodes.I2D, NOpCodes.Conv_R8);
            Of(JOpCodes.L2I, NOpCodes.Conv_I4);
            Of(JOpCodes.L2F, NOpCodes.Conv_R4);
            Of(JOpCodes.L2D, NOpCodes.Conv_R8);
            Of(JOpCodes.F2I, NOpCodes.Conv_I4);
            Of(JOpCodes.F2L, NOpCodes.Conv_I8);
            Of(JOpCodes.F2D, NOpCodes.Conv_R8);
            Of(JOpCodes.D2I, NOpCodes.Conv_I4);
            Of(JOpCodes.D2L, NOpCodes.Conv_I8);
            Of(JOpCodes.D2F, NOpCodes.Conv_R4);
            Of(JOpCodes.I2B, NOpCodes.Conv_I1);
            Of(JOpCodes.I2C, NOpCodes.Conv_U2); //?TODO typeof(char)
            Of(JOpCodes.I2S, NOpCodes.Conv_I2);
            Of(JOpCodes.LCMP, NOpCodes.Cgt);

            Of(JOpCodes.FCMPL, NOpCodes.Cgt); //TODO START
            Of(JOpCodes.FCMPG, NOpCodes.Cgt); 
            Of(JOpCodes.DCMPL, NOpCodes.Cgt);  
            Of(JOpCodes.DCMPG, NOpCodes.Cgt); 
            Of(
                JOpCodes.IFEQ, 
                (j, m) => Create(NOpCodes.Brtrue),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IFNE,
                (j, m) => Create(NOpCodes.Brtrue),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IFLT,
                (j, m) => Create(NOpCodes.Brtrue),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IFGE,
                (j, m) => Create(NOpCodes.Brtrue),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IFGT,
                (j, m) => Create(NOpCodes.Brtrue),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IFLE,
                (j, m) => Create(NOpCodes.Brtrue),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 

            Of(
                JOpCodes.IF_ICMPEQ,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IF_ICMPNE,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IF_ICMPLT,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            );  
            Of(
                JOpCodes.IF_ICMPGE,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IF_ICMPGT,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IF_ICMPLE,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IF_ACMPEQ,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            ); 
            Of(
                JOpCodes.IF_ACMPNE,
                (j, m) => Create(NOpCodes.Beq),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            );

            Of(
                JOpCodes.GOTO,
                (j, m) => Create(NOpCodes.Br),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            );
            Of(
                JOpCodes.JSR,
                (j, m) => Create(NOpCodes.Br),
                (j, m, l) => l.Operand = m.Body.Instructions.IndexOf(l) + 1
            );
            Of(JOpCodes.RET, NOpCodes.Ret); 
            /*Of(JOpCodes.TABLESWITCH, NOpCodes.Nop);  
            Of(JOpCodes.LOOKUPSWITCH, NOpCodes.Nop); //*///TODO END

            Of(JOpCodes.IRETURN, NOpCodes.Ret);
            Of(JOpCodes.LRETURN, NOpCodes.Ret);
            Of(JOpCodes.FRETURN, NOpCodes.Ret);
            Of(JOpCodes.DRETURN, NOpCodes.Ret);
            Of(JOpCodes.ARETURN, NOpCodes.Ret);
            Of(JOpCodes.RETURN, NOpCodes.Ret);
            Of(JOpCodes.GETSTATIC, (j,m) =>
                NInstruction.Create(NOpCodes.Ldsfld, FieldDescriptor.Parse((FieldrefConstant)j[0], m, m.Module.Assembly))); //TODO START ??? FieldDescriptor.Parse((FieldrefConstant)j[0]))
            Of(JOpCodes.PUTSTATIC, (j,m) =>
                NInstruction.Create(NOpCodes.Stsfld, FieldDescriptor.Parse((FieldrefConstant)j[0], m, m.Module.Assembly)));
            Of(JOpCodes.GETFIELD, (j, m) =>
                NInstruction.Create(NOpCodes.Ldfld, FieldDescriptor.Parse((FieldrefConstant)j[0], m, m.Module.Assembly))); //TODO START ??? FieldDescriptor.Parse((FieldrefConstant)j[0]))
            Of(JOpCodes.PUTFIELD, (j, m) =>
                NInstruction.Create(NOpCodes.Stfld, FieldDescriptor.Parse((FieldrefConstant)j[0], m, m.Module.Assembly)));
            Of(JOpCodes.INVOKEVIRTUAL, (j,m) =>
                NInstruction.Create(NOpCodes.Callvirt, MethodDescriptor.Parse((IMethodRefConstant)j[0], m, m.Module.Assembly)));
            Of(JOpCodes.INVOKESPECIAL, (j, m) =>
                NInstruction.Create(NOpCodes.Call, MethodDescriptor.Parse((IMethodRefConstant)j[0], m, m.Module.Assembly)));
            Of(JOpCodes.INVOKESTATIC, (j, m) =>
                NInstruction.Create(NOpCodes.Call, MethodDescriptor.Parse((IMethodRefConstant)j[0], m, m.Module.Assembly)));
            Of(JOpCodes.INVOKEINTERFACE, (j, m) =>
                NInstruction.Create(NOpCodes.Callvirt, MethodDescriptor.Parse((IMethodRefConstant)j[0], m, m.Module.Assembly)));
            Of(JOpCodes.INVOKEDYNAMIC, (j, m) =>
                NInstruction.Create(NOpCodes.Callvirt, MethodDescriptor.Parse((InvokeDynamicConstant)j[0], m, m.Module.Assembly)));
            Of(JOpCodes.NEW, (j, m) => new NInstruction[0]
                /*NInstruction.Create(NOpCodes.Newobj, MethodDescriptor.Parse((MethodrefConstant)j[0], m, m.Module.Assembly))*/);
            Of(JOpCodes.NEWARRAY, (j, m) =>
                NInstruction.Create(NOpCodes.Newarr, (byte)j[0] switch
                {
                    4 => m.Module.TypeSystem.Boolean,
                    5 => m.Module.TypeSystem.Char,
                    6 => m.Module.TypeSystem.Single,
                    7 => m.Module.TypeSystem.Double,
                    8 => m.Module.TypeSystem.Byte,
                    9 => m.Module.TypeSystem.Int16,
                    10 => m.Module.TypeSystem.Int32,
                    11 => m.Module.TypeSystem.Int64,
                    _ => throw new ArrayTypeMismatchException($"Type '{(byte)j[0]}' not supported!")
                }));
            Of(JOpCodes.ANEWARRAY, (j, m) =>
                NInstruction.Create(NOpCodes.Newarr, TypeDescriptor.Read($"L{((ClassConstant)j[0]).Name};", m, m.Module.Assembly))); //*///TODO END
            Of(JOpCodes.ARRAYLENGTH, NOpCodes.Ldlen);
            Of(JOpCodes.ATHROW, NOpCodes.Throw);

            Of(JOpCodes.CHECKCAST, (j, m) =>
                NInstruction.Create(NOpCodes.Isinst, TypeDescriptor.Read($"L{((ClassConstant)j[0]).Name};", m, m.Module.Assembly)));
            Of(JOpCodes.INSTANCEOF, (j, m) =>
                new NInstruction[]
                {
                    NInstruction.Create(NOpCodes.Isinst, TypeDescriptor.Read($"L{((ClassConstant)j[0]).Name};", m, m.Module.Assembly)),
                    NInstruction.Create(NOpCodes.Ldnull),
                    NInstruction.Create(NOpCodes.Cgt_Un),
                }
            );
            Of(JOpCodes.MONITORENTER, (j,m) => NInstruction.Create(NOpCodes.Call, m.Module.ImportReference(typeof(System.Threading.Monitor).GetMethod("Enter"))));
            Of(JOpCodes.MONITOREXIT, (j, m) => NInstruction.Create(NOpCodes.Call, m.Module.ImportReference(typeof(System.Threading.Monitor).GetMethod("Exit"))));
            Of(JOpCodes.WIDE, (j,m) =>
                NInstruction.Create(NOpCodes.Nop));
            Of(JOpCodes.MULTIANEWARRAY, (j, m) =>
                NInstruction.Create(NOpCodes.Call, new Mono.Cecil.ArrayType(TypeDescriptor.Read($"L{((ClassConstant)j[0]).Name};", m, m.Module.Assembly), (byte)j[1])));
            Of(
                JOpCodes.IFNULL,
                (j, m) => NInstruction.Create(NOpCodes.Brtrue),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            );
            Of(
                JOpCodes.IFNONNULL,
                (j, m) => NInstruction.Create(NOpCodes.Brfalse),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            );

            Of(
                JOpCodes.GOTO,
                (j, m) => NInstruction.Create(NOpCodes.Br),
                (j, m, l) => l.Operand = m.Body.Instructions[m.Body.Instructions.IndexOf(l) + (byte)j[0]]
            );
            Of(
                JOpCodes.JSR,
                (j, m) => NInstruction.Create(NOpCodes.Br),
                (j, m, l) => l.Operand = m.Body.Instructions.IndexOf(l) + 1
            );//*///TODO END
        }

        private static Mono.Cecil.MemberReference DescriptorToNet(ref string descriptor, Mono.Cecil.AssemblyDefinition assembly)
        {
            /*
B	byte	signed byte
C	char	Unicode character code point in the Basic Multilingual Plane, encoded with UTF-16
D	double	double-precision floating-point value
F	float	single-precision floating-point value
I	int	integer
J	long	long integer
L ClassName ;	reference	an instance of class ClassName
S	short	signed short
Z	boolean	true or false
[	reference	one array dimension
             */
            char ch = descriptor[0];
            descriptor = descriptor[1..];
            switch (ch)
            {
                case '(':
                    {
                        int next = descriptor.IndexOf(')');
                        string args = descriptor[..next];
                        //List<Mono.Cecil.A>
                        descriptor = descriptor[(next + 1)..];
                        //new Mono.Cecil.MethodReference()
                        return null;
                    }
                case 'B': return assembly.MainModule.TypeSystem.Byte;
                case 'C': return assembly.MainModule.TypeSystem.Char;
                case 'D': return assembly.MainModule.TypeSystem.Double;
                case 'F': return assembly.MainModule.TypeSystem.Single;
                case 'I': return assembly.MainModule.TypeSystem.Int32;
                case 'J': return assembly.MainModule.TypeSystem.Int64;
                case 'L':
                    // java/lang/String.equals(Ljava/lang/Object;)Z
                    {
                        int next = descriptor.IndexOf(';');
                        string[] full_name = descriptor[..next].Split('/');
                        descriptor = descriptor[(next + 1)..];
                        return new Mono.Cecil.TypeReference(string.Join('.', full_name[..^1]), full_name[^1], assembly.MainModule, null);
                    }
                case 'S': return assembly.MainModule.TypeSystem.Int16;
                case 'Z': return assembly.MainModule.TypeSystem.Boolean;
                case '[': return new Mono.Cecil.ArrayType((Mono.Cecil.TypeReference)DescriptorToNet(ref descriptor, assembly));
            }
            return null;
        }

        public static Mono.Cecil.TypeReference DescriptorToNet(this Utf8Constant descriptor, Mono.Cecil.AssemblyDefinition assembly)
        {
            string str = descriptor.Value;
            return (Mono.Cecil.TypeReference)DescriptorToNet(ref str, assembly);
        }

        public static MethodBody ConvertToNet(this JavaMethodBody javaMethodBody, Mono.Cecil.MethodDefinition method)
        {
            MethodBody body = new MethodBody(method);
            method.Body = body;
            foreach (var attr in javaMethodBody.code.Attributes)
            {
                switch (attr.Type)
                {
                    case Model.Attribute.AttributeType.LocalVariableTable:
                        foreach (var variable in ((Model.Attribute.LocalVariableTableAttribute)attr).LocalVariableTable)
                        {
                            // new VariableDefinition(new Mono.Cecil.TypeReference(variable.Descriptor))
                        }
                        break;
                    case Model.Attribute.AttributeType.LocalVariableTypeTable: break;
                }
            }

            List<Action> list = new List<Action>();
            foreach (var kv in javaMethodBody.GetInstructions())
            {
                (Func<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> create, Action<JInstruction, Mono.Cecil.MethodDefinition, NInstruction[]> init)
                    = convert[kv.instruction.OpCode];

                NInstruction[] items = create.Invoke(kv.instruction, method);
                foreach (var item in items) body.Instructions.Add(item);

                list.Add(() => init.Invoke(kv.instruction, method, items));
            }
            foreach (var item in list) item();
            return body;
        }
    }
}
