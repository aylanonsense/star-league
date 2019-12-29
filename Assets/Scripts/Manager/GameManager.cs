using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        private List<Entity> entities;
        private List<Entity> newEntities;

        private void Awake()
        {
            entities = new List<Entity>();
            newEntities = new List<Entity>();
            FindAndInitializeExistingEntities();
            AddNewEntitiesToGame();
        }

        private void Update()
        {
            MakeDecisions();
            UpdateState();
            PrepareToRender();
            AddNewEntitiesToGame();
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
                entity.DoAddedToGame();
            }
            newEntities.Clear();
        }

        public Entity AddEntityToGame(Entity entity)
        {
            newEntities.Add(entity);
            return entity;
        }

        public Entity RemoveEntityFromGame(Entity entity)
        {
            entities.Remove(entity);
            newEntities.Remove(entity);
            entity.DoRemovedFromGame();
            return entity;
        }
    }
}
