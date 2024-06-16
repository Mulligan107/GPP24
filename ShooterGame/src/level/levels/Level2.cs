using System;
using System.Collections;

namespace ShooterGame.level.levels
{
    public class Level2 : Level
    {
        double _cycles = 0;
        int _counter = 0;
        Event _eventFlag = Event.IDLE;
        Random _random = new Random();
        public static ArrayList EntityList = new ArrayList();
        
        public override void Reset()
        {
            _cycles = 0;
            _counter = 0;
            _eventFlag = Event.IDLE;
        }
        
        enum Event
        {
            IDLE,
            FIGHTERROW,
            SCOUTS,
            DREADNAUGHT,
            OVER
        }

        public override void RunLevelLogic(double deltatime, FileHandler fileHandler, ArrayList entityList)
        {
            throw new System.NotImplementedException();
        }
    }
}