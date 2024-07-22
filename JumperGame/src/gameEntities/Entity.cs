using System;
using System.Collections.Generic;

namespace JumperGame.gameEntities;

public class Entity
{
    private readonly Dictionary<Type, object> _components = new Dictionary<Type, object>();
    public int gid { get; set; }
    public EntityType Type { get; set; }

    public enum STATE
    {
        SPAWN,
        IDLE,
        WALKLEFT,
        WALKRIGHT,
        JUMP,
        AIRTIME,
        LANDING,
        ATTACK,
        HIT,
        DEATH
    }

    public STATE activeSTATE;
    
    public enum EntityType
    {
        Player,
        Tile,
        Coin,
        Enemy
    }

    public Entity(int nameId) { 
        gid = nameId;
    }

    public void AddComponent<T>(T component)
    {
        _components[typeof(T)] = component;
    }

    // This method is used to retrieve a component
    public T GetComponent<T>()
    {
        if (_components.TryGetValue(typeof(T), out var component))
        {
            return (T)component;
        }

        return default;
    }
    
    public bool HasComponent<T>()
    {
        return _components.ContainsKey(typeof(T));
    }
}