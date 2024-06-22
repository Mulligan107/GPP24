using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace ShooterGame.level
{
    public abstract class Level
    {
        public Level()
        {
        }
        public abstract void Reset();
        public abstract void RunLevelLogic(double deltatime, FileHandler fileHandler, ArrayList entityList);
    }
}