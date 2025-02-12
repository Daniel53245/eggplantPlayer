using System;
using System.IO;
using System.CommandLine;
using WMPLib;
using Terminal.Gui;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using EggplantPlayer;
using System.CommandLine.Parsing;
using eggplantPlayer;
using System.Runtime.CompilerServices;

namespace EggplantPlayer
{
class MusicPlayerCLI
{
    public static void Main(string[] args)
    {
        Application.Run<PlayerWindow>().Dispose();
        Application.Shutdown();
        Console.WriteLine($@"Username: {PlayerWindow.UserName}");
    }

}


public class PlayerWindow : Window
{
    public static string UserName;

    WindowsMediaPlayer player;
    string currentSong = "None";
    bool isPlaying = false;

    

    private TextView infoDisplay;
    private TextView animationDisplay;
    private CommandParser commandParser;
    private Animation currentAnimation;

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

    private string CenterAscii(string asciiFrame)
    {
        if (animationDisplay == null) return asciiFrame; // Safety check

        int displayWidth = animationDisplay.Frame.Width;  // Available width
        int displayHeight = animationDisplay.Frame.Height; // Available height

        string[] lines = asciiFrame.Split('\n');
        int asciiWidth = lines.Max(line => line.Length); // Longest line width
        int asciiHeight = lines.Length; // Number of lines

        // Calculate horizontal padding
        int leftPadding = Math.Max(0, (displayWidth - asciiWidth) / 2);

        // Calculate vertical padding
        int topPadding = Math.Max(0, (displayHeight - asciiHeight) / 2);

        // Apply horizontal padding (spaces)
        string paddedAscii = string.Join("\n", lines.Select(line => new string(' ', leftPadding) + line));

        // Apply vertical padding (new lines)
        string verticalPadding = new string('\n', topPadding);
        return verticalPadding + paddedAscii;
    }

    private void StartAsciiAnimation()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                string frame = currentAnimation.NextFrame();
                string centeredFrame = CenterAscii(frame);
                Application.Invoke(() => { this.animationDisplay.Text = centeredFrame; });

                await Task.Delay(currentAnimation.GetFrameDelay());
            }
        });
    }

    public PlayerWindow(){
        this.player = new WindowsMediaPlayer();
        this.commandParser = new CommandParser();
        this.currentAnimation = new Animation(AnimationSequences.LoadingBar);
        this.ColorScheme = CustomColorSchemes.BlackAndWhite;

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
            X = 0,
            Y = 0,
            ReadOnly = true,
            Width = Dim.Fill(),
            Height = Dim.Percent(50),
            Text = "Welcome to EggplatPlayer\n\nMore Details here"
        };
        leftPanel.Add(infoDisplay);
        
        var animationDisplay = new TextView()
        {
            X= 0,
            Y = Pos.Bottom(infoDisplay),
            ReadOnly = true,
            Width= Dim.Fill(),
            Height = Dim.Percent(50),
            Text = $"Animations Here"
        };
        this.animationDisplay = animationDisplay;
        leftPanel.Add(animationDisplay);
        this.infoDisplay = infoDisplay;

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
        StartAsciiAnimation();
        Add(leftPanel, rightPanel);

    }
}
}