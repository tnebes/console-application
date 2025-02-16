#region

using console_app.Models;
using console_app.util;

#endregion

namespace console_app.services.impl;

public sealed class StudentCrudServiceImpl : ICrudService<Student>
{
    private readonly IConsoleWriterService _consoleWriter;
    private readonly ICrudService<Group> _groupCrudService;
    private readonly IIdProviderService _idProvider;
    private readonly IJsonService _jsonService;

    public StudentCrudServiceImpl()
    {
        this._jsonService = JsonServiceImpl.Instance;
        this._idProvider = IdProviderServiceImpl.Instance;
        this._consoleWriter = ConsoleWriterServiceImpl.Instance;
        this._groupCrudService = new GroupCrudServiceImpl();
    }

    public void Create()
    {
        try
        {
            string firstName;
            string lastName;
            string oib;
            string email;
            List<long> groups = [];

            while (true)
            {
                firstName = Util.ReadUserStringInput("Enter student first name (or 'exit' to cancel):");
                if (firstName.ToLower() == "exit")
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    break;
                }

                Console.WriteLine("First name cannot be empty. Please try again.");
            }

            while (true)
            {
                lastName = Util.ReadUserStringInput("Enter student last name (or 'exit' to cancel):");
                if (lastName.ToLower() == "exit")
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    break;
                }

                Console.WriteLine("Last name cannot be empty. Please try again.");
            }

            while (true)
            {
                oib = Util.ReadUserStringInput("Enter student OIB (or 'exit' to cancel):");
                if (oib.ToLower() == "exit")
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(oib) && oib.Length == 11)
                {
                    break;
                }

