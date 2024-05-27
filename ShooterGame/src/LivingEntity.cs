using System;
using SDL2;

namespace ShooterGame
{
    class LivingEntity : Entity
    {
        public float lives;
        public bool friendly = false;

        public LivingEntity()
        {
        }

        public void hit()
        {
            Console.WriteLine("Lives befor: " + lives);
            lives = lives - 1;
            if (lives < 0)
            {
                kill();
            }
            Console.WriteLine("Lives after: " + lives);
        }

    }

}