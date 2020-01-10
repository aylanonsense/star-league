using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        private List<Entity> entities;
        private List<Entity> newEntities;
        private List<Entity> entitiesToRemove;
        private Dictionary<EntityType, List<Entity>> entitiesByType;

        private void Awake()
        {
            entities = new List<Entity>();
            newEntities = new List<Entity>();
            entitiesToRemove = new List<Entity>();
            entitiesByType = new Dictionary<EntityType, List<Entity>>();
            FindAndInitializeExistingEntities();
            AddNewEntitiesToGame();
        }

        private void Update()
        {
            MakeDecisions();
            UpdateState();
            AddNewEntitiesToGame();
            RemoveEntitiesFromGame();
            PrepareToRender();
        }

        private void FindAndInitializeExistingEntities()
        {
            List<Entity> existingEntities = new List<Entity>(FindObjectsOfType<Entity>());
            foreach (Entity entity in existingEntities)
            {
                entity.DoCreated();
                entity.DoInitialize();
                newEntities.Add(entity);
            }
        }

        private void MakeDecisions()
        {
            foreach (Entity entity in entities)
            {
                entity.DoMakeDecisions();
            }
        }

        private void UpdateState()
        {
            foreach (Entity entity in entities)
            {
                entity.DoUpdateState();
            }
        }

        private void CheckInteractions()
        {
            foreach (Entity entity in entities)
            {
                entity.DoCheckInteractions();
            }
        }

        private void PrepareToRender()
        {
            foreach (Entity entity in entities)
            {
                entity.DoPrepareToRender();
            }
        }

        private void AddNewEntitiesToGame()
        {
            foreach (Entity entity in newEntities)
            {
                entities.Add(entity);
                GetEntitiesOfType(entity.type).Add(entity);
                entity.DoAddedToGame();
            }
            newEntities.Clear();
        }

        private void RemoveEntitiesFromGame()
        {
            foreach (Entity entity in entitiesToRemove)
            {
                entities.Remove(entity);
                newEntities.Remove(entity);
                GetEntitiesOfType(entity.type).Remove(entity);
                entity.DoRemovedFromGame();
            }
            entitiesToRemove.Clear();
        }

        public List<Entity> GetEntitiesOfType(EntityType type)
        {
            if (!entitiesByType.ContainsKey(type))
            {
                entitiesByType[type] = new List<Entity>();
            }
            return entitiesByType[type];
        }

        public Entity AddEntityToGame(Entity entity)
        {
            newEntities.Add(entity);
            return entity;
        }

        public Entity RemoveEntityFromGame(Entity entity)
        {
            entitiesToRemove.Add(entity);
            return entity;
        }
    }
}
