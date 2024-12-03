using System;

namespace alcocodebnb.ConsoleUtilities
{
    public abstract class ColoredConsoleBase
    {
        protected void WriteLineWithColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        protected void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        protected void WriteWithColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        protected void Write(string message)
        {
            Console.Write(message);
        }
    }
}