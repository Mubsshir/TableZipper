using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TableZipper
{
    class FileCompressor
    {
        private static string CSV_Destination = ConfigurationManager.AppSettings["csvDestination"];

        public static void Fn_CompressDirectory(string Destination)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(CSV_Destination);
            FileInfo[] files = dirInfo.GetFiles();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n");
            var cursor = Console.GetCursorPosition();
            if (!Directory.Exists(Destination))
            {
                Directory.CreateDirectory(Destination);
            }
            var zip = ZipFile.Open(Destination + "Table.zip", ZipArchiveMode.Update);
            foreach (var file in files)
            {
                Thread.Sleep(30);
                Console.SetCursorPosition(cursor.Left, cursor.Top);
                Console.WriteLine("Adding " + file.Name + " to archive.....");
                zip.CreateEntryFromFile(file.FullName, file.Name, CompressionLevel.Optimal);
            }
            zip.Dispose();
            Console.WriteLine("Deleting all the files.........\n\n");
            cursor = Console.GetCursorPosition();
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var file in files)
            {
                Thread.Sleep(30);
                Console.SetCursorPosition(cursor.Left, cursor.Top);
                Console.WriteLine("Deleting File " + file.Name);
                File.Delete(file.FullName);
            }
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nAll files has been deleted from the directory\n------------------------\n");
            Console.WriteLine("Compression completed successfully");
        }
    }
}
