using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading.Channels;

namespace BackupApp
{
    public class FolderWatcher
    {
        private FileSyncManager _syncManager;
        public FolderWatcher()
        {
           _syncManager = new FileSyncManager();
        }
        private void CheckAndCreateFolder()
        {
            if(!Directory.Exists("C:/Backup/"))
            {
                Directory.CreateDirectory(Path.Combine("C:/Backup/"));
            }
        }
        public async Task startFolderWatcher()
        {
            CheckAndCreateFolder();
            using var watcher=new FileSystemWatcher(@"C:/Backup/");
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            AddEventTrackers(watcher);
            await _syncManager.ExecuteTask();
        }
        private void AddEventTrackers(FileSystemWatcher watcher)
        {
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }
        
        private async void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
            await _syncManager.AssignTask(new FileMeta(e.FullPath, Action.Create));
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            await _syncManager.AssignTask(new FileMeta(e.FullPath, Action.Create));
        }

        private async void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath}");
            await _syncManager.AssignTask(new FileMeta(e.FullPath, Action.Delete));
        }

        private async void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
            await _syncManager.AssignTask(new FileMeta(e.FullPath, Action.Rename));
        }
        private void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

    }
}
