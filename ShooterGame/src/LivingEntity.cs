﻿using System;
using SDL2;

namespace ShooterGame
{
    class LivingEntity : Entity
    {

        public double timeAlive;
        public LivingEntity()
        {
            
        }

        public virtual void hit() { }

        public virtual void shootEnemy() { }

        public virtual void movement() { }

    }

}