using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExcelExportChines
{
    public class FrenchTestContentReader
    {
        private static List<string> ignoreFiles = new List<string>()
        {
            /*@"\\Mac\Home\Desktop\Data\grammaire\dÇmonstratifs\lesson_07.txt",
            @"\\Mac\Home\Desktop\Data\grammaire\style indirect\lesson_09.txt",
            @"\\Mac\Home\Desktop\Data\Niveau A1\C'est et s'est , C'Çtait et s'Çtait\lesson_12.txt",
            @"\\Mac\Home\Desktop\Data\Niveau B1\homonymes et homographes non homophones\lesson_01.txt",
            @"\\Mac\Home\Desktop\Data\Niveau B1\impÇratif prÇsent\lesson_03.txt",
            */

            //No errror
            @"\\Mac\Home\Desktop\Data\Niveau A2\Mots commenáant par -Cor\lesson_01.txt",
            @"\\Mac\Home\Desktop\Data\Niveau A2\Mots commenáant par -Cor\lesson_02.txt",
            @"\\Mac\Home\Desktop\Data\Niveau B1\COD-COI\lesson_04.txt",
            @"\\Mac\Home\Desktop\Data\Niveau A2\Mots commen†ant par -Cor\lesson_01.txt",
            @"\\Mac\Home\Desktop\Data\Niveau A2\Mots commen†ant par -Cor\lesson_02.txt"
        };
        private static bool CheckTitle(string text)
        {
            return !string.IsNullOrEmpty((text + "").Trim());
        }

        private static bool CheckQuestion(string text)
        {
            return Regex.IsMatch(text, @"^\d+[\.\)]");
        }

        private static string ExtractQuestion(string text)
        {
            return text.Substring(3).Trim();
        }

        private static bool CheckAnswer(string text)
        {
            return Regex.IsMatch(text, @"^\[\s+]");
        }

        private static string ExtractAnswer(string text)
        {
            return text.Substring(Regex.Match(text, @"^\[\s+]").Length)
                .Replace("--- correct", "")
                .Replace("--- correct", "")
                .Replace("---  correct", "")
                .Replace("---correct", "")
                .Replace("--- Correct", "")
                .Replace("---Correct", "")
                .Replace("– correct", "")
                .Trim();
        }

        private static bool CheckCorrectAnswer(string text)
        {
            return Regex.IsMatch(text.ToLower(), @"-\s*correct");
        }

        public static TestContent ReadTestContent(string fileName)
        {
            var testContent = new TestContent();

            var lines = File.ReadAllLines(fileName);
            Question question = null;

            foreach (var line in lines)
            {
                if (CheckQuestion(line))
                {
                    question = new Question
                    {
                        Title = ExtractQuestion(line),
                        Answers = new List<string>(),
                        Correct = -1
                    };

                    testContent.Add(question);
                    continue;
                }

                if (question != null && CheckAnswer(line))
                {
                    var answer = line.Replace("—", "---").Replace("–", "---")
                        .Replace("corrrect", "correct")
                        .Replace("corect", "correct")
                        .Replace("correcct", "correct");

                    question.Answers.Add(ExtractAnswer(answer));
                    if (CheckCorrectAnswer(answer))
                    {
                        if (question.Correct >= 0)
                        {
                            throw new Exception("Sai sai " + fileName);
                        }
                        question.Correct = question.Answers.Count - 1;
                    }
                    if (question.Answers.Count >= 4)
                    {
                        question = null;
                    }
                }
            }

            if (ignoreFiles.Contains(fileName))
            {
                return testContent;
            }

            if(testContent.Count == 0)
            {
                throw new Exception("Content empty " + fileName);
            }

            if (testContent.Any(x =>x.Correct < 0))
            {
                throw new Exception("Khong co dap an " + fileName);
            }

            var wrong = testContent.FirstOrDefault(x => x.Answers.Any(y => y.ToLower().Contains("correct")));

            if (wrong != null)
            {
                throw new Exception("Sai sai " + fileName);
            }

            return testContent;
        }
    }
}
