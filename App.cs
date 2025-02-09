#region

using console_app.Models;
using console_app.Services;
using console_app.Services.Impl;
using console_app.Util;

#endregion

namespace console_app;

public sealed class App
{
    private readonly IConsoleWriterService _consoleWriterService = ConsoleWriterServiceImpl.Instance;

    private readonly Dictionary<Type, ICrudService> _crudServices = new()
    {
        { typeof(Student), new StudentCrudServiceImpl() },
        { typeof(Group), new GroupCrudServiceImpl() },
        { typeof(Programme), new ProgrammeCrudServiceImpl() }
    };

    private readonly List<EntityTypeInfo> _entityTypes = new()
    {
        new EntityTypeInfo(Enums.EntityTypes.Student, "Student", typeof(Student)),
        new EntityTypeInfo(Enums.EntityTypes.Group, "Group", typeof(Group)),
        new EntityTypeInfo(Enums.EntityTypes.Programme, "Programme", typeof(Programme))
    };

    public App()
    {
        try
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                string[] mainMenuItems = _entityTypes.Select(e => e.Name).Concat(new[] { "Exit" }).ToArray();
                int selection = DisplayMenu("Main Menu", mainMenuItems);

                if (selection == mainMenuItems.Length - 1)
                {
                    exitRequested = true;
                    continue;
                }

                EntityTypeInfo selectedType = _entityTypes[selection];
                HandleCrudOperations(selectedType.EntityType);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private int DisplayMenu(string title, string[] options)
    {
        _consoleWriterService.WriteWithDoubleLine(title);

        for (int i = 0; i < options.Length; i++) Console.WriteLine($"{i + 1}. {options[i]}");

        Console.Write("\nEnter your choice: ");
        return int.Parse(Console.ReadLine()!) - 1;
    }

    private void HandleCrudOperations(Type entityType)
    {
        string[] crudOptions = { "Create", "List All", "View Details", "Update", "Delete", "Back" };
        ICrudService service = _crudServices[entityType];

        while (true)
        {
            int choice = DisplayMenu($"Manage {entityType.Name}s", crudOptions);

            switch (choice)
            {
                case 0: service.Create(); break;
                case 1: service.GetAll(); break;
                case 2: service.GetById(); break;
                case 3: service.Update(); break;
                case 4: service.Delete(); break;
                case 5: return;
            }
        }
    }

    private void WriteIntroduction()
    {
        _consoleWriterService.WriteWithDoubleLine("Welcome to a Simple Console Application");
    }
}