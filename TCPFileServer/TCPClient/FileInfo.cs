using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    internal class FileInfo
    {
        public string FileName {  get; set; }
        public string Extention {  get; set; }
        public byte[] File {  get; set; }
    }
}
