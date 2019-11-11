using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Batter : Entity
    {
        public new readonly EntityType type = EntityType.Batter;

        [SerializeField] private Field field;
        private Plate plate;
        private Plate nextPlate;
        private Plate prevPlate;
        private StateMachine<BatterState> stateMachine;

        public override void Initialize()
        {
            stateMachine = new StateMachine<BatterState>(BatterState.Ready);
        }

        public override void AddedToGame()
        {
            SetPosition(CardinalDirection.South);
        }

        public override void UpdateState()
        {
            stateMachine.Update();
        }

        public void SetPosition(CardinalDirection direction)
        {
            plate = field.GetPlate(direction);
            transform.position = new Vector3(plate.transform.position.x, transform.position.y, plate.transform.position.z);
        }

        public bool CanMove()
        {
            return stateMachine.state == BatterState.Ready;
        }

        public void Move(CardinalDirection direction)
        {
            SetPosition(direction);
            //stateMachine.SetState(BatterState.Moving);
            //nextPlate = field.GetPlate(direction);
            //prevPlate = plate;
            //plate = null;
        }

        private enum BatterState
        {
            Ready,
            Moving
        }
    }
}
