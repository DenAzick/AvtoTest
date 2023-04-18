    namespace Autotest.Mvc.Models;

public class Ticket
{
    public int Index { get; set; }
    public int TicketId { get; set; }   
    public string UserId { get; set; }
        
    public int QuestionsCount { get; set; }


    public int StartIndex { get; set; }
    public int CurrentQuestionIndex { get; set; }

    public bool IsCompleted => QuestionsCount == Answers.Count;

    public List<TicketQuestionAnswer> Answers { get; set; } = new List<TicketQuestionAnswer>();

    public int CorrectCount => Answers.Count(answer => answer.IsCorrect);

    public DateTime Date { get; set; }

    public Ticket()
    {

    }

    public Ticket(int index, int questionsCount)
    {
        TicketId = index;
        QuestionsCount = questionsCount;
        //StartIndex = 0;
        StartIndex = TicketId * questionsCount;
        CurrentQuestionIndex = StartIndex;
    }
}