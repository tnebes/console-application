using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_app.Services.Impl;

public class ConsoleWriterServiceImpl : IConsoleWriterService
{
    public void WriteLine(int messageLength = IConsoleWriterService.LineLength, char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        StringBuilder sb = new StringBuilder();
        Console.WriteLine(sb.AppendJoin("", Enumerable.Repeat(lineCharacter, messageLength)).ToString());
    }

    public void WriteWithDoubleLine(string message, int messageLength = IConsoleWriterService.LineLength, char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        this.WriteLine(messageLength);
        Console.WriteLine(message);
        this.WriteLine(messageLength);
    }

    public void WriteWithSingleLine(string message, int messageLength = IConsoleWriterService.LineLength, char lineCharacter = IConsoleWriterService.DefaultCharacter)
    {
        this.WriteLine(messageLength);
        Console.WriteLine(message);
    }
}
