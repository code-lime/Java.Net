using System.Collections.Generic;

namespace Java.Net.Model.Net;

public static class NetUtils
{
    public static NetStep<Mono.Cecil.AssemblyDefinition> Create(IEnumerable<INet> nets) => new NetStep<Mono.Cecil.AssemblyDefinition>(nets.CreateStep());
    public static NetStep<ICurrent> Create<ICurrent, IParent>(IEnumerable<INet<ICurrent, IParent>> nets, IParent parent) => new NetStep<ICurrent>(nets.CreateStep(parent));

    public static bool RepeatStep<T>(this List<IEnumerator<T>> enumerators)
    {
        foreach (var enumerator in enumerators.ToArray())
            if (!enumerator.MoveNext())
                enumerators.Remove(enumerator);
        return enumerators.Count > 0;
    }
    public static List<IEnumerator<Mono.Cecil.AssemblyDefinition>> CreateStep(this IEnumerable<INet> nets)
    {
        List<IEnumerator<Mono.Cecil.AssemblyDefinition>> types = new List<IEnumerator<Mono.Cecil.AssemblyDefinition>>();
        foreach (var type in nets)
        {
            IEnumerator<Mono.Cecil.AssemblyDefinition> enumerator = type.ToNet().GetEnumerator();
            if (enumerator.MoveNext()) types.Add(enumerator);
        }
        return types;
    }
    public static List<IEnumerator<ICurrent>> CreateStep<ICurrent, IParent>(this IEnumerable<INet<ICurrent, IParent>> nets, IParent parent)
    {
        List<IEnumerator<ICurrent>> types = new List<IEnumerator<ICurrent>>();
        foreach (var type in nets)
        {
            IEnumerator<ICurrent> enumerator = type.ToNet(parent).GetEnumerator();
            if (enumerator.MoveNext()) types.Add(enumerator);
        }
        return types;
    }
}
