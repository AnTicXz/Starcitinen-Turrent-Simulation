using Godot;
using System;
using System.Diagnostics;


public partial class Projectile : RayCast3D
{
    [Export]
    public float Lifetime = 7.0f; // Time in seconds before projectile is destroyed

    [Export]
    public float _ProjectileSpeed = 100;
    [Export]
    public bool IsActive = false;

    [Export]
    public Vector3 Velocity = Vector3.Zero;

    private float _timer = 0.0f;


    [Signal]
    public delegate void ProjectileHitEventHandler(string name);

    [Signal]
    public delegate void ProjectileMissEventHandler();


    public override void _PhysicsProcess(double delta)
    {
        if (!IsActive)
        {
            return;
        }

        GlobalPosition += Velocity * (float)delta;
        // this.TargetPosition = Velocity.Normalized() * 10;
        ForceRaycastUpdate();

        if (IsColliding())
        {
            var collider = GetCollider() as Node3D;
            //GD.Print(collider.Name);
            Hit(collider.Name);
            IsActive = false;
        }

        _timer += (float)delta;
        if (_timer >= Lifetime)
        {
            //GD.Print("Projectile died");
            EmitSignal(SignalName.ProjectileMiss);
            QueueFree();
        }

    }

    private void Hit(string name)
    {
       // GD.Print("Projectile hit something");
        EmitSignal(SignalName.ProjectileHit,name);
        QueueFree();
        
    }
    public void Fire(Vector3 startPos, Vector3 direction, float projectileSpeed)
    {
        // Set initial position
        GlobalPosition = startPos;

        // Set velocity
        Velocity = direction.Normalized() * projectileSpeed;

        // Rotate to face the direction of travel
        if (direction != Vector3.Zero)
        {
            GlobalTransform = GlobalTransform.LookingAt(GlobalPosition + direction, Vector3.Up);
        }
        // Activate the projectile
        IsActive = true;
    }

    public void Fire()
    {

        Velocity = -GlobalTransform.Basis.Z * _ProjectileSpeed;

        IsActive = true;
    }

}
