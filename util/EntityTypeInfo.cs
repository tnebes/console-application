namespace console_app.Util;

public sealed class EntityTypeInfo
{
    public EntityTypeInfo(Enum typeEnum, string name, Type entityType)
    {
        TypeEnum = typeEnum;
        Name = name;
        EntityType = entityType;
    }

    public Enum TypeEnum { get; }
    public string Name { get; }
    public Type EntityType { get; }
}