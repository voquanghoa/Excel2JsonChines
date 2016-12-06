using Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelExportChines
{
    public class QuestionController
    {
        public static string ProcessFile(string fileName)
        {
            try
            {
                var testContent = TestContentReader.ReadTestContent(fileName);
                var jsonFile = Path.GetFileNameWithoutExtension(fileName) + ".json";
                var jsonPath = Path.Combine(Path.GetDirectoryName(fileName), jsonFile);
                File.Delete(jsonPath);
                File.AppendAllText(jsonPath, JsonConvert.SerializeObject(testContent));
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message + "\n\r";
            }
            
        }

        public static string Process(string fileName)
        {
            StringBuilder sb = new StringBuilder();

            var directories = Directory.EnumerateDirectories(fileName).ToList();
            var files = Directory.EnumerateFiles(fileName, "*.xlsx").ToList();

            foreach (var directory in directories)
            {
                sb.Append(Process(directory));
            }

            foreach (var file in files)
            {
                sb.Append(ProcessFile(file));
            }
            return sb.ToString();
        }
    }
}
