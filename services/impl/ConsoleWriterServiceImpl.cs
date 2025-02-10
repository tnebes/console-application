#region

using System.Text;
using console_app.util;

#endregion

namespace console_app.services.impl;

public sealed class ConsoleWriterServiceImpl : IConsoleWriterService
{
    private static ConsoleWriterServiceImpl _instance;

    private ConsoleWriterServiceImpl()
    {
    }

    public static ConsoleWriterServiceImpl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ConsoleWriterServiceImpl();
            }

            return _instance;
        }
    }

    public void WriteLine(int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        StringBuilder sb = new();
        Console.WriteLine(sb.AppendJoin("", Enumerable.Repeat(lineCharacter, messageLength)).ToString());
    }

    public void WriteWithDoubleLine(string message, int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        this.WriteLine(messageLength);
        Console.WriteLine(message);
        this.WriteLine(messageLength);
    }

    public void WriteWithSingleLine(string message, int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        this.WriteLine(messageLength);
        Console.WriteLine(message);
    }

    public void WriteEntityAction<T>(string actionMessage, T entity)
    {
        Util.ClearScreen();
        this.WriteWithDoubleLine(actionMessage);
        Console.WriteLine(entity.ToString());
        Util.WaitForKeyPress();
    }
}