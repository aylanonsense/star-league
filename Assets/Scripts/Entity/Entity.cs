using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Entity : EntityComponent
    {
        public readonly EntityType type = EntityType.None;
        public EntityPool pool;

        private EntityComponent[] components;

        public void DoCreated()
        {
            gameObject.SetActive(false);
            components = GetComponents<EntityComponent>();
            foreach (EntityComponent component in components)
            {
                component.Created();
            }
        }

        public void DoInitialize()
        {
            foreach (EntityComponent component in components)
            {
                component.Initialize();
            }
        }

        public void DoAddedToGame()
        {
            gameObject.SetActive(true);
            foreach (EntityComponent component in components)
            {
                component.AddedToGame();
            }
        }

        public void DoMakeDecisions()
        {
            foreach (EntityComponent component in components)
            {
                component.MakeDecisions();
            }
        }

        public void DoUpdateState()
        {
            foreach (EntityComponent component in components)
            {
                component.UpdateState();
            }
        }

        public void DoRemovedFromGame()
        {
            foreach (EntityComponent component in components)
            {
                component.RemovedFromGame();
            }
        }

        public void DestroyOrReturnToPool()
        {
            if (pool == null)
            {
                foreach (EntityComponent component in components)
                {
                    component.Destroyed();
                }
            }
            else
            {
                pool.Deposit(this);
            }
        }

        public void AddToGame()
        {
            GameManager.Instance.AddEntityToGame(this);
        }

        public void RemoveFromGame()
        {
            GameManager.Instance.RemoveEntityFromGame(this);
            gameObject.SetActive(false);
        }

        public static Entity CreateEntity(GameObject prefab, bool initialize = true)
        {
            GameObject gameObject = Instantiate(prefab);
            Entity entity = gameObject.GetComponent<Entity>();
            entity.DoCreated();
            if (initialize)
            {
                entity.DoInitialize();
            }
            return entity;
        }
    }

    public enum EntityType
    {
        None,
        Batter,
        Pitcher
    }
}
