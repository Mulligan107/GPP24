using ShooterGame.src;
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
            FIGHTER,
            SCOUTS,
            DREADNAUGHT,
            SENTRYS,
            LASERSHIPS,
            OVER
        }

        public override void RunLevelLogic(double deltatime, FileHandler fileHandler, ArrayList entityList)
        {
            _cycles++;

            // SPAWNING
            switch (_eventFlag)
            {
                case Event.FIGHTER:
                    if (_cycles % 20 == 0 && _counter < 10)
                    {
                        _counter++;
                        Fighter erni = new Fighter(fileHandler.getFighter());
                        erni.posX = Program.SCREEN_WIDTH - (Program.SCREEN_WIDTH / 10);
                        erni.posY = Program.SCREEN_HEIGHT - (Program.SCREEN_HEIGHT / 18) - ((Program.SCREEN_HEIGHT / 11) * (_cycles / 20));
                        erni.timeAlive = _random.Next(0, 80);
                        entityList.Add(erni);
                    }
                    break;

                case Event.SCOUTS:

                    if (_cycles % 5 == 0 && _counter < 60)
                    {
                        _counter++;
                        Console.WriteLine(_counter);
                        Scout scott = new Scout(fileHandler.getScout());
                        var x = 1;
                        if (_counter < 20)
                        {
                            x = 0;
                        }
                        if (_counter < 40 && _counter > 20)
                        {
                            x = 2;
                        }
                        if (_counter < 60 && _counter > 40)
                        {
                            x = 4;
                        }

                        scott.spawn(Program.SCREEN_WIDTH, (Program.SCREEN_HEIGHT / 4) + (Program.SCREEN_HEIGHT / 7) * x);
                        entityList.Add(scott);

                        if (Enemy.TotalEnemies < 4)
                        {
                            _eventFlag = Event.IDLE;
                        }
                    }
                    break;

                case Event.DREADNAUGHT:
                    break;
                case Event.SENTRYS:
                    if (_cycles % 20 == 0 && _counter < 8)
                    {
                        _counter++;
                        if (_counter == 5)
                        {
                            _cycles = 20;
                        }
                        Sentry senni = new Sentry(fileHandler.getSentry());
                        if (_counter < 5)
                        {
                            senni.spawn(senni.width + 100 * senni.s + (Program.SCREEN_WIDTH / 8) * (_cycles / 20), senni.height + 100 * senni.s);
                        }
                        else
                        {
                            senni.spawn(senni.width + 100 * senni.s + (Program.SCREEN_WIDTH / 8) * (_cycles / 20), Program.SCREEN_HEIGHT - senni.height - 100 * senni.s);
                        }
                        entityList.Add(senni);
                    }
                    if (_counter >= 8)
                    {
                        _eventFlag = Event.LASERSHIPS;
                        _counter = 0;
                    }

                    break;
                case Event.LASERSHIPS:
                    _counter++;
                    if ( _counter == 1)
                    {
                        Lasership lasar = new Lasership(fileHandler.getLasership());
                        lasar.spawn(Program.SCREEN_WIDTH - lasar.width, 0 + lasar.height);
                        lasar.timeAlive = 0;
                        Lasership luna = new Lasership(fileHandler.getLasership());
                        luna.spawn(Program.SCREEN_WIDTH - lasar.width, Program.SCREEN_HEIGHT - lasar.height);
                        luna.timeAlive = 0;
                        entityList.Add(luna);
                        entityList.Add(lasar);
                        _eventFlag = Event.OVER;
                    }
                    
                    break;
            }


            // SHOOTING
            ArrayList entitiesToProcess = new ArrayList(entityList);
            foreach (LivingEntity levi in entitiesToProcess)
            {
                if (levi.GetType().Name.Equals("Fighter"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive > 100 && !levi.iframe)
                    {
                        levi.shootEnemy(1, deltatime);
                        levi.timeAlive = 0;
                    }

                }
                if (levi.GetType().Name.Equals("Dread"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive > 200)
                    {
                        levi.bulletFan(deltatime);
                        levi.timeAlive = 0;
                    }

                    if (_cycles == 200)
                    {
                        levi.vecY = -3;
                    }
                }
                if (levi.GetType().Name.Equals("Sentry"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive % 30 == 0)
                    {
                        levi.shootTarget(deltatime);
                        if (levi.timeAlive > 200)
                        {
                            levi.timeAlive = 0;
                        }

                    }
                }
                if (levi.GetType().Name.Equals("Lasership"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive > 50)
                    {
                        levi.deathray(deltatime);
                        levi.timeAlive = 0;
                    }
                }
            }

            if (_cycles == 125 && _eventFlag.ToString().Equals("IDLE"))
            {
                _eventFlag = Event.SENTRYS;
                _cycles = 0;
                _counter = 0;
            }



            if (Enemy.TotalEnemies == 0 && _eventFlag.ToString().Equals("OVER"))
            {
                Program.CurrentState = GameState.WIN;
                Program.VisibleMenu = new WinMenu(Program.gRenderer);
            }

            Console.WriteLine(_cycles);
            Console.WriteLine(_eventFlag.ToString());

        }
    }
}