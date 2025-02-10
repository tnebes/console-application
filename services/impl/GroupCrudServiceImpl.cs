#region

using console_app.Models;
using console_app.util;

#endregion

namespace console_app.services.impl;

public sealed class GroupCrudServiceImpl : ICrudService<Group>
{
    private readonly IConsoleWriterService _consoleWriter;
    private readonly IIdProviderService _idProvider;
    private readonly IJsonService _jsonService;
    private readonly ICrudService<Programme> _programmeCrudService;

    public GroupCrudServiceImpl()
    {
        this._jsonService = JsonServiceImpl.Instance;
        this._idProvider = IdProviderServiceImpl.Instance;
        this._consoleWriter = ConsoleWriterServiceImpl.Instance;
        this._programmeCrudService = new ProgrammeCrudServiceImpl();
    }

    public void Create()
    {
        try
        {
            string name;
            long programmeId;
            DateTime startDate;

            while (true)
            {
                name = Util.ReadUserStringInput("Enter group name (or 'exit' to cancel):");
                if (name.ToLower() == "exit")
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }

                Console.WriteLine("Name cannot be empty. Please try again.");
            }

            while (true)
            {
                string userInput =
                    Util.ReadUserStringInput("Enter programme ID (or L to list all programmes, or 'exit' to cancel):");
                if (userInput.ToLower() == "exit")
                {
                    return;
                }

                if (userInput.ToLower() == "l")
                {
                    this._programmeCrudService.ListAll();
                    continue;
                }

                if (long.TryParse(userInput, out programmeId) && programmeId > 0)
                {
                    Programme? programme = this._jsonService.LoadEntity<Programme>(programmeId);
                    if (programme != null)
                    {
                        break;
                    }
                }

                Console.WriteLine("Invalid programme ID. Please try again.");
            }

            while (true)
            {
                string dateInput = Util.ReadUserStringInput("Enter start date (MM/dd/yyyy) or 'exit' to cancel:");
                if (dateInput.ToLower() == "exit")
                {
                    return;
                }

                if (DateTime.TryParse(dateInput, out startDate))
                {
                    break;
                }

                Console.WriteLine("Invalid date format. Please use MM/dd/yyyy format.");
            }

            Group group = new(this._idProvider.GetNextId(typeof(Group)), name, programmeId, startDate);
            this._jsonService.CreateEntity(group);
            this._consoleWriter.WriteEntityAction("Group created", group);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void GetAll()
    {
        this.ListAll();
        Util.WaitForKeyPress();
    }

    public void GetById()
    {
        long id = Util.ReadUserLongInput("Enter group ID:", 1L, long.MaxValue);
        Group? group = this._jsonService.LoadEntity<Group>(id);

        if (group == null)
        {
            Console.WriteLine("Group not found.");
            Util.WaitForKeyPress();
            return;
        }

        this._consoleWriter.WriteEntityAction("Group Details", group);
    }

    public void Update()
    {
        long id;
        while (true)
        {
            string userInput = Util.ReadUserStringInput("Enter group ID to update or L to list all groups, or 'exit' to cancel:");
            if (userInput.ToLower() == "exit")
            {
                return;
            }

            if (userInput.ToLower() == "l")
            {
                this.GetAll();
                this.Update();
                return;
            }

            if (long.TryParse(userInput, out id) && id > 0L)
            {
                break;
            }

            Console.WriteLine("Invalid input. Please enter a valid group ID, 'L' to list, or 'exit' to cancel.");
        }

        Group? existingGroup = this._jsonService.LoadEntity<Group>(id);

        if (existingGroup == null)
        {
            Console.WriteLine("Group not found.");
            Util.WaitForKeyPress();
            return;
        }

        string newName;
        long newProgrammeId;
        DateTime newStartDate = existingGroup.StartDate;

        while (true)
        {
            newName = Util.ReadUserStringInput(
                $"Current name: {existingGroup.Name}\nEnter new name (or press Enter to keep current, or 'exit' to cancel):");
            if (newName.ToLower() == "exit")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(newName) || newName == "")
            {
                break;
            }

            Console.WriteLine("Name cannot be empty. Please try again.");
        }

        while (true)
        {
            string programmeInput = Util.ReadUserStringInput(
                $"Current programme ID: {existingGroup.ProgrammeId}\nEnter new programme ID (or L to list all programmes, press Enter to keep current, or 'exit' to cancel):");

            if (programmeInput.ToLower() == "exit")
            {
                return;
            }

            if (string.IsNullOrEmpty(programmeInput))
            {
                newProgrammeId = existingGroup.ProgrammeId;
                break;
            }

            if (programmeInput.ToLower() == "l")
            {
                this._programmeCrudService.ListAll();
                continue;
            }

            if (long.TryParse(programmeInput, out newProgrammeId) && newProgrammeId > 0)
            {
                Programme? programme = this._jsonService.LoadEntity<Programme>(newProgrammeId);
                if (programme != null)
                {
                    break;
                }
            }

            Console.WriteLine("Invalid programme ID. Please try again.");
        }

        string dateInput = Util.ReadUserStringInput(
            $"Current start date: {existingGroup.StartDate:d}\nEnter new start date (MM/dd/yyyy), press Enter to keep current, or 'exit' to cancel:");

        if (dateInput.ToLower() == "exit")
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(dateInput))
        {
            if (DateTime.TryParse(dateInput, out DateTime parsedDate))
            {
                newStartDate = parsedDate;
            }
            else
            {
                Console.WriteLine("Invalid date format. Keeping current start date.");
            }
        }

        Group updatedGroup = new(
            existingGroup.Id,
            string.IsNullOrWhiteSpace(newName) ? existingGroup.Name : newName,
            newProgrammeId,
            newStartDate
        );

        this._jsonService.UpdateEntity(updatedGroup, existingGroup.Id);
        this._consoleWriter.WriteEntityAction("Group updated", updatedGroup);
    }

    public void Delete()
    {
        long id;
        while (true)
        {
            string userInput = Util.ReadUserStringInput("Enter group ID to delete or L to list all groups, or 'exit' to cancel:");
            if (userInput.ToLower() == "exit")
            {
                return;
            }

            if (userInput.ToLower() == "l")
            {
                this.ListAll();
                this.Delete();
                return;
            }

            if (long.TryParse(userInput, out id) && id > 0L)
            {
                break;
            }

            Console.WriteLine("Invalid input. Please enter a valid group ID, 'L' to list, or 'exit' to cancel.");
        }

        bool deleted = this._jsonService.DeleteEntity<Group>(id);

        if (deleted)
        {
            Console.WriteLine($"Group with ID {id} deleted successfully.");
        }
        else
        {
            Console.WriteLine("Group not found.");
        }

        Util.WaitForKeyPress();
    }

    public void ListAll()
    {
        List<Group> groups = this._jsonService.LoadEntities<Group>();
        if (groups.Count == 0)
        {
            Console.WriteLine("No groups found.");
            Util.WaitForKeyPress();
            return;
        }

        this._consoleWriter.WriteWithDoubleLine("All Groups");
        foreach (Group group in groups)
        {
            Console.WriteLine(group);
        }
        
    }
}