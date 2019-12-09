using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class EntityComponent : BaseMonoBehaviour
    {
        public virtual void Created() { }
        public virtual void Initialize() { }
        public virtual void AddedToGame() { }
        public virtual void MakeDecisions() { }
        public virtual void UpdateState() { }
        public virtual void RemovedFromGame() { }
        public virtual void Destroyed() { }
    }
}
