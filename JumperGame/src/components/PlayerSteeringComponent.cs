namespace JumperGame.components;

public class PlayerSteeringComponent
{
    public PhysicsComponent Physics { get; set; }

    public PlayerSteeringComponent(PhysicsComponent physics)
    {
        Physics = physics;
    }
}
