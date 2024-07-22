using System;
using System.Collections.Generic;

namespace JumperGame.gameEntities;

public class Entity
{
    private readonly Dictionary<Type, object> _components = new Dictionary<Type, object>();
    public int gid { get; set; }
    public string Type { get; set; }

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

    // This method is used to add a component to the entity

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