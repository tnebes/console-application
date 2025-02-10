#region

using console_app.Models;

#endregion

namespace console_app.services.impl;

public sealed class DataSeederServiceImpl : IDataSeederService
{
    private static DataSeederServiceImpl? _instance;
    private readonly Bogus.Faker _faker;
    private readonly IIdProviderService _idProvider;
    private readonly IJsonService _jsonService;

    private DataSeederServiceImpl()
    {
        this._jsonService = JsonServiceImpl.Instance;
        this._idProvider = IdProviderServiceImpl.Instance;
        this._faker = new Bogus.Faker();
    }

    public static DataSeederServiceImpl Instance
    {
        get
        {
            _instance ??= new DataSeederServiceImpl();
            return _instance;
        }
    }

    public void SeedData()
    {
        List<Programme> programmes = this.SeedProgrammes(2);
        List<Group> groups = this.SeedGroups(5, programmes);
        List<Student> students = this.SeedStudents(20, groups);

        foreach (Programme programme in programmes)
        {
            this._jsonService.CreateEntity(programme);
        }

        foreach (Group group in groups)
        {
            this._jsonService.CreateEntity(group);
        }

        foreach (Student student in students)
        {
            this._jsonService.CreateEntity(student);
        }

        Console.WriteLine("Data seeding completed successfully.");
        Console.WriteLine(
            $"Created {programmes.Count} programmes, {groups.Count} groups, and {students.Count} students.");
    }

    private List<Programme> SeedProgrammes(int count)
    {
        List<Programme> programmes = new();
        string[] programmeNames = { "Software Development", "Data Science" };

        for (int i = 0; i < count; i++)
        {
            Programme programme = new(
                this._idProvider.GetNextId(typeof(Programme)),
                programmeNames[i],
                this._faker.Random.Double(500d, 2000d)
            );
            programmes.Add(programme);
        }

        return programmes;
    }

    private List<Group> SeedGroups(int count, List<Programme> programmes)
    {
        List<Group> groups = new();
        string[] groupPrefixes = { "Morning", "Afternoon", "Evening", "Weekend", "Online" };

        for (int i = 0; i < count; i++)
        {
            Programme randomProgramme = this._faker.Random.ListItem(programmes);
            DateTime startDate = this._faker.Date.Between(DateTime.Now.AddMonths(-6), DateTime.Now.AddMonths(6));

            Group group = new(
                this._idProvider.GetNextId(typeof(Group)),
                $"{groupPrefixes[i]} Group",
                randomProgramme.Id,
                startDate
            );
            groups.Add(group);
        }

        return groups;
    }

    private List<Student> SeedStudents(int count, List<Group> groups)
    {
        List<Student> students = new();

        for (int i = 0; i < count; i++)
        {
            int numberOfGroups = this._faker.Random.Int(1, 2);
            List<long> studentGroups = this._faker.Random.ListItems(groups, numberOfGroups).Select(g => g.Id).ToList();

            Student student = new(
                this._idProvider.GetNextId(typeof(Student)),
                this._faker.Name.FirstName(),
                this._faker.Name.LastName(),
                this._faker.Random.Replace("###########"),
                this._faker.Internet.Email(),
                studentGroups
            );
            students.Add(student);
        }

        return students;
    }
}