namespace alcocodebnb
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
    }
}