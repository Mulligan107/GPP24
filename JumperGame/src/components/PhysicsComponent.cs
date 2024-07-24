using System.Numerics;

namespace JumperGame.components;

public class PhysicsComponent
{
    public Vector3 Velocity { get; set; }
    public Vector3 Acceleration { get; set; }
    public float Mass { get; set; }
    public bool Grounded { get; set; } 

    public PhysicsComponent(float mass)
    {
        Mass = mass;
        Velocity = new Vector3(0, 0, 0);
        Grounded = true; 
    }
}
