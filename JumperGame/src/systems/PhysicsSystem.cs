﻿using System;
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
        private bool jumpedOntopOfEnemy;

        public void Update(IEnumerable<Entity> entities, double deltaTime)
        {
            deltaTime = Math.Min(deltaTime, MaxDeltaTime);

            foreach (var entity in entities)
            {
                if (!entity.IsActive) continue; // Skip inactive entities early

                var physicsComponent = entity.GetComponent<PhysicsComponent>();
                var positionComponent = entity.GetComponent<PositionComponent>();
                var collisionComponent = entity.GetComponent<CollisionComponent>();
                

                if (physicsComponent != null && positionComponent != null)
                {
                    if (physicsComponent.Grounded && entity.Type != Entity.EntityType.Player) continue;
                    
                    physicsComponent.Acceleration += new Vector3(0, Gravity, 0) * physicsComponent.Mass;
                    physicsComponent.Velocity += physicsComponent.Acceleration * (float)deltaTime;
                    physicsComponent.Acceleration = new Vector3(0, 0, 0);

                    var newPosition = positionComponent.Position + physicsComponent.Velocity * (float)deltaTime;

                    if (collisionComponent != null && (entity.Type == Entity.EntityType.Player || entity.Type == Entity.EntityType.Enemy))
                    {
                        foreach (var otherEntity in entities)
                        {
                            if (otherEntity == entity || !otherEntity.IsActive) continue; // Skip inactive entities

                            var otherPositionComponent = otherEntity.GetComponent<PositionComponent>();
                            var otherCollisionComponent = otherEntity.GetComponent<CollisionComponent>();

                            if (otherPositionComponent != null && otherCollisionComponent != null)
                            {
                                if (IsColliding(newPosition, collisionComponent.Size, otherPositionComponent.Position, otherCollisionComponent.Size))
                                {
                                    // Check if the other entity is a coin and increment the coin count
                                    if (otherEntity.Type == Entity.EntityType.Coin)
                                    {
                                        if (entity.Type == Entity.EntityType.Player)
                                        {
                                            CoinCounterSystem.Instance.IncrementCoinCount(1);
                                            otherEntity.IsActive = false;
                                        }

                                        continue; // Skip to the next entity without resolving collision
                                    }

                                    ResolveCollision(entity, physicsComponent, positionComponent, collisionComponent, otherPositionComponent, otherCollisionComponent, ref newPosition, otherEntity);
                                    break;
                                }
                            }
                        }
                    }

                    positionComponent.Position = newPosition;

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
        private void ResolveCollision(Entity entity, PhysicsComponent physicsComponent, PositionComponent positionComponent, CollisionComponent collisionComponent,
            PositionComponent otherPositionComponent, CollisionComponent otherCollisionComponent, ref Vector3 newPosition, Entity otherEnti)
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
                    // If hit from above
                    newPosition.Y = otherPositionComponent.Position.Y + otherCollisionComponent.Size.Y;
                }
                else
                {
                    if ((entity.Type == Entity.EntityType.Enemy) && (otherEnti.Type == Entity.EntityType.Player || otherEnti.Type == Entity.EntityType.Enemy))
                    {
                        physicsComponent.Grounded = false; 
                    }
                    else
                    {
                        // If hit from below
                        newPosition.Y = otherPositionComponent.Position.Y - collisionComponent.Size.Y;
                        physicsComponent.Grounded = true;  // If the player hits from below, they can jump again

                        entity.activeSTATE = Entity.STATE.LANDING;

                        if (entity.HasComponent<PlayerSteeringComponent>() && otherEnti.Type == Entity.EntityType.Enemy)
                        {
                            CoinCounterSystem.Instance.IncrementCoinCount(5);
                            otherEnti.IsActive = false;
                            jumpedOntopOfEnemy = true;
                        } 
                    }
                    
                }
                physicsComponent.Velocity = new Vector3(physicsComponent.Velocity.X, 0, 0); // Stop vertical movement
            }

            //Needs to be here because if the player jumps on top of an enemy,
            //inside the if statement above, the player will not be thrown up
            if (jumpedOntopOfEnemy)
            {
                physicsComponent.Velocity = new Vector3(physicsComponent.Velocity.X, -300, 0);
                jumpedOntopOfEnemy = false;
            }
        }

        public bool Initialize()
        {
            return true; // or false
        }
    }
}
