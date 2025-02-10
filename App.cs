#region

using console_app.Models;
using console_app.services;
using console_app.services.impl;
using console_app.util;

#endregion

namespace console_app;

public sealed class App
{
    private const ConsoleColor MenuItemColor = ConsoleColor.Green;
    private const ConsoleColor SelectedItemColor = ConsoleColor.White;
    private readonly IConsoleWriterService _consoleWriterService = ConsoleWriterServiceImpl.Instance;

    private readonly Dictionary<Type, ICrudService> _crudServices = new()
    {
        { typeof(Student), new StudentCrudServiceImpl() },
        { typeof(Group), new GroupCrudServiceImpl() },
        { typeof(Programme), new ProgrammeCrudServiceImpl() }
    };

    private readonly IDataSeederService _dataSeederService = DataSeederServiceImpl.Instance;

    private readonly List<EntityTypeInfo> _entityTypes =
    [
        new(Enums.EntityTypes.Student, "Student", typeof(Student)),
        new(Enums.EntityTypes.Group, "Group", typeof(Group)),
        new(Enums.EntityTypes.Programme, "Programme", typeof(Programme))
    ];

    private readonly IGroupStatisticsService _statisticsService = GroupStatisticsServiceImpl.Instance;

    public App()
    {
        try
        {
            this.WriteIntroduction();
            bool exitRequested = false;

            while (!exitRequested)
            {
                string[] mainMenuItems = this._entityTypes.Select(e => e.Name)
                    .Concat(["Statistics", "Seed Sample Data", "Exit"])
                    .ToArray();
                int selection = this.DisplayMenu("Main Menu", mainMenuItems);

                if (selection == mainMenuItems.Length - 1)
                {
                    exitRequested = true;
                    continue;
                }

                Util.ClearScreen();
                if (selection == mainMenuItems.Length - 2)
                {
                    this._dataSeederService.SeedData();
                    Util.WaitForKeyPress();
                }
                else if (selection == mainMenuItems.Length - 3)
                {
                    this.DisplayStatistics();
                }
                else
                {
                    EntityTypeInfo selectedType = this._entityTypes[selection];
                    this.HandleCrudOperations(selectedType.EntityType);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ForegroundColor = MenuItemColor;
        }
    }

    private int DisplayMenu(string title, string[] options)
    {
        this._consoleWriterService.WriteWithDoubleLine(title);
        Console.WriteLine();

        for (int i = 0; i < options.Length; i++)
        {
            Console.ForegroundColor = MenuItemColor;
            Console.Write($"  {i + 1}. ");
            Console.ForegroundColor = SelectedItemColor;
            Console.WriteLine(options[i]);
        }

        Console.WriteLine();
        return Util.ReadUserIntInput("Enter your choice:", 1, options.Length) - 1;
    }

    private void HandleCrudOperations(Type entityType)
    {
        string[] crudOptions = { "Create", "List All", "View Details", "Update", "Delete", "Back" };
        ICrudService service = this._crudServices[entityType];

        while (true)
        {
            int choice = this.DisplayMenu($"Manage {entityType.Name}s", crudOptions);

            Action? selectedMethod = null;

            switch (choice)
            {
                case 0: selectedMethod = () => service.Create(); break;
                case 1: selectedMethod = () => service.GetAll(); break;
                case 2: selectedMethod = () => service.GetById(); break;
                case 3: selectedMethod = () => service.Update(); break;
                case 4: selectedMethod = () => service.Delete(); break;
                case 5: return;
            }

            Util.ClearScreen();
            selectedMethod();
        }
    }

    private void WriteIntroduction()
    {
        Util.ClearScreen();
        this._consoleWriterService.WriteWithDoubleLine("Welcome to Student Management System");
        Console.WriteLine();
    }

    private void DisplayStatistics()
    {
        this._consoleWriterService.WriteWithDoubleLine("Group Statistics");

        int totalStudents = this._statisticsService.GetTotalStudents();
        Console.WriteLine($"Total number of students: {totalStudents}");

        double avgStudentsPerGroup = this._statisticsService.GetAverageStudentsPerGroup();
        Console.WriteLine($"Average students per group: {avgStudentsPerGroup:F2}");

        Dictionary<string, double> revenueByProgramme = this._statisticsService.GetRevenueByProgramme();
        Console.WriteLine("\nRevenue by Programme:");
        foreach (KeyValuePair<string, double> revenue in revenueByProgramme)
        {
            Console.WriteLine($"  {revenue.Key}: \u20ac{revenue.Value:F2}");
        }

        double avgRevenuePerParticipant = this._statisticsService.GetAverageRevenuePerParticipant();
        Console.WriteLine($"\nAverage revenue per participant: \u20ac{avgRevenuePerParticipant:F2}");

        (DateTime? Earliest, DateTime? Latest, int? DaysBetween) dateStats =
            this._statisticsService.GetGroupDateStatistics();
        if (dateStats.Earliest.HasValue && dateStats.Latest.HasValue)
        {
            Console.WriteLine($"\nEarliest group start date: {dateStats.Earliest.Value:d}");
            Console.WriteLine($"Latest group start date: {dateStats.Latest.Value:d}");
            Console.WriteLine($"Days between: {dateStats.DaysBetween}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }
}