using Godot;
using System;

public partial class MakePath : Path3D
{
    [Export]
    public float Radius { get; set; } = 5.0f; // Radius of the circular path
    [Export]
    public int Segments { get; set; } = 32; // Number of points in the circle (higher = smoother)


    private float _previousRadius;
    private int _previousSegments;

    public override void _Ready()
    {
        // Initialize the curve and generate the circular path
        Curve = new Curve3D();
        GenerateCircularPath();
        // Store initial values for editor updates
        _previousRadius = Radius;
        _previousSegments = Segments;
    }

    public void UpdateRadius(double _radius)
    {
        Radius = (float)_radius;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Update the path if radius or segments change in the editor
        if (_previousRadius != Radius || _previousSegments != Segments)
        {
            GenerateCircularPath();
            _previousRadius = Radius;
            _previousSegments = Segments;
        }
    }

    private void GenerateCircularPath()
    {
        // Ensure valid parameters
        if (Segments < 3) Segments = 3; // Minimum for a closed shape
        if (Radius <= 0) Radius = 0.1f; // Avoid zero or negative radius

        // Clear existing curve points
        Curve.ClearPoints();

        // Generate points for a circle in the XZ plane
        for (int i = 0; i <= Segments; i++)
        {
            float angle = (float)i / Segments * Mathf.Tau; // Full circle (2π)
            float x = Mathf.Cos(angle) * Radius;
            float y = Mathf.Sin(angle) * Radius;
            Curve.AddPoint(new Vector3(x, y, 0));
        }
        //RotateX(0.785398f);
        var weapon = GetTree().CurrentScene.GetNodeOrNull<Node3D>("Weapon");
        if (weapon != null)
            LookAt(weapon.GlobalPosition);
    }

    public void MoveObject(float distance)
    {
        GlobalPosition = new Vector3(0, distance, distance);
    }
}