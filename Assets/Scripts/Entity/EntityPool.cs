using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EntityPool
    {
        private readonly Queue<Entity> entities;
        private readonly GameObject prefab;

        private bool isDestroyed = false;

        public EntityPool(GameObject prefab, int initialAmount = 0)
        {
            entities = new Queue<Entity>();
            this.prefab = prefab;
            Create(initialAmount);
        }

        public T Withdraw<T>(bool addToGame = true, bool initialize = true) where T : Entity
        {
            if (entities.Count == 0)
            {
                Create(1);
            }
            T entity = (T)entities.Dequeue();
            if (initialize)
            {
                entity.DoInitialize();
            }
            if (addToGame)
            {
                entity.AddToGame();
            }
            return entity;
        }

        public void Deposit(Entity entity)
        {
            entities.Enqueue(entity);
        }

        private void Create(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Entity entity = Entity.CreateEntity(prefab, false, false);
                entity.pool = this;
                Deposit(entity);
            }
        }

        public void Destroy()
        {
            isDestroyed = true;
            foreach (Entity entity in entities)
            {
                entity.pool = null;
                if (!entity.IsDestroyed())
                {
                    entity.Destroy();
                }
            }
            entities.Clear();
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }
    }
}
