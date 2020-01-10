using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class StrikeZone : Entity
    {
        public new readonly EntityType type = EntityType.StrikeZone;

        [SerializeField] private SpriteController gridSprite;
        [SerializeField] private SpriteController reticleSprite;

        private int aimX = 0;
        private int aimY = 0;

        public void SetAim(int x, int y)
        {
            aimX = x;
            aimY = y;
        }

        public override void PrepareToRender()
        {
            reticleSprite.transform.localPosition = new Vector3(25.0f * aimX, 25.0f * aimY, 0.0f);
        }
    }
}
