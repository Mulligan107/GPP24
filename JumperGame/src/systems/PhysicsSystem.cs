using System;
using System.Collections.Generic;
using System.Numerics;
using JumperGame.components;
using JumperGame.gameEntities;

namespace JumperGame.systems
{
    public class PhysicsSystem
    {
        private const float Gravity = 10f; // Simplified gravity constant
        private const double MaxDeltaTime = 0.1; // Max allowed deltaTime to prevent large initial steps

        public void Update(IEnumerable<Entity> entities, double deltaTime)
        {
            // Clamp deltaTime to a maximum value
            deltaTime = Math.Min(deltaTime, MaxDeltaTime);

            foreach (var entity in entities)
            {
                var physicsComponent = entity.GetComponent<PhysicsComponent>();
                var positionComponent = entity.GetComponent<PositionComponent>();
                var collisionComponent = entity.GetComponent<CollisionComponent>();

                if (physicsComponent != null && positionComponent != null)
                {
                    // Apply gravity
                    physicsComponent.Acceleration += new Vector3(0, Gravity, 0) * physicsComponent.Mass;
                    physicsComponent.Velocity += physicsComponent.Acceleration * (float)deltaTime;
                    // Reset acceleration after applying it
                    physicsComponent.Acceleration = new Vector3(0, 0, 0);

                    // Calculate potential new position
                    var newPosition = positionComponent.Position + physicsComponent.Velocity * (float)deltaTime;

                    // Check for collisions if the entity has a CollisionComponent
                    if (collisionComponent != null)
                    {
                        foreach (var otherEntity in entities)
                        {
                            if (otherEntity == entity) continue;

                            var otherPositionComponent = otherEntity.GetComponent<PositionComponent>();
                            var otherCollisionComponent = otherEntity.GetComponent<CollisionComponent>();

                            if (otherPositionComponent != null && otherCollisionComponent != null)
                            {
                                if (IsColliding(newPosition, collisionComponent.Size, otherPositionComponent.Position, otherCollisionComponent.Size))
                                {
                                    // Collision detected, stop the entity's fall
                                    physicsComponent.Velocity = new Vector3(0, 0, 0);
                                    newPosition.Y = positionComponent.Position.Y; // Reset to original Y position
                                    break;
                                }
                            }
                        }
                    }

                    // Update entity position
                    positionComponent.Position = newPosition;
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

        private bool IsColliding(Vector3 pos1, Vector2 size1, Vector3 pos2, Vector2 size2)
        {
            // Axis-Aligned Bounding Box (AABB) collision detection
            return pos1.X < pos2.X + size2.X &&
                   pos1.X + size1.X > pos2.X &&
                   pos1.Y < pos2.Y + size2.Y &&
                   pos1.Y + size1.Y > pos2.Y;
        }

        public bool Initialize()
        {
            return true; // or false
        }
    }
}
