using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net;

internal static class Utils
{
    internal static byte[] ReadAllBytes(this Stream instream)
    {
        if (instream is MemoryStream memory) return memory.ToArray();
        using var memoryStream = new MemoryStream();
        instream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
    internal static void WriteAllBytes(this Stream outstream, byte[] bytes)
        => outstream.Write(bytes);
    internal static async ValueTask<byte[]> ReadAllBytesAsync(this Stream instream, CancellationToken cancellationToken)
    {
        if (instream is MemoryStream memory) return memory.ToArray();
        using var memoryStream = new MemoryStream();
        await instream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
    internal static ValueTask WriteAllBytesAsync(this Stream outstream, byte[] bytes, CancellationToken cancellationToken)
        => outstream.WriteAsync(bytes, cancellationToken);
    public static IEnumerable<Mono.Cecil.TypeDefinition> GetAllTypes(this Mono.Cecil.ModuleDefinition module)
    {
        foreach (var type in module.Types)
        {
            yield return type;
            foreach (var ret in type.GetAllTypes())
                yield return ret;
        }
    }
    public static IEnumerable<Mono.Cecil.TypeDefinition> GetAllTypes(this Mono.Cecil.TypeDefinition type)
    {
        foreach (var nested in type.NestedTypes)
        {
            yield return nested;
            foreach (var ret in nested.GetAllTypes())
                yield return ret;
        }
    }
}
