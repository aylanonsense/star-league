using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BallRenderer : EntityComponent
    {
        private SpriteController spriteController;

        public override void Created()
        {
            spriteController = GetComponent<SpriteController>();
        }

        public override void PrepareToRender()
        {
            spriteController.SetSprite(0);
        }
    }
}