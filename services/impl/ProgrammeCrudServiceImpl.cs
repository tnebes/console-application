#region

using console_app.Models;
using console_app.util;

#endregion

namespace console_app.services.impl;

public sealed class ProgrammeCrudServiceImpl : ICrudService<Programme>
{
    private readonly IConsoleWriterService _consoleWriter;
    private readonly IIdProviderService _idProvider;
    private readonly IJsonService _jsonService;

    public ProgrammeCrudServiceImpl()
    {
        this._jsonService = JsonServiceImpl.Instance;
        this._idProvider = IdProviderServiceImpl.Instance;
        this._consoleWriter = ConsoleWriterServiceImpl.Instance;
    }

    public void Create()
    {
        try
        {
            string name;
            double price;

            while (true)
            {
                name = Util.ReadUserStringInput("Enter programme name (or 'exit' to cancel):");
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
                price = Util.ReadUserDoubleInput("Enter programme price (or 0 to cancel):", 1, double.MaxValue);
                if (Math.Abs(price - 0d) < double.Epsilon)
                {
                    return;
                }

                if (price > 0d)
                {
                    break;
                }

                Console.WriteLine("Price must be positive. Please try again.");
            }

            Programme programme = new(this._idProvider.GetNextId(typeof(Programme)), name, price);
            this._jsonService.CreateEntity(programme);
            this._consoleWriter.WriteEntityAction("Programme created", programme);
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
        long id = Util.ReadUserLongInput("Enter programme ID:", 1L, long.MaxValue);
        Programme? programme = this._jsonService.LoadEntity<Programme>(id);

        if (programme == null)
        {
            Console.WriteLine("Programme not found.");
            Util.WaitForKeyPress();
            return;
        }

        this._consoleWriter.WriteEntityAction("Programme Details", programme);
    }

    public void Update()
    {
        string userInput = Util.ReadUserStringInput("Enter programme ID to update or L to list all programmes:");
        if (userInput.ToLower() == "l")
        {
            this.GetAll();
            this.Update();
            return;
        }

        long id = long.Parse(userInput);
        Programme? existingProgramme = this._jsonService.LoadEntity<Programme>(id);

        if (existingProgramme == null)
        {
            Console.WriteLine("Programme not found.");
            Util.WaitForKeyPress();
            return;
        }


        string newName;
        double newPrice;

        while (true)
        {
            newName = Util.ReadUserStringInput(
                $"Current name: {existingProgramme.Name}\nEnter new name (or press Enter to keep current):");
            if (!string.IsNullOrWhiteSpace(newName) || newName == "")
            {
                break;
            }

            Console.WriteLine("Name cannot be empty. Please try again.");
        }

        while (true)
        {
            string priceInput = Util.ReadUserStringInput(
                $"Current price: {existingProgramme.Price}\nEnter new price (or press Enter to keep current):");
            if (string.IsNullOrEmpty(priceInput))
            {
                newPrice = existingProgramme.Price;
                break;
            }

            if (double.TryParse(priceInput, out newPrice) && newPrice > 0d)
            {
                break;
            }

            Console.WriteLine("Invalid price. Please enter a positive number.");
        }

        Programme updatedProgramme = new(
            existingProgramme.Id,
            string.IsNullOrWhiteSpace(newName) ? existingProgramme.Name : newName,
            newPrice
        );

        this._jsonService.UpdateEntity(updatedProgramme, existingProgramme.Id);
        this._consoleWriter.WriteEntityAction("Programme updated", updatedProgramme);
    }

    public void Delete()
    {
        string userInput = Util.ReadUserStringInput("Enter programme ID to delete or L to list all programmes:");
        if (userInput.ToLower() == "l")
        {
            this.ListAll();
            this.Delete();
            return;
        }

        long id = long.Parse(userInput);
        bool deleted = this._jsonService.DeleteEntity<Programme>(id);

        if (deleted)
        {
            Console.WriteLine($"Programme with ID {id} deleted successfully.");
        }
        else
        {
            Console.WriteLine("Programme not found.");
        }
        Util.WaitForKeyPress();
    }

    public void ListAll()
    {
        List<Programme> programmes = this._jsonService.LoadEntities<Programme>();
        if (programmes.Count == 0)
        {
            Console.WriteLine("No programmes found.");
            return;
        }

        this._consoleWriter.WriteWithDoubleLine("All Programmes");
        foreach (Programme programme in programmes)
        {
            Console.WriteLine(programme);
        }
        Util.WaitForKeyPress();
    }
}