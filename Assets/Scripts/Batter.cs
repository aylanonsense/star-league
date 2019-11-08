using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class Batter : Entity
    {
        //private enum BatterState
        //{
        //    Ready,
        //    Moving
        //}

        [SerializeField] private Field field;
        //private BatterState state;
        private Plate plate;
        private Animator stateMachine;

        private void Awake()
        {
            stateMachine = GetComponent<Animator>();
            //state = BatterState.Ready;
        }

        private void Start()
        {
            SetPosition(CardinalDirection.South);
        }

        private void Update()
        {
            
        }

        public void SetPosition(CardinalDirection direction)
        {
            plate = field.GetPlate(direction);
            transform.position = new Vector3(plate.transform.position.x, transform.position.y, plate.transform.position.z);
        }

        public void MoveInDirection(CardinalDirection direction)
        {
            stateMachine.SetTrigger("StartMoving");
        }

        //private void SetState(BatterState state)
        //{
        //    this.state = state;
        //}
    }
}
