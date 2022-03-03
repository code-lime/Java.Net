using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Java.Net.Code;
using Java.Net.Model.Attribute;
using Mono.Cecil;

namespace Java.Net.Model
{
    public class JavaMethodBody : IHandle
    {
        public CodeAttribute code;
        public JavaClass Handle => code.Handle;
        public IEnumerable<(long offset, Instruction instruction)> GetInstructions()
        {
            using MemoryStream stream = new MemoryStream(code.Code);
            JavaByteCodeReader reader = new JavaByteCodeReader(stream);
            long start_index = reader.Position;
            while (reader.CanReadNext)
            {
                long offset = reader.Position - start_index;
                yield return (offset, reader.ReadInstrustion(this));
            }
        }

        public JavaMethodBody(CodeAttribute code)
        {
            this.code = code;
            Console.WriteLine(string.Join(", ", code.Code.Select(v => "0x" + v.ToString("X2"))));
        }
        /*
DEFINE PRIVATE getBukkitData()Lnet/minecraft/nbt/NBTTagCompound;
A:
LINE A 188
ALOAD this
INVOKEVIRTUAL org/bukkit/craftbukkit/v1_18_R1/CraftOfflinePlayer.getData()Lnet/minecraft/nbt/NBTTagCompound;
ASTORE result
B:
LINE B 190
ALOAD result
IFNULL F
C:
LINE C 191
ALOAD result
LDC "bukkit"
INVOKEVIRTUAL net/minecraft/nbt/NBTTagCompound.e(Ljava/lang/String;)Z
IFNE E
D:
LINE D 192
ALOAD result
LDC "bukkit"
NEW net/minecraft/nbt/NBTTagCompound
DUP
INVOKESPECIAL net/minecraft/nbt/NBTTagCompound.<init>()V
INVOKEVIRTUAL net/minecraft/nbt/NBTTagCompound.a(Ljava/lang/String;Lnet/minecraft/nbt/NBTBase;)Lnet/minecraft/nbt/NBTBase;
POP
E:
LINE E 194
ALOAD result
LDC "bukkit"
INVOKEVIRTUAL net/minecraft/nbt/NBTTagCompound.p(Ljava/lang/String;)Lnet/minecraft/nbt/NBTTagCompound;
ASTORE result
F:
LINE F 197
ALOAD result
ARETURN
G:
        */
    }
}
