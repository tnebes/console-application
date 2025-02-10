#region

using console_app.Models;

#endregion

namespace console_app.services.impl;

public sealed class GroupStatisticsServiceImpl : IGroupStatisticsService
{
    private static GroupStatisticsServiceImpl? _instance;
    private readonly IJsonService _jsonService;

    private GroupStatisticsServiceImpl()
    {
        this._jsonService = JsonServiceImpl.Instance;
    }

    public static GroupStatisticsServiceImpl Instance
    {
        get
        {
            _instance ??= new GroupStatisticsServiceImpl();
            return _instance;
        }
    }

    public int GetTotalStudents()
    {
        List<Student> students = this._jsonService.LoadEntities<Student>();
        return students.Count;
    }

    public double GetAverageStudentsPerGroup()
    {
        List<Group> groups = this._jsonService.LoadEntities<Group>();
        List<Student> students = this._jsonService.LoadEntities<Student>();

        if (groups.Count == 0)
        {
            return 0;
        }

        int totalStudentsInGroups = students.Sum(s => s.Groups.Count);
        return (double)totalStudentsInGroups / groups.Count;
    }

    public Dictionary<string, double> GetRevenueByProgramme()
    {
        List<Programme> programmes = this._jsonService.LoadEntities<Programme>();
        List<Group> groups = this._jsonService.LoadEntities<Group>();
        List<Student> students = this._jsonService.LoadEntities<Student>();

        Dictionary<string, double> revenue = new();

        foreach (Programme programme in programmes)
        {
            List<Group> programmeGroups = groups.Where(g => g.ProgrammeId == programme.Id).ToList();
            int totalStudents = students.Count(s => s.Groups.Any(g => programmeGroups.Any(pg => pg.Id == g)));
            revenue[programme.Name] = totalStudents * programme.Price;
        }

        return revenue;
    }

    public double GetAverageRevenuePerParticipant()
    {
        List<Programme> programmes = this._jsonService.LoadEntities<Programme>();
        List<Group> groups = this._jsonService.LoadEntities<Group>();
        List<Student> students = this._jsonService.LoadEntities<Student>();

        if (students.Count == 0)
        {
            return 0;
        }

        double totalRevenue = 0;
        int totalEnrollments = 0;

        foreach (Student student in students)
        {
            foreach (long groupId in student.Groups)
            {
                Group? group = groups.FirstOrDefault(g => g.Id == groupId);
                if (group != null)
                {
                    Programme? programme = programmes.FirstOrDefault(p => p.Id == group.ProgrammeId);
                    if (programme != null)
                    {
                        totalRevenue += programme.Price;
                        totalEnrollments++;
                    }
                }
            }
        }

        return totalEnrollments > 0 ? totalRevenue / totalEnrollments : 0;
    }

    public (DateTime? Earliest, DateTime? Latest, int? DaysBetween) GetGroupDateStatistics()
    {
        List<Group> groups = this._jsonService.LoadEntities<Group>();

        if (!groups.Any())
        {
            return (null, null, null);
        }

        DateTime earliest = groups.Min(g => g.StartDate);
        DateTime latest = groups.Max(g => g.StartDate);
        int daysBetween = (latest - earliest).Days;

        return (earliest, latest, daysBetween);
    }
}