using Godot;
using System;


public partial class Limitfps : Node
{
     public override void _Ready()
    {
        // Set target FPS
        Engine.MaxFps = 180;
        // Optional: Disable VSync if needed (might interfere)
        //OS.VsyncEnabled = false;
    }
}
