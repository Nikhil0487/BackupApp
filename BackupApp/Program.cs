// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using BackupApp;
namespace classes
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BackupApp.FileWatcher fileWatcher = new BackupApp.FileWatcher();
            await fileWatcher.startFolderWatcher();
        }
    }
}