using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;

namespace TableZipper
{
    class MakeCSV
    {
        private static string CSV_Destination = ConfigurationManager.AppSettings["csvDestination"];
        private static string ZIP_Destination = ConfigurationManager.AppSettings["ZipDestination"];
        public static void Fn_Make_CSV_From_Table()
        {
            if (!Directory.Exists(CSV_Destination))
            {
                Directory.CreateDirectory(CSV_Destination);
            }
            int start = 0;
            int end = 1000;
            int fileIndex = 1;
           
            while (true)
            {
                DataTable network = DbAccess.Get_table(start, end);
                if (network.Rows.Count == 0)
                {
                    break;
                }
                Thread thread = new Thread(() =>
                {
                    StringBuilder text = new StringBuilder("");
                    foreach (DataColumn dc in network.Columns)
                    {
                        text.Append(dc.ColumnName + ",");
                    }
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(CSV_Destination + "data" + fileIndex.ToString() + ".csv"))
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
                    catch (Exception  ex)
                    {
                        Console.WriteLine("Error while writing CSV files\n------\n"+ex.Message);
                    }
                }
                );
                thread.Start();
                thread.Join();
                start += end;
                fileIndex++;
            }
            if (!Directory.Exists(ZIP_Destination))
            {
                Directory.CreateDirectory(ZIP_Destination);
            }
        }
    }
}