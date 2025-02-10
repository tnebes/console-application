namespace console_app.util;

public static class Util
{
    public static int ReadUserIntInput(string message, int min, int max)
    {
        Console.WriteLine(message);
        try
        {
            int number = Convert.ToInt32(Console.ReadLine());
            if (number < min || number > max)
            {
                throw new ArgumentException($"Number must be larger than {min}");
            }

            return number;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ReadUserIntInput(message, min, max);
        }
    }

    public static long ReadUserLongInput(string message, long min, long max)
    {
        Console.WriteLine(message);
        try
        {
            long number = Convert.ToInt64(Console.ReadLine());
            if (number < min || number > max)
            {
                throw new ArgumentException($"Number must be between {min}");
            }

            return number;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ReadUserLongInput(message, min, max);
        }
    }

    public static double ReadUserDoubleInput(string message, double min, double max)
    {
        Console.WriteLine(message);
        try
        {
            double number = Convert.ToDouble(Console.ReadLine());
            if (number < min || number > max)
            {
                throw new ArgumentException($"Number must be between {min}");
            }

            return number;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return ReadUserDoubleInput(message, min, max);
        }
    }

    public static string ReadUserStringInput(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine() ?? "";
    }

    public static void ClearScreen()
    {
        Console.Clear();
    }

    public static void WaitForKeyPress()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public static bool EscapePressed(ConsoleKeyInfo keyInfo)
    {
        return keyInfo.Key == ConsoleKey.Escape;
    }
}