using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TableZipper
{
    class MakeCSV
    {
        private static string CSV_Destination = ConfigurationManager.AppSettings["csvDestination"];
        public static void  Fn_Make_CSV_From_Table()
        {
            if (!Directory.Exists(CSV_Destination))
            {
                Directory.CreateDirectory(CSV_Destination);
            }
            int start = 0;
            int end = 1000;
            int fileIndex = 1;
            List<Thread> threads = new List<Thread>();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("CSV build started......\n");
            Console.WriteLine("----------------------");
            Console.WriteLine("All thread started");
            var cursor = Console.GetCursorPosition();
            while (true)
            {
                DataTable fetchedTable = DbAccess.Get_table(start, end);
                if (fetchedTable.Rows.Count == 0)
                {
                    break;
                }
                Thread thread = new Thread(() =>
                {
                    StringBuilder text = new StringBuilder("");
                    foreach (DataColumn dc in fetchedTable.Columns)
                    {
                        text.Append(dc.ColumnName + ",");
                    }
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(CSV_Destination + "data" + fileIndex.ToString() + ".csv"))
                        {
                            sw.WriteLine(text.ToString().TrimEnd(','));
                            foreach (DataRow dr in fetchedTable.Rows)
                            {
                                StringBuilder row = new StringBuilder("");
                                foreach (DataColumn dc in fetchedTable.Columns)
                                {
                                    row.Append(dr[dc] + ",");
                                }
                                sw.WriteLine(row.ToString().TrimEnd(','));
                            }
                            sw.Flush();
                            sw.Close();
                        }
                    }
                    catch (Exception  ex)
                    {
                        Console.WriteLine("Error while writing CSV files\n------\n"+ex.Message);
                    }
                }
                );
                thread.Start();
                threads.Add(thread);
                start += end;
                fileIndex++;
            }
            foreach(var thread in threads)
            {
                thread.Join();
            }
            Console.WriteLine("CSV build Completed");
        }
    }
}