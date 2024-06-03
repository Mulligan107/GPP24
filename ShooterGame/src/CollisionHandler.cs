using SDL2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShooterGame
{
    class CollisionHandler
    {

        public CollisionHandler() { 
        }

        public static ArrayList checkCollision(ArrayList entityLsit)
        {
            ArrayList newList = new ArrayList();

            foreach (LivingEntity enti in entityLsit)
            {
                foreach (LivingEntity counterEnti in entityLsit)
                {
                    if (enti != counterEnti && enti.alive && counterEnti.alive)
                    {
                        
                       
                        if (SDL.SDL_HasIntersection(ref enti.destRect, ref counterEnti.destRect) == SDL.SDL_bool.SDL_TRUE 
                            && (!enti.friendly || !counterEnti.friendly) ) //Hitten wen Hitboxen überschneiden und beide nicht friendly sind
                        {
                            Console.WriteLine("HIT _________________________");
                            enti.hit();
                            counterEnti.hit();
                        }
                    }
                }

                if (enti.alive)
                {
                    newList.Add(enti);
                }
            }
            return newList;
        }


    }



}
