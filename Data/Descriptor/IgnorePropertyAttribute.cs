#nullable enable
using System;

namespace Java.Net.Data.Descriptor;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class IgnorePropertyAttribute : System.Attribute { }
