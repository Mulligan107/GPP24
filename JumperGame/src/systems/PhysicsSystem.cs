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
                if (entity.Type != Entity.EntityType.Player && entity.Type != Entity.EntityType.Tile) continue; //C# ist geil
                
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
                    if (collisionComponent != null && entity.Type == Entity.EntityType.Player)                    {
                        foreach (var otherEntity in entities)
                        {
                            if (otherEntity == entity) continue;

                            var otherPositionComponent = otherEntity.GetComponent<PositionComponent>();
                            var otherCollisionComponent = otherEntity.GetComponent<CollisionComponent>();

                            if (otherPositionComponent != null && otherCollisionComponent != null)
                            {
                                //just checks if it collides
                                if (IsColliding(newPosition, collisionComponent.Size, otherPositionComponent.Position, otherCollisionComponent.Size))
                                {
                                    //Actual calculation on what to do in case of collision is done in Resolve Collision
                                    ResolveCollision(physicsComponent, positionComponent, collisionComponent, otherPositionComponent, otherCollisionComponent, ref newPosition, otherEntity);
                                    break;
                                }
                            }
                        }
                    }

                    

                    // Update entity position
                    positionComponent.Position = newPosition;

                    // Update the dstRect in RenderComponent
                    var renderComponent = entity.GetComponent<RenderComponent>();
                    if (renderComponent != null)
                    {
                        renderComponent.UpdateDstRect(positionComponent.Position);
                    }
                    
                    
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

        /**
         * Ok so this can be confusing af to read so to explain it in one text:
         * The ResolveCollision method calculates the overlap in both horizontal (X-axis) and vertical (Y-axis) directions.
         * It then determines the direction with the least overlap and resolves the collision by adjusting the position accordingly.
         *  - If the horizontal overlap is smaller, it resolves the collision horizontally and stops horizontal movement.
         *  - If the vertical overlap is smaller, it resolves the collision vertically and stops vertical movement.
         */
        private void ResolveCollision(PhysicsComponent physicsComponent, PositionComponent positionComponent, CollisionComponent collisionComponent,
            PositionComponent otherPositionComponent, CollisionComponent otherCollisionComponent, ref Vector3 newPosition, Entity enti)
        {
            // Calculate overlap in both axes
            float overlapX = Math.Min(newPosition.X + collisionComponent.Size.X, otherPositionComponent.Position.X + otherCollisionComponent.Size.X) - 
                             Math.Max(newPosition.X, otherPositionComponent.Position.X);
            float overlapY = Math.Min(newPosition.Y + collisionComponent.Size.Y, otherPositionComponent.Position.Y + otherCollisionComponent.Size.Y) - 
                             Math.Max(newPosition.Y, otherPositionComponent.Position.Y);

            if (overlapX < overlapY)
            {
                // Resolve horizontal collision
                if (newPosition.X > otherPositionComponent.Position.X)
                {
                    newPosition.X = otherPositionComponent.Position.X + otherCollisionComponent.Size.X;
                }
                else
                {
                    newPosition.X = otherPositionComponent.Position.X - collisionComponent.Size.X;
                }
                physicsComponent.Velocity = new Vector3(0, physicsComponent.Velocity.Y, 0); // Stop horizontal movement
            }
            else
            {
                // Resolve vertical collision
                if (newPosition.Y > otherPositionComponent.Position.Y)
                {
                    //If hit from above
                    newPosition.Y = otherPositionComponent.Position.Y + otherCollisionComponent.Size.Y;
                }
                else
                {
                    //if hit from below
                    newPosition.Y = otherPositionComponent.Position.Y - collisionComponent.Size.Y;
                    physicsComponent.Grounded = true;  //if the player hits from below he can jump again
                    enti.activeSTATE = Entity.STATE.LANDING;
                    
                }
                physicsComponent.Velocity = new Vector3(physicsComponent.Velocity.X, 0, 0); // Stop vertical movement
            }
        }

        public bool Initialize()
        {
            return true; // or false
        }
    }
}
