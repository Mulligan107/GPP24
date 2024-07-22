using System;
using JumperGame.src.manager;
using SDL2;
using System.Numerics;
using JumperGame.components;
using JumperGame.gameEntities;
using TiledCSPlus;
using System.Linq;

namespace JumperGame.systems;

public class MovementSystem
{
        
    private entitySystem _entitySystem;
    bool speedlimitX;

    public MovementSystem(entitySystem entitySystem)
    {
        _entitySystem = entitySystem;
    }

    public void Update(SDL.SDL_Keycode keycode)
    {
        foreach (var entity in _entitySystem.GetAllEntities())
        {
            if (entity.HasComponent<PlayerSteeringComponent>() && entity.HasComponent<PhysicsComponent>())
            {
                var physics = entity.GetComponent<PhysicsComponent>();
                var renderer = entity.GetComponent<RenderComponent>();

                

                if (physics.Grounded)
                {
                    entity.activeSTATE = Entity.STATE.IDLE;
                }

                switch (keycode)
                {
                    case SDL.SDL_Keycode.SDLK_w:
                        if (physics.Grounded)
                        {
                            entity.activeSTATE = Entity.STATE.JUMP;
                        }
                        break;
                    case SDL.SDL_Keycode.SDLK_s:                       
                        break;
                    case SDL.SDL_Keycode.SDLK_a:
                        renderer.flip = SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
                        if (physics.Grounded)
                        {
                            entity.activeSTATE = Entity.STATE.WALKLEFT;
                        }
                        break;
                    case SDL.SDL_Keycode.SDLK_d:
                        renderer.flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE;
                        if (physics.Grounded)
                        {
                            entity.activeSTATE = Entity.STATE.WALKRIGHT;
                        }
                        break;
                    case SDL.SDL_Keycode.SDLK_SPACE:
                        if (physics.Grounded)
                        {

                            entity.activeSTATE = Entity.STATE.JUMP;


                            physics.Grounded = false;
                            
                        }
                        break;
                }
            }
        }
    }

    public void UpdatePlayerState()
    {
        var player = _entitySystem.GetEntityByGID(281);
        var physics = player.GetComponent<PhysicsComponent>();
        Vector3 newVelocity = physics.Velocity;

        switch (player.activeSTATE)
        {
            case Entity.STATE.JUMP:
                newVelocity.Y = -250;
                break;
            case Entity.STATE.WALKLEFT:
                newVelocity.X = -200;
                break;
            case Entity.STATE.WALKRIGHT:
                newVelocity.X = 200;
                break;
            case Entity.STATE.IDLE:
                if (newVelocity.X > 0)
                {
                    newVelocity.X -= 5;
                }
                if (newVelocity.X < 0)
                {
                    newVelocity.X += 10;
                }
                if (Enumerable.Range(-20, 20).Contains((int)newVelocity.X))
                {
                    newVelocity.X = 0;
                }
                break;
            case Entity.STATE.AIRTIME:
                
                break;

        }


        if (!physics.Grounded)
        { 
            player.activeSTATE = Entity.STATE.AIRTIME;
            newVelocity.Y = 0;
        }


        physics.Velocity = newVelocity;

    }
        
    public void OnKeyReleased(SDL.SDL_Keycode keycode)
    {
        foreach (var entity in _entitySystem.GetAllEntities())
        {
            if (entity.HasComponent<PlayerSteeringComponent>() && entity.HasComponent<PhysicsComponent>())
            {
                var physics = entity.GetComponent<PhysicsComponent>();
                Vector3 newVelocity = physics.Velocity;

                

                switch (keycode)
                {
                    case SDL.SDL_Keycode.SDLK_w:
                    case SDL.SDL_Keycode.SDLK_s:
                        newVelocity.Y = 0;
                        break;
                    case SDL.SDL_Keycode.SDLK_SPACE:
                        if (!physics.Grounded)
                        {
                            entity.activeSTATE = Entity.STATE.AIRTIME;
                        }
                        break;
                case SDL.SDL_Keycode.SDLK_a:
                case SDL.SDL_Keycode.SDLK_d:
                        if (physics.Grounded)
                        {
                            entity.activeSTATE = Entity.STATE.IDLE;
                        }
                        break;
                }
                // Apply new velocity
                physics.Velocity = newVelocity;
            }
        }
    }

}