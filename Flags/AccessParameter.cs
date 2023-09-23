using System;

namespace Java.Net.Flags;

[Flags]
public enum AccessParameter
{
    NONE = 0x0000,
    FINAL = 0x0010, //Indicates that the formal parameter was declared final
    SYNTHETIC = 0x1000, //Indicates that the formal parameter was not explicitly or implicitly declared in source code, according to the specification of the language in which the source code was written (JLS §13.1). (The formal parameter is an implementation artifact of the compiler which produced this class file.)
    MANDATED = 0x8000, //Indicates that the formal parameter was implicitly declared in source code, according to the specification of the language in which the source code was written (JLS §13.1). (The formal parameter is mandated by a language specification, so all compilers for the language must emit it.)
}
