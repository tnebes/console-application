namespace console_app.util;

public sealed class EntityTypeInfo
{
    public EntityTypeInfo(Enum typeEnum, string name, Type entityType)
    {
        this.TypeEnum = typeEnum;
        this.Name = name;
        this.EntityType = entityType;
    }

    public Enum TypeEnum { get; }
    public string Name { get; }
    public Type EntityType { get; }
}