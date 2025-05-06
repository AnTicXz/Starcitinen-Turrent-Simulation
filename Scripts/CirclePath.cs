using Godot;

public partial class CirclePath : Path3D
{
    [Export]
    private float _radius = 5.0f; // Radius of the circular path
    [Export]
    private int _segments = 32; // Number of segments for the circle (higher = smoother)
    [Export]
    private float _speed = 2.0f; // Speed of movement along the path (units per second)

    private PathFollow3D _pathFollow;

    [Export] public Cubemap Node3d;

    public override void _Ready()
    {
        // Create a new Curve3D for the Path3D
        Curve = new Curve3D();
        GenerateCircularPath();

        // Create a PathFollow3D as a child
        _pathFollow = new PathFollow3D();
        AddChild(_pathFollow);

    
    }

    public override void _Process(double delta)
    {
        // Update the PathFollow3D's progress to move along the path
        _pathFollow.Progress += _speed * (float)delta;
    }

    private void GenerateCircularPath()
    {
        // Clear existing curve points
        Curve.ClearPoints();

        // Generate points for a circle in the XZ plane
        for (int i = 0; i <= _segments; i++)
        {
            float angle = (float)i / _segments * Mathf.Tau; // Full circle (2π)
            float x = Mathf.Cos(angle) * _radius;
            float z = Mathf.Sin(angle) * _radius;
            Curve.AddPoint(new Vector3(x, 0, z));
        }
    }
}