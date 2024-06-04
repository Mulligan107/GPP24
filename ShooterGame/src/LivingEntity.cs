using System;
using SDL2;

namespace ShooterGame
{
    class LivingEntity : Entity
    {
        public bool friendly = false;


        public LivingEntity()
        {
            
        }

        public void hit()
        {
            if (!iframe)
            {
                iframe = true;
                Console.WriteLine("Lives befor: " + lives);
                lives = lives - 1;
                if (lives < 0)
                {
                    if (!friendly)
                    {
                        choosenAnim = 2;
                        setupAnimation(9);
                        repeats = 1;
                        animationCounter = 0;
                        animationSpeed = 2;
                    }
                }
                else
                {
                    if (!friendly)
                    {
                        choosenAnim = 3;
                        setupAnimation(10);
                        repeats = 1;
                        animationCounter = 0;
                        animationSpeed = 4;
                    }   
                }
                Console.WriteLine("Lives after: " + lives);
            }
        }

    }

}