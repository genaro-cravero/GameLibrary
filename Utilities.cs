namespace GameLibrary;

public class Utilities
{
    public static string FirstCharToUpper(string sentence)
    {
        string str;
        str = "";
        var firstWord = sentence[0].ToString().ToUpper();
        //str += firstWord;

        for (int i = 1; i < sentence.Length; i++)
        {
            bool previousWasSpace = sentence[i - 1] == ' ';
            var currentChar = sentence[i].ToString();
            currentChar = previousWasSpace ? currentChar.ToUpper() : currentChar.ToLower();
            str += currentChar;
        }

        str = firstWord + str;

        return str;
    }

    public static void PrintColoredLine(string text, ConsoleColor font = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
    {
        Console.ForegroundColor = font;
        Console.BackgroundColor = background;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;

    }

}
