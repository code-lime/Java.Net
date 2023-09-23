using System;

namespace Java.Net.Flags;

[Flags]
public enum AccessClass
{
    NONE = 0x0000,
    PUBLIC = 0x0001, //Declared public; may be accessed from outside its package.
    FINAL = 0x0010, //Declared final; no subclasses allowed.
    SUPER = 0x0020, //Treat superclass methods specially when invoked by the invokespecial instruction.
    INTERFACE = 0x0200, //Is an interface, not a class.
    ABSTRACT = 0x0400, //Declared abstract; must not be instantiated.
    SYNTHETIC = 0x1000, //Declared synthetic; not present in the source code.
    ANNOTATION = 0x2000, //Declared as an annotation type.
    ENUM = 0x4000, //Declared as an enum type.
}
