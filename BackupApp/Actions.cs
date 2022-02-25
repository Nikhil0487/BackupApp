using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupApp
{
    internal enum Action: int
    {
        Create=0,
        Update=1,
        Delete=2,
        Rename=3
    }
}
