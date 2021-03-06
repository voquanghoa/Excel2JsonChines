﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExcelExportChines
{
    public class English4AllReader
    {
        private static List<string> ignoreFiles = new List<string>()
        {
            @"\\Mac\Home\Downloads\Update_grammar (1)\Used to\lesson_13.txt"
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
            return true;
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
                if (string.IsNullOrEmpty((string.Empty + line).Trim()))
                {
                    question = null;
                    continue;
                }

                if (question == null)
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

                if (CheckAnswer(line))
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
                }
            }

            if (ignoreFiles.Contains(fileName))
            {
                return testContent;
            }

            if (testContent.Count == 0)
            {
                throw new Exception("Content empty " + fileName);
            }

            if (testContent.Any(x => x.Correct < 0))
            {
                throw new Exception("Khong co dap an " + fileName);
            }

            testContent.ForEach(FixManyAnswer);

            var wrong = testContent.FirstOrDefault(x => x.Answers.Any(y => y.ToLower().Contains("correct")));

            if (wrong != null)
            {
                throw new Exception("Sai sai " + fileName);
            }

            return testContent;
        }

        private static void FixManyAnswer(Question question)
        {
            if (question.Answers.Count > 4)
            {
                var index = new Random().Next(question.Answers.Count - 1);
                if (index == question.Correct)
                {
                    if (question.Correct > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index++;
                    }
                }
                question.Answers.RemoveAt(index);

                if (index < question.Correct)
                {
                    question.Correct--;
                }
                FixManyAnswer(question);
            }
        }
    }
}
