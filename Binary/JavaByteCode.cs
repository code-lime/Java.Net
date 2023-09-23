using System.IO;

namespace Java.Net.Binary;

public abstract class JavaByteCode
{
    public Stream Stream => stream;
    public long Length => stream.Length;
    public long Position => stream.Position;
    public bool CanReadNext => stream.Position < stream.Length;
    protected readonly Stream stream;
    protected JavaByteCode(Stream stream) => this.stream = stream;
}