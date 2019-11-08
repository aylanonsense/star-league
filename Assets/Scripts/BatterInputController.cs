using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Batter))]
    public class BatterInputController : BaseMonoBehaviour
    {
        private Batter batter;

        private void Start()
        {
            batter = GetComponent<Batter>();
        }

        void Update()
        {
            CardinalDirection direction = CardinalDirection.None;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = CardinalDirection.North;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = CardinalDirection.East;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = CardinalDirection.South;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = CardinalDirection.West;
            }
            if (direction != CardinalDirection.None)
            {
                batter.MoveInDirection(direction);
            }
        }
    }
}
