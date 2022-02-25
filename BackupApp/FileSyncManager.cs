using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace BackupApp
{
    internal class FileSyncManager
    {
        internal Channel<FileMeta> _channel;
        internal FileSyncManager()
        {
            _channel = Channel.CreateUnbounded<FileMeta>();
        }
        internal async Task AssignTask(FileMeta file)
        {
            Console.WriteLine("Assigned " + file);
            await _channel.Writer.WriteAsync(file);
        }
        internal async Task ExecuteTask()
        {
            while (true)
            {
                Console.WriteLine("Awaiting...");
                FileMeta item = await _channel.Reader.ReadAsync();
                Console.WriteLine("Received event:" + item);
            }
        }
    }
}
