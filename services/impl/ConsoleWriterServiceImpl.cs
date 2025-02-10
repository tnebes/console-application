#region

using System.Text;

#endregion

namespace console_app.Services.Impl;

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
            if (_instance == null) _instance = new ConsoleWriterServiceImpl();

            return _instance;
        }
    }

    public void WriteLine(int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        StringBuilder sb = new StringBuilder();
        Console.WriteLine(sb.AppendJoin("", Enumerable.Repeat(lineCharacter, messageLength)).ToString());
    }

    public void WriteWithDoubleLine(string message, int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        WriteLine(messageLength);
        Console.WriteLine(message);
        WriteLine(messageLength);
    }

    public void WriteWithSingleLine(string message, int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        WriteLine(messageLength);
        Console.WriteLine(message);
    }
}