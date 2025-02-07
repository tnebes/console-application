using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_app.Services;


public interface IConsoleWriterService
{
    protected const char DefaultCharacter = '=';

    protected const int LineLength = 40;
    void WriteLine(int messageLength = LineLength, char lineCharacter = DefaultCharacter);
    void WriteWithSingleLine(string message, int messageLength = LineLength, char lineCharacter = DefaultCharacter);
    void WriteWithDoubleLine(string message, int messageLength = LineLength, char lineCharacter = DefaultCharacter);

}
