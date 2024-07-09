using System;
using System.Collections.Generic;

namespace JumperGame.gameEntities;

public class Entity
{
    private readonly Dictionary<Type, object> _components = new Dictionary<Type, object>();
    public int gid;

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
}