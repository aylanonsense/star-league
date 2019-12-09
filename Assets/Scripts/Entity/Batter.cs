using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Batter : Entity
    {
        public new readonly EntityType type = EntityType.Batter;

        private StateMachine<BatterState> stateMachine;

        public override void Initialize()
        {
            stateMachine = new StateMachine<BatterState>(BatterState.Ready);
        }

        public override void UpdateState()
        {
            stateMachine.Update();
        }

        public bool CanMove()
        {
            return stateMachine.state == BatterState.Ready;
        }

        private enum BatterState
        {
            Ready,
            Moving
        }
    }
}
