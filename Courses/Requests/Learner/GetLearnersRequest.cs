namespace Courses.Requests.Learner;

public class GetLearnersRequest
{
    public int Limit { get; set; } = 10;
    public string Cursor { get; set; } = string.Empty;
}