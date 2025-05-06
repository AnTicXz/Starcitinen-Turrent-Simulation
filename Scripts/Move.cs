using Godot;
using System;
using System.Diagnostics;



public partial class Move : Node3D
{
    private float _speed = 100f; // Pixels per second

    public override void _PhysicsProcess(double delta)
    {
        Vector3 newPosition = GlobalPosition;
        newPosition.X += _speed * (float)delta; // Move right
        GlobalPosition = newPosition;
    }
}
