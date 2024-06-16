using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame.src
{
    class EventHandler
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
            Over
        }



        public EventHandler() {
            cycles = 0;
        }


        public void timedEvent(double deltatime, FileHandler fileHandler)
        {
            cycles++;
            
            // SPAWNING
            switch (eventFlag)
            {
                case Event.Fighterrow:
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
                case Event.Scouts:

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
            }

            if (cycles == 200 && eventFlag.ToString().Equals("Idle"))
            {
                eventFlag = Event.Fighterrow;
                cycles = 0;
            }


            if (cycles == 200 && !eventFlag.ToString().Equals("Scouts"))
            {
                counter = 0;
                cycles = 0;
                eventFlag = Event.Scouts;
            }

        }

   
        public void updateList(ArrayList enti)
        {
            entityList = enti;
        }

    }
}
