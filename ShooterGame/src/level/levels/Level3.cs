using System;
using System.Collections;
using ShooterGame.src;

namespace ShooterGame.level.levels
{
    public class Level3 : Level
    {
        double _cycles = 0;
        int _counter = 0;
        Event _eventFlag = Event.Idle;
        Random _random = new Random();
        public static ArrayList EntityList = new ArrayList();

        enum Event
        {
            Idle,
            Fighterrow,
            Scouts,
            Dreadnaught,
            Scouts_D,
            Over
        }
        
        public override void Reset()
        {
            _cycles = 0;
            _counter = 0;
            _eventFlag = Event.Idle;
        }

        public override void RunLevelLogic(double deltatime, FileHandler fileHandler, ArrayList entityList)
        {
            _cycles++;
            
            // SPAWNING
            switch (_eventFlag)
            {
                case Level3.Event.Fighterrow:
                    if (_cycles % 5 == 0 && _counter < 10)
                    {
                        _counter++;
                        Fighter erni = new Fighter(fileHandler.getFighter());
                        erni.posX = Program.SCREEN_WIDTH - (Program.SCREEN_WIDTH / 10);
                        erni.posY = Program.SCREEN_HEIGHT - (Program.SCREEN_HEIGHT / 18) - ((Program.SCREEN_HEIGHT / 11) * (_cycles / 5));
                        erni.timeAlive = _random.Next(0, 80);
                        entityList.Add(erni);
                    }
                    break;
                
                case Level3.Event.Scouts:

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

                    }
                    break;

                case Level3.Event.Scouts_D:

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

                    }
                    if (_counter >= 60)
                    {
                        _eventFlag = Event.Over;
                    }
                    break;

                case Level3.Event.Dreadnaught:
                        _counter++;
                    
                    if (_counter == 1)
                    {
                        Dread andre = new Dread(fileHandler.getDread());
                        andre.hit();
                        entityList.Add(andre);
                        
                    }
                    if (_counter < 5)
                    {   
                        Sentry senni = new Sentry(fileHandler.getSentry());
                        switch (_counter){
                            case 1:
                                senni.spawn(Program.SCREEN_WIDTH - senni.width - 100 * senni.s, Program.SCREEN_HEIGHT - senni.height - 100 * senni.s);
                                break;
                            case 2:
                                senni.spawn(Program.SCREEN_WIDTH - senni.width - 100 * senni.s, senni.height + 100 * senni.s);
                                break;
                            case 3:
                                senni.spawn(senni.width + 100 * senni.s, senni.height + 100 * senni.s);
                                break;
                            case 4:
                                senni.spawn(senni.width + 100 * senni.s, Program.SCREEN_HEIGHT - senni.height - 100 * senni.s);
                                break;
                        }
                        entityList.Add(senni);

                    }
                    else
                    {
                        _eventFlag = Level3.Event.Scouts_D;
                        _counter = 0;
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

                    if (_cycles == 800)
                    {
                        levi.vecY = -3;
                    }
                }
                if (levi.GetType().Name.Equals("Sentry"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive > 200)
                    {
                        levi.shootTarget(deltatime);
                        levi.timeAlive = 0;
                    }
                }
            }

            // Event transitions
            if (_eventFlag == Event.Idle && _cycles >= 125)
            {
                _eventFlag = Event.Fighterrow;
                _cycles = 0;
                _counter = 0;
            }
            else if (_eventFlag == Event.Fighterrow && _cycles >= 200)
            {
                _eventFlag = Event.Scouts;
                _cycles = 0;
                _counter = 0;
            }
            else if (_eventFlag == Event.Scouts && _cycles >= 200 && Enemy.TotalEnemies == 0)
            {
                _eventFlag = Event.Dreadnaught;
                _cycles = 0;
                _counter = 0;
            }
            else if (_eventFlag == Event.Dreadnaught && Enemy.TotalEnemies == 0)
            {
                _eventFlag = Event.Over;
                _cycles = 0;
                _counter = 0;
            }

            // Check for win condition
            if (_eventFlag == Event.Over && Enemy.TotalEnemies == 0)
            {
                Program.CurrentState = GameState.WIN;
                Program.VisibleMenu = new WinMenu(Program.gRenderer);
            }
        }
    }
}