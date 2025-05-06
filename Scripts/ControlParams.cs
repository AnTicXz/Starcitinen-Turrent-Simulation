using Godot;
using System;

public partial class ControlParams : Control
{



    [Export]
    public TargetingSystem targetingSystem;
    [Export]
    public Weapon weapon;
    [Export]
    public HSlider hSlider;
    [Export]
    public HSlider hSlider2;
    [Export]
    public HSlider hSlider3;
    [Export]
    public HSlider hSliderConeOfFire;
    [Export]
    public HSlider hSliderFireRate;
    [Export]
    public MakePath makePath;
    [Export]
    public MovePath movePath;
    [Export]
    public Label label;
    [Export]
    public Label label2;
    [Export]
    public Label label3;
    [Export]
    public Label labelConeofFire;
    [Export]
    public Label labelFireRate;


    [Export] public Label F8HitcountLable; [Export] public Label F8misscountlable; [Export] public Label f8percentLable;

    [Export] public HSlider DistnaceSlider; [Export] public Label DistanceLable;



    [Export] Label counterlable; [Export] Label misslable; [Export] Label Hitpercentage;
    [Export] Button resetButton;

    private int _hitCount; private int _misscount;
    private int _f8hitcount; private int _f8misscount;

    private int _totalShots;

    public override void _Ready()
    {
        // Connect the ValueChanged signal to a function
        hSlider.ValueChanged += OnHSliderValueChanged;
        hSlider2.ValueChanged += OnHSliderValueChanged2;
        hSlider3.ValueChanged += OnHSliderValueChanged3;
        hSliderConeOfFire.ValueChanged += OnHSliderValueChanged4;
        hSliderFireRate.ValueChanged += OnHSliderValueChanged5;
        DistnaceSlider.ValueChanged += OnHSliderValueChanged6;
        resetButton.Pressed += OnResetButtonPressed;
    }
    private void OnResetButtonPressed()
    {
        _hitCount = 0;
        counterlable.Text = "Gladius Projectiles hit: " + _hitCount;
        _misscount = 0;
        misslable.Text = "Gladius Projectiles miss: " + _misscount;
        _f8hitcount = 0;
        F8HitcountLable.Text = "F8 Projectiles hit: " + _f8hitcount;
        _f8misscount = 0;
        F8misscountlable.Text = "Projectiles miss: " + _f8misscount;
        _totalShots = 0;
        UpdateHitPercent();
    }

    public void OnProjectileHit(string name)
    {
        _totalShots++;

        if (name == "GladiusArea3D")
        {
            _hitCount++;
            _f8hitcount++; // Gladius hit also counts as F8 hit
            counterlable.Text = "Gladius Projectiles hit: " + _hitCount;
            F8HitcountLable.Text = "F8 Projectiles hit: " + _f8hitcount;
            UpdateHitPercent();
            return;
        }

        if (name == "F8Area3D")
        {
            _f8hitcount++; // F8 hit only counts for F8
            F8HitcountLable.Text = "F8 Projectiles hit: " + _f8hitcount;
            UpdateHitPercent();
            return;
        }

        GD.PrintErr("Projectile hit but has no name: " + name);
    }

    public void OnProjectileMiss()
    {
        _totalShots++;
        _misscount++;
        _f8misscount++; // Miss counts for both Gladius and F8
        misslable.Text = "Projectiles miss: " + _misscount;
        F8misscountlable.Text = "Projectiles miss: " + _f8misscount;
        UpdateHitPercent();
    }


    private void UpdateHitPercent()
    {
        if (Hitpercentage == null || f8percentLable == null)
        {
            return;
        }

        // Calculate Gladius hit percentage
        int totalNormalShots = _hitCount + _misscount;
        if (totalNormalShots == 0)
        {
            Hitpercentage.Text = "Hit percentage: 0%";
        }
        else
        {
            float percent = ((float)_hitCount / totalNormalShots) * 100;
            Hitpercentage.Text = $"Gladius Hit percentage: {percent:F1}%";
        }

        // Calculate F8 hit percentage
        int totalF8Shots = _f8hitcount + _f8misscount;
        if (totalF8Shots == 0)
        {
            f8percentLable.Text = "F8 Hit percentage: 0%";
        }
        else
        {
            float f8hitpercent = ((float)_f8hitcount / totalF8Shots) * 100;
            f8percentLable.Text = $"F8 Hit percentage: {f8hitpercent:F1}%";
        }
    }

    // Function to handle the slider value change
    private void OnHSliderValueChanged(double value)
    {
        // Add your logic here for when the slider value changes
        GD.Print($"HSlider value changed to: {value}");
        makePath.UpdateRadius(value);
        label2.Text = value.ToString();
    }
    private void OnHSliderValueChanged2(double value)
    {
        // Add your logic here for when the slider value changes
        GD.Print($"HSlider value changed to: {value}");
        movePath.UpdateMoveSpeed(value);
        label.Text = value.ToString();
    }

    private void OnHSliderValueChanged3(double value)
    {
        weapon.SetProjectileSpeed((float)value);
        targetingSystem.SetProjectileSpeed((float)value);
        label3.Text = value.ToString();

    }

    private void OnHSliderValueChanged4(double value)
    {
        weapon.SetConeOfFire((float)value);
        //targetingSystem.SetProjectileSpeed((float)value);
        labelConeofFire.Text = Math.Round(value, 3).ToString();
    }

    private void OnHSliderValueChanged5(double value)
    {
        weapon.SetFireRate((float)value);
        //targetingSystem.SetProjectileSpeed((float)value);
        labelFireRate.Text = value.ToString();
    }

    private void OnHSliderValueChanged6(double value)
    {
        makePath.MoveObject((float)value);

        var distance = weapon.GlobalPosition - makePath.GlobalPosition;
        DistanceLable.Text = distance.Length().ToString();
    }

}