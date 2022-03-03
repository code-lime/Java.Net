using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Java.Net.Progress
{
    public class ProgressStream : Stream
    {
        private Stream m_input = null;
        private long m_length = 0L;
        private long m_position = 0L;
        private string m_name;
        public event Action<ProgressEventArgs> UpdateProgress;

        public ProgressStream(string name, Stream input)
        {
            m_name = name;
            m_input = input;
            m_length = input.Length;
        }
        public override void Flush() => throw new System.NotImplementedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new System.NotImplementedException();
        public override void SetLength(long value) => throw new System.NotImplementedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new System.NotImplementedException();
        public override int Read(byte[] buffer, int offset, int count)
        {
            int n = m_input.Read(buffer, offset, count);
            m_position += n;
            UpdateProgress?.Invoke(new ProgressEventArgs(m_name, m_position, m_length));
            return n;
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => m_length;
        public override long Position
        {
            get { return m_position; }
            set { throw new System.NotImplementedException(); }
        }
    }
}
