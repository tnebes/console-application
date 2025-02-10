namespace console_app.services;

public interface IGroupStatisticsService
{
    int GetTotalStudents();
    double GetAverageStudentsPerGroup();
    Dictionary<string, double> GetRevenueByProgramme();
    double GetAverageRevenuePerParticipant();
    (DateTime? Earliest, DateTime? Latest, int? DaysBetween) GetGroupDateStatistics();
}