using Autotest.Mvc.Models;

namespace Avtotest.Models;

public class UserTickets : Ticket
{
    public List<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();
}

public class QuestionAnswer
{
    public int QuestionIndex { get; set; }
    public int ChoiceIndex { get; set; }
    public int CorrectIndex { get; set; }
}