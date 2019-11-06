using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Player))]
    public class PlayerInputController : MonoBehaviour
    {
        private Player player;

        private void Start()
        {
            player = GetComponent<Player>();
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
                player.MoveInDirection(direction);
            }
        }
    }
}
