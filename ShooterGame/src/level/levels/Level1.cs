using System;
using System.Collections;
using ShooterGame.src;

namespace ShooterGame.level.levels
{
    public class Level1 : Level
    {
        double cycles = 0;
        int counter = 0;
        Event eventFlag = Event.Idle;
        Random random = new Random();
        public static ArrayList entityList = new ArrayList();

        enum Event
        {
            Idle,
            Fighterrow,
            Scouts,
            Dreadnaught,
            Over
        }
        
        public override void Reset()
        {
            cycles = 0;
            counter = 0;
            eventFlag = Event.Idle;
        }

        public override void RunLevelLogic(double deltatime, FileHandler fileHandler, ArrayList entityList)
        {
            cycles++;
            
            // SPAWNING
            switch (eventFlag)
            {
                case Level1.Event.Fighterrow:
                    if (cycles % 20 == 0 && counter < 10)
                    {
                        counter++;
                        Fighter erni = new Fighter(fileHandler.getFighter());
                        erni.posX = Program.SCREEN_WIDTH - (Program.SCREEN_WIDTH / 10);
                        erni.posY = Program.SCREEN_HEIGHT - (Program.SCREEN_HEIGHT / 18) - ((Program.SCREEN_HEIGHT / 11) * (cycles / 20));
                        erni.timeAlive = random.Next(0, 80);
                        entityList.Add(erni);
                    }
                    break;
                
                case Level1.Event.Scouts:

                    if (cycles % 20 == 0 && counter < 60)
                    {
                        counter++;
                        Console.WriteLine(counter);
                        Scout scott = new Scout(fileHandler.getScout());
                        var x = 1;
                        if (counter < 20)
                        {
                            x = 0;
                        }
                        if (counter < 40 && counter > 20)
                        {
                            x = 2;
                        }
                        if (counter < 60 && counter > 40)
                        {
                            x = 4;
                        }

                        scott.spawn(Program.SCREEN_WIDTH, (Program.SCREEN_HEIGHT / 4) + (Program.SCREEN_HEIGHT / 7) * x);
                        entityList.Add(scott);

                    }
                    break;

                case Level1.Event.Dreadnaught:
                        counter++;
                    
                    if (counter == 1)
                    {
                        Dread andre = new Dread(fileHandler.getDread());
                        andre.hit();
                        entityList.Add(andre);
                        
                    }
                    if (counter < 5)
                    {   
                        Sentry senni = new Sentry(fileHandler.getSentry());
                        switch (counter){
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
                        eventFlag = Level1.Event.Over;
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
                        levi.shootEnemy();
                        levi.timeAlive = 0;
                    }

                }
                if (levi.GetType().Name.Equals("Dread"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive > 200)
                    {
                        levi.bulletFan();
                        levi.timeAlive = 0;
                    }

                    if (cycles == 800)
                    {
                        levi.vecY = -3;
                    }
                }
                if (levi.GetType().Name.Equals("Sentry"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive > 200)
                    {
                        levi.shootTarget();
                        levi.timeAlive = 0;
                    }
                }
            }

            if (cycles == 500 && eventFlag.ToString().Equals("Idle"))
            {
                eventFlag = Level1.Event.Fighterrow;
                cycles = 0;
            }


            if (cycles == 800 && !eventFlag.ToString().Equals("Scouts"))
            {
                counter = 0;
                cycles = 0;
                eventFlag = Level1.Event.Scouts;
            }

            Console.WriteLine(Enemy.TotalEnemies);

            if (cycles > 800 && Enemy.TotalEnemies == 0 && !eventFlag.ToString().Equals("Over"))
            {
                eventFlag = Level1.Event.Dreadnaught;
                cycles = 0;
                counter = 0;
            }
        }
    }
}