using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EntityPool
    {
        private readonly Queue<Entity> entities;
        private readonly GameObject prefab;

        public EntityPool(GameObject prefab, int initialAmount = 0)
        {
            entities = new Queue<Entity>();
            this.prefab = prefab;
            Create(initialAmount);
        }

        public Entity Withdraw()
        {
            if (entities.Count == 0)
            {
                Create(1);
            }
            Entity entity = entities.Dequeue();
            entity.gameObject.SetActive(true);
            return entity;
        }

        public void Deposit(Entity entity)
        {
            entity.gameObject.SetActive(false);
            entities.Enqueue(entity);
        }

        private void Create(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                Entity entity = Entity.CreateEntity(prefab);
                entity.pool = this;
                Deposit(entity);
            }
        }
    }
}
