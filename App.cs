using console_app.Services;
using console_app.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_app;

public sealed class App
{
    private readonly IConsoleWriterService ConsoleWriterService = new ConsoleWriterServiceImpl();
   public App()
    {
        try
        {
            while (true)
            {
                this.WriteIntroduction();
                this.WriteMainMenu();
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void WriteIntroduction()
    {
        this.ConsoleWriterService.WriteWithDoubleLine("Welcome to a Simple Console Application");
    }

    private void WriteMainMenu()
    {

    }
}
