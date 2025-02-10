#region

using console_app.Models;
using console_app.services;
using console_app.services.impl;
using console_app.util;

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

    private readonly List<EntityTypeInfo> _entityTypes =
    [
        new(Enums.EntityTypes.Student, "Student", typeof(Student)),
        new(Enums.EntityTypes.Group, "Group", typeof(Group)),
        new(Enums.EntityTypes.Programme, "Programme", typeof(Programme))
    ];

    public App()
    {
        try
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                string[] mainMenuItems = this._entityTypes.Select(e => e.Name).Concat(["Exit"]).ToArray();
                int selection = this.DisplayMenu("Main Menu", mainMenuItems);

                if (selection == mainMenuItems.Length - 1)
                {
                    exitRequested = true;
                    continue;
                }

                Util.ClearScreen();
                EntityTypeInfo selectedType = this._entityTypes[selection];
                this.HandleCrudOperations(selectedType.EntityType);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private int DisplayMenu(string title, string[] options)
    {
        this._consoleWriterService.WriteWithDoubleLine(title);

        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }

        return Util.ReadUserIntInput("\nEnter your choice: ", 1, options.Length) - 1;
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
        this._consoleWriterService.WriteWithDoubleLine("Welcome to a Simple Console Application");
    }
}