                Console.WriteLine("OIB must be exactly 11 digits. Please try again.");
            }

            while (true)
            {
                email = Util.ReadUserStringInput("Enter student email (or 'exit' to cancel):");
                if (email.ToLower() == "exit")
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(email) && email.Contains('@'))
                {
                    break;
                }

                Console.WriteLine("Please enter a valid email address.");
            }

            while (true)
            {
                string userInput =
                    Util.ReadUserStringInput(
                        "Add group ID (or L to list all groups, Enter to finish, or 'exit' to cancel):");
                if (userInput.ToLower() == "exit")
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    break;
                }

                if (userInput.ToLower() == "l")
                {
                    this._groupCrudService.ListAll();
                    continue;
                }

                if (long.TryParse(userInput, out long groupId) && groupId > 0)
                {
                    Group? group = this._jsonService.LoadEntity<Group>(groupId);
                    if (group != null)
                    {
                        groups.Add(groupId);
                        Console.WriteLine($"Added to group: {group.Name}");
                    }
                    else
                    {
                        Console.WriteLine("Group not found.");
                        Util.WaitForKeyPress();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid group ID. Please try again.");
                }
            }

            Student student = new(this._idProvider.GetNextId(typeof(Student)), firstName, lastName, oib, email, groups);
            this._jsonService.CreateEntity(student);
            this._consoleWriter.WriteEntityAction("Student created", student);
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
        long id = Util.ReadUserLongInput("Enter student ID:", 1L, long.MaxValue);
        Student? student = this._jsonService.LoadEntity<Student>(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            Util.WaitForKeyPress();
            return;
        }

        this._consoleWriter.WriteEntityAction("Student Details", student);
    }

    public void Update()
    {
        string userInput =
            Util.ReadUserStringInput("Enter student ID to update or L to list all students, or 'exit' to cancel:");
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

        long id = long.Parse(userInput);
        Student? existingStudent = this._jsonService.LoadEntity<Student>(id);

        if (existingStudent == null)
        {
            Console.WriteLine("Student not found.");
            Util.WaitForKeyPress();
            return;
        }

        string newFirstName;
        string newLastName;
        string newOib;
        string newEmail;
        List<long> newGroups = new(existingStudent.Groups);

        while (true)
        {
            newFirstName = Util.ReadUserStringInput(
                $"Current first name: {existingStudent.FirstName}\nEnter new first name (or press Enter to keep current, or 'exit' to cancel):");
            if (newFirstName.ToLower() == "exit")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(newFirstName) || newFirstName == "")
            {
                break;
            }

            Console.WriteLine("First name cannot be empty. Please try again.");
        }

        while (true)
        {
            newLastName = Util.ReadUserStringInput(
                $"Current last name: {existingStudent.LastName}\nEnter new last name (or press Enter to keep current, or 'exit' to cancel):");
            if (newLastName.ToLower() == "exit")
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(newLastName) || newLastName == "")
            {
                break;
            }

            Console.WriteLine("Last name cannot be empty. Please try again.");
        }

        while (true)
        {
            newOib = Util.ReadUserStringInput(
                $"Current OIB: {existingStudent.Oib}\nEnter new OIB (or press Enter to keep current, or 'exit' to cancel):");
            if (newOib.ToLower() == "exit")
            {
                return;
            }

            if (string.IsNullOrEmpty(newOib))
            {
                newOib = existingStudent.Oib;
                break;
            }

            if (newOib.Length == 11)
            {
                break;
            }

            Console.WriteLine("OIB must be exactly 11 digits. Please try again.");
        }

        while (true)
        {
            newEmail = Util.ReadUserStringInput(
                $"Current email: {existingStudent.Email}\nEnter new email (or press Enter to keep current, or 'exit' to cancel):");
            if (newEmail.ToLower() == "exit")
            {
                return;
            }

            if (string.IsNullOrEmpty(newEmail))
            {
                newEmail = existingStudent.Email;
                break;
            }

            if (newEmail.Contains('@'))
            {
                break;
            }

            Console.WriteLine("Please enter a valid email address.");
        }

        while (true)
        {
            Console.WriteLine("\nCurrent groups:");
            foreach (long groupId in existingStudent.Groups)
            {
                Group? group = this._jsonService.LoadEntity<Group>(groupId);
                Console.WriteLine(group?.ToString() ?? $"Unknown group (ID: {groupId})");
            }

            string groupInput = Util.ReadUserStringInput(
                "\nGroup management options:\n1. Add group\n2. Remove group\n3. List all groups\n4. Finish\n5. Exit\nEnter your choice:");

            switch (groupInput)
            {
                case "1":
                    string addGroupId = Util.ReadUserStringInput("Enter group ID to add (or 'exit' to cancel):");
                    if (addGroupId.ToLower() == "exit")
                    {
                        continue;
                    }

                    if (long.TryParse(addGroupId, out long groupIdToAdd) && groupIdToAdd > 0)
                    {
                        Group? group = this._jsonService.LoadEntity<Group>(groupIdToAdd);
                        if (group != null && !newGroups.Contains(groupIdToAdd))
                        {
                            newGroups.Add(groupIdToAdd);
                            Console.WriteLine($"Added to group: {group.Name}");
                        }
                        else if (newGroups.Contains(groupIdToAdd))
                        {
                            Console.WriteLine("Student is already in this group.");
                            Util.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("Group not found.");
                            Util.WaitForKeyPress();
                        }
                    }

                    break;

                case "2":
                    string removeGroupId = Util.ReadUserStringInput("Enter group ID to remove (or 'exit' to cancel):");
                    if (removeGroupId.ToLower() == "exit")
                    {
                        continue;
                    }

                    if (long.TryParse(removeGroupId, out long groupIdToRemove) && groupIdToRemove > 0)
                    {
                        if (newGroups.Remove(groupIdToRemove))
                        {
                            Console.WriteLine("Group removed successfully.");
                            Util.WaitForKeyPress();
                        }
                        else
                        {
                            Console.WriteLine("Student is not in this group.");
                            Util.WaitForKeyPress();
                        }
                    }

                    break;

                case "3":
                    this._groupCrudService.ListAll();
                    break;

                case "4":
                    goto exitGroupManagement;

                case "5":
                    return;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        exitGroupManagement:

        Student updatedStudent = new(
            existingStudent.Id,
            string.IsNullOrWhiteSpace(newFirstName) ? existingStudent.FirstName : newFirstName,
            string.IsNullOrWhiteSpace(newLastName) ? existingStudent.LastName : newLastName,
            newOib,
            newEmail,
            newGroups
        );

        this._jsonService.UpdateEntity(updatedStudent, existingStudent.Id);
        this._consoleWriter.WriteEntityAction("Student updated", updatedStudent);
    }

    public void Delete()
    {
        string userInput =
            Util.ReadUserStringInput("Enter student ID to delete or L to list all students, or 'exit' to cancel:");
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

        long id = long.Parse(userInput);
        bool deleted = this._jsonService.DeleteEntity<Student>(id);

        if (deleted)
        {
            Console.WriteLine($"Student with ID {id} deleted successfully.");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

        Util.WaitForKeyPress();
    }

    public void ListAll()
    {
        List<Student> students = this._jsonService.LoadEntities<Student>();
        if (students.Count == 0)
        {
            Console.WriteLine("No students found.");
            Util.WaitForKeyPress();
            return;
        }

        this._consoleWriter.WriteWithDoubleLine("All Students");
        foreach (Student student in students)
        {
            Console.WriteLine(student);
        }

    }
}