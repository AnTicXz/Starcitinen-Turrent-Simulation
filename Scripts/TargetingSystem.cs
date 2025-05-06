using Godot;
using System;

public partial class TargetingSystem : Node3D
{
    [Export]
    public Node3D TargetNode { get; set; }

    [Export]
    public Node3D FiringPointNode { get; set; }

    [Export]
    public float ProjectileSpeed { get; set; } // Increased for better intercept chances

    [Export]
    public float SmoothingFactor { get; set; } = 0.1f; // Controls velocity smoothing (0.0 = no smoothing, 1.0 = max smoothing)

    private Vector3 _previousTargetPosition;
    private Vector3 _targetVelocity;
    private bool _isFirstFrame = true;

    public override void _Ready()
    {
        if (TargetNode != null)
            _previousTargetPosition = TargetNode.GlobalPosition;
        else
            GD.PrintErr("TargetNode is not assigned in the editor.");

        if (FiringPointNode == null)
            GD.PrintErr("FiringPointNode is not assigned in the editor.");
    }

    public void SetProjectileSpeed(float _speed)
    {
        ProjectileSpeed = _speed;
    }
    public override void _Process(double delta)
    {
        if (TargetNode == null || FiringPointNode == null)
        {
            GD.PrintErr("TargetNode or FiringPointNode is not assigned.");
            return;
        }

        UpdateTargetVelocity(delta);

        Vector3 shooterPos = FiringPointNode.GlobalPosition;
        Vector3 targetPos = TargetNode.GlobalPosition;

        Vector3 interceptPoint = CalculateInterceptPoint(shooterPos, targetPos, _targetVelocity, ProjectileSpeed);

        // Move this node to the calculated intercept point (indicator)
        GlobalPosition = interceptPoint;
    }

    private void UpdateTargetVelocity(double delta)
    {
        Vector3 currentPosition = TargetNode.GlobalPosition;

        if (_isFirstFrame || delta <= 0)
        {
            _previousTargetPosition = currentPosition;
            _targetVelocity = Vector3.Zero;
            _isFirstFrame = false;
            return;
        }

        // Calculate raw velocity as change in position over time
        Vector3 rawVelocity = (currentPosition - _previousTargetPosition) / (float)delta;

        // Apply exponential moving average to smooth velocity
        _targetVelocity = _targetVelocity.Lerp(rawVelocity, 1f - Mathf.Pow(SmoothingFactor, (float)delta));

        _previousTargetPosition = currentPosition;
    }

    private Vector3 CalculateInterceptPoint(Vector3 shooterPos, Vector3 targetPos, Vector3 targetVel, float projectileSpeed)
    {
        // Relative position from shooter to target
        Vector3 toTarget = targetPos - shooterPos;

        // Quadratic coefficients
        float a = targetVel.LengthSquared() - projectileSpeed * projectileSpeed;
        float b = 2f * toTarget.Dot(targetVel);
        float c = toTarget.LengthSquared();

        // Calculate discriminant
        float discriminant = b * b - 4f * a * c;

        // If discriminant is negative, no intercept is possible
        if (discriminant < 0)
        {
            GD.Print("Negative discriminant, falling back to targetPos");
            return targetPos;
        }

        // Solve quadratic equation
        float sqrtDisc = Mathf.Sqrt(discriminant);
        float t1 = (-b + sqrtDisc) / (2f * a);
        float t2 = (-b - sqrtDisc) / (2f * a);

        // Select the smallest positive time
        float t;
        if (t1 > 0 && t2 > 0)
            t = Mathf.Min(t1, t2);
        else if (t1 > 0)
            t = t1;
        else if (t2 > 0)
            t = t2;
        else
        {
            GD.Print($"No positive time (t1: {t1}, t2: {t2}), falling back to targetPos");
            return targetPos;
        }

        // Return the predicted intercept position
        return targetPos + targetVel * t;
    }
}