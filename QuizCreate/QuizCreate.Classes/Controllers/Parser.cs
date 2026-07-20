using QuizCreate.Classes.Models;
using System.Runtime.CompilerServices;

namespace QuizCreate.Classes.Controllers
{
    public static class Parser
    {
        public static ParseResult Parse(string text)
        {
            ParseResult result = new ParseResult();

            Section currentSection = new Section();

            Question? currentQuestion = null;

            foreach (string rawLine in text.Split('\n'))
            {
                string line = rawLine.Trim();

                if (IsComment(line))
                {
                    continue;
                }

                if (IsEndOfQuestion(line))
                {
                    TryAddQuestion(currentSection, currentQuestion, result.Errors);
                    currentQuestion = null;
                    continue;
                }

                if (IsNewSection(line))
                {
                    TryAddQuestion(currentSection, currentQuestion, result.Errors);
                    currentQuestion = null;

                    if (currentSection.HasContent)
                    {
                        result.Sections.Add(currentSection);
                    }

                    currentSection = new Section { Name = line.Substring(8).Trim() };
                    continue;
                }

                if (IsQuestion(line))
                {
                    TryAddQuestion(currentSection, currentQuestion, result.Errors);
                    currentQuestion = new Question { Text = line.Substring(2).Trim() };
                    continue;
                }

                if (currentQuestion == null)
                {
                    continue;
                }

                if (IsOption(line))
                {
                    currentQuestion.Options.Add(line.Substring(2).Trim());
                    continue;
                }

                if (IsAnswer(line))
                {
                    string letterStr = line.Substring(4).Trim().ToUpperInvariant();
                    char letter = letterStr.FirstOrDefault(char.IsLetter);

                    if (letter != '\0')
                    {
                        currentQuestion.AnswerIndex = letter - 'A';
                    }
                    continue;
                }

                if (IsExplanation(line))
                {
                    currentQuestion.Explanation = line.Substring(4).Trim();
                    continue;
                }

                if (!string.IsNullOrEmpty(currentQuestion.Explanation))
                {
                    currentQuestion.Explanation += " " + line;
                }
            }

            return result;
        }

        private static void TryAddQuestion(Section section, Question? question, List<string> errors)
        {
            if (question == null)
            {
                errors.Add("Question is null when trying to add to section.");
                return;
            }

            if (string.IsNullOrWhiteSpace(question.Text))
            {
                errors.Add("Question text is empty.");
                return;
            }

            if (question.Options.Count < 2)
            {
                errors.Add($"Question '{question.Text}' has less than 2 options.");
                return;
            }

            if (question.AnswerIndex < 0 || question.AnswerIndex >= question.Options.Count)
            {
                errors.Add($"Question '{question.Text}' has an invalid answer index.");
                return;
            }

            section.Questions.Add(question);
        }
        private static bool IsComment(string text)
        {
            return text.StartsWith("#") || text.StartsWith("//");
        }

        private static bool IsEndOfQuestion(string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        private static bool IsNewSection(string text)
        {
            return text.ToUpper().StartsWith("SECTION:");
        }

        private static bool IsQuestion(string text)
        {
            return text.ToUpper().StartsWith("Q:");
        }

        private static bool IsOption(string text)
        {
            return (text.Length >= 2) && (char.IsLetter(text[0])) && (text[1] == ':' || text[1] == '.');
        }

        private static bool IsAnswer(string text)
        {
            return text.ToUpper().StartsWith("ANS:");
        }

        private static bool IsExplanation(string text)
        {
            return text.ToUpper().StartsWith("EXP:");
        }
    }
 }
