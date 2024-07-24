using System;
using System.Numerics;
using JumperGame.components;
using JumperGame.gameEntities;
using JumperGame.src.manager;
using SDL2;

namespace JumperGame.systems
{
    public class EnemyMovementSystem
    {
        private entitySystem _entitySystem;
        private float _jumpTimer = 0;
        private const float JumpInterval = 2.0f; // Seconds
        private const float JumpForce = 75f;
        private const float MoveForce = 50f;
        private bool _jumpLeft = true;

        public EnemyMovementSystem(entitySystem entitySystem)
        {
            _entitySystem = entitySystem;
        }

        public void Update(double deltaTime)
        {
            foreach (var entity in _entitySystem.GetAllEntities())
            {
                if (entity.HasComponent<SlimeSteeringComponent>() && entity.HasComponent<PhysicsComponent>() && entity.IsActive)
                {
                    var physics = entity.GetComponent<PhysicsComponent>();

                    if (physics.Grounded)
                    {
                        //Console.WriteLine("Slime is grounded");
                        // Only increment the jump timer if the slime is grounded
                        _jumpTimer += (float)deltaTime;

                        // Prevent horizontal movement while waiting to jump
                        physics.Velocity = new Vector3(0, physics.Velocity.Y, 0);
                    }

                    if (_jumpTimer >= JumpInterval && physics.Grounded)
                    {
                        Vector3 newVelocity = physics.Velocity;
                        newVelocity.Y = -JumpForce; // Apply vertical force

                        // Apply horizontal force based on the jump direction
                        if (_jumpLeft)
                        {
                            newVelocity.X = -MoveForce; // Jump left
                        }
                        else
                        {
                            newVelocity.X = MoveForce; // Jump right
                        }

                        physics.Velocity = newVelocity;
                        entity.activeSTATE = Entity.STATE.JUMP;

                        // Reset the jump timer and toggle jump direction after the jump
                        _jumpLeft = !_jumpLeft;
                        _jumpTimer = 0;
                        physics.Grounded = false;
                    }
                    else if (!physics.Grounded)
                    {
                        // Reset the jump timer if the slime is in the air to prevent immediate re-jump upon landing
                        //Console.WriteLine("Slime is in the air");
                        _jumpTimer = 0;
                    }
                }
            }
        }
    }
}