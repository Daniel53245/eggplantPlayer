using System;

public class PlayerCommand
{
	public PlayerCommand()
	{
	}
}

public class CommandParser
{
    public Action<string, string> OnCommandParsed;

    public void Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return;

        // Split command into name and argument
        string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string commandName = parts[0].ToLower();
        string argument = parts.Length > 1 ? parts[1] : string.Empty;

        // Invoke callback for execution
        OnCommandParsed?.Invoke(commandName, argument);
    }
}
