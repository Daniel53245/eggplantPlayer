using System;
using System.IO;
using System.CommandLine;
using WMPLib;

class MusicPlayerCLI
{
    static WindowsMediaPlayer player = new WindowsMediaPlayer();
    static string currentSong = "None";
    static bool isPlaying = false;

    static void Main(string[] args)
    {
        // Create root command
        var rootCommand = new RootCommand("🎵 CLI Music Player with System.CommandLine");

        // Play command
        var playCommand = new Command("play", "Play a music file");
        var filePath = new Argument<string>("filepath", "Path to the music file");        
        rootCommand.AddCommand(playCommand);
        playCommand.AddArgument(filePath);
        playCommand.SetHandler((filePath) =>
        {
            PlayMusic(filePath);
        },filePath);
        

        // Pause command
        var pauseCommand = new Command("pause", "Pause the currently playing song");
        pauseCommand.SetHandler(PauseMusic);
        rootCommand.AddCommand(pauseCommand);

        // Resume command
        var resumeCommand = new Command("resume", "Resume the paused song");
        resumeCommand.SetHandler(ResumeMusic);
        rootCommand.AddCommand(resumeCommand);

        // Stop command
        var stopCommand = new Command("stop", "Stop the currently playing song");
        stopCommand.SetHandler(StopMusic);
        rootCommand.AddCommand(stopCommand);

        // Show status command
        var statusCommand = new Command("status", "Show the currently playing song");
        statusCommand.SetHandler(ShowStatus);
        rootCommand.AddCommand(statusCommand);

        // Run the CLI
        while (true)
        {
            Console.Write("\nCommand> ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                continue; // Ignore empty input

            if (input.ToLower() == "exit")
            {
                Console.WriteLine("👋 Exiting music player...");
                return; // Exits the loop
            }

            string[] commandArgs = input.Split(' '); // Split input into command + arguments
            rootCommand.InvokeAsync(commandArgs).Wait(); // Run the command
        }
    }
    static Task PlayMusic(string filepath)
    {
        if (!File.Exists(filepath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ File not found!");
            Console.ResetColor();
            return Task.CompletedTask;
        }

        StopMusic();
        player.URL = filepath;
        player.controls.play();
        currentSong = Path.GetFileName(filepath);
        isPlaying = true;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"🎵 Now Playing: {currentSong}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    static void PauseMusic()
    {
        if (isPlaying)
        {
            player.controls.pause();
            isPlaying = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⏸️ Music Paused.");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("⚠️ No music is playing.");
        }
    }

    static void ResumeMusic()
    {
        if (!isPlaying && currentSong != "None")
        {
            player.controls.play();
            isPlaying = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("▶️ Music Resumed.");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("⚠️ No music to resume.");
        }
    }

    static void StopMusic()
    {
        if (currentSong != "None")
        {
            player.controls.stop();
            isPlaying = false;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("🛑 Music Stopped.");
            Console.ResetColor();
            currentSong = "None";
        }
        else
        {
            Console.WriteLine("⚠️ No music is playing.");
        }
    }

    static void ShowStatus()
    {
        Console.WriteLine($"🎵 Now Playing: {currentSong}");
        Console.WriteLine($"📌 Status: {(isPlaying ? "Playing" : "Paused/Stopped")}");
    }
}
