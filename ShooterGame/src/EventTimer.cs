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
                Enemy erni = new Enemy(fileHandler.getFighter());
                erni.posX = Program.SCREEN_WIDTH - (Program.SCREEN_WIDTH / 10);
                erni.posY = Program.SCREEN_HEIGHT - (Program.SCREEN_HEIGHT / 18) - ((Program.SCREEN_HEIGHT / 11)*(cycles/20));
                entityList.Add(erni);
            }
            if (cycles > 200)
            {
                ArrayList entitiesToProcess = new ArrayList(entityList);

                foreach (LivingEntity levi in entitiesToProcess)
                {
                    if (levi.GetType().Name.Equals("Enemy"))
                    {
                        levi.timeAlive = cycles;
                        levi.shootEnemy();
                    }
                }
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
