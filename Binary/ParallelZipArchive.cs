using Java.Net.Progress;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net.Binary;

public class ParallelZipArchive
{
    public static Dictionary<string, MemoryStream> ReadAll(string file) => From(file).Extract();
    public static Dictionary<string, MemoryStream> ReadAll(byte[] data) => From(data).Extract();

    public static ParallelZipArchive From(string file, Action<ProgressEventArgs>? progress = null) => new ParallelZipArchive(file, progress);
    public static ParallelZipArchive From(byte[] data, Action<ProgressEventArgs>? progress = null) => new ParallelZipArchive(data, progress);

    private static IEnumerable<IEnumerable<T>> Chunk<T>(IEnumerable<T> source, int chunkSize)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        while (source.Any())
        {
            yield return source.Take(chunkSize);
            source = source.Skip(chunkSize);
        }
    }
    private readonly Func<Stream> reader;
    private ParallelZipArchive(string filePath, Action<ProgressEventArgs>? progress = null) : this(() => File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), progress) { }
    private ParallelZipArchive(byte[] data, Action<ProgressEventArgs>? progress = null) : this(() => new MemoryStream(data), progress) { }
    private ParallelZipArchive(Func<Stream> reader, Action<ProgressEventArgs>? progress = null)
    {
        this.reader = () =>
        {
            if (progress == null) return reader.Invoke();
            ProgressStream stream = new ProgressStream("ZIP", reader.Invoke());
            stream.UpdateProgress += progress;
            return stream;
        };
    }

    private Stream GetStream() => reader.Invoke();
    private IReadOnlyCollection<string> GetEntries()
    {
        using Stream fs = GetStream();
        using var archive = new ZipArchive(fs, ZipArchiveMode.Read, true);
        return archive.Entries.Select(e => e.FullName).ToList();
    }
    public Dictionary<string, MemoryStream> Extract() => Extract(GetEntries(), 1000, CancellationToken.None);
    public MemoryStream Extract(string path) => Extract(new string[] { path }, 1000, CancellationToken.None)[path];
    private Dictionary<string, MemoryStream> Extract(IEnumerable<string> entries, int maxFilesPerThread, CancellationToken cancellationToken)
    {
        var result = new ConcurrentDictionary<string, MemoryStream>();
        IEnumerable<IEnumerable<string>> batched = Chunk(entries, maxFilesPerThread);
        try
        {
            Parallel.ForEach(batched, new ParallelOptions { CancellationToken = cancellationToken }, entry => ExtractSequentiall(entry, result, cancellationToken));
        }
        catch (OperationCanceledException)
        {

        }
        return new Dictionary<string, MemoryStream>(result);
    }
    private void ExtractSequentiall(IEnumerable<string> entries, ConcurrentDictionary<string, MemoryStream> result, CancellationToken cancellationToken)
    {
        using Stream fs = GetStream();
        using var archive = new ZipArchive(fs, ZipArchiveMode.Read, true);
        foreach (string entryName in entries)
        {
            if (cancellationToken.IsCancellationRequested) return;
            ZipArchiveEntry entry = archive.GetEntry(entryName)!;
            if (entry.Name == "") continue;
            using Stream stream = entry.Open();
            MemoryStream memory = new MemoryStream();
            stream.CopyTo(memory);
            result[entryName] = memory;
        }
    }
    public static void ToZip(Stream output, Dictionary<string, Stream> files)
    {
        using var archive = new ZipArchive(output, ZipArchiveMode.Create, true);
        foreach (var file in files)
        {
            ZipArchiveEntry entry = archive.CreateEntry(file.Key);
            using Stream to = entry.Open();
            file.Value.CopyTo(to);
        }
    }
    public static void SetToZip(Stream archive, Dictionary<string, Stream> files)
    {
        using var _archive = new ZipArchive(archive, ZipArchiveMode.Update, true);
        foreach (var file in files)
        {
            ZipArchiveEntry entry = _archive.GetEntry(file.Key) ?? _archive.CreateEntry(file.Key);
            using Stream to = entry.Open();
            file.Value.Position = 0;
            file.Value.CopyTo(to);
        }
    }
}