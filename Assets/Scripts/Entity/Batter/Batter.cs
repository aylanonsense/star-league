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

        public override void CheckInteractions()
        {
            //List<Entity> balls = GameManager.Instance.GetEntitiesOfType(EntityType.Ball);
            //int i = 1;
            //foreach (Ball ball in balls)
            //{
            //    i++;
            //    Debug.Log("a " + i);
            //}
        }

        public void SetAim(int x, int y)
        {
            strikeZone.SetReticle(x, y);
        }

        public void Swing(int x, int y)
        {
            stateMachine.SetState(BatterState.Swinging);
        }

        private enum BatterState
        {
            Ready,
            Swinging
        }
    }
}
