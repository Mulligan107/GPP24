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
        private const float JumpInterval = 2.0f; // Seconds
        private const float JumpForce = 75f;
        private const float MoveForce = 50f;
        private const float EyeMoveSpeed = 50f;
        private const float DetectionRadius = 300f;
        private bool _jumpLeft = true;

        public EnemyMovementSystem(entitySystem entitySystem)
        {
            _entitySystem = entitySystem;
        }

        public void Update(double deltaTime)
        {
            foreach (var entity in _entitySystem.GetAllEntities())
            {
                SlimeJumpMovement(entity, deltaTime);
                EyeMovement(entity, deltaTime);
            }
        }
        
        private void EyeMovement(Entity entity, double deltaTime)
        {
            if (entity.HasComponent<EyeSteeringComponent>() && entity.HasComponent<PositionComponent>() && entity.IsActive)
            {
                var eyePosition = entity.GetComponent<PositionComponent>().Position;
                var player = _entitySystem.GetEntityByGID(281); 
                var playerPosition = player.GetComponent<PositionComponent>().Position;

                var distance = Vector3.Distance(eyePosition, playerPosition);
                Console.WriteLine("Distance: " + distance);
                
                if (distance <= DetectionRadius)
                {
                    Console.WriteLine("Player detected by eye");
                    var direction = Vector3.Normalize(playerPosition - eyePosition);
                    var newPosition = eyePosition + direction * EyeMoveSpeed * (float)deltaTime;
                    entity.GetComponent<PositionComponent>().Position = newPosition;
                }
            }
        }

        private void SlimeJumpMovement(Entity entity, double deltaTime)
        {
            if (entity.HasComponent<SlimeSteeringComponent>() && entity.HasComponent<PhysicsComponent>() && entity.IsActive)
            {
                var physics = entity.GetComponent<PhysicsComponent>();
                var slimeSteering = entity.GetComponent<SlimeSteeringComponent>();
                var renderer = entity.GetComponent<RenderComponent>();

                if (physics.Grounded)
                {
                    // Only increment the jump timer if the slime is grounded
                    slimeSteering.JumpTimer += (float)deltaTime;

                    // Prevent horizontal movement while waiting to jump
                    physics.Velocity = new Vector3(0, physics.Velocity.Y, 0);
                }

                if (slimeSteering.JumpTimer >= JumpInterval && physics.Grounded)
                {
                    Vector3 newVelocity = physics.Velocity;
                    newVelocity.Y = -JumpForce; // Apply vertical force

                    // Apply horizontal force based on the jump direction
                    if (_jumpLeft)
                    {
                        renderer.flip = SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
                        newVelocity.X = -MoveForce; // Jump left
                    }
                    else
                    {
                        renderer.flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE;
                        newVelocity.X = MoveForce; // Jump right
                    }

                    physics.Velocity = newVelocity;
                    entity.activeSTATE = Entity.STATE.JUMP;

                    // Reset the jump timer and toggle jump direction after the jump
                    _jumpLeft = !_jumpLeft;
                    slimeSteering.JumpTimer = 0;
                    physics.Grounded = false;
                }
                else if (!physics.Grounded)
                {
                    // Reset the jump timer if the slime is in the air to prevent immediate re-jump upon landing
                    slimeSteering.JumpTimer = 0;
                }
            }
        }
    }
}