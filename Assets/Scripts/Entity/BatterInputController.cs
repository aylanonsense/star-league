using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Batter))]
    public class BatterInputController : EntityComponent
    {
        private Batter batter;

        public override void  Initialize()
        {
            batter = GetComponent<Batter>();
        }

        public override void MakeDecisions()
        {
            if (batter.CanMove())
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
                    batter.Move(direction);
                }
            }
        }
    }
}
