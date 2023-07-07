using System;
using System.Data;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Threading;

namespace TableZipper
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\Welcome\source\repos\TableZipper\TableZipper\ToZip\";
            string zipPath = @"C:\Users\Welcome\Desktop\zips\";
            int start = 0;
            int end = 1000;
            int fileIndex = 1;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(zipPath))
            {
                Directory.CreateDirectory(zipPath);
            }
            while (true)
            {
                DataTable network = DbAccess.Get_table(start, end);
                if (network.Rows.Count == 0)
                {
                    break;
                }
                StringBuilder text = new StringBuilder("");
                foreach (DataColumn dc in network.Columns)
                {
                    text.Append(dc.ColumnName + ",");
                }

                try
                {
                    using (StreamWriter sw = new StreamWriter(path+"data"+fileIndex.ToString()+".csv"))
                    {
                        sw.WriteLine(text.ToString().TrimEnd(','));
                        foreach (DataRow dr in network.Rows)
                        {
                            StringBuilder row = new StringBuilder("");
                            foreach (DataColumn dc in network.Columns)
                            {
                                row.Append(dr[dc] + ",");
                            }
                            sw.WriteLine(row.ToString().TrimEnd(','));
                        }
                        sw.Flush();
                        sw.Close();
                    }
                }
                catch (Exception)
                {

                    throw;
                }   
                start += end ;
                fileIndex++;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Thread.Sleep(3000);
            ZipFile.CreateFromDirectory(zipPath, zipPath + "table.zip");

        }
    }
}
