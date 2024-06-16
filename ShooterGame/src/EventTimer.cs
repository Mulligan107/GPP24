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
        bool eventFlag = false;
        Random random = new Random();
        public static ArrayList entityList = new ArrayList();
        public EventTimer() {
            cycles = 0;
            eventFlag = false;

        }


        public void timedEvent(double deltatime, FileHandler fileHandler)
        {
            cycles++;

            if (cycles % 20 == 0 && eventFlag && counter < 10)
            {
                counter++;
                Fighter erni = new Fighter(fileHandler.getFighter());
                erni.posX = Program.SCREEN_WIDTH - (Program.SCREEN_WIDTH / 10);
                erni.posY = Program.SCREEN_HEIGHT - (Program.SCREEN_HEIGHT / 18) - ((Program.SCREEN_HEIGHT / 11) * (cycles / 20));
                erni.timeAlive = random.Next(0,150);
                entityList.Add(erni);
            }

            ArrayList entitiesToProcess = new ArrayList(entityList);

            foreach (LivingEntity levi in entitiesToProcess)
            {
                if (levi.GetType().Name.Equals("Fighter"))
                {
                    levi.timeAlive++;

                    if (levi.timeAlive > 100)
                    {
                        levi.shootEnemy();
                        levi.timeAlive = 0;
                    }

                }
            }

            if (cycles > 200)
            {
                eventFlag = true;
                cycles = 0;
            }
                
        }

   
        public void updateList(ArrayList enti)
        {
            entityList = enti;
        }

    }
}
