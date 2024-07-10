using System;
using System.Collections.Generic;
using System.Numerics;
using JumperGame.components;
using JumperGame.gameEntities;

namespace JumperGame.systems;

public class PhysicsSystem
{
    
    private const float Gravity = 0f; // Simplified gravity constant

    public void Update(IEnumerable<Entity> entities, double deltaTime)
    {
        foreach (var entity in entities)
        {
            var physicsComponent = entity.GetComponent<PhysicsComponent>();
            if (physicsComponent != null)
            {
                // Apply gravity
                physicsComponent.Acceleration += new Vector3(0, Gravity, 0) * physicsComponent.Mass;
                physicsComponent.Velocity += physicsComponent.Acceleration * (float)deltaTime;
                // Reset acceleration after applying it
                physicsComponent.Acceleration = new Vector3(0, 0, 0);

                // Update entity position
                var positionComponent = entity.GetComponent<PositionComponent>();
                if (positionComponent != null)
                {
                    positionComponent.Position += physicsComponent.Velocity * (float)deltaTime;

                    // Update the dstRect in RenderComponent
                    var renderComponent = entity.GetComponent<RenderComponent>();
                    if (renderComponent != null)
                    {
                        renderComponent.UpdateDstRect(positionComponent.Position);
                    }
                }

                
                if (entity.Type == "knight")
                {
                   // Console.WriteLine($"Entity ID: {entity.gid}");
                   // Console.WriteLine($"DeltaTime: {deltaTime}");
                   // Console.WriteLine($"Velocity: {physicsComponent.Velocity}");
                    Console.WriteLine($"Position: {positionComponent?.Position}");
                }
            }
        }
    }

    
    public bool Initialize()
    {
        return true; // or false
    }
}