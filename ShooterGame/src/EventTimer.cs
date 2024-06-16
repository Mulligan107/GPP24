using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame.src
{
    class EventTimer
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
            Scouts
        }



        public EventTimer() {
            cycles = 0;
        }


        public void timedEvent(double deltatime, FileHandler fileHandler)
        {
            cycles++;

            // SPAWNING
            if (cycles % 20 == 0 && eventFlag.ToString().Equals("Fighterrow") && counter < 10)
            {
                counter++;
                Fighter erni = new Fighter(fileHandler.getFighter());
                erni.posX = Program.SCREEN_WIDTH - (Program.SCREEN_WIDTH / 10);
                erni.posY = Program.SCREEN_HEIGHT - (Program.SCREEN_HEIGHT / 18) - ((Program.SCREEN_HEIGHT / 11) * (cycles / 20));
                erni.timeAlive = random.Next(0,80);
                entityList.Add(erni);
            }

            if (eventFlag.ToString().Equals("Scouts"))
            {
                Scout scott = new Scout(fileHandler.getScout());
                scott.spawn(Program.SCREEN_WIDTH/2, Program.SCREEN_HEIGHT/2);
                entityList.Add(scott);
              
                eventFlag = Event.Idle;
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

            if (cycles == 200 && !eventFlag.ToString().Equals("Fighterrow"))
            {
                eventFlag = Event.Fighterrow;
                cycles = 0;
            }


            if (cycles == 200 && !eventFlag.ToString().Equals("Scouts"))
            {
                eventFlag = Event.Scouts;
            }

        }

   
        public void updateList(ArrayList enti)
        {
            entityList = enti;
        }

    }
}
