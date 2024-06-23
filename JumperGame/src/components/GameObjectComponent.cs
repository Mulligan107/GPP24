using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumperGame.src.components
{
    public class GameObjectComponent
    {
        PhysicsComponent _physics;
        RenderingComponent _rendering;
        AudioComponent _audio;
        InputComponent _input;
        CollisionsComponent _collision;
        ResourceComponent _resource;

        internal void AddComponent(Component component)
        {
            
        }
    }
}
