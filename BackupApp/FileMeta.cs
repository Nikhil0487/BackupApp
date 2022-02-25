using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupApp
{
    internal class FileMeta
    {
        private string _filePath;
        private Action _action;
        public FileMeta(string filePath, Action action)
        {
            _filePath = filePath;
            _action = action;
        }
    }
}
