namespace Courses.Requests.Learner;

public class AddLearnerRequest
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string NationalId { get; set; }
    public string Department { get; set; }
}