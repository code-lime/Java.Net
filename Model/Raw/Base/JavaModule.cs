#define DEBUGGING
#undef DEBUGGING
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Model.Net;
using Java.Net.Progress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net.Model.Raw.Base;

public class JavaModule : INet
{
    public Version Version { get; private set; } = null!;

    public string ImplementationTitle { get; private set; } = null!;
    public string? ImplementationVersion { get; private set; }
    public string? ImplementationVendor { get; private set; }

    public string? MainClass { get; private set; }

    public Dictionary<string, JavaClass> Classes { get; private set; } = new Dictionary<string, JavaClass>();
    public Dictionary<string, byte[]> Files { get; private set; } = new Dictionary<string, byte[]>();

    public override string ToString() => $"JavaModule: {ImplementationTitle} v{Version}";

#if DEBUGGING
    public static async ValueTask SyncForEachAsync<TSource>(IEnumerable<TSource> source, CancellationToken cancellationToken, Func<TSource, CancellationToken, ValueTask> body)
    {
        foreach (var item in source)
            await body.Invoke(item, cancellationToken);
    }
#endif
    
    public static JavaModule Empty() => new JavaModule();
    private async ValueTask ReadAsync(ParallelZipArchive zip, Action<ProgressEventArgs>? progress = null, CancellationToken cancellationToken = default)
    {
        //Console.Write($"Unzipping...");
        Dictionary<string, MemoryStream> zipEntries = await zip.ExtractAsync(cancellationToken);// ParallelZipArchive.ReadAll(stream);
        //Console.WriteLine("OK!");

        Version version = new Version(0, 0, 1);
        string implementation_title = "JavaAssembly";
        string? implementation_version = null;
        string? implementation_vendor = null;
        string? main = null;
        int count = zipEntries.Count;
        int index = 0;
        JavaClass[] classes = new JavaClass[count];
        Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
#if DEBUGGING
        await SyncForEachAsync
#else
        await Parallel.ForEachAsync
#endif
        (zipEntries, cancellationToken, async (kv, token) =>
        {
            using MemoryStream stream = kv.Value;
            stream.Position = 0;
            string fullName = kv.Key;
            string name = Path.GetFileName(fullName);
            if (name.Equals("MANIFEST.MF"))
            {
                try
                {
                    StreamReader reader = new StreamReader(stream);
                    foreach (var line in (await reader.ReadToEndAsync(token)).Replace("\r", "").Split('\n'))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        int index = line.IndexOf(':');
                        if (index == -1) continue;
                        string value = line[(index + 2)..];
                        switch (line[..index])
                        {
                            case "Manifest-Version": version = Version.Parse(value); break;
                            case "Implementation-Title": implementation_title = value; break;
                            case "Implementation-Vendor": implementation_vendor = value; break;
                            case "Implementation-Version": implementation_version = value; break;
                            case "Main-Class": main = value; break;
                        }
                    }
                }
                catch
                {

                }
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (name.EndsWith(".class") && !fullName.StartsWith("META-INF/"))
                {
#if !DEBUGGING
                    try
#endif
                    {
                        stream.Position = 0;
                        JavaClass _class = await IRaw.ReadAsync<JavaClass>(new JavaByteCodeReader(stream), cancellationToken: token);
                        lock (classes)
                        {
                            classes[index] = _class;
                            /*Console.Title = $"[{s}/{count}] Read class '{fullName}'...OK!";
                            if (s % 100 == 0)
                            {
                                Console.WriteLine($"[{s}/{count}] Read class '{fullName}'...OK!");
                            }*/
                            index++;
                            progress?.Invoke(new ProgressEventArgs(fullName, index, count));
                        }
                        return;
                    }
#if !DEBUGGING
                    catch
                    {
                        /*Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{s}/{count}] Read class '{fullName}'...ERROR!");
                        Console.ResetColor();*/
                    }
#endif
                }
                stream.Position = 0;
                lock (files)
                {
                    index++;
                    files[fullName] = stream.ToArray();
                    progress?.Invoke(new ProgressEventArgs(fullName, index, count));
                }
                //if (s % 100 == 0) Console.WriteLine($"[{s}/{count}] Read file '{fullName}'...OK!");
            }
        });
        Version = version;
        ImplementationTitle = implementation_title;
        ImplementationVendor = implementation_vendor;
        ImplementationVersion = implementation_version;
        Classes = classes.Where(v => v != null).ToDictionary(v => v.ThisClass.Name, v => v);
        Files = files;
        MainClass = main;
    }
    public async ValueTask WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        Dictionary<string, Stream> files = new Dictionary<string, Stream>();
        foreach (var _class in Classes.Values)
        {
            MemoryStream _new = new MemoryStream();
            await _class.WriteAsync(new JavaByteCodeWriter(_new), cancellationToken);
            _new.Position = 0;
            files[_class.ThisClassPath] = _new;
        }
        foreach (var file in Files)
        {
            MemoryStream _new = new MemoryStream(file.Value);
            files[file.Key] = _new;
        }
        await ParallelZipArchive.ToZipAsync(stream, files, cancellationToken);
        foreach (var _s in files.Values) _s.Dispose();
    }
    public static async ValueTask<JavaClass> ReadFromAsync(string file, string path, CancellationToken cancellationToken)
    {
        using MemoryStream stream = await ParallelZipArchive.From(file).ExtractAsync(path, cancellationToken);
        stream.Position = 0;
        return await IRaw.ReadAsync<JavaClass>(new JavaByteCodeReader(stream), cancellationToken: cancellationToken);
    }
    public static async ValueTask<JavaModule> ReadFromAsync(string file, Action<ProgressEventArgs>? progress = null, CancellationToken cancellationToken = default)
    {
        JavaModule module = new JavaModule();
        await module.ReadAsync(ParallelZipArchive.From(file, progress), progress, cancellationToken);
        return module;
    }
    public static async ValueTask<JavaClass> ReadFromAsync(byte[] bytes, string path, CancellationToken cancellationToken)
    {
        using MemoryStream stream = await ParallelZipArchive.From(bytes).ExtractAsync(path, cancellationToken);
        stream.Position = 0;
        return await IRaw.ReadAsync<JavaClass>(new JavaByteCodeReader(stream), cancellationToken: cancellationToken);
    }
    public static async ValueTask<JavaModule> ReadFromAsync(byte[] bytes, Action<ProgressEventArgs>? progress = null, CancellationToken cancellationToken = default)
    {
        JavaModule module = new JavaModule();
        await module.ReadAsync(ParallelZipArchive.From(bytes, progress), progress, cancellationToken);
        return module;
    }
    public async ValueTask WriteToAsync(string file, CancellationToken cancellationToken)
    {
        using Stream stream = File.Open(file, FileMode.Create, FileAccess.Write, FileShare.Write);
        await WriteToAsync(stream, cancellationToken);
    }
    public static async ValueTask WriteTo(string file, JavaClass javaClass, CancellationToken cancellationToken)
    {
        using Stream stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        using MemoryStream _stream = new MemoryStream();
        await javaClass.WriteAsync(new JavaByteCodeWriter(_stream), cancellationToken);
        await ParallelZipArchive.SetToZipAsync(stream, new Dictionary<string, Stream>() { [javaClass.ThisClassPath] = _stream }, cancellationToken);
    }

    public JavaModule Join(JavaModule other)
    {
        foreach (var kv in other.Classes) Classes[kv.Key] = kv.Value;
        foreach (var kv in other.Files) Files[kv.Key] = kv.Value;
        return this;
    }
    public JavaModule Append(JavaClass javaClass)
    {
        Classes[javaClass.ThisClass.Name] = javaClass;
        return this;
    }

    public IEnumerable<Mono.Cecil.AssemblyDefinition> ToNet()
    {
        throw new NotSupportedException();
        /*
        Mono.Cecil.AssemblyNameDefinition assemblyName = new Mono.Cecil.AssemblyNameDefinition(ImplementationTitle, Version);
        Mono.Cecil.AssemblyDefinition assembly = Mono.Cecil.AssemblyDefinition.CreateAssembly(assemblyName, "jar", MainClass == null ? Mono.Cecil.ModuleKind.Dll : Mono.Cecil.ModuleKind.Console);
        INetStep<Mono.Cecil.TypeDefinition> types = INetExt.Create(Classes.Values, assembly);
        yield return assembly;
        Console.Write("Converting");
        while (types.MoveNext())
        {
            Console.Write(".");
            yield return assembly;
        }
        Console.WriteLine("OK!");

        Console.WriteLine(string.Join("\n", assembly.MainModule.GetAllTypes().Select(v => v.FullName)));
        */
    }
}
