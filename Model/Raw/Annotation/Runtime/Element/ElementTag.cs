﻿namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public enum ElementTag : byte
{
    Byte = (byte)'B',
    Char = (byte)'C',
    Double = (byte)'D',
    Float = (byte)'F',
    Integer = (byte)'I',
    Long = (byte)'J',
    Short = (byte)'S',
    Boolean = (byte)'Z',

    String = (byte)'s',

    Enum = (byte)'e',
    Class = (byte)'c',
    Annotation = (byte)'@',
    Array = (byte)'['
}
