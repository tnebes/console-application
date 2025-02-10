#region

using console_app.Models;

#endregion

namespace console_app.services.impl;

public sealed class IdProviderServiceImpl : IIdProviderService
{
    private static IdProviderServiceImpl? _instance;
    private readonly IJsonService? _jsonService;

    private long _groupId;
    private long _programmeId;
    private long _studentId;

    private IdProviderServiceImpl(IJsonService jsonService)
    {
        this._jsonService = jsonService;
        this._groupId = this._jsonService.GetMaxId<Group>();
        this._programmeId = this._jsonService.GetMaxId<Programme>();
        this._studentId = this._jsonService.GetMaxId<Student>();
        this.Init();
    }

    public static IdProviderServiceImpl Instance => _instance ??= new IdProviderServiceImpl(JsonServiceImpl.Instance);

    public long GetNextId(Type entity)
    {
        return entity.Name switch
        {
            nameof(Student) => ++this._studentId,
            nameof(Group) => ++this._groupId,
            nameof(Programme) => ++this._programmeId,
            _ => throw new ArgumentException("Invalid entity type")
        };
    }

    private void Init()
    {
        this._programmeId = this._jsonService.GetMaxId<Programme>();
        this._groupId = this._jsonService.GetMaxId<Group>();
        this._studentId = this._jsonService.GetMaxId<Student>();
    }
}