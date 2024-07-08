using System.Numerics;

namespace JumperGame.components;

public class PositionComponent
{
    public Vector2 Position { get; set; }

    public PositionComponent(Vector2 initialPosition)
    {
        Position = initialPosition;
    }

    public PositionComponent() : this(new Vector2(0, 0))
    {
    }
}