using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JumperGame.gameEntities;

namespace JumperGame.src.manager
{
    public class entitySystem
    {
        private List<Entity> _entities;
        

        public entitySystem()
        {
            _entities = new List<Entity>();
            
        }

        public bool Initialize()
        {
            // Initialize or load entities if necessary
            return true;
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public IEnumerable<Entity> GetAllEntities()
        {
            return _entities;
        }

        public void Update(double deltaTime, double timeElapsed)
        {
            
        }

        public Entity GetEntityByGID(int gid) {
            foreach (Entity enti in _entities)
            {
                if (enti.gid == gid)
                {
                    return enti;
                }
            }
            return null;
        }

        /*
        public Entity FindEntity(Func<Entity, bool> predicate)
        {
            return _entities.Find(predicate);
        }*/
    }
}

