using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpriteController : EntityComponent
    {
        [SerializeField]
        private GameObject spriteObject;
        [SerializeField]
        private SpriteSheet spriteSheet;

        private SpriteRenderer spriteRenderer;
        private Sprite[] sprites;

        public override void Created()
        {
            spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            sprites = spriteSheet.GetSprites();
        }

        public int GetNumSprites()
        {
            return sprites.Length;
        }

        public void SetSprite(int index = 0)
        {
            spriteRenderer.sprite = sprites[index];
        }
    }
}