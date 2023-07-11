using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableZipper
{
    class MainProgram
    {
        private static string ZIP_Destination = ConfigurationManager.AppSettings["ZipDestination"];
        public static void StartProgram()
        {
            MakeCSV.Fn_Make_CSV_From_Table();
            FileCompressor.Fn_CompressDirectory(ZIP_Destination);
            Console.ReadKey();
        }
    }
}
