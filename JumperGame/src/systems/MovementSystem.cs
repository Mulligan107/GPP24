using System;
using JumperGame.src.manager;
using SDL2;
using System.Numerics;
using JumperGame.components;
using JumperGame.gameEntities;
using TiledCSPlus;
using System.Linq;
using System.Runtime.InteropServices;

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

                keySetup(keycode, entity);

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
                break;
            case Entity.STATE.WALKLEFT:
                break;
            case Entity.STATE.WALKRIGHT:
                break;
            case Entity.STATE.IDLE:
                break;
            case Entity.STATE.AIRTIME:
                
                break;

        }


        if (!physics.Grounded)
        { 
            player.activeSTATE = Entity.STATE.AIRTIME;

        }


    }
        
    public void OnKeyReleased(SDL.SDL_Keycode keycode)
    {
        foreach (var entity in _entitySystem.GetAllEntities())
        {
            if (entity.HasComponent<PlayerSteeringComponent>() && entity.HasComponent<PhysicsComponent>())
            {
                var physics = entity.GetComponent<PhysicsComponent>();
                var renderer = entity.GetComponent<RenderComponent>();
                Vector3 newVelocity = physics.Velocity;

                

                switch (keycode)
                {
                    case SDL.SDL_Keycode.SDLK_w:
                    case SDL.SDL_Keycode.SDLK_s:
                        newVelocity.Y = 0;
                        break;
                    case SDL.SDL_Keycode.SDLK_SPACE:
                        entity.activeSTATE = Entity.STATE.AIRTIME;
                        /*
                        if (!physics.Grounded)
                        {
                            entity.activeSTATE = Entity.STATE.AIRTIME;
                        }
                        */
                        break;
                case SDL.SDL_Keycode.SDLK_a:
                            newVelocity.X = 0;

                        if (physics.Grounded)
                        {
                            entity.activeSTATE = Entity.STATE.IDLE;
                        }
                        break;

                    case SDL.SDL_Keycode.SDLK_d:
                            newVelocity.X = 0;            
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

    public void keySetup(SDL.SDL_Keycode keycode, Entity entity)
    {
        var physics = entity.GetComponent<PhysicsComponent>();
        var renderer = entity.GetComponent<RenderComponent>();
        Vector3 newVelocity = physics.Velocity;

        switch (keycode)
        {
            case SDL.SDL_Keycode.SDLK_w:
                entity.activeSTATE = Entity.STATE.JUMP;
                newVelocity.Y = -250;
                break;

            case SDL.SDL_Keycode.SDLK_s:
                break;

            case SDL.SDL_Keycode.SDLK_a:
                if (entity.activeSTATE != Entity.STATE.AIRTIME)
                {
                    renderer.flip = SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
                }

                newVelocity.X = -250;
                if (physics.Grounded)
                {
                    entity.activeSTATE = Entity.STATE.WALKLEFT;
                }
                break;

            case SDL.SDL_Keycode.SDLK_d:
                if (entity.activeSTATE != Entity.STATE.AIRTIME)
                {
                    renderer.flip = SDL.SDL_RendererFlip.SDL_FLIP_NONE;
                }

                newVelocity.X = 250;
                if (physics.Grounded)
                {
                    entity.activeSTATE = Entity.STATE.WALKRIGHT;
                }
                break;
            case SDL.SDL_Keycode.SDLK_SPACE:
                if (physics.Grounded)
                {

                    entity.activeSTATE = Entity.STATE.JUMP;
                    newVelocity.Y = -250;

                    physics.Grounded = false;

                }
                break;


        }
        physics.Velocity = newVelocity;
    }

 


}