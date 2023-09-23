using System;

namespace Java.Net.Flags;

[Flags]
public enum AccessProperty
{
    NONE = 0x0000,
    PUBLIC = 0x0001, //Marked or implicitly public in source.
    PRIVATE = 0x0002, //Marked private in source.
    PROTECTED = 0x0004, //Marked protected in source.
    STATIC = 0x0008, //Marked or implicitly static in source.
    FINAL = 0x0010, //Marked final in source.
    INTERFACE = 0x0200, //Was an interface in source.
    ABSTRACT = 0x0400, //Marked or implicitly abstract in source.
    SYNTHETIC = 0x1000, //Declared synthetic; not present in the source code.
    ANNOTATION = 0x2000, //Declared as an annotation type.
    ENUM = 0x4000, //Declared as an enum type.
}
