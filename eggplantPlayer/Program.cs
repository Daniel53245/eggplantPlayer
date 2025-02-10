using System;
using System.IO;
using System.CommandLine;
using WMPLib;
using Terminal.Gui;

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
        var playCommand = new System.CommandLine.Command("play", "Play a music file");
        var filePath = new Argument<string>("filepath", "Path to the music file");        
        rootCommand.AddCommand(playCommand);
        playCommand.AddArgument(filePath);
        playCommand.SetHandler((filePath) =>
        {
            PlayMusic(filePath);
        },filePath);
        

        // Pause command
        var pauseCommand = new System.CommandLine.Command("pause", "Pause the currently playing song");
        pauseCommand.SetHandler(PauseMusic);
        rootCommand.AddCommand(pauseCommand);

        // Resume command
        var resumeCommand = new System.CommandLine.Command("resume", "Resume the paused song");
        resumeCommand.SetHandler(ResumeMusic);
        rootCommand.AddCommand(resumeCommand);

        // Stop command
        var stopCommand = new System.CommandLine.Command("stop", "Stop the currently playing song");
        stopCommand.SetHandler(StopMusic);
        rootCommand.AddCommand(stopCommand);

        // Show status command
        var statusCommand = new System.CommandLine.Command("status", "Show the currently playing song");
        statusCommand.SetHandler(ShowStatus);
        rootCommand.AddCommand(statusCommand);

        // Override the default configuration for the application to use the Light theme

        Application.Run<PlayerWindow>().Dispose();

        // Before the application exits, reset Terminal.Gui for clean shutdown
        Application.Shutdown();

        // To see this output on the screen it must be done after shutdown,
        // which restores the previous screen.
        Console.WriteLine($@"Username: {PlayerWindow.UserName}");

        //// Main Loop
        //while (true)
        //{
        //    Console.Write("\nCommand> ");
        //    string? input = Console.ReadLine()?.Trim();

        //    if (string.IsNullOrEmpty(input))
        //        continue; // Ignore empty input

        //    if (input.ToLower() == "exit")
        //    {
        //        Console.WriteLine("👋 Exiting music player...");
        //        return; // Exits the loop
        //    }

        //    string[] commandArgs = input.Split(' '); // Split input into command + arguments
        //    rootCommand.InvokeAsync(commandArgs).Wait(); // Run the command
        //}
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

public class PlayerWindow : Window
{
    public static string UserName;

    public PlayerWindow()
    {
        Title = $"Example App ({Application.QuitKey} to quit)";

        // Create input components and labels
        var usernameLabel = new Label { Text = "Username:" };

        var userNameText = new TextField
        {
            // Position text field adjacent to the label
            X = Pos.Right(usernameLabel) + 1,

            // Fill remaining horizontal space
            Width = Dim.Fill()
        };

        var passwordLabel = new Label
        {
            Text = "Password:",
            X = Pos.Left(usernameLabel),
            Y = Pos.Bottom(usernameLabel) + 1
        };

        var passwordText = new TextField
        {
            Secret = true,

            // align with the text box above
            X = Pos.Left(userNameText),
            Y = Pos.Top(passwordLabel),
            Width = Dim.Fill()
        };

        // Create login button
        var btnLogin = new Button
        {
            Text = "Login",
            Y = Pos.Bottom(passwordLabel) + 1,

            // center the login button horizontally
            X = Pos.Center(),
            IsDefault = true
        };
        btnLogin.Accepting += (s,e) => { 
            var n = MessageBox.Query("Quit Demo", "Are you sure you want to quit this demo?", "Yes", "No");
            if (n == 0) {
                Application.RequestStop();
            }
            else
            {
                MessageBox.ErrorQuery("Hi", "you got ther wrong one", "return");
            }
        };


        // Add the views to the Window
        Add(usernameLabel, userNameText, passwordLabel, passwordText, btnLogin);
    }
}