using Godot;
using System;

public partial class Weapon : Node3D
{
    // Export properties for easy tweaking in the editor
    [Export]
    public float FireRate = 2.0f; // Shots per second
    [Export]
    public float ProjectileSpeed = 50.0f; // Speed of the projectile
    [Export]
    public float ConeAngle = 5.0f; // Cone of fire angle in degrees

    [Export]
    public PackedScene ProjectileScene; // Reference to the projectile scene

    [Export]
    public ControlParams _ControlParams;

    [Export]
    public Node3D targetNodeLookAt;

    private float _timeSinceLastShot = 0.0f;

    public void SetProjectileSpeed(float _speed)
    {
        ProjectileSpeed = _speed;
    }

    public override void _Process(double delta)
    {
        // Ensure the target node exists
        if (targetNodeLookAt != null)
        {
            // Make this node look at the target's global position
            LookAt(targetNodeLookAt.GlobalPosition, Vector3.Up );

            // GD.Print(GlobalRotation);
            // GD.Print(Rotation);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // Update the timer
        _timeSinceLastShot += (float)delta;

        // Fire when enough time has passed based on fire rate
        if (_timeSinceLastShot >= 1.0f / FireRate)
        {
            Fire();
            _timeSinceLastShot = 0.0f; // Reset the timer
        }
    }

    public void SetConeOfFire(float ang)
    {
        ConeAngle = ang;
    }

    public void SetFireRate(float rate)
    {
        FireRate = rate;
    }

    private void Fire()
    {

        if (ProjectileScene == null)
        {
            GD.PrintErr("ProjectileScene not set!");
            return;
        }

        // Instantiate a new projectile
        var projectile = ProjectileScene.Instantiate<Projectile>();
        projectile.GlobalTransform = this.GlobalTransform;
        GetTree().CurrentScene.AddChild(projectile);

        // Calculate random spread within the cone of fire
        float angleRad = Mathf.DegToRad(ConeAngle);
        Vector2 randomSpread = new Vector2(
            (float)GD.RandRange(-angleRad, angleRad),
            (float)GD.RandRange(-angleRad, angleRad)
        );

        // Connect ProjectileHit signal to ControlParams' OnProjectileHit
        if (_ControlParams != null)
        {
            projectile.ProjectileHit += _ControlParams.OnProjectileHit;
            projectile.ProjectileMiss += _ControlParams.OnProjectileMiss;
        }



        //GetTree().CurrentScene.AddChild(projectile);

        Vector3 forward = -GlobalTransform.Basis.Z; // Weapon's forward direction
        Vector3 direction = forward.Rotated(Vector3.Up, randomSpread.X).Rotated(Vector3.Right, randomSpread.Y);

        //projectile.Fire();
        projectile.Fire(GlobalPosition, direction, ProjectileSpeed);


    }
}