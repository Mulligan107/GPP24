using System.Numerics;

namespace JumperGame.components
{
    public class CollisionComponent
    {
        public Vector2 Size { get; set; } // Width and Height of the entity for collision detection

        public CollisionComponent(Vector2 size)
        {
            Size = size;
        }
    }
}