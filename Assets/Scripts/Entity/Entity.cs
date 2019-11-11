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

        public void DoInitialize()
        {
            components = GetComponents<EntityComponent>();
            //Array.Sort(components, new Comparison<EntityComponent>((a, b) => a.executionOrder.CompareTo(b.executionOrder)));
            foreach (EntityComponent component in components)
            {
                component.Initialize();
            }
        }

        public void DoAddedToGame()
        {
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

        public static Entity CreateEntity(GameObject prefab)
        {
            GameObject gameObject = Instantiate(prefab);
            Entity entity = gameObject.GetComponent<Entity>();
            entity.DoInitialize();
            return entity;
        }
    }

    public enum EntityType
    {
        None,
        Pitcher,
        Batter
    }
}
