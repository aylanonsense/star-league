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
        }
    }
}
