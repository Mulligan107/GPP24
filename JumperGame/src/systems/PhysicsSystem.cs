using System.Collections.Generic;
using System.Numerics;
using JumperGame.components;
using JumperGame.gameEntities;

namespace JumperGame.systems;

public class PhysicsSystem
{
    
    private const float Gravity = 9.8f; // Simplified gravity constant

    public void Update(IEnumerable<Entity> entities/*, float deltaTime*/)
    {
        foreach (var entity in entities)
        {
            var physicsComponent = entity.GetComponent<PhysicsComponent>();
            if (physicsComponent != null)
            {
                // Apply gravity
                physicsComponent.Acceleration += new Vector3(0, Gravity, 0) * physicsComponent.Mass;
                physicsComponent.Velocity += physicsComponent.Acceleration /* * deltaTime) */;
                // Reset acceleration after applying it
                physicsComponent.Acceleration = new Vector3(0, 0, 0);

                // Update entity position
                var positionComponent = entity.GetComponent<PositionComponent>();
                if (positionComponent != null)
                {
                    positionComponent.Position += physicsComponent.Velocity /* * deltaTime */;
                }
            }
        }
    }
    
    public bool Initialize()
    {
        return true; // or false
    }
}