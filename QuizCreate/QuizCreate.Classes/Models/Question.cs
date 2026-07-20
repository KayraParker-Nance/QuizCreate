namespace QuizCreate.Classes.Models
{
    public class Question
    {
        public string Text { get; set; } = "";
        public List<string> Options { get; set; } = [];
        public int AnswerIndex { get; set; } = -1;
        public string Explanation { get; set; } = "";
    }
}
