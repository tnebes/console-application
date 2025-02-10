#region

using System.Text.Json;
using console_app.Models;

#endregion

namespace console_app.services.impl;

public sealed class JsonServiceImpl : IJsonService
{
    private static JsonServiceImpl? _instance;
    private static readonly string _jsonDirectory = Path.Combine(AppContext.BaseDirectory, "data");
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    private static readonly Dictionary<Type, string> _jsonFilePaths = new()
    {
        { typeof(Programme), Path.Combine(_jsonDirectory, "programmes.json") },
        { typeof(Group), Path.Combine(_jsonDirectory, "groups.json") },
        { typeof(Student), Path.Combine(_jsonDirectory, "students.json") }
    };

    private JsonServiceImpl()
    {
        Directory.CreateDirectory(_jsonDirectory);
        foreach (string filePath in _jsonFilePaths.Values)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                File.WriteAllText(filePath, "[]");
                Console.WriteLine($"Created file: {filePath}");
            }
        }
    }

    public static JsonServiceImpl Instance => _instance ??= new JsonServiceImpl();

    public T CreateEntity<T>(T entity) where T : Entity
    {
        List<T> entities = [.. this.LoadEntities<T>()];
        entities.Add(entity);
        this.SaveEntities(entities);
        return entity;
    }

    public T? LoadEntity<T>(long id) where T : Entity
    {
        return this.LoadEntities<T>().FirstOrDefault(e => e.Id == id);
    }

    public List<T> LoadEntities<T>() where T : Entity
    {
        string filePath = _jsonFilePaths[typeof(T)];
        if (!File.Exists(filePath))
        {
            return [];
        }

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? [];
    }

    public T UpdateEntity<T>(T entity, long id) where T : Entity
    {
        List<T> entities = [.. this.LoadEntities<T>()];
        int index = entities.FindIndex(e => e.Id == id);
        if (index >= 0)
        {
            entities[index] = entity;
        }

        this.SaveEntities(entities);
        return entity;
    }

    public bool DeleteEntity<T>(long id) where T : Entity
    {
        List<T> entities = [.. this.LoadEntities<T>()];
        int count = entities.RemoveAll(e => e.Id == id);
        this.SaveEntities(entities);
        return count > 0;
    }

    public int GetMaxId<T>() where T : Entity
    {
        return this.LoadEntities<T>().Select(e => (int)e.Id).DefaultIfEmpty(0).Max();
    }

    private void SaveEntities<T>(List<T> entities) where T : Entity
    {
        string filePath = _jsonFilePaths[typeof(T)];
        string json = JsonSerializer.Serialize(entities, _jsonOptions);
        File.WriteAllText(filePath, json);
    }
}