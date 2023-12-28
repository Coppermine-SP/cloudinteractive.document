using System.Net.Mime;

namespace DocumentTestApp
{
    internal static partial class Util
    {
        public enum PrintType { Info, Warn, Error, Question }
        public static void ConsolePrint(PrintType type, string text)
        {
            ConsoleColor color;
            string prefix;

            if (type == PrintType.Info) {
                color = ConsoleColor.DarkBlue;
                prefix = "[i]";
            }
            else if (type == PrintType.Warn) {
                color = ConsoleColor.Yellow;
                prefix = "[!]";
            }
            else if(type == PrintType.Error) {
                color = ConsoleColor.Red;
                prefix = "[!]";
            }
            else
            {
                color = ConsoleColor.Cyan;
                prefix = "[?]";
            }

            Console.ForegroundColor = color;
            Console.Write(prefix + " ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(text);
        }

        public static string ConsoleInput(string text)
        {
            ConsolePrint(PrintType.Question, text +":");
            Console.Write("> ");
            return Console.ReadLine() ?? " ";
        }

        public static bool ConsoleInputBool(string text)
        {
            do
            {
                string input = ConsoleInput(text + " (y/n)").ToLower();
                if (input == "y")
                {
                    return true;
                }
                else if (input == "n")
                {
                    return false;
                }
            } while (true);
        }

        public static void ConsoleExit()
        {
            Console.WriteLine("\n\nPress any key to close this window...");
            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}
