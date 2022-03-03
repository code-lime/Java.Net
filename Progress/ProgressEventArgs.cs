using System;
using System.Collections.Generic;
using System.Text;

namespace Java.Net.Progress
{
    public class ProgressEventArgs : EventArgs
    {
        public string Name { get; }
        public long Received { get; }
        public long TotalToReceive { get; }
        public long ProgressPercentage { get; }

        public ProgressEventArgs(string name, long received, long total_to_receive)
        {
            Name = name;
            Received = received;
            TotalToReceive = total_to_receive;
            ProgressPercentage = (received * 100) / total_to_receive;
        }
    }
}
