namespace QuizCreate.Classes.Models
{
    public record Theme(string Name, string Background, string PrimaryColour, string SecondaryColour, string Text, string Muted, string Correct, string Wrong, string Card)
    {
        public override string ToString()
        {
            return Name;
        }
    }
}
