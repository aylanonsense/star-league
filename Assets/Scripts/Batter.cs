using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Batter : Entity
    {
        [SerializeField] private Field field;
        private Plate plate;

        public void MoveInDirection(CardinalDirection direction)
        {
            plate = field.GetPlate(direction);
            transform.position = new Vector3(plate.transform.position.x, transform.position.y, plate.transform.position.z);
        }
    }
}
