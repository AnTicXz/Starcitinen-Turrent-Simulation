using Godot;

public partial class MovePath : PathFollow3D
{
    [Export]
    public float MoveSpeed { get; set; } = 200.0f; // Speed in units per second

    [Export]
    public HSlider hSlider;

    public override void _Process(double delta)
    {
        ///MoveSpeed = (float)hSlider.Value;
        Progress += MoveSpeed * (float)delta; // Smooth movement along the path
    }

    public void UpdateMoveSpeed(double _speed)
    {
        MoveSpeed = (float)_speed;
    }
}
