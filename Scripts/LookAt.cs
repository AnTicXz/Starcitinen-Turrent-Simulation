using Godot;

public partial class LookAt : Node3D
{
    [Export]
    public Node3D targetNode;

    public override void _Ready()
    {
        // Get the target node when the scene is loaded
        if (targetNode == null)
        {
            return;
        }

    }
    public override void _Process(double delta)
    {
        // Ensure the target node exists
        if (targetNode != null)
        {
            // Make this node look at the target's global position
            LookAt(targetNode.GlobalPosition, Vector3.Up );

            // GD.Print(GlobalRotation);
            // GD.Print(Rotation);
        }
    }
}