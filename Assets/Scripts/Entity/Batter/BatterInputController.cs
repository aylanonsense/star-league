using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Batter))]
    public class BatterInputController : EntityComponent
    {
        private Batter batter;

        public override void Created()
        {
            batter = GetComponent<Batter>();
        }

        public override void MakeDecisions()
        {
            int horizontal = ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) ? 1 : 0) - ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) ? 1 : 0);
            int vertical = ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) ? 1 : 0) - ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) ? 1 : 0);
            bool swing = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift);
            batter.SetAim(horizontal, vertical);
            if (swing)
            {
                batter.Swing(horizontal, vertical);
            }
        }
    }
}
