using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Field : Entity
    {
        [SerializeField] private Plate northPlate;
        [SerializeField] private Plate eastPlate;
        [SerializeField] private Plate southPlate;
        [SerializeField] private Plate westplate;

        public Plate GetPlate(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.North:
                    return northPlate;
                case CardinalDirection.East:
                    return eastPlate;
                case CardinalDirection.South:
                    return southPlate;
                case CardinalDirection.West:
                    return westplate;
                default:
                    return null;
            }
        }
    }
}
