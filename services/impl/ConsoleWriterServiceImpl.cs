#region

using System.Text;
using console_app.util;

#endregion

namespace console_app.services.impl;

public sealed class ConsoleWriterServiceImpl : IConsoleWriterService
{
    private const ConsoleColor PrimaryColor = ConsoleColor.Green;
    private const ConsoleColor SecondaryColor = ConsoleColor.DarkGreen;
    private const ConsoleColor HighlightColor = ConsoleColor.White;
    private static ConsoleWriterServiceImpl _instance;

    private ConsoleWriterServiceImpl()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = PrimaryColor;
        Console.Clear();
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
        Console.ForegroundColor = SecondaryColor;
        Console.WriteLine(sb.AppendJoin("", Enumerable.Repeat(lineCharacter, messageLength)).ToString());
        Console.ForegroundColor = PrimaryColor;
    }

    public void WriteWithDoubleLine(string message, int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        this.WriteLine(messageLength);
        WriteHighlightedCentered(message, messageLength);
        this.WriteLine(messageLength);
    }

    public void WriteWithSingleLine(string message, int messageLength = IConsoleWriterService.LineLength,
        char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        this.WriteLine(messageLength);
        WriteHighlightedCentered(message, messageLength);
    }

    public void WriteEntityAction<T>(string actionMessage, T entity)
    {
        Util.ClearScreen();
        this.WriteWithDoubleLine(actionMessage);
        Console.ForegroundColor = HighlightColor;
        Console.WriteLine(entity.ToString());
        Console.ForegroundColor = PrimaryColor;
        Util.WaitForKeyPress();
    }

    private static void WriteHighlightedCentered(string message, int totalLength)
    {
        Console.ForegroundColor = HighlightColor;
        int padding = (totalLength - message.Length) / 2;
        Console.WriteLine(message.PadLeft(padding + message.Length).PadRight(totalLength));
        Console.ForegroundColor = PrimaryColor;
    }
}