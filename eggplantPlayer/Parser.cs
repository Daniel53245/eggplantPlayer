using System;
namespace EggplantPlayer
{

public class CommandParser
{
    // Delegate to trigger execution after parsing
    public Action<string, string> OnCommandParsed;

    public void Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return; // Ignore empty input

        // Split the input string into a command and an optional argument
        string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string commandName = parts[0].ToLower(); // First word is the command
        string argument = parts.Length > 1 ? parts[1] : string.Empty; // Everything else is an argument

        // Call the event to trigger execution
        OnCommandParsed?.Invoke(commandName, argument);
    }
}

}
