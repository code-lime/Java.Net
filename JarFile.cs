using System;
using System.Collections.Generic;
using System.Text;

namespace Java.Net
{
    public class JarFile
    {
        private byte[] data;
        public JarFile(byte[] data)
        {
            this.data = data;
            //Module = ReadModule(data);
        }
        public JarFile(string file) : this(System.IO.File.ReadAllBytes(file)) { }

    }
}
