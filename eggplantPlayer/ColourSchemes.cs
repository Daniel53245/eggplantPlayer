using Terminal.Gui;

namespace EggplantPlayer
{
    public class CustomColorSchemes
    {
        public static ColorScheme BlackAndWhite = new ColorScheme
            {
                Normal = new Terminal.Gui.Attribute(Color.White, Color.Black),    // Standard text: White on Black
                Focus = new Terminal.Gui.Attribute(Color.BrightGreen, Color.Black), // Focused element: Bright White on Black
                HotNormal = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Black), // Hotkey in normal state: Bright Yellow on Black
                HotFocus = new Terminal.Gui.Attribute(Color.BrightYellow, Color.Black),  // Hotkey in focused state: Bright Yellow on Black
                Disabled = new Terminal.Gui.Attribute(Color.Gray, Color.Black)    // Disabled elements: Gray on Black
            };
    }
}
