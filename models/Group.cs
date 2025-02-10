namespace console_app.Models;

public sealed class Group(long id, string name, long programmeId, DateTime startDate) : Entity(id)
{
    public string Name { get; } =
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name cannot be null") : name;

    public long ProgrammeId { get; } =
        programmeId > 0 ? programmeId : throw new ArgumentException("ProgrammeId must be positive");

    public DateTime StartDate { get; } = startDate;

    public override string ToString()
    {
        return
            $"{this.GetType().Name} [Id={this.Id}, Name={this.Name}, ProgrammeId={this.ProgrammeId}, StartDate={this.StartDate:d}]";
    }
}