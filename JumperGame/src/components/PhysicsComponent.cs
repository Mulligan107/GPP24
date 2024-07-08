using System.Numerics;

namespace JumperGame.components;

public class PhysicsComponent
{
    public Vector2 Velocity { get; set; }
    public Vector2 Acceleration { get; set; }
    public float Mass { get; set; }

    public PhysicsComponent(float mass)
    {
        Mass = mass;
        Velocity = new Vector2(0, 0);
        Acceleration = new Vector2(0, 0);
    }
}