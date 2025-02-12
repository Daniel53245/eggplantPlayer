using System;
using System.IO;
using System.CommandLine;
using WMPLib;
using Terminal.Gui;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using EggplantPlayer;
using System.CommandLine.Parsing;

namespace EggplantPlayer
{
class MusicPlayerCLI
{


    public static void Main(string[] args)
    {

        //// Create root command
        //var rootCommand = new RootCommand("🎵 CLI Music Player with System.CommandLine");

        //// Play command
        //var playCommand = new System.CommandLine.Command("play", "Play a music file");
        //var filePath = new Argument<string>("filepath", "Path to the music file");        
        //rootCommand.AddCommand(playCommand);
        //playCommand.AddArgument(filePath);
        //playCommand.SetHandler((filePath) =>
        //{
        //    PlayMusic(filePath);
        //},filePath);


        //// Pause command
        //var pauseCommand = new System.CommandLine.Command("pause", "Pause the currently playing song");
        //pauseCommand. (PauseMusic);
        //rootCommand.AddCommand(pauseCommand);

        //// Resume command
        //var resumeCommand = new System.CommandLine.Command("resume", "Resume the paused song");
        //resumeCommand.SetHandler(ResumeMusic);
        //rootCommand.AddCommand(resumeCommand);

        //// Stop command
        //var stopCommand = new System.CommandLine.Command("stop", "Stop the currently playing song");
        //stopCommand.SetHandler(StopMusic);
        //rootCommand.AddCommand(stopCommand);

        //// Show status command
        //var statusCommand = new System.CommandLine.Command("status", "Show the currently playing song");
        //statusCommand.SetHandler(ShowStatus);
        //rootCommand.AddCommand(statusCommand);

        // Override the default configuration for the application to use the Light theme

        Application.Run<PlayerWindow>().Dispose();

        // Before the application exits, reset Terminal.Gui for clean shutdown
        Application.Shutdown();

        // To see this output on the screen it must be done after shutdown,
        // which restores the previous screen.
        Console.WriteLine($@"Username: {PlayerWindow.UserName}");
    }











}

public class CommandTextView : TextView
{
    public Action<string> OnCommandEntered;

    protected override bool OnKeyDown(Key keyEvent)
    {
        if (keyEvent == Key.Enter)
        {
            string command = this.Text.ToString().Trim(); // Get input
            if (!string.IsNullOrWhiteSpace(command))
            {
                OnCommandEntered?.Invoke(command); // Trigger command execution
            }

            this.Text = string.Empty; // Clear input after execution
            return true; // Mark event as handled
                            //Process Command here 
        }
        else
        {
            return base.OnKeyDown(keyEvent);
        }
    }

}

public class PlayerWindow : Window
{
    public static string UserName;

    WindowsMediaPlayer player;
    string currentSong = "None";
    bool isPlaying = false;

    private TextView infoDisplay;
    private CommandParser commandParser;

    public Task PlayMusic(string filepath)
    {
        if (!File.Exists(filepath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ File not found!");
            Console.ResetColor();
            return Task.CompletedTask;
        }

        StopMusic();
        this.player.URL = filepath;
        this.player.controls.play();
        this.currentSong = Path.GetFileName(filepath);
        this.isPlaying = true;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"🎵 Now Playing: {this.currentSong}");
        Console.ResetColor();
        return Task.CompletedTask;
    }

    public void PauseMusic()
    {
        if (this.isPlaying)
        {
            this.player.controls.pause();
            this.isPlaying = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⏸️ Music Paused.");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("⚠️ No music is playing.");
        }
    }

    public void ResumeMusic()
    {
        if (!this.isPlaying && this.currentSong != "None")
        {
            this.player.controls.play();
            this.isPlaying = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("▶️ Music Resumed.");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine("⚠️ No music to resume.");
        }
    }

    public void StopMusic()
    {
        if (this.currentSong != "None")
        {
            this.player.controls.stop();
            this.isPlaying = false;
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

    public void ShowStatus()
    {
        Console.WriteLine($"🎵 Now Playing: {this.currentSong}");
        Console.WriteLine($"📌 Status: {(this.isPlaying ? "Playing" : "Paused/Stopped")}");
    }

    private void ProcessCommand(string command, string arugment)
    {
        switch (command.ToLower())
        {
            case "play":
                PlayMusic(arugment);
                UpdateInfoPanel("🎵 Now Playing: example.mp3");
                break;

            case "pause":
                PauseMusic();
                UpdateInfoPanel("⏸️ Music Paused.");
                break;

            case "resume":
                ResumeMusic();
                UpdateInfoPanel("▶️ Music Resumed.");
                break;

            case "stop":
                StopMusic();
                UpdateInfoPanel("🛑 Music Stopped.");
                break;

            case "status":
                UpdateInfoPanel($"🎵 Now Playing: {currentSong}\n📌 Status: {(isPlaying ? "Playing" : "Paused/Stopped")}");
                break;

            case "exit":
                Application.RequestStop();
                break;

            default:
                UpdateInfoPanel($"⚠️ Unknown command: {command}");
                break;
        }
    }

    private void UpdateInfoPanel(string newText)
    {
        Application.Invoke(() => { infoDisplay.Text = newText; });
    }


    public PlayerWindow()
    {
        this.player = new WindowsMediaPlayer();
        this.commandParser = new CommandParser();

        Title = $"EggplantPlayer ({Application.QuitKey} to quit)";
        
        // Component Construction>>>
        var leftPanel = new FrameView()
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(50),
            Height = Dim.Fill(),

        };
        var infoDisplay = new TextView()
        {
            X = 2,
            Y = 1,
            ReadOnly = true,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            Text = "Welcome to EggplatPlayer\n\nMore Details here"
        };
        this.infoDisplay = infoDisplay;
        leftPanel.Add(infoDisplay);

        var rightPanel = new FrameView()
        {
            X = Pos.Right(leftPanel),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        //>>>
        var commmandField = new CommandTextView()
        {
            X = 1,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        commmandField.OnCommandEntered = commandParser.Parse;
        commandParser.OnCommandParsed = ProcessCommand;
        rightPanel.Add(commmandField);
        Add(leftPanel, rightPanel);

    }
}
}