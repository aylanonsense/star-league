using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Batter : Entity
    {
        public new readonly EntityType type = EntityType.Batter;

        [SerializeField] private StrikeZone strikeZone;

        private StateMachine<BatterState> stateMachine;

        public override void Initialize()
        {
            stateMachine = new StateMachine<BatterState>(BatterState.Ready);
        }

        public override void UpdateState()
        {
            stateMachine.Update();
        }

        public void SetAim(int x, int y)
        {
            strikeZone.SetAim(x, y);
        }

        private enum BatterState
        {
            Ready,
            Swinging
        }
    }
}
