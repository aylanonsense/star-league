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
        private bool isAddedToGame = false;
        private bool isDestroyed = false;

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
            isAddedToGame = true;
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

        public void DoCheckInteractions()
        {
            foreach (EntityComponent component in components)
            {
                component.CheckInteractions();
            }
        }

        public void DoPrepareToRender()
        {
            foreach (EntityComponent component in components)
            {
                component.PrepareToRender();
            }
        }

        public void DoRemovedFromGame()
        {
            isAddedToGame = false;
            foreach (EntityComponent component in components)
            {
                component.RemovedFromGame();
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

        public void Destroy()
        {
            if (isAddedToGame)
            {
                RemoveFromGame();
            }
            if (pool == null || pool.IsDestroyed())
            {
                isDestroyed = true;
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

        public bool IsAddedToGame()
        {
            return isAddedToGame;
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }

        public static Entity CreateEntity(GameObject prefab, bool addToGame = true, bool initialize = true)
        {
            GameObject gameObject = Instantiate(prefab);
            Entity entity = gameObject.GetComponent<Entity>();
            entity.DoCreated();
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

        public static T CreateEntity<T>(GameObject prefab, bool addToGame = true, bool initialize = true) where T : Entity
        {
            return (T)CreateEntity(prefab, initialize);
        }
    }

    public enum EntityType
    {
        None,
        Batter,
        Pitcher,
        Ball,
        StrikeZone
    }
}
