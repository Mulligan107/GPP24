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
        private const float JumpForce = 250f;
        private bool _jumpLeft = true;

        public EnemyMovementSystem(entitySystem entitySystem)
        {
            _entitySystem = entitySystem;
        }

        public void Update(double deltaTime)
        {
            _jumpTimer += (float) deltaTime;

            if (_jumpTimer >= JumpInterval)
            {
                foreach (var entity in _entitySystem.GetAllEntities())
                {
                    if (entity.HasComponent<SlimeSteeringComponent>() && entity.HasComponent<PhysicsComponent>())
                    {
                        var physics = entity.GetComponent<PhysicsComponent>();

                        Vector3 newVelocity = physics.Velocity;

                        if (_jumpLeft)
                        {
                            newVelocity.X = -JumpForce; // Jump left
                            //Console.WriteLine("Jump left");
                        }
                        else
                        {
                            newVelocity.X = JumpForce; // Jump right
                            //Console.WriteLine("Jump right");
                        }

                        physics.Velocity = newVelocity;
                        entity.activeSTATE = Entity.STATE.JUMP;
                    }
                }

                _jumpLeft = !_jumpLeft; // Toggle direction for the next jump
                _jumpTimer = 0; // Reset timer
            }
        }
    }
}