namespace console_app.Models;

public class Student(long id, string firstName, string lastName, string oib, string email, List<long> groups)
    : Entity(id)
{
    public string FirstName { get; } = string.IsNullOrWhiteSpace(firstName)
        ? throw new ArgumentException("First name cannot be null")
        : firstName;

    public string LastName { get; } = string.IsNullOrWhiteSpace(lastName)
        ? throw new ArgumentException("Last name cannot be null")
        : lastName;

    public string Oib { get; } = string.IsNullOrWhiteSpace(oib) || oib.Length != 11
        ? throw new ArgumentException("Oib must be 11 characters long")
        : oib;

    public string Email { get; } =
        string.IsNullOrWhiteSpace(email) ? throw new ArgumentException("Email cannot be null") : email;

    public List<long> Groups { get; } = groups ?? [];

    public override string ToString()
    {
        return
            $"{this.GetType().Name} [Id={this.Id}, FirstName={this.FirstName}, LastName={this.LastName}, Oib={this.Oib}, Email={this.Email}, Groups={string.Join(", ", this.Groups)}]";
    }
}