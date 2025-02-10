namespace console_app.util;

public static class Util
{
    private static readonly ConsoleColor PromptColor = ConsoleColor.DarkGreen;
    private static readonly ConsoleColor InputColor = ConsoleColor.Green;
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    private static readonly ConsoleColor InfoColor = ConsoleColor.Cyan;

    public static int ReadUserIntInput(string message, int min, int max)
    {
        WritePrompt(message);
        try
        {
            Console.ForegroundColor = InputColor;
            int number = Convert.ToInt32(Console.ReadLine());
            if (number < min || number > max)
            {
                throw new ArgumentException($"Number must be between {min} and {max}");
            }

            return number;
        }
        catch (Exception ex)
        {
            WriteError(ex.Message);
            return ReadUserIntInput(message, min, max);
        }
    }

    public static long ReadUserLongInput(string message, long min, long max)
    {
        WritePrompt(message);
        try
        {
            Console.ForegroundColor = InputColor;
            long number = Convert.ToInt64(Console.ReadLine());
            if (number < min || number > max)
            {
                throw new ArgumentException($"Number must be between {min} and {max}");
            }

            return number;
        }
        catch (Exception ex)
        {
            WriteError(ex.Message);
            return ReadUserLongInput(message, min, max);
        }
    }

    public static double ReadUserDoubleInput(string message, double min, double max)
    {
        WritePrompt(message);
        try
        {
            Console.ForegroundColor = InputColor;
            double number = Convert.ToDouble(Console.ReadLine());
            if (number < min || number > max)
            {
                throw new ArgumentException($"Number must be between {min} and {max}");
            }

            return number;
        }
        catch (Exception ex)
        {
            WriteError(ex.Message);
            return ReadUserDoubleInput(message, min, max);
        }
    }

    public static string ReadUserStringInput(string message)
    {
        WritePrompt(message);
        Console.ForegroundColor = InputColor;
        return Console.ReadLine() ?? "";
    }

    public static void ClearScreen()
    {
        Console.Clear();
    }

    public static void WaitForKeyPress()
    {
        Console.ForegroundColor = InfoColor;
        Console.WriteLine("\nPress any key to continue...");
        Console.ForegroundColor = InputColor;
        Console.ReadKey();
    }

    public static bool EscapePressed(ConsoleKeyInfo keyInfo)
    {
        return keyInfo.Key == ConsoleKey.Escape;
    }

    private static void WritePrompt(string message)
    {
        Console.ForegroundColor = PromptColor;
        Console.Write($"{message} ");
        Console.ForegroundColor = InputColor;
    }

    private static void WriteError(string message)
    {
        Console.ForegroundColor = ErrorColor;
        Console.WriteLine($"Error: {message}");
        Console.ForegroundColor = InputColor;
    }
}