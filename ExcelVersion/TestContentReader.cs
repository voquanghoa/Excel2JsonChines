using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ExcelVersion
{
    public class TestContentReader
    {
        private static string Reduce(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }
            return text.Replace(".", ".").Replace("…", ".").Replace("--", "-").Replace(". .", "..")
                .Replace("-", "—");
        }

        private static string Tiny(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }

            while (text.IndexOf("..") >= 0)
            {
                text = text.Replace("..", ".");
            }
            while (text.IndexOf("。") >= 0)
            {
                text = text.Replace("。", "");
            }
            
            while (text.IndexOf("——") >= 0)
            {
                text = text.Replace("——", "—");
            }

            return text.Trim();
        }

        private static bool CompareString(string text1, string text2)
        {
            return string.Equals(Tiny(text1), Tiny(text2));
        }

        public static TestContent ReadTestContent(string fileName)
        {
            TestContent testContent = new TestContent();
            IExcelDataReader excelReader = null;

            try
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(File.Open(fileName, FileMode.Open, FileAccess.Read));
                excelReader.Read();
                while (excelReader.Read())
                {
                    if (testContent.Questions == null)
                    {
                        testContent.Title = Reduce(excelReader.GetString(0));
                        testContent.Questions = new List<Question>();
                        continue;
                    }
                    else
                    {
                        var question = new Question();
                        question.Title = (string.Empty + excelReader.GetString(0)).Trim();

                        question.Answers = new List<string>();
                        for (int i = 0; i < 4; i++)
                        {
                            question.Answers.Add(excelReader.GetString(i + 1));
                        }

                        question.Answers = question.Answers
                            .Where(x => !string.IsNullOrEmpty(x))
                            .Select(Reduce)
                            .ToList();

                        var correct = Reduce(excelReader.GetString(5));
                        question.Correct = question.Answers.FindIndex(x => CompareString(x, correct));

                        if (string.IsNullOrEmpty(question.Title) && string.IsNullOrEmpty(correct) && question.Answers.Count == 0)
                        {
                            continue;
                        }

                        if (question.Correct < 0)
                        {
                            throw new Exception($"File {fileName} cau hoi {question.Answers.Count} khong co cau tra loi");
                        }

                        testContent.Questions.Add(question);
                    }
                }
                return testContent;
            }
            finally
            {
                excelReader.Close();
            }
        }
    }
}
