namespace console_app.Models;

public abstract class Entity
{
    protected Entity(long id)
    {
        if (id < 0L)
        {
            throw new ArgumentException("Id must be positive");
        }

        this.Id = id;
    }

    public long Id { get; }

    public abstract override string ToString();
}