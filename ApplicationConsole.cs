using System;

namespace alcocodebnb.ConsoleUtilities
{
    public class ApplicationConsole : ColoredConsoleBase
    {
        public void WriteMenuTitle(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Yellow);
        }

        public void WriteMenuOption(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Magenta);
        }

        public void WriteError(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Red);
        }

        public void WriteSuccess(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Green);
        }

        public void WriteInfo(string message)
        {
            WriteLineWithColor(message, ConsoleColor.Cyan);
        }

        public void WriteNeutralLine(string message)
        {
            WriteLine(message);
        }

        public void WriteColoredLine(string message, ConsoleColor color)
        {
            WriteLineWithColor(message, color);
        }
    }
}