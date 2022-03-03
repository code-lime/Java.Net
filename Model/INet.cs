using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Java.Net.Model
{
    public interface IAttributes
    {
        List<JavaAttribute> Attributes { get; set; }
    } 
    public interface IFlag<T> where T : Enum, IConvertible
    {
        T Flags { get; set; }
    }
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
            Mono.Cecil.AssemblyDefinition assembly = null;
            foreach (Mono.Cecil.AssemblyDefinition a in net.ToNet()) assembly = a;
            return assembly;
        }
    }

    public class INetStep<T> : IEnumerator<T>
    {
        private readonly List<IEnumerator<T>> list;
        internal INetStep(List<IEnumerator<T>> list) => this.list = list;
        public bool MoveNext() => list.RepeatStep();

        public T Current => throw new NotSupportedException();
        object System.Collections.IEnumerator.Current => throw new NotSupportedException();
        public void Dispose() => throw new NotSupportedException();
        public void Reset() => throw new NotSupportedException();

        public static INetStep<T> Of(params IEnumerator<T>[] list) => new INetStep<T>(new List<IEnumerator<T>>(list));
        public static INetStep<T> Of(IEnumerable<IEnumerator<T>> list) => new INetStep<T>(list.ToList());
    }
    public static class INetExt
    {
        public static INetStep<Mono.Cecil.AssemblyDefinition> Create(IEnumerable<INet> nets) => new INetStep<Mono.Cecil.AssemblyDefinition>(nets.CreateStep());
        public static INetStep<ICurrent> Create<ICurrent, IParent>(IEnumerable<INet<ICurrent, IParent>> nets, IParent parent) => new INetStep<ICurrent>(nets.CreateStep(parent));

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
}
