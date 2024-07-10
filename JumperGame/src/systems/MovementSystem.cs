using System;
using JumperGame.src.manager;
using SDL2;
using System.Numerics;
using JumperGame.components;

public class MovementSystem
{
    private entitySystem _entitySystem;

    public MovementSystem(entitySystem entitySystem)
    {
        _entitySystem = entitySystem;
    }

    public void Update(SDL.SDL_Keycode keycode)
    {
        //Console.WriteLine("Pressed");
        foreach (var entity in _entitySystem.GetAllEntities())
        {
            if (entity.HasComponent<PlayerSteeringComponent>() && entity.HasComponent<PhysicsComponent>())
            {
                var playerSteering = entity.GetComponent<PlayerSteeringComponent>();
                var physics = entity.GetComponent<PhysicsComponent>();
                Vector3 newVelocity = physics.Velocity;

                switch (keycode)
                {
                    case SDL.SDL_Keycode.SDLK_w:
                        newVelocity.Y = -50;
                        break;
                    case SDL.SDL_Keycode.SDLK_s:
                        newVelocity.Y = 50;
                        break;
                    case SDL.SDL_Keycode.SDLK_a:
                        newVelocity.X = -50;
                        break;
                    case SDL.SDL_Keycode.SDLK_d:
                        newVelocity.X = 50;
                        break;
                    default:
                        newVelocity.X = 0;
                        newVelocity.Y = 0;
                        break;
                }

                physics.Velocity = newVelocity;
            }
        }
    }
    
    public void OnKeyReleased(SDL.SDL_Keycode keycode)
    {
        //Console.WriteLine("Released");
        foreach (var entity in _entitySystem.GetAllEntities())
        {
            if (entity.HasComponent<PlayerSteeringComponent>() && entity.HasComponent<PhysicsComponent>())
            {
                var playerSteering = entity.GetComponent<PlayerSteeringComponent>();
                var physics = entity.GetComponent<PhysicsComponent>();
                Vector3 newVelocity = physics.Velocity;

                switch (keycode)
                {
                    case SDL.SDL_Keycode.SDLK_w:
                    case SDL.SDL_Keycode.SDLK_s:
                        newVelocity.Y = 0;
                        break;
                    case SDL.SDL_Keycode.SDLK_a:
                    case SDL.SDL_Keycode.SDLK_d:
                        newVelocity.X = 0;
                        break;
                }

                // Apply new velocity
                physics.Velocity = newVelocity;
            }
        }
    }

}