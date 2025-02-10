namespace console_app.services;

public interface IConsoleWriterService
{
    protected const char DefaultCharacter = '=';

    protected const int LineLength = 40;
    void WriteLine(int messageLength = LineLength, char lineCharacter = DefaultCharacter);
    void WriteWithSingleLine(string message, int messageLength = LineLength, char lineCharacter = DefaultCharacter);
    void WriteWithDoubleLine(string message, int messageLength = LineLength, char lineCharacter = DefaultCharacter);
    void WriteEntityAction<T>(string actionMessage, T entity);
}