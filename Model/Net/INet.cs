using System;
using System.Collections.Generic;

namespace Java.Net.Model.Net;
public interface INet<ICurrent, IParent>
{
    IEnumerable<ICurrent> ToNet(IParent parent);
}
public interface INet<ICurrent> : INet<ICurrent, Mono.Cecil.AssemblyDefinition> { }
public interface INet
{
    IEnumerable<Mono.Cecil.AssemblyDefinition> ToNet();

    public static Mono.Cecil.AssemblyDefinition Create(INet net)
    {
        throw new NotImplementedException();
        /*Mono.Cecil.AssemblyDefinition assembly = null;
        foreach (Mono.Cecil.AssemblyDefinition a in net.ToNet()) assembly = a;
        return assembly;*/
    }
}
