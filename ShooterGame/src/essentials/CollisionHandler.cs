﻿using SDL2;
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

        public static ArrayList checkCollision(ArrayList entityList)
        {
            ArrayList newList = new ArrayList();

            for (int i = 0; i < entityList.Count; i++)
            {
                LivingEntity enti = (LivingEntity)entityList[i];

                for (int j = i+1; j < entityList.Count; j++) // Verhindert Doppelte Abfrage
                {
                    LivingEntity counterEnti = (LivingEntity)entityList[j];

                    if (enti != counterEnti && enti.alive && counterEnti.alive)
                    {

                        if (SDL.SDL_HasIntersection(ref enti.hitbox, ref counterEnti.hitbox) == SDL.SDL_bool.SDL_TRUE 
                            && (enti.friendly && !counterEnti.friendly || !enti.friendly && counterEnti.friendly
                            && !enti.animationFlag.Equals("death")) ) //Hitten wen Hitboxen überschneiden und beide nicht friendly sind
                        {
                            if (enti.GetType().Name.Equals("Bullet") && counterEnti.GetType().Name.Equals("Bullet"))
                            {

                            }
                            else
                            {
                                Console.WriteLine(enti.GetType().Name);
                                enti.hit();
                                counterEnti.hit();
                            }
                            
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
