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
        public int ProgressPercentage { get; }

        public ProgressEventArgs(string name, long received, long total_to_receive) :
            this(name, received, total_to_receive, (int)((received * 100) / total_to_receive)) { }
        public ProgressEventArgs(string name, long received, long total_to_receive, int progress_percentage)
        {
            Name = name;
            Received = received;
            TotalToReceive = total_to_receive;
            ProgressPercentage = progress_percentage;
        }
    }
}
