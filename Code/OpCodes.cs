using Java.Net.Binary;
using Java.Net.Code.Instruction;
using Java.Net.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Java.Net.Code
{
    public static class OpCodes
    {
        #region OpCode List
        /// <summary>
        /// Do nothing.
        /// <code>
        /// No change
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> NOP = new OpCode<SingleInstruction>(Names.NOP, "Do nothing");
        /// <summary>
        /// Push the null object reference onto the operand stack.
        /// <code>
        /// ... →
        /// ..., null
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ACONST_NULL = new OpCode<SingleInstruction>(Names.ACONST_NULL, "Push null");
        /// <summary>
        /// Push the int constant [i] (-1, 0, 1, 2, 3, 4 or 5) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [i]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ICONST_M1 = new OpCode<SingleInstruction>(Names.ICONST_M1, "Push int constant");
        /// <summary>
        /// Push the int constant [i] (-1, 0, 1, 2, 3, 4 or 5) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [i]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ICONST_0 = new OpCode<SingleInstruction>(Names.ICONST_0, "Push int constant");
        /// <summary>
        /// Push the int constant [i] (-1, 0, 1, 2, 3, 4 or 5) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [i]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ICONST_1 = new OpCode<SingleInstruction>(Names.ICONST_1, "Push int constant");
        /// <summary>
        /// Push the int constant [i] (-1, 0, 1, 2, 3, 4 or 5) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [i]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ICONST_2 = new OpCode<SingleInstruction>(Names.ICONST_2, "Push int constant");
        /// <summary>
        /// Push the int constant [i] (-1, 0, 1, 2, 3, 4 or 5) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [i]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ICONST_3 = new OpCode<SingleInstruction>(Names.ICONST_3, "Push int constant");
        /// <summary>
        /// Push the int constant [i] (-1, 0, 1, 2, 3, 4 or 5) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [i]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ICONST_4 = new OpCode<SingleInstruction>(Names.ICONST_4, "Push int constant");
        /// <summary>
        /// Push the int constant [i] (-1, 0, 1, 2, 3, 4 or 5) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [i]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ICONST_5 = new OpCode<SingleInstruction>(Names.ICONST_5, "Push int constant");
        /// <summary>
        /// Push the long constant [l] (0 or 1) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [l]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LCONST_0 = new OpCode<SingleInstruction>(Names.LCONST_0, "Push long constant");
        /// <summary>
        /// Push the long constant [l] (0 or 1) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [l]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LCONST_1 = new OpCode<SingleInstruction>(Names.LCONST_1, "Push long constant");
        /// <summary>
        /// Push the float constant [f] (0.0, 1.0, or 2.0) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [f]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FCONST_0 = new OpCode<SingleInstruction>(Names.FCONST_0, "Push float");
        /// <summary>
        /// Push the float constant [f] (0.0, 1.0, or 2.0) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [f]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FCONST_1 = new OpCode<SingleInstruction>(Names.FCONST_1, "Push float");
        /// <summary>
        /// Push the float constant [f] (0.0, 1.0, or 2.0) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [f]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FCONST_2 = new OpCode<SingleInstruction>(Names.FCONST_2, "Push float");
        /// <summary>
        /// Push the double constant [d] (0.0 or 1.0) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [d]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DCONST_0 = new OpCode<SingleInstruction>(Names.DCONST_0, "Push double");
        /// <summary>
        /// Push the double constant [d] (0.0 or 1.0) onto the operand stack.
        /// <code>
        /// ... →
        /// ..., [d]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DCONST_1 = new OpCode<SingleInstruction>(Names.DCONST_1, "Push double");
        /// <summary>
        /// The immediate byte is sign-extended to an int value. That value is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// byte
        /// </code>
        /// </summary>
        public static readonly OpCode<BiPushInstruction> BIPUSH = new OpCode<BiPushInstruction>(Names.BIPUSH, "Push byte");
        /// <summary>
        /// The immediate unsigned byte1 and byte2 values are assembled into an intermediate short where the value of the short is (byte1 [[ 8) | byte2. The intermediate value is then sign-extended to an int value. That value is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// byte1 byte2
        /// </code>
        /// </summary>
        public static readonly OpCode<SiPushInstruction> SIPUSH = new OpCode<SiPushInstruction>(Names.SIPUSH, "Push short");
        /// <summary>
        /// The index is an unsigned byte that must be a valid index into the run-time constant pool of the current class (§2.6). The run-time constant pool entry at index either must be a run-time constant of type int or float, or a reference to a string literal, or a symbolic reference to a class, method type, or method handle (§5.1).
        /// If the run-time constant pool entry is a run-time constant of type int or float, the numeric value of that run-time constant is pushed onto the operand stack as an int or float, respectively.
        /// Otherwise, if the run-time constant pool entry is a reference to an instance of class String representing a string literal (§5.1), then a reference to that instance, value, is pushed onto the operand stack.
        /// Otherwise, if the run-time constant pool entry is a symbolic reference to a class (§5.1), then the named class is resolved (§5.4.3.1) and a reference to the Class object representing that class, value, is pushed onto the operand stack.
        /// Otherwise, the run-time constant pool entry must be a symbolic reference to a method type or a method handle (§5.1). The method type or method handle is resolved (§5.4.3.5) and a reference to the resulting instance of java.lang.invoke.MethodType or java.lang.invoke.MethodHandle, value, is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantInstruction> LDC = new OpCode<ConstantInstruction>(Names.LDC, "Push item from run-time constant pool");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are assembled into an unsigned 16-bit index into the run-time constant pool of the current class (§2.6), where the value of the index is calculated as (indexbyte1 [[ 8) | indexbyte2. The index must be a valid index into the run-time constant pool of the current class. The run-time constant pool entry at the index either must be a run-time constant of type int or float, or a reference to a string literal, or a symbolic reference to a class, method type, or method handle (§5.1).
        /// If the run-time constant pool entry is a run-time constant of type int or float, the numeric value of that run-time constant is pushed onto the operand stack as an int or float, respectively.
        /// Otherwise, if the run-time constant pool entry is a reference to an instance of class String representing a string literal (§5.1), then a reference to that instance, value, is pushed onto the operand stack.
        /// Otherwise, if the run-time constant pool entry is a symbolic reference to a class (§4.4.1). The named class is resolved (§5.4.3.1) and a reference to the Class object representing that class, value, is pushed onto the operand stack.
        /// Otherwise, the run-time constant pool entry must be a symbolic reference to a method type or a method handle (§5.1). The method type or method handle is resolved (§5.4.3.5) and a reference to the resulting instance of java.lang.invoke.MethodType or java.lang.invoke.MethodHandle, value, is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> LDC_W = new OpCode<ConstantWideInstruction>(Names.LDC_W, "Push item from run-time constant pool (wide index)");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are assembled into an unsigned 16-bit index into the run-time constant pool of the current class (§2.6), where the value of the index is calculated as (indexbyte1 [[ 8) | indexbyte2. The index must be a valid index into the run-time constant pool of the current class. The run-time constant pool entry at the index must be a run-time constant of type long or double (§5.1). The numeric value of that run-time constant is pushed onto the operand stack as a long or double, respectively.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> LDC2_W = new OpCode<ConstantWideInstruction>(Names.LDC2_W, "Push long or double from run-time constant pool (wide index)");
        /// <summary>
        /// The index is an unsigned byte that must be an index into the local variable array of the current frame (§2.6). The local variable at index must contain an int. The value of the local variable at index is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> ILOAD = new OpCode<FrameIndexInstruction>(Names.ILOAD, "Load int from local variable");
        /// <summary>
        /// The index is an unsigned byte. Both index and index+1 must be indices into the local variable array of the current frame (§2.6). The local variable at index must contain a long. The value of the local variable at index is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> LLOAD = new OpCode<FrameIndexInstruction>(Names.LLOAD, "Load long from local variable");
        /// <summary>
        /// The index is an unsigned byte that must be an index into the local variable array of the current frame (§2.6). The local variable at index must contain a float. The value of the local variable at index is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> FLOAD = new OpCode<FrameIndexInstruction>(Names.FLOAD, "Load float from local variable");
        /// <summary>
        /// The index is an unsigned byte. Both index and index+1 must be indices into the local variable array of the current frame (§2.6). The local variable at index must contain a double. The value of the local variable at index is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> DLOAD = new OpCode<FrameIndexInstruction>(Names.DLOAD, "Load double from local variable");
        /// <summary>
        /// The index is an unsigned byte that must be an index into the local variable array of the current frame (§2.6). The local variable at index must contain a reference. The objectref in the local variable at index is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., objectref
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> ALOAD = new OpCode<FrameIndexInstruction>(Names.ALOAD, "Load reference from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain an int. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ILOAD_0 = new OpCode<SingleInstruction>(Names.ILOAD_0, "Load int from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain an int. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ILOAD_1 = new OpCode<SingleInstruction>(Names.ILOAD_1, "Load int from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain an int. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ILOAD_2 = new OpCode<SingleInstruction>(Names.ILOAD_2, "Load int from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain an int. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ILOAD_3 = new OpCode<SingleInstruction>(Names.ILOAD_3, "Load int from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a long. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LLOAD_0 = new OpCode<SingleInstruction>(Names.LLOAD_0, "Load long from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a long. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LLOAD_1 = new OpCode<SingleInstruction>(Names.LLOAD_1, "Load long from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a long. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LLOAD_2 = new OpCode<SingleInstruction>(Names.LLOAD_2, "Load long from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a long. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LLOAD_3 = new OpCode<SingleInstruction>(Names.LLOAD_3, "Load long from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a float. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FLOAD_0 = new OpCode<SingleInstruction>(Names.FLOAD_0, "Load float from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a float. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FLOAD_1 = new OpCode<SingleInstruction>(Names.FLOAD_1, "Load float from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a float. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FLOAD_2 = new OpCode<SingleInstruction>(Names.FLOAD_2, "Load float from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a float. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FLOAD_3 = new OpCode<SingleInstruction>(Names.FLOAD_3, "Load float from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a double. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DLOAD_0 = new OpCode<SingleInstruction>(Names.DLOAD_0, "Load double from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a double. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DLOAD_1 = new OpCode<SingleInstruction>(Names.DLOAD_1, "Load double from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a double. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DLOAD_2 = new OpCode<SingleInstruction>(Names.DLOAD_2, "Load double from local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The local variable at [n] must contain a double. The value of the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DLOAD_3 = new OpCode<SingleInstruction>(Names.DLOAD_3, "Load double from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a reference. The objectref in the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., objectref
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ALOAD_0 = new OpCode<SingleInstruction>(Names.ALOAD_0, "Load reference from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a reference. The objectref in the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., objectref
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ALOAD_1 = new OpCode<SingleInstruction>(Names.ALOAD_1, "Load reference from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a reference. The objectref in the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., objectref
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ALOAD_2 = new OpCode<SingleInstruction>(Names.ALOAD_2, "Load reference from local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The local variable at [n] must contain a reference. The objectref in the local variable at [n] is pushed onto the operand stack.
        /// <code>
        /// ... →
        /// ..., objectref
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ALOAD_3 = new OpCode<SingleInstruction>(Names.ALOAD_3, "Load reference from local variable");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type int. The index must be of type int. Both arrayref and index are popped from the operand stack. The int value in the component of the array at index is retrieved and pushed onto the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IALOAD = new OpCode<SingleInstruction>(Names.IALOAD, "Load int from array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type long. The index must be of type int. Both arrayref and index are popped from the operand stack. The long value in the component of the array at index is retrieved and pushed onto the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LALOAD = new OpCode<SingleInstruction>(Names.LALOAD, "Load long from array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type float. The index must be of type int. Both arrayref and index are popped from the operand stack. The float value in the component of the array at index is retrieved and pushed onto the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FALOAD = new OpCode<SingleInstruction>(Names.FALOAD, "Load float from array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type double. The index must be of type int. Both arrayref and index are popped from the operand stack. The double value in the component of the array at index is retrieved and pushed onto the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DALOAD = new OpCode<SingleInstruction>(Names.DALOAD, "Load double from array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type reference. The index must be of type int. Both arrayref and index are popped from the operand stack. The reference value in the component of the array at index is retrieved and pushed onto the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> AALOAD = new OpCode<SingleInstruction>(Names.AALOAD, "Load reference from array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type byte or of type boolean. The index must be of type int. Both arrayref and index are popped from the operand stack. The byte value in the component of the array at index is retrieved, sign-extended to an int value, and pushed onto the top of the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> BALOAD = new OpCode<SingleInstruction>(Names.BALOAD, "Load byte or boolean from array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type char. The index must be of type int. Both arrayref and index are popped from the operand stack. The component of the array at index is retrieved and zero-extended to an int value. That value is pushed onto the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> CALOAD = new OpCode<SingleInstruction>(Names.CALOAD, "Load char from array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type short. The index must be of type int. Both arrayref and index are popped from the operand stack. The component of the array at index is retrieved and sign-extended to an int value. That value is pushed onto the operand stack.
        /// <code>
        /// ..., arrayref, index →
        /// ..., value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> SALOAD = new OpCode<SingleInstruction>(Names.SALOAD, "Load short from array");
        /// <summary>
        /// The index is an unsigned byte that must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type int. It is popped from the operand stack, and the value of the local variable at index is set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> ISTORE = new OpCode<FrameIndexInstruction>(Names.ISTORE, "Store int into local variable");
        /// <summary>
        /// The index is an unsigned byte. Both index and index+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type long. It is popped from the operand stack, and the local variables at index and index+1 are set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> LSTORE = new OpCode<FrameIndexInstruction>(Names.LSTORE, "Store long into local variable");
        /// <summary>
        /// The index is an unsigned byte that must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The value of the local variable at index is set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> FSTORE = new OpCode<FrameIndexInstruction>(Names.FSTORE, "Store float into local variable");
        /// <summary>
        /// The index is an unsigned byte. Both index and index+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The local variables at index and index+1 are set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> DSTORE = new OpCode<FrameIndexInstruction>(Names.DSTORE, "Store double into local variable");
        /// <summary>
        /// The index is an unsigned byte that must be an index into the local variable array of the current frame (§2.6). The objectref on the top of the operand stack must be of type returnAddress or of type reference. It is popped from the operand stack, and the value of the local variable at index is set to objectref.
        /// <code>
        /// ..., objectref →
        /// ...
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> ASTORE = new OpCode<FrameIndexInstruction>(Names.ASTORE, "Store reference into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type int. It is popped from the operand stack, and the value of the local variable at [n] is set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ISTORE_0 = new OpCode<SingleInstruction>(Names.ISTORE_0, "Store int into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type int. It is popped from the operand stack, and the value of the local variable at [n] is set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ISTORE_1 = new OpCode<SingleInstruction>(Names.ISTORE_1, "Store int into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type int. It is popped from the operand stack, and the value of the local variable at [n] is set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ISTORE_2 = new OpCode<SingleInstruction>(Names.ISTORE_2, "Store int into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type int. It is popped from the operand stack, and the value of the local variable at [n] is set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ISTORE_3 = new OpCode<SingleInstruction>(Names.ISTORE_3, "Store int into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type long. It is popped from the operand stack, and the local variables at [n] and [n]+1 are set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LSTORE_0 = new OpCode<SingleInstruction>(Names.LSTORE_0, "Store long into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type long. It is popped from the operand stack, and the local variables at [n] and [n]+1 are set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LSTORE_1 = new OpCode<SingleInstruction>(Names.LSTORE_1, "Store long into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type long. It is popped from the operand stack, and the local variables at [n] and [n]+1 are set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LSTORE_2 = new OpCode<SingleInstruction>(Names.LSTORE_2, "Store long into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type long. It is popped from the operand stack, and the local variables at [n] and [n]+1 are set to value.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LSTORE_3 = new OpCode<SingleInstruction>(Names.LSTORE_3, "Store long into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The value of the local variable at [n] is set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FSTORE_0 = new OpCode<SingleInstruction>(Names.FSTORE_0, "Store float into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The value of the local variable at [n] is set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FSTORE_1 = new OpCode<SingleInstruction>(Names.FSTORE_1, "Store float into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The value of the local variable at [n] is set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FSTORE_2 = new OpCode<SingleInstruction>(Names.FSTORE_2, "Store float into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The value of the local variable at [n] is set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FSTORE_3 = new OpCode<SingleInstruction>(Names.FSTORE_3, "Store float into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The local variables at [n] and [n]+1 are set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DSTORE_0 = new OpCode<SingleInstruction>(Names.DSTORE_0, "Store double into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The local variables at [n] and [n]+1 are set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DSTORE_1 = new OpCode<SingleInstruction>(Names.DSTORE_1, "Store double into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The local variables at [n] and [n]+1 are set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DSTORE_2 = new OpCode<SingleInstruction>(Names.DSTORE_2, "Store double into local variable");
        /// <summary>
        /// Both [n] and [n]+1 must be indices into the local variable array of the current frame (§2.6). The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The local variables at [n] and [n]+1 are set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DSTORE_3 = new OpCode<SingleInstruction>(Names.DSTORE_3, "Store double into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The objectref on the top of the operand stack must be of type returnAddress or of type reference. It is popped from the operand stack, and the value of the local variable at [n] is set to objectref.
        /// <code>
        /// ..., objectref →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ASTORE_0 = new OpCode<SingleInstruction>(Names.ASTORE_0, "Store reference into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The objectref on the top of the operand stack must be of type returnAddress or of type reference. It is popped from the operand stack, and the value of the local variable at [n] is set to objectref.
        /// <code>
        /// ..., objectref →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ASTORE_1 = new OpCode<SingleInstruction>(Names.ASTORE_1, "Store reference into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The objectref on the top of the operand stack must be of type returnAddress or of type reference. It is popped from the operand stack, and the value of the local variable at [n] is set to objectref.
        /// <code>
        /// ..., objectref →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ASTORE_2 = new OpCode<SingleInstruction>(Names.ASTORE_2, "Store reference into local variable");
        /// <summary>
        /// The [n] must be an index into the local variable array of the current frame (§2.6). The objectref on the top of the operand stack must be of type returnAddress or of type reference. It is popped from the operand stack, and the value of the local variable at [n] is set to objectref.
        /// <code>
        /// ..., objectref →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ASTORE_3 = new OpCode<SingleInstruction>(Names.ASTORE_3, "Store reference into local variable");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type int. Both index and value must be of type int. The arrayref, index, and value are popped from the operand stack. The int value is stored as the component of the array indexed by index.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IASTORE = new OpCode<SingleInstruction>(Names.IASTORE, "Store into int array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type long. The index must be of type int, and value must be of type long. The arrayref, index, and value are popped from the operand stack. The long value is stored as the component of the array indexed by index.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LASTORE = new OpCode<SingleInstruction>(Names.LASTORE, "Store into long array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type float. The index must be of type int, and the value must be of type float. The arrayref, index, and value are popped from the operand stack. The float value undergoes value set conversion (§2.8.3), resulting in value', and value' is stored as the component of the array indexed by index.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FASTORE = new OpCode<SingleInstruction>(Names.FASTORE, "Store into float array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type double. The index must be of type int, and value must be of type double. The arrayref, index, and value are popped from the operand stack. The double value undergoes value set conversion (§2.8.3), resulting in value', which is stored as the component of the array indexed by index.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DASTORE = new OpCode<SingleInstruction>(Names.DASTORE, "Store into double array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type reference. The index must be of type int and value must be of type reference. The arrayref, index, and value are popped from the operand stack. The reference value is stored as the component of the array at index.
        /// At run time, the type of value must be compatible with the type of the components of the array referenced by arrayref. Specifically, assignment of a value of reference type S (source) to an array component of reference type T (target) is allowed only if:
        /// If S is a class type, then:
        ///
        /// If T is a class type, then S must be the same class as T, or S must be a subclass of T;
        ///
        /// If T is an interface type, then S must implement interface T.
        ///
        /// If S is an interface type, then:
        ///
        /// If T is a class type, then T must be Object.
        ///
        /// If T is an interface type, then T must be the same interface as S or a superinterface of S.
        ///
        /// If S is an array type, namely, the type SC[], that is, an array of components of type SC, then:
        ///
        /// If T is a class type, then T must be Object.
        ///
        /// If T is an interface type, then T must be one of the interfaces implemented by arrays (JLS §4.10.3).
        ///
        /// If T is an array type TC[], that is, an array of components of type TC, then one of the following must be true:
        ///
        /// TC and SC are the same primitive type.
        ///
        /// TC and SC are reference types, and type SC is assignable to TC by these run-time rules.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> AASTORE = new OpCode<SingleInstruction>(Names.AASTORE, "Store into reference array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type byte or of type boolean. The index and the value must both be of type int. The arrayref, index, and value are popped from the operand stack. The int value is truncated to a byte and stored as the component of the array indexed by index.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> BASTORE = new OpCode<SingleInstruction>(Names.BASTORE, "Store into byte or boolean array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type char. The index and the value must both be of type int. The arrayref, index, and value are popped from the operand stack. The int value is truncated to a char and stored as the component of the array indexed by index.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> CASTORE = new OpCode<SingleInstruction>(Names.CASTORE, "Store into char array");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array whose components are of type short. Both index and value must be of type int. The arrayref, index, and value are popped from the operand stack. The int value is truncated to a short and stored as the component of the array indexed by index.
        /// <code>
        /// ..., arrayref, index, value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> SASTORE = new OpCode<SingleInstruction>(Names.SASTORE, "Store into short array");
        /// <summary>
        /// Pop the top value from the operand stack.
        /// The pop instruction must not be used unless value is a value of a category 1 computational type (§2.11.1).
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> POP = new OpCode<SingleInstruction>(Names.POP, "Pop the top operand stack value");
        /// <summary>
        /// Pop the top one or two values from the operand stack.
        /// <code>
        /// Form 1:
        /// ..., value2, value1 →
        /// ...
        /// where each of value1 and value2 is a value of a category 1 computational type (§2.11.1).
        /// Form 2:
        /// ..., value →
        /// ...
        /// where value is a value of a category 2 computational type (§2.11.1).
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> POP2 = new OpCode<SingleInstruction>(Names.POP2, "Pop the top one or two operand stack values");
        /// <summary>
        /// Duplicate the top value on the operand stack and push the duplicated value onto the operand stack.
        /// The dup instruction must not be used unless value is a value of a category 1 computational type (§2.11.1).
        /// <code>
        /// ..., value →
        /// ..., value, value
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DUP = new OpCode<SingleInstruction>(Names.DUP, "Duplicate the top operand stack value");
        /// <summary>
        /// Duplicate the top value on the operand stack and insert the duplicated value two values down in the operand stack.
        /// The dup_x1 instruction must not be used unless both value1 and value2 are values of a category 1 computational type (§2.11.1).
        /// <code>
        /// ..., value2, value1 →
        /// ..., value1, value2, value1
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DUP_X1 = new OpCode<SingleInstruction>(Names.DUP_X1, "Duplicate the top operand stack value and insert two values down");
        /// <summary>
        /// Duplicate the top value on the operand stack and insert the duplicated value two or three values down in the operand stack.
        /// <code>
        /// Form 1:
        /// ..., value3, value2, value1 →
        /// ..., value1, value3, value2, value1
        /// where value1, value2, and value3 are all values of a category 1 computational type (§2.11.1).
        /// Form 2:
        /// ..., value2, value1 →
        /// ..., value1, value2, value1
        /// where value1 is a value of a category 1 computational type and value2 is a value of a category 2 computational type (§2.11.1).
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DUP_X2 = new OpCode<SingleInstruction>(Names.DUP_X2, "Duplicate the top operand stack value and insert two or three values down");
        /// <summary>
        /// Duplicate the top one or two values on the operand stack and push the duplicated value or values back onto the operand stack in the original order.
        /// <code>
        /// Form 1:
        /// ..., value2, value1 →
        /// ..., value2, value1, value2, value1
        /// where both value1 and value2 are values of a category 1 computational type (§2.11.1).
        /// Form 2:
        /// ..., value →
        /// ..., value, value
        /// where value is a value of a category 2 computational type (§2.11.1).
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DUP2 = new OpCode<SingleInstruction>(Names.DUP2, "Duplicate the top one or two operand stack values");
        /// <summary>
        /// Duplicate the top one or two values on the operand stack and insert the duplicated values, in the original order, one value beneath the original value or values in the operand stack.
        /// <code>
        /// Form 1:
        /// ..., value3, value2, value1 →
        /// ..., value2, value1, value3, value2, value1
        /// where value1, value2, and value3 are all values of a category 1 computational type (§2.11.1).
        /// Form 2:
        /// ..., value2, value1 →
        /// ..., value1, value2, value1
        /// where value1 is a value of a category 2 computational type and value2 is a value of a category 1 computational type (§2.11.1).
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DUP2_X1 = new OpCode<SingleInstruction>(Names.DUP2_X1, "Duplicate the top one or two operand stack values and insert two or three values down");
        /// <summary>
        /// Duplicate the top one or two values on the operand stack and insert the duplicated values, in the original order, into the operand stack.
        /// <code>
        /// Form 1:
        /// ..., value4, value3, value2, value1 →
        /// ..., value2, value1, value4, value3, value2, value1
        /// where value1, value2, value3, and value4 are all values of a category 1 computational type (§2.11.1).
        /// Form 2:
        /// ..., value3, value2, value1 →
        /// ..., value1, value3, value2, value1
        /// where value1 is a value of a category 2 computational type and value2 and value3 are both values of a category 1 computational type (§2.11.1).
        /// Form 3:
        /// ..., value3, value2, value1 →
        /// ..., value2, value1, value3, value2, value1
        /// where value1 and value2 are both values of a category 1 computational type and value3 is a value of a category 2 computational type (§2.11.1).
        /// Form 4:
        /// ..., value2, value1 →
        /// ..., value1, value2, value1
        /// where value1 and value2 are both values of a category 2 computational type (§2.11.1).
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DUP2_X2 = new OpCode<SingleInstruction>(Names.DUP2_X2, "Duplicate the top one or two operand stack values and insert two, three, or four values down");
        /// <summary>
        /// Swap the top two values on the operand stack.
        /// The swap instruction must not be used unless value1 and value2 are both values of a category 1 computational type (§2.11.1).
        /// <code>
        /// ..., value2, value1 →
        /// ..., value1, value2
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> SWAP = new OpCode<SingleInstruction>(Names.SWAP, "Swap the top two operand stack values");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. The int result is value1 + value2. The result is pushed onto the operand stack.
        /// The result is the 32 low-order bits of the true mathematical result in a sufficiently wide two's-complement format, represented as a value of type int. If overflow occurs, then the sign of the result may not be the same as the sign of the mathematical sum of the two values.
        /// Despite the fact that overflow may occur, execution of an iadd instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IADD = new OpCode<SingleInstruction>(Names.IADD, "Add int");
        /// <summary>
        /// Both value1 and value2 must be of type long. The values are popped from the operand stack. The long result is value1 + value2. The result is pushed onto the operand stack.
        /// The result is the 64 low-order bits of the true mathematical result in a sufficiently wide two's-complement format, represented as a value of type long. If overflow occurs, the sign of the result may not be the same as the sign of the mathematical sum of the two values.
        /// Despite the fact that overflow may occur, execution of an ladd instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LADD = new OpCode<SingleInstruction>(Names.LADD, "Add long");
        /// <summary>
        /// Both value1 and value2 must be of type float. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The float result is value1' + value2'. The result is pushed onto the operand stack.
        /// The result of an fadd instruction is governed by the rules of IEEE arithmetic:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// The sum of two infinities of opposite sign is NaN.
        ///
        /// The sum of two infinities of the same sign is the infinity of that sign.
        ///
        /// The sum of an infinity and any finite value is equal to the infinity.
        ///
        /// The sum of two zeroes of opposite sign is positive zero.
        ///
        /// The sum of two zeroes of the same sign is the zero of that sign.
        ///
        /// The sum of a zero and a nonzero finite value is equal to the nonzero value.
        ///
        /// The sum of two nonzero finite values of the same magnitude and opposite sign is positive zero.
        ///
        /// In the remaining cases, where neither operand is an infinity, a zero, or NaN and the values have the same sign or have different magnitudes, the sum is computed and rounded to the nearest representable value using IEEE 754 round to nearest mode. If the magnitude is too large to represent as a float, we say the operation overflows; the result is then an infinity of appropriate sign. If the magnitude is too small to represent as a float, we say the operation underflows; the result is then a zero of appropriate sign.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, or loss of precision may occur, execution of an fadd instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FADD = new OpCode<SingleInstruction>(Names.FADD, "Add float");
        /// <summary>
        /// Both value1 and value2 must be of type double. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The double result is value1' + value2'. The result is pushed onto the operand stack.
        /// The result of a dadd instruction is governed by the rules of IEEE arithmetic:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// The sum of two infinities of opposite sign is NaN.
        ///
        /// The sum of two infinities of the same sign is the infinity of that sign.
        ///
        /// The sum of an infinity and any finite value is equal to the infinity.
        ///
        /// The sum of two zeroes of opposite sign is positive zero.
        ///
        /// The sum of two zeroes of the same sign is the zero of that sign.
        ///
        /// The sum of a zero and a nonzero finite value is equal to the nonzero value.
        ///
        /// The sum of two nonzero finite values of the same magnitude and opposite sign is positive zero.
        ///
        /// In the remaining cases, where neither operand is an infinity, a zero, or NaN and the values have the same sign or have different magnitudes, the sum is computed and rounded to the nearest representable value using IEEE 754 round to nearest mode. If the magnitude is too large to represent as a double, we say the operation overflows; the result is then an infinity of appropriate sign. If the magnitude is too small to represent as a double, we say the operation underflows; the result is then a zero of appropriate sign.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, or loss of precision may occur, execution of a dadd instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DADD = new OpCode<SingleInstruction>(Names.DADD, "Add double");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. The int result is value1 - value2. The result is pushed onto the operand stack.
        /// For int subtraction, a-b produces the same result as a+(-b). For int values, subtraction from zero is the same as negation.
        /// The result is the 32 low-order bits of the true mathematical result in a sufficiently wide two's-complement format, represented as a value of type int. If overflow occurs, then the sign of the result may not be the same as the sign of the mathematical difference of the two values.
        /// Despite the fact that overflow may occur, execution of an isub instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ISUB = new OpCode<SingleInstruction>(Names.ISUB, "Subtract int");
        /// <summary>
        /// Both value1 and value2 must be of type long. The values are popped from the operand stack. The long result is value1 - value2. The result is pushed onto the operand stack.
        /// For long subtraction, a-b produces the same result as a+(-b). For long values, subtraction from zero is the same as negation.
        /// The result is the 64 low-order bits of the true mathematical result in a sufficiently wide two's-complement format, represented as a value of type long. If overflow occurs, then the sign of the result may not be the same as the sign of the mathematical sum of the two values.
        /// Despite the fact that overflow may occur, execution of an lsub instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LSUB = new OpCode<SingleInstruction>(Names.LSUB, "Subtract long");
        /// <summary>
        /// Both value1 and value2 must be of type float. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The float result is value1' - value2'. The result is pushed onto the operand stack.
        /// For float subtraction, it is always the case that a-b produces the same result as a+(-b). However, for the fsub instruction, subtraction from zero is not the same as negation, because if x is +0.0, then 0.0-x equals +0.0, but -x equals -0.0.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, or loss of precision may occur, execution of an fsub instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FSUB = new OpCode<SingleInstruction>(Names.FSUB, "Subtract float");
        /// <summary>
        /// Both value1 and value2 must be of type double. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The double result is value1' - value2'. The result is pushed onto the operand stack.
        /// For double subtraction, it is always the case that a-b produces the same result as a+(-b). However, for the dsub instruction, subtraction from zero is not the same as negation, because if x is +0.0, then 0.0-x equals +0.0, but -x equals -0.0.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, or loss of precision may occur, execution of a dsub instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DSUB = new OpCode<SingleInstruction>(Names.DSUB, "Subtract double");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. The int result is value1 * value2. The result is pushed onto the operand stack.
        /// The result is the 32 low-order bits of the true mathematical result in a sufficiently wide two's-complement format, represented as a value of type int. If overflow occurs, then the sign of the result may not be the same as the sign of the mathematical sum of the two values.
        /// Despite the fact that overflow may occur, execution of an imul instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IMUL = new OpCode<SingleInstruction>(Names.IMUL, "Multiply int");
        /// <summary>
        /// Both value1 and value2 must be of type long. The values are popped from the operand stack. The long result is value1 * value2. The result is pushed onto the operand stack.
        /// The result is the 64 low-order bits of the true mathematical result in a sufficiently wide two's-complement format, represented as a value of type long. If overflow occurs, the sign of the result may not be the same as the sign of the mathematical sum of the two values.
        /// Despite the fact that overflow may occur, execution of an lmul instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LMUL = new OpCode<SingleInstruction>(Names.LMUL, "Multiply long");
        /// <summary>
        /// Both value1 and value2 must be of type float. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The float result is value1' * value2'. The result is pushed onto the operand stack.
        /// The result of an fmul instruction is governed by the rules of IEEE arithmetic:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// If neither value1' nor value2' is NaN, the sign of the result is positive if both values have the same sign, and negative if the values have different signs.
        ///
        /// Multiplication of an infinity by a zero results in NaN.
        ///
        /// Multiplication of an infinity by a finite value results in a signed infinity, with the sign-producing rule just given.
        ///
        /// In the remaining cases, where neither an infinity nor NaN is involved, the product is computed and rounded to the nearest representable value using IEEE 754 round to nearest mode. If the magnitude is too large to represent as a float, we say the operation overflows; the result is then an infinity of appropriate sign. If the magnitude is too small to represent as a float, we say the operation underflows; the result is then a zero of appropriate sign.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, or loss of precision may occur, execution of an fmul instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FMUL = new OpCode<SingleInstruction>(Names.FMUL, "Multiply float");
        /// <summary>
        /// Both value1 and value2 must be of type double. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The double result is value1' * value2'. The result is pushed onto the operand stack.
        /// The result of a dmul instruction is governed by the rules of IEEE arithmetic:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// If neither value1' nor value2' is NaN, the sign of the result is positive if both values have the same sign and negative if the values have different signs.
        ///
        /// Multiplication of an infinity by a zero results in NaN.
        ///
        /// Multiplication of an infinity by a finite value results in a signed infinity, with the sign-producing rule just given.
        ///
        /// In the remaining cases, where neither an infinity nor NaN is involved, the product is computed and rounded to the nearest representable value using IEEE 754 round to nearest mode. If the magnitude is too large to represent as a double, we say the operation overflows; the result is then an infinity of appropriate sign. If the magnitude is too small to represent as a double, we say the operation underflows; the result is then a zero of appropriate sign.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, or loss of precision may occur, execution of a dmul instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DMUL = new OpCode<SingleInstruction>(Names.DMUL, "Multiply double");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. The int result is the value of the Java programming language expression value1 / value2. The result is pushed onto the operand stack.
        /// An int division rounds towards 0; that is, the quotient produced for int values in n/d is an int value q whose magnitude is as large as possible while satisfying |d · q| ? |n|. Moreover, q is positive when |n| ? |d| and n and d have the same sign, but q is negative when |n| ? |d| and n and d have opposite signs.
        /// There is one special case that does not satisfy this rule: if the dividend is the negative integer of largest possible magnitude for the int type, and the divisor is -1, then overflow occurs, and the result is equal to the dividend. Despite the overflow, no exception is thrown in this case.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IDIV = new OpCode<SingleInstruction>(Names.IDIV, "Divide int");
        /// <summary>
        /// Both value1 and value2 must be of type long. The values are popped from the operand stack. The long result is the value of the Java programming language expression value1 / value2. The result is pushed onto the operand stack.
        /// A long division rounds towards 0; that is, the quotient produced for long values in n / d is a long value q whose magnitude is as large as possible while satisfying |d · q| ? |n|. Moreover, q is positive when |n| ? |d| and n and d have the same sign, but q is negative when |n| ? |d| and n and d have opposite signs.
        /// There is one special case that does not satisfy this rule: if the dividend is the negative integer of largest possible magnitude for the long type and the divisor is -1, then overflow occurs and the result is equal to the dividend; despite the overflow, no exception is thrown in this case.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LDIV = new OpCode<SingleInstruction>(Names.LDIV, "Divide long");
        /// <summary>
        /// Both value1 and value2 must be of type float. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The float result is value1' / value2'. The result is pushed onto the operand stack.
        /// The result of an fdiv instruction is governed by the rules of IEEE arithmetic:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// If neither value1' nor value2' is NaN, the sign of the result is positive if both values have the same sign, negative if the values have different signs.
        ///
        /// Division of an infinity by an infinity results in NaN.
        ///
        /// Division of an infinity by a finite value results in a signed infinity, with the sign-producing rule just given.
        ///
        /// Division of a finite value by an infinity results in a signed zero, with the sign-producing rule just given.
        ///
        /// Division of a zero by a zero results in NaN; division of zero by any other finite value results in a signed zero, with the sign-producing rule just given.
        ///
        /// Division of a nonzero finite value by a zero results in a signed infinity, with the sign-producing rule just given.
        ///
        /// In the remaining cases, where neither operand is an infinity, a zero, or NaN, the quotient is computed and rounded to the nearest float using IEEE 754 round to nearest mode. If the magnitude is too large to represent as a float, we say the operation overflows; the result is then an infinity of appropriate sign. If the magnitude is too small to represent as a float, we say the operation underflows; the result is then a zero of appropriate sign.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, division by zero, or loss of precision may occur, execution of an fdiv instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FDIV = new OpCode<SingleInstruction>(Names.FDIV, "Divide float");
        /// <summary>
        /// Both value1 and value2 must be of type double. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The double result is value1' / value2'. The result is pushed onto the operand stack.
        /// The result of a ddiv instruction is governed by the rules of IEEE arithmetic:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// If neither value1' nor value2' is NaN, the sign of the result is positive if both values have the same sign, negative if the values have different signs.
        ///
        /// Division of an infinity by an infinity results in NaN.
        ///
        /// Division of an infinity by a finite value results in a signed infinity, with the sign-producing rule just given.
        ///
        /// Division of a finite value by an infinity results in a signed zero, with the sign-producing rule just given.
        ///
        /// Division of a zero by a zero results in NaN; division of zero by any other finite value results in a signed zero, with the sign-producing rule just given.
        ///
        /// Division of a nonzero finite value by a zero results in a signed infinity, with the sign-producing rule just given.
        ///
        /// In the remaining cases, where neither operand is an infinity, a zero, or NaN, the quotient is computed and rounded to the nearest double using IEEE 754 round to nearest mode. If the magnitude is too large to represent as a double, we say the operation overflows; the result is then an infinity of appropriate sign. If the magnitude is too small to represent as a double, we say the operation underflows; the result is then a zero of appropriate sign.
        /// The Java Virtual Machine requires support of gradual underflow as defined by IEEE 754. Despite the fact that overflow, underflow, division by zero, or loss of precision may occur, execution of a ddiv instruction never throws a run-time exception.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DDIV = new OpCode<SingleInstruction>(Names.DDIV, "Divide double");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. The int result is value1 - (value1 / value2) * value2. The result is pushed onto the operand stack.
        /// The result of the irem instruction is such that (a/b)*b + (a%b) is equal to a. This identity holds even in the special case in which the dividend is the negative int of largest possible magnitude for its type and the divisor is -1 (the remainder is 0). It follows from this rule that the result of the remainder operation can be negative only if the dividend is negative and can be positive only if the dividend is positive. Moreover, the magnitude of the result is always less than the magnitude of the divisor.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IREM = new OpCode<SingleInstruction>(Names.IREM, "Remainder int");
        /// <summary>
        /// Both value1 and value2 must be of type long. The values are popped from the operand stack. The long result is value1 - (value1 / value2) * value2. The result is pushed onto the operand stack.
        /// The result of the lrem instruction is such that (a/b)*b + (a%b) is equal to a. This identity holds even in the special case in which the dividend is the negative long of largest possible magnitude for its type and the divisor is -1 (the remainder is 0). It follows from this rule that the result of the remainder operation can be negative only if the dividend is negative and can be positive only if the dividend is positive; moreover, the magnitude of the result is always less than the magnitude of the divisor.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LREM = new OpCode<SingleInstruction>(Names.LREM, "Remainder long");
        /// <summary>
        /// Both value1 and value2 must be of type float. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The result is calculated and pushed onto the operand stack as a float.
        /// The result of an frem instruction is not the same as that of the so-called remainder operation defined by IEEE 754. The IEEE 754 "remainder" operation computes the remainder from a rounding division, not a truncating division, and so its behavior is not analogous to that of the usual integer remainder operator. Instead, the Java Virtual Machine defines frem to behave in a manner analogous to that of the Java Virtual Machine integer remainder instructions (irem and lrem); this may be compared with the C library function fmod.
        /// The result of an frem instruction is governed by these rules:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// If neither value1' nor value2' is NaN, the sign of the result equals the sign of the dividend.
        ///
        /// If the dividend is an infinity or the divisor is a zero or both, the result is NaN.
        ///
        /// If the dividend is finite and the divisor is an infinity, the result equals the dividend.
        ///
        /// If the dividend is a zero and the divisor is finite, the result equals the dividend.
        ///
        /// In the remaining cases, where neither operand is an infinity, a zero, or NaN, the floating-point remainder result from a dividend value1' and a divisor value2' is defined by the mathematical relation result = value1' - (value2' * q), where q is an integer that is negative only if value1' / value2' is negative and positive only if value1' / value2' is positive, and whose magnitude is as large as possible without exceeding the magnitude of the true mathematical quotient of value1' and value2'.
        /// Despite the fact that division by zero may occur, evaluation of an frem instruction never throws a run-time exception. Overflow, underflow, or loss of precision cannot occur.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FREM = new OpCode<SingleInstruction>(Names.FREM, "Remainder float");
        /// <summary>
        /// Both value1 and value2 must be of type double. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. The result is calculated and pushed onto the operand stack as a double.
        /// The result of a drem instruction is not the same as that of the so-called remainder operation defined by IEEE 754. The IEEE 754 "remainder" operation computes the remainder from a rounding division, not a truncating division, and so its behavior is not analogous to that of the usual integer remainder operator. Instead, the Java Virtual Machine defines drem to behave in a manner analogous to that of the Java Virtual Machine integer remainder instructions (irem and lrem); this may be compared with the C library function fmod.
        /// The result of a drem instruction is governed by these rules:
        /// If either value1' or value2' is NaN, the result is NaN.
        ///
        /// If neither value1' nor value2' is NaN, the sign of the result equals the sign of the dividend.
        ///
        /// If the dividend is an infinity or the divisor is a zero or both, the result is NaN.
        ///
        /// If the dividend is finite and the divisor is an infinity, the result equals the dividend.
        ///
        /// If the dividend is a zero and the divisor is finite, the result equals the dividend.
        ///
        /// In the remaining cases, where neither operand is an infinity, a zero, or NaN, the floating-point remainder result from a dividend value1' and a divisor value2' is defined by the mathematical relation result = value1' - (value2' * q), where q is an integer that is negative only if value1' / value2' is negative, and positive only if value1' / value2' is positive, and whose magnitude is as large as possible without exceeding the magnitude of the true mathematical quotient of value1' and value2'.
        /// Despite the fact that division by zero may occur, evaluation of a drem instruction never throws a run-time exception. Overflow, underflow, or loss of precision cannot occur.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DREM = new OpCode<SingleInstruction>(Names.DREM, "Remainder double");
        /// <summary>
        /// The value must be of type int. It is popped from the operand stack. The int result is the arithmetic negation of value, -value. The result is pushed onto the operand stack.
        /// For int values, negation is the same as subtraction from zero. Because the Java Virtual Machine uses two's-complement representation for integers and the range of two's-complement values is not symmetric, the negation of the maximum negative int results in that same maximum negative number. Despite the fact that overflow has occurred, no exception is thrown.
        /// For all int values x, -x equals (~x)+1.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> INEG = new OpCode<SingleInstruction>(Names.INEG, "Negate int");
        /// <summary>
        /// The value must be of type long. It is popped from the operand stack. The long result is the arithmetic negation of value, -value. The result is pushed onto the operand stack.
        /// For long values, negation is the same as subtraction from zero. Because the Java Virtual Machine uses two's-complement representation for integers and the range of two's-complement values is not symmetric, the negation of the maximum negative long results in that same maximum negative number. Despite the fact that overflow has occurred, no exception is thrown.
        /// For all long values x, -x equals (~x)+1.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LNEG = new OpCode<SingleInstruction>(Names.LNEG, "Negate long");
        /// <summary>
        /// The value must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The float result is the arithmetic negation of value'. This result is pushed onto the operand stack.
        /// For float values, negation is not the same as subtraction from zero. If x is +0.0, then 0.0-x equals +0.0, but -x equals -0.0. Unary minus merely inverts the sign of a float.
        /// Special cases of interest:
        /// If the operand is NaN, the result is NaN (recall that NaN has no sign).
        ///
        /// If the operand is an infinity, the result is the infinity of opposite sign.
        ///
        /// If the operand is a zero, the result is the zero of opposite sign.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FNEG = new OpCode<SingleInstruction>(Names.FNEG, "Negate float");
        /// <summary>
        /// The value must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The double result is the arithmetic negation of value'. The result is pushed onto the operand stack.
        /// For double values, negation is not the same as subtraction from zero. If x is +0.0, then 0.0-x equals +0.0, but -x equals -0.0. Unary minus merely inverts the sign of a double.
        /// Special cases of interest:
        /// If the operand is NaN, the result is NaN (recall that NaN has no sign).
        ///
        /// If the operand is an infinity, the result is the infinity of opposite sign.
        ///
        /// If the operand is a zero, the result is the zero of opposite sign.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DNEG = new OpCode<SingleInstruction>(Names.DNEG, "Negate double");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. An int result is calculated by shifting value1 left by s bit positions, where s is the value of the low 5 bits of value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ISHL = new OpCode<SingleInstruction>(Names.ISHL, "Shift left int");
        /// <summary>
        /// The value1 must be of type long, and value2 must be of type int. The values are popped from the operand stack. A long result is calculated by shifting value1 left by s bit positions, where s is the low 6 bits of value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LSHL = new OpCode<SingleInstruction>(Names.LSHL, "Shift left long");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. An int result is calculated by shifting value1 right by s bit positions, with sign extension, where s is the value of the low 5 bits of value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ISHR = new OpCode<SingleInstruction>(Names.ISHR, "Arithmetic shift right int");
        /// <summary>
        /// The value1 must be of type long, and value2 must be of type int. The values are popped from the operand stack. A long result is calculated by shifting value1 right by s bit positions, with sign extension, where s is the value of the low 6 bits of value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LSHR = new OpCode<SingleInstruction>(Names.LSHR, "Arithmetic shift right long");
        /// <summary>
        /// Both value1 and value2 must be of type int. The values are popped from the operand stack. An int result is calculated by shifting value1 right by s bit positions, with zero extension, where s is the value of the low 5 bits of value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IUSHR = new OpCode<SingleInstruction>(Names.IUSHR, "Logical shift right int");
        /// <summary>
        /// The value1 must be of type long, and value2 must be of type int. The values are popped from the operand stack. A long result is calculated by shifting value1 right logically (with zero extension) by the amount indicated by the low 6 bits of value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LUSHR = new OpCode<SingleInstruction>(Names.LUSHR, "Logical shift right long");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are popped from the operand stack. An int result is calculated by taking the bitwise AND (conjunction) of value1 and value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IAND = new OpCode<SingleInstruction>(Names.IAND, "Boolean AND int");
        /// <summary>
        /// Both value1 and value2 must be of type long. They are popped from the operand stack. A long result is calculated by taking the bitwise AND of value1 and value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LAND = new OpCode<SingleInstruction>(Names.LAND, "Boolean AND long");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are popped from the operand stack. An int result is calculated by taking the bitwise inclusive OR of value1 and value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IOR = new OpCode<SingleInstruction>(Names.IOR, "Boolean OR int");
        /// <summary>
        /// Both value1 and value2 must be of type long. They are popped from the operand stack. A long result is calculated by taking the bitwise inclusive OR of value1 and value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LOR = new OpCode<SingleInstruction>(Names.LOR, "Boolean OR long");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are popped from the operand stack. An int result is calculated by taking the bitwise exclusive OR of value1 and value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IXOR = new OpCode<SingleInstruction>(Names.IXOR, "Boolean XOR int");
        /// <summary>
        /// Both value1 and value2 must be of type long. They are popped from the operand stack. A long result is calculated by taking the bitwise exclusive OR of value1 and value2. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LXOR = new OpCode<SingleInstruction>(Names.LXOR, "Boolean XOR long");
        /// <summary>
        /// The index is an unsigned byte that must be an index into the local variable array of the current frame (§2.6). The const is an immediate signed byte. The local variable at index must contain an int. The value const is first sign-extended to an int, and then the local variable at index is incremented by that amount.
        /// <code>
        /// No change
        /// </code>
        /// <code>
        /// index const
        /// </code>
        /// </summary>
        public static readonly OpCode<IIncInstruction> IINC = new OpCode<IIncInstruction>(Names.IINC, "Increment local variable by constant");
        /// <summary>
        /// The value on the top of the operand stack must be of type int. It is popped from the operand stack and sign-extended to a long result. That result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> I2L = new OpCode<SingleInstruction>(Names.I2L, "Convert int to long");
        /// <summary>
        /// The value on the top of the operand stack must be of type int. It is popped from the operand stack and converted to the float result using IEEE 754 round to nearest mode. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> I2F = new OpCode<SingleInstruction>(Names.I2F, "Convert int to float");
        /// <summary>
        /// The value on the top of the operand stack must be of type int. It is popped from the operand stack and converted to a double result. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> I2D = new OpCode<SingleInstruction>(Names.I2D, "Convert int to double");
        /// <summary>
        /// The value on the top of the operand stack must be of type long. It is popped from the operand stack and converted to an int result by taking the low-order 32 bits of the long value and discarding the high-order 32 bits. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> L2I = new OpCode<SingleInstruction>(Names.L2I, "Convert long to int");
        /// <summary>
        /// The value on the top of the operand stack must be of type long. It is popped from the operand stack and converted to a float result using IEEE 754 round to nearest mode. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> L2F = new OpCode<SingleInstruction>(Names.L2F, "Convert long to float");
        /// <summary>
        /// The value on the top of the operand stack must be of type long. It is popped from the operand stack and converted to a double result using IEEE 754 round to nearest mode. The result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> L2D = new OpCode<SingleInstruction>(Names.L2D, "Convert long to double");
        /// <summary>
        /// The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. Then value' is converted to an int result. This result is pushed onto the operand stack:
        /// If the value' is NaN, the result of the conversion is an int 0.
        ///
        /// Otherwise, if the value' is not an infinity, it is rounded to an integer value V, rounding towards zero using IEEE 754 round towards zero mode. If this integer value V can be represented as an int, then the result is the int value V.
        ///
        /// Otherwise, either the value' must be too small (a negative value of large magnitude or negative infinity), and the result is the smallest representable value of type int, or the value' must be too large (a positive value of large magnitude or positive infinity), and the result is the largest representable value of type int.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> F2I = new OpCode<SingleInstruction>(Names.F2I, "Convert float to int");
        /// <summary>
        /// The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. Then value' is converted to a long result. This result is pushed onto the operand stack:
        /// If the value' is NaN, the result of the conversion is a long 0.
        ///
        /// Otherwise, if the value' is not an infinity, it is rounded to an integer value V, rounding towards zero using IEEE 754 round towards zero mode. If this integer value V can be represented as a long, then the result is the long value V.
        ///
        /// Otherwise, either the value' must be too small (a negative value of large magnitude or negative infinity), and the result is the smallest representable value of type long, or the value' must be too large (a positive value of large magnitude or positive infinity), and the result is the largest representable value of type long.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> F2L = new OpCode<SingleInstruction>(Names.F2L, "Convert float to long");
        /// <summary>
        /// The value on the top of the operand stack must be of type float. It is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. Then value' is converted to a double result. This result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> F2D = new OpCode<SingleInstruction>(Names.F2D, "Convert float to double");
        /// <summary>
        /// The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3) resulting in value'. Then value' is converted to an int. The result is pushed onto the operand stack:
        /// If the value' is NaN, the result of the conversion is an int 0.
        ///
        /// Otherwise, if the value' is not an infinity, it is rounded to an integer value V, rounding towards zero using IEEE 754 round towards zero mode. If this integer value V can be represented as an int, then the result is the int value V.
        ///
        /// Otherwise, either the value' must be too small (a negative value of large magnitude or negative infinity), and the result is the smallest representable value of type int, or the value' must be too large (a positive value of large magnitude or positive infinity), and the result is the largest representable value of type int.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> D2I = new OpCode<SingleInstruction>(Names.D2I, "Convert double to int");
        /// <summary>
        /// The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3) resulting in value'. Then value' is converted to a long. The result is pushed onto the operand stack:
        /// If the value' is NaN, the result of the conversion is a long 0.
        ///
        /// Otherwise, if the value' is not an infinity, it is rounded to an integer value V, rounding towards zero using IEEE 754 round towards zero mode. If this integer value V can be represented as a long, then the result is the long value V.
        ///
        /// Otherwise, either the value' must be too small (a negative value of large magnitude or negative infinity), and the result is the smallest representable value of type long, or the value' must be too large (a positive value of large magnitude or positive infinity), and the result is the largest representable value of type long.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> D2L = new OpCode<SingleInstruction>(Names.D2L, "Convert double to long");
        /// <summary>
        /// The value on the top of the operand stack must be of type double. It is popped from the operand stack and undergoes value set conversion (§2.8.3) resulting in value'. Then value' is converted to a float result using IEEE 754 round to nearest mode. The result is pushed onto the operand stack.
        /// Where an d2f instruction is FP-strict (§2.8.2), the result of the conversion is always rounded to the nearest representable value in the float value set (§2.3.2).
        /// Where an d2f instruction is not FP-strict, the result of the conversion may be taken from the float-extended-exponent value set (§2.3.2); it is not necessarily rounded to the nearest representable value in the float value set.
        /// A finite value' too small to be represented as a float is converted to a zero of the same sign; a finite value' too large to be represented as a float is converted to an infinity of the same sign. A double NaN is converted to a float NaN.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> D2F = new OpCode<SingleInstruction>(Names.D2F, "Convert double to float");
        /// <summary>
        /// The value on the top of the operand stack must be of type int. It is popped from the operand stack, truncated to a byte, then sign-extended to an int result. That result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> I2B = new OpCode<SingleInstruction>(Names.I2B, "Convert int to byte");
        /// <summary>
        /// The value on the top of the operand stack must be of type int. It is popped from the operand stack, truncated to char, then zero-extended to an int result. That result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> I2C = new OpCode<SingleInstruction>(Names.I2C, "Convert int to char");
        /// <summary>
        /// The value on the top of the operand stack must be of type int. It is popped from the operand stack, truncated to a short, then sign-extended to an int result. That result is pushed onto the operand stack.
        /// <code>
        /// ..., value →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> I2S = new OpCode<SingleInstruction>(Names.I2S, "Convert int to short");
        /// <summary>
        /// Both value1 and value2 must be of type long. They are both popped from the operand stack, and a signed integer comparison is performed. If value1 is greater than value2, the int value 1 is pushed onto the operand stack. If value1 is equal to value2, the int value 0 is pushed onto the operand stack. If value1 is less than value2, the int value -1 is pushed onto the operand stack.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LCMP = new OpCode<SingleInstruction>(Names.LCMP, "Compare long");
        /// <summary>
        /// Both value1 and value2 must be of type float. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. A floating-point comparison is performed:
        /// If value1' is greater than value2', the int value 1 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is equal to value2', the int value 0 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is less than value2', the int value -1 is pushed onto the operand stack.
        ///
        /// Otherwise, at least one of value1' or value2' is NaN. The fcmpg instruction pushes the int value 1 onto the operand stack and the fcmpl instruction pushes the int value -1 onto the operand stack.
        /// Floating-point comparison is performed in accordance with IEEE 754. All values other than NaN are ordered, with negative infinity less than all finite values and positive infinity greater than all finite values. Positive zero and negative zero are considered equal.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FCMPL = new OpCode<SingleInstruction>(Names.FCMPL, "Compare float");
        /// <summary>
        /// Both value1 and value2 must be of type float. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. A floating-point comparison is performed:
        /// If value1' is greater than value2', the int value 1 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is equal to value2', the int value 0 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is less than value2', the int value -1 is pushed onto the operand stack.
        ///
        /// Otherwise, at least one of value1' or value2' is NaN. The fcmpg instruction pushes the int value 1 onto the operand stack and the fcmpl instruction pushes the int value -1 onto the operand stack.
        /// Floating-point comparison is performed in accordance with IEEE 754. All values other than NaN are ordered, with negative infinity less than all finite values and positive infinity greater than all finite values. Positive zero and negative zero are considered equal.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FCMPG = new OpCode<SingleInstruction>(Names.FCMPG, "Compare float");
        /// <summary>
        /// Both value1 and value2 must be of type double. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. A floating-point comparison is performed:
        /// If value1' is greater than value2', the int value 1 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is equal to value2', the int value 0 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is less than value2', the int value -1 is pushed onto the operand stack.
        ///
        /// Otherwise, at least one of value1' or value2' is NaN. The dcmpg instruction pushes the int value 1 onto the operand stack and the dcmpl instruction pushes the int value -1 onto the operand stack.
        /// Floating-point comparison is performed in accordance with IEEE 754. All values other than NaN are ordered, with negative infinity less than all finite values and positive infinity greater than all finite values. Positive zero and negative zero are considered equal.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DCMPL = new OpCode<SingleInstruction>(Names.DCMPL, "Compare double");
        /// <summary>
        /// Both value1 and value2 must be of type double. The values are popped from the operand stack and undergo value set conversion (§2.8.3), resulting in value1' and value2'. A floating-point comparison is performed:
        /// If value1' is greater than value2', the int value 1 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is equal to value2', the int value 0 is pushed onto the operand stack.
        ///
        /// Otherwise, if value1' is less than value2', the int value -1 is pushed onto the operand stack.
        ///
        /// Otherwise, at least one of value1' or value2' is NaN. The dcmpg instruction pushes the int value 1 onto the operand stack and the dcmpl instruction pushes the int value -1 onto the operand stack.
        /// Floating-point comparison is performed in accordance with IEEE 754. All values other than NaN are ordered, with negative infinity less than all finite values and positive infinity greater than all finite values. Positive zero and negative zero are considered equal.
        /// <code>
        /// ..., value1, value2 →
        /// ..., result
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DCMPG = new OpCode<SingleInstruction>(Names.DCMPG, "Compare double");
        /// <summary>
        /// The value must be of type int. It is popped from the operand stack and compared against zero. All comparisons are signed. The results of the comparisons are as follows:
        /// ifeq succeeds if and only if value = 0
        ///
        /// ifne succeeds if and only if value ? 0
        ///
        /// iflt succeeds if and only if value [ 0
        ///
        /// ifle succeeds if and only if value ? 0
        ///
        /// ifgt succeeds if and only if value ] 0
        ///
        /// ifge succeeds if and only if value ? 0
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if[cond] instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFEQ = new OpCode<OffsetInstruction>(Names.IFEQ, "Branch if int comparison with zero succeeds");
        /// <summary>
        /// The value must be of type int. It is popped from the operand stack and compared against zero. All comparisons are signed. The results of the comparisons are as follows:
        /// ifeq succeeds if and only if value = 0
        ///
        /// ifne succeeds if and only if value ? 0
        ///
        /// iflt succeeds if and only if value [ 0
        ///
        /// ifle succeeds if and only if value ? 0
        ///
        /// ifgt succeeds if and only if value ] 0
        ///
        /// ifge succeeds if and only if value ? 0
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if[cond] instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFNE = new OpCode<OffsetInstruction>(Names.IFNE, "Branch if int comparison with zero succeeds");
        /// <summary>
        /// The value must be of type int. It is popped from the operand stack and compared against zero. All comparisons are signed. The results of the comparisons are as follows:
        /// ifeq succeeds if and only if value = 0
        ///
        /// ifne succeeds if and only if value ? 0
        ///
        /// iflt succeeds if and only if value [ 0
        ///
        /// ifle succeeds if and only if value ? 0
        ///
        /// ifgt succeeds if and only if value ] 0
        ///
        /// ifge succeeds if and only if value ? 0
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if[cond] instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFLT = new OpCode<OffsetInstruction>(Names.IFLT, "Branch if int comparison with zero succeeds");
        /// <summary>
        /// The value must be of type int. It is popped from the operand stack and compared against zero. All comparisons are signed. The results of the comparisons are as follows:
        /// ifeq succeeds if and only if value = 0
        ///
        /// ifne succeeds if and only if value ? 0
        ///
        /// iflt succeeds if and only if value [ 0
        ///
        /// ifle succeeds if and only if value ? 0
        ///
        /// ifgt succeeds if and only if value ] 0
        ///
        /// ifge succeeds if and only if value ? 0
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if[cond] instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFGE = new OpCode<OffsetInstruction>(Names.IFGE, "Branch if int comparison with zero succeeds");
        /// <summary>
        /// The value must be of type int. It is popped from the operand stack and compared against zero. All comparisons are signed. The results of the comparisons are as follows:
        /// ifeq succeeds if and only if value = 0
        ///
        /// ifne succeeds if and only if value ? 0
        ///
        /// iflt succeeds if and only if value [ 0
        ///
        /// ifle succeeds if and only if value ? 0
        ///
        /// ifgt succeeds if and only if value ] 0
        ///
        /// ifge succeeds if and only if value ? 0
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if[cond] instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFGT = new OpCode<OffsetInstruction>(Names.IFGT, "Branch if int comparison with zero succeeds");
        /// <summary>
        /// The value must be of type int. It is popped from the operand stack and compared against zero. All comparisons are signed. The results of the comparisons are as follows:
        /// ifeq succeeds if and only if value = 0
        ///
        /// ifne succeeds if and only if value ? 0
        ///
        /// iflt succeeds if and only if value [ 0
        ///
        /// ifle succeeds if and only if value ? 0
        ///
        /// ifgt succeeds if and only if value ] 0
        ///
        /// ifge succeeds if and only if value ? 0
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if[cond] instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFLE = new OpCode<OffsetInstruction>(Names.IFLE, "Branch if int comparison with zero succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are both popped from the operand stack and compared. All comparisons are signed. The results of the comparison are as follows:
        /// if_icmpeq succeeds if and only if value1 = value2
        ///
        /// if_icmpne succeeds if and only if value1 ? value2
        ///
        /// if_icmplt succeeds if and only if value1 [ value2
        ///
        /// if_icmple succeeds if and only if value1 ? value2
        ///
        /// if_icmpgt succeeds if and only if value1 ] value2
        ///
        /// if_icmpge succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_icmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_icmp[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if_icmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ICMPEQ = new OpCode<OffsetInstruction>(Names.IF_ICMPEQ, "Branch if int comparison succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are both popped from the operand stack and compared. All comparisons are signed. The results of the comparison are as follows:
        /// if_icmpeq succeeds if and only if value1 = value2
        ///
        /// if_icmpne succeeds if and only if value1 ? value2
        ///
        /// if_icmplt succeeds if and only if value1 [ value2
        ///
        /// if_icmple succeeds if and only if value1 ? value2
        ///
        /// if_icmpgt succeeds if and only if value1 ] value2
        ///
        /// if_icmpge succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_icmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_icmp[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if_icmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ICMPNE = new OpCode<OffsetInstruction>(Names.IF_ICMPNE, "Branch if int comparison succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are both popped from the operand stack and compared. All comparisons are signed. The results of the comparison are as follows:
        /// if_icmpeq succeeds if and only if value1 = value2
        ///
        /// if_icmpne succeeds if and only if value1 ? value2
        ///
        /// if_icmplt succeeds if and only if value1 [ value2
        ///
        /// if_icmple succeeds if and only if value1 ? value2
        ///
        /// if_icmpgt succeeds if and only if value1 ] value2
        ///
        /// if_icmpge succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_icmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_icmp[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if_icmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ICMPLT = new OpCode<OffsetInstruction>(Names.IF_ICMPLT, "Branch if int comparison succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are both popped from the operand stack and compared. All comparisons are signed. The results of the comparison are as follows:
        /// if_icmpeq succeeds if and only if value1 = value2
        ///
        /// if_icmpne succeeds if and only if value1 ? value2
        ///
        /// if_icmplt succeeds if and only if value1 [ value2
        ///
        /// if_icmple succeeds if and only if value1 ? value2
        ///
        /// if_icmpgt succeeds if and only if value1 ] value2
        ///
        /// if_icmpge succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_icmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_icmp[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if_icmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ICMPGE = new OpCode<OffsetInstruction>(Names.IF_ICMPGE, "Branch if int comparison succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are both popped from the operand stack and compared. All comparisons are signed. The results of the comparison are as follows:
        /// if_icmpeq succeeds if and only if value1 = value2
        ///
        /// if_icmpne succeeds if and only if value1 ? value2
        ///
        /// if_icmplt succeeds if and only if value1 [ value2
        ///
        /// if_icmple succeeds if and only if value1 ? value2
        ///
        /// if_icmpgt succeeds if and only if value1 ] value2
        ///
        /// if_icmpge succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_icmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_icmp[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if_icmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ICMPGT = new OpCode<OffsetInstruction>(Names.IF_ICMPGT, "Branch if int comparison succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type int. They are both popped from the operand stack and compared. All comparisons are signed. The results of the comparison are as follows:
        /// if_icmpeq succeeds if and only if value1 = value2
        ///
        /// if_icmpne succeeds if and only if value1 ? value2
        ///
        /// if_icmplt succeeds if and only if value1 [ value2
        ///
        /// if_icmple succeeds if and only if value1 ? value2
        ///
        /// if_icmpgt succeeds if and only if value1 ] value2
        ///
        /// if_icmpge succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_icmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_icmp[cond] instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this if_icmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ICMPLE = new OpCode<OffsetInstruction>(Names.IF_ICMPLE, "Branch if int comparison succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type reference. They are both popped from the operand stack and compared. The results of the comparison are as follows:
        /// if_acmpeq succeeds if and only if value1 = value2
        ///
        /// if_acmpne succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_acmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_acmp[cond] instruction.
        /// Otherwise, if the comparison fails, execution proceeds at the address of the instruction following this if_acmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ACMPEQ = new OpCode<OffsetInstruction>(Names.IF_ACMPEQ, "Branch if reference comparison succeeds");
        /// <summary>
        /// Both value1 and value2 must be of type reference. They are both popped from the operand stack and compared. The results of the comparison are as follows:
        /// if_acmpeq succeeds if and only if value1 = value2
        ///
        /// if_acmpne succeeds if and only if value1 ? value2
        /// If the comparison succeeds, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this if_acmp[cond] instruction. The target address must be that of an opcode of an instruction within the method that contains this if_acmp[cond] instruction.
        /// Otherwise, if the comparison fails, execution proceeds at the address of the instruction following this if_acmp[cond] instruction.
        /// <code>
        /// ..., value1, value2 →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IF_ACMPNE = new OpCode<OffsetInstruction>(Names.IF_ACMPNE, "Branch if reference comparison succeeds");
        /// <summary>
        /// The unsigned bytes branchbyte1 and branchbyte2 are used to construct a signed 16-bit branchoffset, where branchoffset is (branchbyte1 [[ 8) | branchbyte2. Execution proceeds at that offset from the address of the opcode of this goto instruction. The target address must be that of an opcode of an instruction within the method that contains this goto instruction.
        /// <code>
        /// No change
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> GOTO = new OpCode<OffsetInstruction>(Names.GOTO, "Branch always");
        /// <summary>
        /// The address of the opcode of the instruction immediately following this jsr instruction is pushed onto the operand stack as a value of type returnAddress. The unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is (branchbyte1 [[ 8) | branchbyte2. Execution proceeds at that offset from the address of this jsr instruction. The target address must be that of an opcode of an instruction within the method that contains this jsr instruction.
        /// <code>
        /// ... →
        /// ..., address
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> JSR = new OpCode<OffsetInstruction>(Names.JSR, "Jump subroutine");
        /// <summary>
        /// The index is an unsigned byte between 0 and 255, inclusive. The local variable at index in the current frame (§2.6) must contain a value of type returnAddress. The contents of the local variable are written into the Java Virtual Machine's pc register, and execution continues there.
        /// <code>
        /// No change
        /// </code>
        /// <code>
        /// index
        /// </code>
        /// </summary>
        public static readonly OpCode<FrameIndexInstruction> RET = new OpCode<FrameIndexInstruction>(Names.RET, "Return from subroutine");
        /// <summary>
        /// A tableswitch is a variable-length instruction. Immediately after the tableswitch opcode, between zero and three bytes must act as padding, such that defaultbyte1 begins at an address that is a multiple of four bytes from the start of the current method (the opcode of its first instruction). Immediately after the padding are bytes constituting three signed 32-bit values: default, low, and high. Immediately following are bytes constituting a series of high - low + 1 signed 32-bit offsets. The value low must be less than or equal to high. The high - low + 1 signed 32-bit offsets are treated as a 0-based jump table. Each of these signed 32-bit values is constructed as (byte1 [[ 24) | (byte2 [[ 16) | (byte3 [[ 8) | byte4.
        /// The index must be of type int and is popped from the operand stack. If index is less than low or index is greater than high, then a target address is calculated by adding default to the address of the opcode of this tableswitch instruction. Otherwise, the offset at position index - low of the jump table is extracted. The target address is calculated by adding that offset to the address of the opcode of this tableswitch instruction. Execution then continues at the target address.
        /// The target address that can be calculated from each jump table offset, as well as the one that can be calculated from default, must be the address of an opcode of an instruction within the method that contains this tableswitch instruction.
        /// <code>
        /// ..., index →
        /// ...
        /// </code>
        /// <code>
        /// [0-3 byte pad] defaultbyte1 defaultbyte2 defaultbyte3 defaultbyte4 lowbyte1 lowbyte2 lowbyte3 lowbyte4 highbyte1 highbyte2 highbyte3 highbyte4 jump offsets...
        /// </code>
        /// </summary>
        public static readonly OpCode<TableSwitchInstruction> TABLESWITCH = new OpCode<TableSwitchInstruction>(Names.TABLESWITCH, "Access jump table by index and jump");
        /// <summary>
        /// A lookupswitch is a variable-length instruction. Immediately after the lookupswitch opcode, between zero and three bytes must act as padding, such that defaultbyte1 begins at an address that is a multiple of four bytes from the start of the current method (the opcode of its first instruction). Immediately after the padding follow a series of signed 32-bit values: default, npairs, and then npairs pairs of signed 32-bit values. The npairs must be greater than or equal to 0. Each of the npairs pairs consists of an int match and a signed 32-bit offset. Each of these signed 32-bit values is constructed from four unsigned bytes as (byte1 [[ 24) | (byte2 [[ 16) | (byte3 [[ 8) | byte4.
        /// The table match-offset pairs of the lookupswitch instruction must be sorted in increasing numerical order by match.
        /// The key must be of type int and is popped from the operand stack. The key is compared against the match values. If it is equal to one of them, then a target address is calculated by adding the corresponding offset to the address of the opcode of this lookupswitch instruction. If the key does not match any of the match values, the target address is calculated by adding default to the address of the opcode of this lookupswitch instruction. Execution then continues at the target address.
        /// The target address that can be calculated from the offset of each match-offset pair, as well as the one calculated from default, must be the address of an opcode of an instruction within the method that contains this lookupswitch instruction.
        /// <code>
        /// ..., key →
        /// ...
        /// </code>
        /// <code>
        /// [0-3 byte pad] defaultbyte1 defaultbyte2 defaultbyte3 defaultbyte4 npairs1 npairs2 npairs3 npairs4 match-offset pairs...
        /// </code>
        /// </summary>
        public static readonly OpCode<LookupSwitchInstruction> LOOKUPSWITCH = new OpCode<LookupSwitchInstruction>(Names.LOOKUPSWITCH, "Access jump table by key match and jump");
        /// <summary>
        /// The current method must have return type boolean, byte, short, char, or int. The value must be of type int. If the current method is a synchronized method, the monitor entered or reentered on invocation of the method is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread. If no exception is thrown, value is popped from the operand stack of the current frame (§2.6) and pushed onto the operand stack of the frame of the invoker. Any other values on the operand stack of the current method are discarded.
        /// The interpreter then returns control to the invoker of the method, reinstating the frame of the invoker.
        /// <code>
        /// ..., value →
        /// [empty]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> IRETURN = new OpCode<SingleInstruction>(Names.IRETURN, "Return int from method");
        /// <summary>
        /// The current method must have return type long. The value must be of type long. If the current method is a synchronized method, the monitor entered or reentered on invocation of the method is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread. If no exception is thrown, value is popped from the operand stack of the current frame (§2.6) and pushed onto the operand stack of the frame of the invoker. Any other values on the operand stack of the current method are discarded.
        /// The interpreter then returns control to the invoker of the method, reinstating the frame of the invoker.
        /// <code>
        /// ..., value →
        /// [empty]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> LRETURN = new OpCode<SingleInstruction>(Names.LRETURN, "Return long from method");
        /// <summary>
        /// The current method must have return type float. The value must be of type float. If the current method is a synchronized method, the monitor entered or reentered on invocation of the method is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread. If no exception is thrown, value is popped from the operand stack of the current frame (§2.6) and undergoes value set conversion (§2.8.3), resulting in value'. The value' is pushed onto the operand stack of the frame of the invoker. Any other values on the operand stack of the current method are discarded.
        /// The interpreter then returns control to the invoker of the method, reinstating the frame of the invoker.
        /// <code>
        /// ..., value →
        /// [empty]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> FRETURN = new OpCode<SingleInstruction>(Names.FRETURN, "Return float from method");
        /// <summary>
        /// The current method must have return type double. The value must be of type double. If the current method is a synchronized method, the monitor entered or reentered on invocation of the method is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread. If no exception is thrown, value is popped from the operand stack of the current frame (§2.6) and undergoes value set conversion (§2.8.3), resulting in value'. The value' is pushed onto the operand stack of the frame of the invoker. Any other values on the operand stack of the current method are discarded.
        /// The interpreter then returns control to the invoker of the method, reinstating the frame of the invoker.
        /// <code>
        /// ..., value →
        /// [empty]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> DRETURN = new OpCode<SingleInstruction>(Names.DRETURN, "Return double from method");
        /// <summary>
        /// The objectref must be of type reference and must refer to an object of a type that is assignment compatible (JLS §5.2) with the type represented by the return descriptor (§4.3.3) of the current method. If the current method is a synchronized method, the monitor entered or reentered on invocation of the method is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread. If no exception is thrown, objectref is popped from the operand stack of the current frame (§2.6) and pushed onto the operand stack of the frame of the invoker. Any other values on the operand stack of the current method are discarded.
        /// The interpreter then reinstates the frame of the invoker and returns control to the invoker.
        /// <code>
        /// ..., objectref →
        /// [empty]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ARETURN = new OpCode<SingleInstruction>(Names.ARETURN, "Return reference from method");
        /// <summary>
        /// The current method must have return type void. If the current method is a synchronized method, the monitor entered or reentered on invocation of the method is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread. If no exception is thrown, any values on the operand stack of the current frame (§2.6) are discarded.
        /// The interpreter then returns control to the invoker of the method, reinstating the frame of the invoker.
        /// <code>
        /// ... →
        /// [empty]
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> RETURN = new OpCode<SingleInstruction>(Names.RETURN, "Return void from method");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a field (§5.1), which gives the name and descriptor of the field as well as a symbolic reference to the class or interface in which the field is to be found. The referenced field is resolved (§5.4.3.2).
        /// On successful resolution of the field, the class or interface that declared the resolved field is initialized (§5.5) if that class or interface has not already been initialized.
        /// The value of the class or interface field is fetched and pushed onto the operand stack.
        /// <code>
        /// ..., →
        /// ..., value
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> GETSTATIC = new OpCode<ConstantWideInstruction>(Names.GETSTATIC, "Get static field from class");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a field (§5.1), which gives the name and descriptor of the field as well as a symbolic reference to the class or interface in which the field is to be found. The referenced field is resolved (§5.4.3.2).
        /// On successful resolution of the field, the class or interface that declared the resolved field is initialized (§5.5) if that class or interface has not already been initialized.
        /// The type of a value stored by a putstatic instruction must be compatible with the descriptor of the referenced field (§4.3.2). If the field descriptor type is boolean, byte, char, short, or int, then the value must be an int. If the field descriptor type is float, long, or double, then the value must be a float, long, or double, respectively. If the field descriptor type is a reference type, then the value must be of a type that is assignment compatible (JLS §5.2) with the field descriptor type. If the field is final, it must be declared in the current class, and the instruction must occur in the [clinit] method of the current class (§2.9).
        /// The value is popped from the operand stack and undergoes value set conversion (§2.8.3), resulting in value'. The class field is set to value'.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> PUTSTATIC = new OpCode<ConstantWideInstruction>(Names.PUTSTATIC, "Set static field in class");
        /// <summary>
        /// The objectref, which must be of type reference, is popped from the operand stack. The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a field (§5.1), which gives the name and descriptor of the field as well as a symbolic reference to the class in which the field is to be found. The referenced field is resolved (§5.4.3.2). The value of the referenced field in objectref is fetched and pushed onto the operand stack.
        /// The type of objectref must not be an array type. If the field is protected (§4.6), and it is a member of a superclass of the current class, and the field is not declared in the same run-time package (§5.3) as the current class, then the class of objectref must be either the current class or a subclass of the current class.
        /// <code>
        /// ..., objectref →
        /// ..., value
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> GETFIELD = new OpCode<ConstantWideInstruction>(Names.GETFIELD, "Fetch field from object");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a field (§5.1), which gives the name and descriptor of the field as well as a symbolic reference to the class in which the field is to be found. The class of objectref must not be an array. If the field is protected (§4.6), and it is a member of a superclass of the current class, and the field is not declared in the same run-time package (§5.3) as the current class, then the class of objectref must be either the current class or a subclass of the current class.
        /// The referenced field is resolved (§5.4.3.2). The type of a value stored by a putfield instruction must be compatible with the descriptor of the referenced field (§4.3.2). If the field descriptor type is boolean, byte, char, short, or int, then the value must be an int. If the field descriptor type is float, long, or double, then the value must be a float, long, or double, respectively. If the field descriptor type is a reference type, then the value must be of a type that is assignment compatible (JLS §5.2) with the field descriptor type. If the field is final, it must be declared in the current class, and the instruction must occur in an instance initialization method ([init]) of the current class (§2.9).
        /// The value and objectref are popped from the operand stack. The objectref must be of type reference. The value undergoes value set conversion (§2.8.3), resulting in value', and the referenced field in objectref is set to value'.
        /// <code>
        /// ..., objectref, value →
        /// ...
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> PUTFIELD = new OpCode<ConstantWideInstruction>(Names.PUTFIELD, "Set field in object");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a method (§5.1), which gives the name and descriptor (§4.3.3) of the method as well as a symbolic reference to the class in which the method is to be found. The named method is resolved (§5.4.3.3). The resolved method must not be an instance initialization method (§2.9) or the class or interface initialization method (§2.9). Finally, if the resolved method is protected (§4.6), and it is a member of a superclass of the current class, and the method is not declared in the same run-time package (§5.3) as the current class, then the class of objectref must be either the current class or a subclass of the current class.
        /// If the resolved method is not signature polymorphic (§2.9), then the invokevirtual instruction proceeds as follows.
        /// Let C be the class of objectref. The actual method to be invoked is selected by the following lookup procedure:
        /// If C contains a declaration for an instance method m that overrides (§5.4.5) the resolved method, then m is the method to be invoked, and the lookup procedure terminates.
        ///
        /// Otherwise, if C has a superclass, this same lookup procedure is performed recursively using the direct superclass of C; the method to be invoked is the result of the recursive invocation of this lookup procedure.
        ///
        /// Otherwise, an AbstractMethodError is raised.
        /// The objectref must be followed on the operand stack by nargs argument values, where the number, type, and order of the values must be consistent with the descriptor of the selected instance method.
        /// If the method is synchronized, the monitor associated with objectref is entered or reentered as if by execution of a monitorenter instruction (§monitorenter) in the current thread.
        /// If the method is not native, the nargs argument values and objectref are popped from the operand stack. A new frame is created on the Java Virtual Machine stack for the method being invoked. The objectref and the argument values are consecutively made the values of local variables of the new frame, with objectref in local variable 0, arg1 in local variable 1 (or, if arg1 is of type long or double, in local variables 1 and 2), and so on. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being stored in a local variable. The new frame is then made current, and the Java Virtual Machine pc is set to the opcode of the first instruction of the method to be invoked. Execution continues with the first instruction of the method.
        /// If the method is native and the platform-dependent code that implements it has not yet been bound (§5.6) into the Java Virtual Machine, that is done. The nargs argument values and objectref are popped from the operand stack and are passed as parameters to the code that implements the method. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being passed as a parameter. The parameters are passed and the code is invoked in an implementation-dependent manner. When the platform-dependent code returns, the following take place:
        /// If the native method is synchronized, the monitor associated with objectref is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread.
        ///
        /// If the native method returns a value, the return value of the platform-dependent code is converted in an implementation-dependent way to the return type of the native method and pushed onto the operand stack.
        /// If the resolved method is signature polymorphic (§2.9), then the invokevirtual instruction proceeds as follows.
        /// First, a reference to an instance of java.lang.invoke.MethodType is obtained as if by resolution of a symbolic reference to a method type (§5.4.3.5) with the same parameter and return types as the descriptor of the method referenced by the invokevirtual instruction.
        /// If the named method is invokeExact, the instance of java.lang.invoke.MethodType must be semantically equal to the type descriptor of the receiving method handle objectref. The method handle to be invoked is objectref.
        ///
        /// If the named method is invoke, and the instance of java.lang.invoke.MethodType is semantically equal to the type descriptor of the receiving method handle objectref, then the method handle to be invoked is objectref.
        ///
        /// If the named method is invoke, and the instance of java.lang.invoke.MethodType is not semantically equal to the type descriptor of the receiving method handle objectref, then the Java Virtual Machine attempts to adjust the type descriptor of the receiving method handle, as if by a call to java.lang.invoke.MethodHandle.asType, to obtain an exactly invokable method handle m. The method handle to be invoked is m.
        /// The objectref must be followed on the operand stack by nargs argument values, where the number, type, and order of the values must be consistent with the type descriptor of the method handle to be invoked. (This type descriptor will correspond to the method descriptor appropriate for the kind of the method handle to be invoked, as specified in §5.4.3.5.)
        /// Then, if the method handle to be invoked has bytecode behavior, the Java Virtual Machine invokes the method handle as if by execution of the bytecode behavior associated with the method handle's kind. If the kind is 5 (REF_invokeVirtual), 6 (REF_invokeStatic), 7 (REF_invokeSpecial), 8 (REF_newInvokeSpecial), or 9 (REF_invokeInterface), then a frame will be created and made current in the course of executing the bytecode behavior; when the method invoked by the bytecode behavior completes (normally or abruptly), the frame of its invoker is considered to be the frame for the method containing this invokevirtual instruction.
        /// The frame in which the bytecode behavior itself executes is not visible.
        /// Otherwise, if the method handle to be invoked has no bytecode behavior, the Java Virtual Machine invokes it in an implementation-dependent manner.
        /// <code>
        /// ..., objectref, [arg1, [arg2 ...]] →
        /// ...
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> INVOKEVIRTUAL = new OpCode<ConstantWideInstruction>(Names.INVOKEVIRTUAL, "Invoke instance method; dispatch based on class");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a method (§5.1), which gives the name and descriptor (§4.3.3) of the method as well as a symbolic reference to the class in which the method is to be found. The named method is resolved (§5.4.3.3). Finally, if the resolved method is protected (§4.6), and it is a member of a superclass of the current class, and the method is not declared in the same run-time package (§5.3) as the current class, then the class of objectref must be either the current class or a subclass of the current class.
        /// Next, the resolved method is selected for invocation unless all of the following conditions are true:
        /// The ACC_SUPER flag (Table 4.1) is set for the current class.
        ///
        /// The class of the resolved method is a superclass of the current class.
        ///
        /// The resolved method is not an instance initialization method (§2.9).
        /// If the above conditions are true, the actual method to be invoked is selected by the following lookup procedure. Let C be the direct superclass of the current class:
        /// If C contains a declaration for an instance method with the same name and descriptor as the resolved method, then this method will be invoked. The lookup procedure terminates.
        ///
        /// Otherwise, if C has a superclass, this same lookup procedure is performed recursively using the direct superclass of C. The method to be invoked is the result of the recursive invocation of this lookup procedure.
        ///
        /// Otherwise, an AbstractMethodError is raised.
        /// The objectref must be of type reference and must be followed on the operand stack by nargs argument values, where the number, type, and order of the values must be consistent with the descriptor of the selected instance method.
        /// If the method is synchronized, the monitor associated with objectref is entered or reentered as if by execution of a monitorenter instruction (§monitorenter) in the current thread.
        /// If the method is not native, the nargs argument values and objectref are popped from the operand stack. A new frame is created on the Java Virtual Machine stack for the method being invoked. The objectref and the argument values are consecutively made the values of local variables of the new frame, with objectref in local variable 0, arg1 in local variable 1 (or, if arg1 is of type long or double, in local variables 1 and 2), and so on. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being stored in a local variable. The new frame is then made current, and the Java Virtual Machine pc is set to the opcode of the first instruction of the method to be invoked. Execution continues with the first instruction of the method.
        /// If the method is native and the platform-dependent code that implements it has not yet been bound (§5.6) into the Java Virtual Machine, that is done. The nargs argument values and objectref are popped from the operand stack and are passed as parameters to the code that implements the method. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being passed as a parameter. The parameters are passed and the code is invoked in an implementation-dependent manner. When the platform-dependent code returns, the following take place:
        /// If the native method is synchronized, the monitor associated with objectref is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread.
        ///
        /// If the native method returns a value, the return value of the platform-dependent code is converted in an implementation-dependent way to the return type of the native method and pushed onto the operand stack.
        /// <code>
        /// ..., objectref, [arg1, [arg2 ...]] →
        /// ...
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> INVOKESPECIAL = new OpCode<ConstantWideInstruction>(Names.INVOKESPECIAL, "Invoke instance method; special handling for superclass, private, and instance initialization method invocations");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a method (§5.1), which gives the name and descriptor (§4.3.3) of the method as well as a symbolic reference to the class in which the method is to be found. The named method is resolved (§5.4.3.3). The resolved method must not be an instance initialization method (§2.9) or the class or interface initialization method (§2.9). It must be static, and therefore cannot be abstract.
        /// On successful resolution of the method, the class that declared the resolved method is initialized (§5.5) if that class has not already been initialized.
        /// The operand stack must contain nargs argument values, where the number, type, and order of the values must be consistent with the descriptor of the resolved method.
        /// If the method is synchronized, the monitor associated with the resolved Class object is entered or reentered as if by execution of a monitorenter instruction (§monitorenter) in the current thread.
        /// If the method is not native, the nargs argument values are popped from the operand stack. A new frame is created on the Java Virtual Machine stack for the method being invoked. The nargs argument values are consecutively made the values of local variables of the new frame, with arg1 in local variable 0 (or, if arg1 is of type long or double, in local variables 0 and 1) and so on. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being stored in a local variable. The new frame is then made current, and the Java Virtual Machine pc is set to the opcode of the first instruction of the method to be invoked. Execution continues with the first instruction of the method.
        /// If the method is native and the platform-dependent code that implements it has not yet been bound (§5.6) into the Java Virtual Machine, that is done. The nargs argument values are popped from the operand stack and are passed as parameters to the code that implements the method. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being passed as a parameter. The parameters are passed and the code is invoked in an implementation-dependent manner. When the platform-dependent code returns, the following take place:
        /// If the native method is synchronized, the monitor associated with the resolved Class object is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread.
        ///
        /// If the native method returns a value, the return value of the platform-dependent code is converted in an implementation-dependent way to the return type of the native method and pushed onto the operand stack.
        /// <code>
        /// ..., [arg1, [arg2 ...]] →
        /// ...
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> INVOKESTATIC = new OpCode<ConstantWideInstruction>(Names.INVOKESTATIC, "Invoke a class (static) method");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to an interface method (§5.1), which gives the name and descriptor (§4.3.3) of the interface method as well as a symbolic reference to the interface in which the interface method is to be found. The named interface method is resolved (§5.4.3.4). The resolved interface method must not be an instance initialization method (§2.9) or the class or interface initialization method (§2.9).
        /// The count operand is an unsigned byte that must not be zero. The objectref must be of type reference and must be followed on the operand stack by nargs argument values, where the number, type, and order of the values must be consistent with the descriptor of the resolved interface method. The value of the fourth operand byte must always be zero.
        /// Let C be the class of objectref. The actual method to be invoked is selected by the following lookup procedure:
        /// If C contains a declaration for an instance method with the same name and descriptor as the resolved method, then this is the method to be invoked, and the lookup procedure terminates.
        ///
        /// Otherwise, if C has a superclass, this same lookup procedure is performed recursively using the direct superclass of C; the method to be invoked is the result of the recursive invocation of this lookup procedure.
        ///
        /// Otherwise, an AbstractMethodError is raised.
        /// If the method is synchronized, the monitor associated with objectref is entered or reentered as if by execution of a monitorenter instruction (§monitorenter) in the current thread.
        /// If the method is not native, the nargs argument values and objectref are popped from the operand stack. A new frame is created on the Java Virtual Machine stack for the method being invoked. The objectref and the argument values are consecutively made the values of local variables of the new frame, with objectref in local variable 0, arg1 in local variable 1 (or, if arg1 is of type long or double, in local variables 1 and 2), and so on. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being stored in a local variable. The new frame is then made current, and the Java Virtual Machine pc is set to the opcode of the first instruction of the method to be invoked. Execution continues with the first instruction of the method.
        /// If the method is native and the platform-dependent code that implements it has not yet been bound (§5.6) into the Java Virtual Machine, that is done. The nargs argument values and objectref are popped from the operand stack and are passed as parameters to the code that implements the method. Any argument value that is of a floating-point type undergoes value set conversion (§2.8.3) prior to being passed as a parameter. The parameters are passed and the code is invoked in an implementation-dependent manner. When the platform-dependent code returns:
        /// If the native method is synchronized, the monitor associated with objectref is updated and possibly exited as if by execution of a monitorexit instruction (§monitorexit) in the current thread.
        ///
        /// If the native method returns a value, the return value of the platform-dependent code is converted in an implementation-dependent way to the return type of the native method and pushed onto the operand stack.
        /// <code>
        /// ..., objectref, [arg1, [arg2 ...]] →
        /// ...
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2 count 0
        /// </code>
        /// </summary>
        public static readonly OpCode<InvokeInterfaceInstruction> INVOKEINTERFACE = new OpCode<InvokeInterfaceInstruction>(Names.INVOKEINTERFACE, "Invoke interface method");
        /// <summary>
        /// Each specific lexical occurrence of an invokedynamic instruction is called a dynamic call site.
        /// First, the unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a call site specifier (§5.1). The values of the third and fourth operand bytes must always be zero.
        /// The call site specifier is resolved (§5.4.3.6) for this specific dynamic call site to obtain a reference to a java.lang.invoke.MethodHandle instance, a reference to a java.lang.invoke.MethodType instance, and references to static arguments.
        /// Next, as part of the continuing resolution of the call site specifier, the bootstrap method is invoked as if by execution of an invokevirtual instruction (§invokevirtual) that contains a run-time constant pool index to a symbolic reference to a method (§5.1) with the following properties:
        /// The method's name is invoke;
        ///
        /// The method's descriptor has a return type of java.lang.invoke.CallSite;
        ///
        /// The method's descriptor has parameter types derived from the items pushed on to the operand stack, as follows.
        ///
        /// The first four parameter types in the descriptor are java.lang.invoke.MethodHandle, java.lang.invoke.MethodHandles.Lookup, String, and java.lang.invoke.MethodType, in that order.
        ///
        /// If the call site specifier has any static arguments, then a parameter type for each argument is appended to the parameter types of the method descriptor in the order that the arguments were pushed on to the operand stack. These parameter types may be Class, java.lang.invoke.MethodHandle, java.lang.invoke.MethodType, String, int, long, float, or double.
        ///
        /// The method's symbolic reference to the class in which the method is to be found indicates the class java.lang.invoke.MethodHandle.
        /// where it is as if the following items were pushed, in order, onto the operand stack:
        /// the reference to the java.lang.invoke.MethodHandle object for the bootstrap method;
        ///
        /// a reference to a java.lang.invoke.MethodHandles.Lookup object for the class in which this dynamic call site occurs;
        ///
        /// a reference to the String for the method name in the call site specifier;
        ///
        /// the reference to the java.lang.invoke.MethodType object obtained for the method descriptor in the call site specifier;
        ///
        /// references to classes, method types, method handles, and string literals denoted as static arguments in the call site specifier, and numeric values (§2.3.1, §2.3.2) denoted as static arguments in the call site specifier, in the order in which they appear in the call site specifier. (That is, no boxing occurs for primitive values.)
        /// As long as the bootstrap method can be correctly invoked by the invoke method, its descriptor is arbitrary. For example, the first parameter type could be Object instead of java.lang.invoke.MethodHandles.Lookup, and the return type could also be Object instead of java.lang.invoke.CallSite.
        /// If the bootstrap method is a variable arity method, then some or all of the arguments on the operand stack specified above may be collected into a trailing array parameter.
        /// The invocation of a bootstrap method occurs within a thread that is attempting resolution of the symbolic reference to the call site specifier of this dynamic call site. If there are several such threads, the bootstrap method may be invoked in several threads concurrently. Therefore, bootstrap methods which access global application data must take the usual precautions against race conditions.
        /// The result returned by the bootstrap method must be a reference to an object whose class is java.lang.invoke.CallSite or a subclass of java.lang.invoke.CallSite. This object is known as the call site object. The reference is popped from the operand stack used as if in the execution of an invokevirtual instruction.
        /// If several threads simultaneously execute the bootstrap method for the same dynamic call site, the Java Virtual Machine must choose one returned call site object and install it visibly to all threads. Any other bootstrap methods executing for the dynamic call site are allowed to complete, but their results are ignored, and the threads' execution of the dynamic call site proceeds with the chosen call site object.
        /// The call site object has a type descriptor (an instance of java.lang.invoke.MethodType) which must be semantically equal to the java.lang.invoke.MethodType object obtained for the method descriptor in the call site specifier.
        /// The result of successful call site specifier resolution is a call site object which is permanently bound to the dynamic call site.
        /// The method handle represented by the target of the bound call site object is invoked. The invocation occurs as if by execution of an invokevirtual instruction (§invokevirtual) that indicates a run-time constant pool index to a symbolic reference to a method (§5.1) with the following properties:
        /// The method's name is invokeExact;
        ///
        /// The method's descriptor is the method descriptor in the call site specifier; and
        ///
        /// The method's symbolic reference to the class in which the method is to be found indicates the class java.lang.invoke.MethodHandle.
        /// The operand stack will be interpreted as containing a reference to the target of the call site object, followed by nargs argument values, where the number, type, and order of the values must be consistent with the method descriptor in the call site specifier.
        /// <code>
        /// ..., [arg1, [arg2 ...]] →
        /// ...
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2 0 0
        /// </code>
        /// </summary>
        public static readonly OpCode<InvokeDynamicInstruction> INVOKEDYNAMIC = new OpCode<InvokeDynamicInstruction>(Names.INVOKEDYNAMIC, "Invoke dynamic method");
        /// <summary>
        /// The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at the index must be a symbolic reference to a class or interface type. The named class or interface type is resolved (§5.4.3.1) and should result in a class type. Memory for a new instance of that class is allocated from the garbage-collected heap, and the instance variables of the new object are initialized to their default initial values (§2.3, §2.4). The objectref, a reference to the instance, is pushed onto the operand stack.
        /// On successful resolution of the class, it is initialized (§5.5) if it has not already been initialized.
        /// <code>
        /// ... →
        /// ..., objectref
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> NEW = new OpCode<ConstantWideInstruction>(Names.NEW, "Create new object");
        /// <summary>
        /// The count must be of type int. It is popped off the operand stack. The count represents the number of elements in the array to be created.
        /// The atype is a code that indicates the type of array to create. It must take one of the following values:
        /// Table 6.1. Array type codes
        ///
        /// Array Type  atype
        /// T_BOOLEAN   4
        /// T_CHAR      5
        /// T_FLOAT     6
        /// T_DOUBLE    7
        /// T_BYTE      8
        /// T_SHORT     9
        /// T_INT       10
        /// T_LONG      11
        /// A new array whose components are of type atype and of length count is allocated from the garbage-collected heap. A reference arrayref to this new array object is pushed into the operand stack. Each of the elements of the new array is initialized to the default initial value (§2.3, §2.4) for the element type of the array type.
        /// <code>
        /// ..., count →
        /// ..., arrayref
        /// </code>
        /// <code>
        /// atype
        /// </code>
        /// </summary>
        public static readonly OpCode<NewArrayInstruction> NEWARRAY = new OpCode<NewArrayInstruction>(Names.NEWARRAY, "Create new array");
        /// <summary>
        /// The count must be of type int. It is popped off the operand stack. The count represents the number of components of the array to be created. The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at that index must be a symbolic reference to a class, array, or interface type. The named class, array, or interface type is resolved (§5.4.3.1). A new array with components of that type, of length count, is allocated from the garbage-collected heap, and a reference arrayref to this new array object is pushed onto the operand stack. All components of the new array are initialized to null, the default value for reference types (§2.4).
        /// <code>
        /// ..., count →
        /// ..., arrayref
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> ANEWARRAY = new OpCode<ConstantWideInstruction>(Names.ANEWARRAY, "Create new array of reference");
        /// <summary>
        /// The arrayref must be of type reference and must refer to an array. It is popped from the operand stack. The length of the array it references is determined. That length is pushed onto the operand stack as an int.
        /// <code>
        /// ..., arrayref →
        /// ..., length
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ARRAYLENGTH = new OpCode<SingleInstruction>(Names.ARRAYLENGTH, "Get length of array");
        /// <summary>
        /// The objectref must be of type reference and must refer to an object that is an instance of class Throwable or of a subclass of Throwable. It is popped from the operand stack. The objectref is then thrown by searching the current method (§2.6) for the first exception handler that matches the class of objectref, as given by the algorithm in §2.10.
        /// If an exception handler that matches objectref is found, it contains the location of the code intended to handle this exception. The pc register is reset to that location, the operand stack of the current frame is cleared, objectref is pushed back onto the operand stack, and execution continues.
        /// If no matching exception handler is found in the current frame, that frame is popped. If the current frame represents an invocation of a synchronized method, the monitor entered or reentered on invocation of the method is exited as if by execution of a monitorexit instruction (§monitorexit). Finally, the frame of its invoker is reinstated, if such a frame exists, and the objectref is rethrown. If no such frame exists, the current thread exits.
        /// <code>
        /// ..., objectref →
        /// objectref
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> ATHROW = new OpCode<SingleInstruction>(Names.ATHROW, "Throw exception or error");
        /// <summary>
        /// The objectref must be of type reference. The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at the index must be a symbolic reference to a class, array, or interface type.
        /// If objectref is null, then the operand stack is unchanged.
        /// Otherwise, the named class, array, or interface type is resolved (§5.4.3.1). If objectref can be cast to the resolved class, array, or interface type, the operand stack is unchanged; otherwise, the checkcast instruction throws a ClassCastException.
        /// The following rules are used to determine whether an objectref that is not null can be cast to the resolved type: if S is the class of the object referred to by objectref and T is the resolved class, array, or interface type, checkcast determines whether objectref can be cast to type T as follows:
        /// If S is an ordinary (nonarray) class, then:
        ///
        /// If T is a class type, then S must be the same class as T, or S must be a subclass of T;
        ///
        /// If T is an interface type, then S must implement interface T.
        ///
        /// If S is an interface type, then:
        ///
        /// If T is a class type, then T must be Object.
        ///
        /// If T is an interface type, then T must be the same interface as S or a superinterface of S.
        ///
        /// If S is a class representing the array type SC[], that is, an array of components of type SC, then:
        ///
        /// If T is a class type, then T must be Object.
        ///
        /// If T is an interface type, then T must be one of the interfaces implemented by arrays (JLS §4.10.3).
        ///
        /// If T is an array type TC[], that is, an array of components of type TC, then one of the following must be true:
        ///
        /// TC and SC are the same primitive type.
        ///
        /// TC and SC are reference types, and type SC can be cast to TC by recursive application of these rules.
        /// <code>
        /// ..., objectref →
        /// ..., objectref
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> CHECKCAST = new OpCode<ConstantWideInstruction>(Names.CHECKCAST, "Check whether object is of given type");
        /// <summary>
        /// The objectref, which must be of type reference, is popped from the operand stack. The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at the index must be a symbolic reference to a class, array, or interface type.
        /// If objectref is null, the instanceof instruction pushes an int result of 0 as an int on the operand stack.
        /// Otherwise, the named class, array, or interface type is resolved (§5.4.3.1). If objectref is an instance of the resolved class or array or implements the resolved interface, the instanceof instruction pushes an int result of 1 as an int on the operand stack; otherwise, it pushes an int result of 0.
        /// The following rules are used to determine whether an objectref that is not null is an instance of the resolved type: If S is the class of the object referred to by objectref and T is the resolved class, array, or interface type, instanceof determines whether objectref is an instance of T as follows:
        /// If S is an ordinary (nonarray) class, then:
        ///
        /// If T is a class type, then S must be the same class as T, or S must be a subclass of T;
        ///
        /// If T is an interface type, then S must implement interface T.
        ///
        /// If S is an interface type, then:
        ///
        /// If T is a class type, then T must be Object.
        ///
        /// If T is an interface type, then T must be the same interface as S or a superinterface of S.
        ///
        /// If S is a class representing the array type SC[], that is, an array of components of type SC, then:
        ///
        /// If T is a class type, then T must be Object.
        ///
        /// If T is an interface type, then T must be one of the interfaces implemented by arrays (JLS §4.10.3).
        ///
        /// If T is an array type TC[], that is, an array of components of type TC, then one of the following must be true:
        ///
        /// TC and SC are the same primitive type.
        ///
        /// TC and SC are reference types, and type SC can be cast to TC by these run-time rules.
        /// <code>
        /// ..., objectref →
        /// ..., result
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<ConstantWideInstruction> INSTANCEOF = new OpCode<ConstantWideInstruction>(Names.INSTANCEOF, "Determine if object is of given type");
        /// <summary>
        /// The objectref must be of type reference.
        /// Each object is associated with a monitor. A monitor is locked if and only if it has an owner. The thread that executes monitorenter attempts to gain ownership of the monitor associated with objectref, as follows:
        /// If the entry count of the monitor associated with objectref is zero, the thread enters the monitor and sets its entry count to one. The thread is then the owner of the monitor.
        ///
        /// If the thread already owns the monitor associated with objectref, it reenters the monitor, incrementing its entry count.
        ///
        /// If another thread already owns the monitor associated with objectref, the thread blocks until the monitor's entry count is zero, then tries again to gain ownership.
        /// <code>
        /// ..., objectref →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> MONITORENTER = new OpCode<SingleInstruction>(Names.MONITORENTER, "Enter monitor for object");
        /// <summary>
        /// The objectref must be of type reference.
        /// The thread that executes monitorexit must be the owner of the monitor associated with the instance referenced by objectref.
        /// The thread decrements the entry count of the monitor associated with objectref. If as a result the value of the entry count is zero, the thread exits the monitor and is no longer its owner. Other threads that are blocking to enter the monitor are allowed to attempt to do so.
        /// <code>
        /// ..., objectref →
        /// ...
        /// </code>
        /// </summary>
        public static readonly OpCode<SingleInstruction> MONITOREXIT = new OpCode<SingleInstruction>(Names.MONITOREXIT, "Exit monitor for object");
        /// <summary>
        /// The wide instruction modifies the behavior of another instruction. It takes one of two formats, depending on the instruction being modified. The first form of the wide instruction modifies one of the instructions iload, fload, aload, lload, dload, istore, fstore, astore, lstore, dstore, or ret (§iload, §fload, §aload, §lload, §dload, §istore, §fstore, §astore, §lstore, §dstore, §ret). The second form applies only to the iinc instruction (§iinc).
        /// In either case, the wide opcode itself is followed in the compiled code by the opcode of the instruction wide modifies. In either form, two unsigned bytes indexbyte1 and indexbyte2 follow the modified opcode and are assembled into a 16-bit unsigned index to a local variable in the current frame (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The calculated index must be an index into the local variable array of the current frame. Where the wide instruction modifies an lload, dload, lstore, or dstore instruction, the index following the calculated index (index + 1) must also be an index into the local variable array. In the second form, two immediate unsigned bytes constbyte1 and constbyte2 follow indexbyte1 and indexbyte2 in the code stream. Those bytes are also assembled into a signed 16-bit constant, where the constant is (constbyte1 [[ 8) | constbyte2.
        /// The widened bytecode operates as normal, except for the use of the wider index and, in the case of the second form, the larger increment range.
        /// <code>
        /// Same as modified instruction
        /// </code>
        /// <code>
        /// [opcode] indexbyte1 indexbyte2  where [opcode] is one of iload, fload, aload, lload, dload, istore, fstore, astore, lstore, dstore, or ret
        /// iinc indexbyte1 indexbyte2 constbyte1 constbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<WideInstruction> WIDE = new OpCode<WideInstruction>(Names.WIDE, "Extend local variable index by additional bytes");
        /// <summary>
        /// The dimensions operand is an unsigned byte that must be greater than or equal to 1. It represents the number of dimensions of the array to be created. The operand stack must contain dimensions values. Each such value represents the number of components in a dimension of the array to be created, must be of type int, and must be non-negative. The count1 is the desired length in the first dimension, count2 in the second, etc.
        /// All of the count values are popped off the operand stack. The unsigned indexbyte1 and indexbyte2 are used to construct an index into the run-time constant pool of the current class (§2.6), where the value of the index is (indexbyte1 [[ 8) | indexbyte2. The run-time constant pool item at the index must be a symbolic reference to a class, array, or interface type. The named class, array, or interface type is resolved (§5.4.3.1). The resulting entry must be an array class type of dimensionality greater than or equal to dimensions.
        /// A new multidimensional array of the array type is allocated from the garbage-collected heap. If any count value is zero, no subsequent dimensions are allocated. The components of the array in the first dimension are initialized to subarrays of the type of the second dimension, and so on. The components of the last allocated dimension of the array are initialized to the default initial value (§2.3, §2.4) for the element type of the array type. A reference arrayref to the new array is pushed onto the operand stack.
        /// <code>
        /// ..., count1, [count2, ...] →
        /// ..., arrayref
        /// </code>
        /// <code>
        /// indexbyte1 indexbyte2 dimensions
        /// </code>
        /// </summary>
        public static readonly OpCode<MultiANewArrayInstruction> MULTIANEWARRAY = new OpCode<MultiANewArrayInstruction>(Names.MULTIANEWARRAY, "Create new multidimensional array");
        /// <summary>
        /// The value must of type reference. It is popped from the operand stack. If value is null, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this ifnull instruction. The target address must be that of an opcode of an instruction within the method that contains this ifnull instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this ifnull instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFNULL = new OpCode<OffsetInstruction>(Names.IFNULL, "Branch if reference is null");
        /// <summary>
        /// The value must be of type reference. It is popped from the operand stack. If value is not null, the unsigned branchbyte1 and branchbyte2 are used to construct a signed 16-bit offset, where the offset is calculated to be (branchbyte1 [[ 8) | branchbyte2. Execution then proceeds at that offset from the address of the opcode of this ifnonnull instruction. The target address must be that of an opcode of an instruction within the method that contains this ifnonnull instruction.
        /// Otherwise, execution proceeds at the address of the instruction following this ifnonnull instruction.
        /// <code>
        /// ..., value →
        /// ...
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetInstruction> IFNONNULL = new OpCode<OffsetInstruction>(Names.IFNONNULL, "Branch if reference not null");
        /// <summary>
        /// The unsigned bytes branchbyte1, branchbyte2, branchbyte3, and branchbyte4 are used to construct a signed 32-bit branchoffset, where branchoffset is (branchbyte1 [[ 24) | (branchbyte2 [[ 16) | (branchbyte3 [[ 8) | branchbyte4. Execution proceeds at that offset from the address of the opcode of this goto_w instruction. The target address must be that of an opcode of an instruction within the method that contains this goto_w instruction.
        /// <code>
        /// No change
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2 branchbyte3 branchbyte4
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetWideInstruction> GOTO_W = new OpCode<OffsetWideInstruction>(Names.GOTO_W, "Branch always (wide index)");
        /// <summary>
        /// The address of the opcode of the instruction immediately following this jsr_w instruction is pushed onto the operand stack as a value of type returnAddress. The unsigned branchbyte1, branchbyte2, branchbyte3, and branchbyte4 are used to construct a signed 32-bit offset, where the offset is (branchbyte1 [[ 24) | (branchbyte2 [[ 16) | (branchbyte3 [[ 8) | branchbyte4. Execution proceeds at that offset from the address of this jsr_w instruction. The target address must be that of an opcode of an instruction within the method that contains this jsr_w instruction.
        /// <code>
        /// ... →
        /// ..., address
        /// </code>
        /// <code>
        /// branchbyte1 branchbyte2 branchbyte3 branchbyte4
        /// </code>
        /// </summary>
        public static readonly OpCode<OffsetWideInstruction> JSR_W = new OpCode<OffsetWideInstruction>(Names.JSR_W, "Jump subroutine (wide index)");
        public static IOpCode[] OpCodeList => new IOpCode[]
        {
            NOP,
            ACONST_NULL,
            ICONST_M1,
            ICONST_0,
            ICONST_1,
            ICONST_2,
            ICONST_3,
            ICONST_4,
            ICONST_5,
            LCONST_0,
            LCONST_1,
            FCONST_0,
            FCONST_1,
            FCONST_2,
            DCONST_0,
            DCONST_1,
            BIPUSH,
            SIPUSH,
            LDC,
            LDC_W,
            LDC2_W,
            ILOAD,
            LLOAD,
            FLOAD,
            DLOAD,
            ALOAD,
            ILOAD_0,
            ILOAD_1,
            ILOAD_2,
            ILOAD_3,
            LLOAD_0,
            LLOAD_1,
            LLOAD_2,
            LLOAD_3,
            FLOAD_0,
            FLOAD_1,
            FLOAD_2,
            FLOAD_3,
            DLOAD_0,
            DLOAD_1,
            DLOAD_2,
            DLOAD_3,
            ALOAD_0,
            ALOAD_1,
            ALOAD_2,
            ALOAD_3,
            IALOAD,
            LALOAD,
            FALOAD,
            DALOAD,
            AALOAD,
            BALOAD,
            CALOAD,
            SALOAD,
            ISTORE,
            LSTORE,
            FSTORE,
            DSTORE,
            ASTORE,
            ISTORE_0,
            ISTORE_1,
            ISTORE_2,
            ISTORE_3,
            LSTORE_0,
            LSTORE_1,
            LSTORE_2,
            LSTORE_3,
            FSTORE_0,
            FSTORE_1,
            FSTORE_2,
            FSTORE_3,
            DSTORE_0,
            DSTORE_1,
            DSTORE_2,
            DSTORE_3,
            ASTORE_0,
            ASTORE_1,
            ASTORE_2,
            ASTORE_3,
            IASTORE,
            LASTORE,
            FASTORE,
            DASTORE,
            AASTORE,
            BASTORE,
            CASTORE,
            SASTORE,
            POP,
            POP2,
            DUP,
            DUP_X1,
            DUP_X2,
            DUP2,
            DUP2_X1,
            DUP2_X2,
            SWAP,
            IADD,
            LADD,
            FADD,
            DADD,
            ISUB,
            LSUB,
            FSUB,
            DSUB,
            IMUL,
            LMUL,
            FMUL,
            DMUL,
            IDIV,
            LDIV,
            FDIV,
            DDIV,
            IREM,
            LREM,
            FREM,
            DREM,
            INEG,
            LNEG,
            FNEG,
            DNEG,
            ISHL,
            LSHL,
            ISHR,
            LSHR,
            IUSHR,
            LUSHR,
            IAND,
            LAND,
            IOR,
            LOR,
            IXOR,
            LXOR,
            IINC,
            I2L,
            I2F,
            I2D,
            L2I,
            L2F,
            L2D,
            F2I,
            F2L,
            F2D,
            D2I,
            D2L,
            D2F,
            I2B,
            I2C,
            I2S,
            LCMP,
            FCMPL,
            FCMPG,
            DCMPL,
            DCMPG,
            IFEQ,
            IFNE,
            IFLT,
            IFGE,
            IFGT,
            IFLE,
            IF_ICMPEQ,
            IF_ICMPNE,
            IF_ICMPLT,
            IF_ICMPGE,
            IF_ICMPGT,
            IF_ICMPLE,
            IF_ACMPEQ,
            IF_ACMPNE,
            GOTO,
            JSR,
            RET,
            TABLESWITCH,
            LOOKUPSWITCH,
            IRETURN,
            LRETURN,
            FRETURN,
            DRETURN,
            ARETURN,
            RETURN,
            GETSTATIC,
            PUTSTATIC,
            GETFIELD,
            PUTFIELD,
            INVOKEVIRTUAL,
            INVOKESPECIAL,
            INVOKESTATIC,
            INVOKEINTERFACE,
            INVOKEDYNAMIC,
            NEW,
            NEWARRAY,
            ANEWARRAY,
            ARRAYLENGTH,
            ATHROW,
            CHECKCAST,
            INSTANCEOF,
            MONITORENTER,
            MONITOREXIT,
            WIDE,
            MULTIANEWARRAY,
            IFNULL,
            IFNONNULL,
            GOTO_W,
            JSR_W
        };
        public enum Names : byte
        {
            NOP = 0x00,
            ACONST_NULL = 0x01,
            ICONST_M1 = 0x02,
            ICONST_0 = 0x03,
            ICONST_1 = 0x04,
            ICONST_2 = 0x05,
            ICONST_3 = 0x06,
            ICONST_4 = 0x07,
            ICONST_5 = 0x08,
            LCONST_0 = 0x09,
            LCONST_1 = 0x0A,
            FCONST_0 = 0x0B,
            FCONST_1 = 0x0C,
            FCONST_2 = 0x0D,
            DCONST_0 = 0x0E,
            DCONST_1 = 0x0F,
            BIPUSH = 0x10,
            SIPUSH = 0x11,
            LDC = 0x12,
            LDC_W = 0x13,
            LDC2_W = 0x14,
            ILOAD = 0x15,
            LLOAD = 0x16,
            FLOAD = 0x17,
            DLOAD = 0x18,
            ALOAD = 0x19,
            ILOAD_0 = 0x1A,
            ILOAD_1 = 0x1B,
            ILOAD_2 = 0x1C,
            ILOAD_3 = 0x1D,
            LLOAD_0 = 0x1E,
            LLOAD_1 = 0x1F,
            LLOAD_2 = 0x20,
            LLOAD_3 = 0x21,
            FLOAD_0 = 0x22,
            FLOAD_1 = 0x23,
            FLOAD_2 = 0x24,
            FLOAD_3 = 0x25,
            DLOAD_0 = 0x26,
            DLOAD_1 = 0x27,
            DLOAD_2 = 0x28,
            DLOAD_3 = 0x29,
            ALOAD_0 = 0x2A,
            ALOAD_1 = 0x2B,
            ALOAD_2 = 0x2C,
            ALOAD_3 = 0x2D,
            IALOAD = 0x2E,
            LALOAD = 0x2F,
            FALOAD = 0x30,
            DALOAD = 0x31,
            AALOAD = 0x32,
            BALOAD = 0x33,
            CALOAD = 0x34,
            SALOAD = 0x35,
            ISTORE = 0x36,
            LSTORE = 0x37,
            FSTORE = 0x38,
            DSTORE = 0x39,
            ASTORE = 0x3A,
            ISTORE_0 = 0x3B,
            ISTORE_1 = 0x3C,
            ISTORE_2 = 0x3D,
            ISTORE_3 = 0x3E,
            LSTORE_0 = 0x3F,
            LSTORE_1 = 0x40,
            LSTORE_2 = 0x41,
            LSTORE_3 = 0x42,
            FSTORE_0 = 0x43,
            FSTORE_1 = 0x44,
            FSTORE_2 = 0x45,
            FSTORE_3 = 0x46,
            DSTORE_0 = 0x47,
            DSTORE_1 = 0x48,
            DSTORE_2 = 0x49,
            DSTORE_3 = 0x4A,
            ASTORE_0 = 0x4B,
            ASTORE_1 = 0x4C,
            ASTORE_2 = 0x4D,
            ASTORE_3 = 0x4E,
            IASTORE = 0x4F,
            LASTORE = 0x50,
            FASTORE = 0x51,
            DASTORE = 0x52,
            AASTORE = 0x53,
            BASTORE = 0x54,
            CASTORE = 0x55,
            SASTORE = 0x56,
            POP = 0x57,
            POP2 = 0x58,
            DUP = 0x59,
            DUP_X1 = 0x5A,
            DUP_X2 = 0x5B,
            DUP2 = 0x5C,
            DUP2_X1 = 0x5D,
            DUP2_X2 = 0x5E,
            SWAP = 0x5F,
            IADD = 0x60,
            LADD = 0x61,
            FADD = 0x62,
            DADD = 0x63,
            ISUB = 0x64,
            LSUB = 0x65,
            FSUB = 0x66,
            DSUB = 0x67,
            IMUL = 0x68,
            LMUL = 0x69,
            FMUL = 0x6A,
            DMUL = 0x6B,
            IDIV = 0x6C,
            LDIV = 0x6D,
            FDIV = 0x6E,
            DDIV = 0x6F,
            IREM = 0x70,
            LREM = 0x71,
            FREM = 0x72,
            DREM = 0x73,
            INEG = 0x74,
            LNEG = 0x75,
            FNEG = 0x76,
            DNEG = 0x77,
            ISHL = 0x78,
            LSHL = 0x79,
            ISHR = 0x7A,
            LSHR = 0x7B,
            IUSHR = 0x7C,
            LUSHR = 0x7D,
            IAND = 0x7E,
            LAND = 0x7F,
            IOR = 0x80,
            LOR = 0x81,
            IXOR = 0x82,
            LXOR = 0x83,
            IINC = 0x84,
            I2L = 0x85,
            I2F = 0x86,
            I2D = 0x87,
            L2I = 0x88,
            L2F = 0x89,
            L2D = 0x8A,
            F2I = 0x8B,
            F2L = 0x8C,
            F2D = 0x8D,
            D2I = 0x8E,
            D2L = 0x8F,
            D2F = 0x90,
            I2B = 0x91,
            I2C = 0x92,
            I2S = 0x93,
            LCMP = 0x94,
            FCMPL = 0x95,
            FCMPG = 0x96,
            DCMPL = 0x97,
            DCMPG = 0x98,
            IFEQ = 0x99,
            IFNE = 0x9A,
            IFLT = 0x9B,
            IFGE = 0x9C,
            IFGT = 0x9D,
            IFLE = 0x9E,
            IF_ICMPEQ = 0x9F,
            IF_ICMPNE = 0xA0,
            IF_ICMPLT = 0xA1,
            IF_ICMPGE = 0xA2,
            IF_ICMPGT = 0xA3,
            IF_ICMPLE = 0xA4,
            IF_ACMPEQ = 0xA5,
            IF_ACMPNE = 0xA6,
            GOTO = 0xA7,
            JSR = 0xA8,
            RET = 0xA9,
            TABLESWITCH = 0xAA,
            LOOKUPSWITCH = 0xAB,
            IRETURN = 0xAC,
            LRETURN = 0xAD,
            FRETURN = 0xAE,
            DRETURN = 0xAF,
            ARETURN = 0xB0,
            RETURN = 0xB1,
            GETSTATIC = 0xB2,
            PUTSTATIC = 0xB3,
            GETFIELD = 0xB4,
            PUTFIELD = 0xB5,
            INVOKEVIRTUAL = 0xB6,
            INVOKESPECIAL = 0xB7,
            INVOKESTATIC = 0xB8,
            INVOKEINTERFACE = 0xB9,
            INVOKEDYNAMIC = 0xBA,
            NEW = 0xBB,
            NEWARRAY = 0xBC,
            ANEWARRAY = 0xBD,
            ARRAYLENGTH = 0xBE,
            ATHROW = 0xBF,
            CHECKCAST = 0xC0,
            INSTANCEOF = 0xC1,
            MONITORENTER = 0xC2,
            MONITOREXIT = 0xC3,
            WIDE = 0xC4,
            MULTIANEWARRAY = 0xC5,
            IFNULL = 0xC6,
            IFNONNULL = 0xC7,
            GOTO_W = 0xC8,
            JSR_W = 0xC9
        }
        #endregion

        public static IOpCode ByName(Names name) => OpCodeList[(byte)name];

        public static IOpCode ReadOpCode(this JavaByteCodeReader reader) => OpCodeList[reader.ReadByte()];
        public static IInstruction ReadInstrustion(this JavaByteCodeReader reader, IRaw handle) => IRaw.Read<IInstruction>(handle, reader);
    }
}
