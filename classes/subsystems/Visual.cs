using MyGame2;

/// <summary>
/// Subsystem for showing visual.
/// </summary>
public class Subsystem_Visual : Subsystem
{
    public override double max_time_part => 0.1;
    public Subsystem_Visual(Game1 game1) : base(game1) {}
}