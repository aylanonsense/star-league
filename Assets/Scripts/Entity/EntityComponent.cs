using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class EntityComponent : BaseMonoBehaviour
    {
        // Called once as soon as the object is created
        public virtual void Created() { }

        // Called when the object is created and when it's withdrawn from an entity pool
        public virtual void Initialize() { }

        // Called when the Entity is added to the game
        public virtual void AddedToGame() { }

        // Called once per frame, this is the time to check input and make AI decisions
        public virtual void MakeDecisions() { }

        // Called once per frame, this is the time to update the entity's state
        public virtual void UpdateState() { }

        // Called once per frame, this is where the entity can prepare itself for rendering
        public virtual void PrepareToRender() { }

        // Called when the Entity is removed from the game
        public virtual void RemovedFromGame() { }

        // Called when the Entity is destroyed, which happens after it's removed from the game (unless it has a pool)
        public virtual void Destroyed() { }
    }
}
