using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame.src
{
    class EventTimer
    {
        double cycles = 0;
        public static ArrayList entityList = new ArrayList();
        public EventTimer() {
        }


        public void timedEvent(double deltatime)
        {
            cycles++;
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
                cycles = 0;
            }
        }

        public void updateList(ArrayList enti)
        {
            entityList = enti;
        }

    }
}
