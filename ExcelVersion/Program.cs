using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ExcelVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = @"\\Mac\Home\Desktop\Chines";
            ProcessFolder(folder);
        }

        private static void ProcessFolder(string str)
        {
            Directory.EnumerateDirectories(str).ToList().ForEach(ProcessFolder);

            Directory.EnumerateFiles(str).ToList().ForEach(ProcessFile);
        }

        private static void ProcessFile(string file)
        {
            var testContent = TestContentReader.ReadTestContent(file);
            var jsonPath = file.Replace(".xlsx", ".json");
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(testContent));
        }
    }
}
