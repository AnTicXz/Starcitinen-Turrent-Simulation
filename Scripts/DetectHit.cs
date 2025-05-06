using Godot;


public partial class DetectHit : Area3D
{
    private int count;
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        //throw new NotImplementedException();
        GD.Print("Prj Hit Target " + count);
        count++;
    }

}
