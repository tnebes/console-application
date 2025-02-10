namespace console_app.Models;

public sealed class Programme(long id, string name, double price) : Entity(id)
{
    public string Name { get; } =
        string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name cannot be null") : name;

    public double Price { get; } = price > 0 ? price : throw new ArgumentException("Price must be positive");

    public override string ToString()
    {
        return $"{this.GetType().Name} [Id={this.Id}, Name={this.Name}, Price={this.Price}]";
    }
}