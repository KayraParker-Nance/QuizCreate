namespace QuizCreate.Classes.Models
{
    public class Section
    {
        public string Name { get; set; } = "Questions";
        public List<Question> Questions { get; set; } = [];
        public int TotalQuestions => Questions.Count;
        public bool HasContent => TotalQuestions > 0;
    }
}
