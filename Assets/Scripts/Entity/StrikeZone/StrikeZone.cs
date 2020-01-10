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

        private int reticleX = 0;
        private int reticleY = 0;

        public void SetReticle(int x, int y)
        {
            reticleX = x;
            reticleY = y;
        }

        public override void PrepareToRender()
        {
            reticleSprite.transform.localPosition = new Vector3(25.0f * reticleX, 25.0f * reticleY, 0.0f);
        }
    }
}
