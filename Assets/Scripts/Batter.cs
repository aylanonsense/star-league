using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Batter : Entity
    {
        private enum BatterState
        {
            Ready,
            Moving
        }

        [SerializeField] private Field field;
        private Plate plate;
        private Plate nextPlate;
        private Plate prevPlate;
        private StateMachine<BatterState> stateMachine;

        private void Awake()
        {
            stateMachine = new StateMachine<BatterState>(BatterState.Ready);
        }

        private void Start()
        {
            SetPosition(CardinalDirection.South);
        }

        private void Update()
        {
            stateMachine.Update();
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
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
            stateMachine.SetState(BatterState.Moving);
            nextPlate = field.GetPlate(direction);
            prevPlate = plate;
            plate = null;
        }
    }
}
