using System;

namespace Java.Net.Flags;

[Flags]
public enum AccessField
{
    NONE = 0x0000,
    PUBLIC = 0x0001, //Declared public; may be accessed from outside its package.
    PRIVATE = 0x0002, //Declared private; usable only within the defining class.
    PROTECTED = 0x0004, //Declared protected; may be accessed within subclasses.
    STATIC = 0x0008, //Declared static.
    FINAL = 0x0010, //Declared final; never directly assigned to after object construction (JLS §17.5).
    VOLATILE = 0x0040, //Declared volatile; cannot be cached.
    TRANSIENT = 0x0080, //Declared transient; not written or read by a persistent object manager.
    SYNTHETIC = 0x1000, //Declared synthetic; not present in the source code.
    ENUM = 0x4000, //Declared as an element of an enum.
}
