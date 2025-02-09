namespace console_app.Models;

public class Student : Entity
{
    private string FirstName { get; set; }
    private string LastName { get; set; }
    private List<long> Groups { get; set; }
}