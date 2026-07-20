namespace QuizCreate.Classes.Models
{
    public class ParseResult
    {
        public List<Section> Sections { get; set; } = [];
        public List<string> Errors { get; set; } = [];
        public List<string> Warnings { get; set; } = [];
        public int TotalQuestions => Sections.Sum(s => s.Questions.Count);
        public bool HasContent => TotalQuestions > 0;
    }
}
