using System.Collections.Generic;
using System.Net;

namespace ShooterGame.level
{
    public abstract class Level
    {
        public Level()
        {
        }
        
        public abstract void Load();

        // Update the level
        public abstract void Update(double elapsed);
    }
}