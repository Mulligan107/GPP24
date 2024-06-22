using JumperGame.src.components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumperGame.src.manager
{
    class RenderManager
    {
        List<RenderingComponent> _activeRenderComponents;
        internal Component CreateComponent()
        {
            RenderingComponent rc = new RenderingComponent(this);
            _activeRenderComponents.Add(rc);
            return rc;
        }
    }
}
