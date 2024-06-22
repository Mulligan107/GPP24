using System;
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

        public virtual void shootEnemy(int flipX, double delta) { }

        public virtual void bulletFan(double delta) { }
        public virtual void shootTarget(double delta) { }
        public virtual void deathray(double delta) { }
    }

}