using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteController : BaseMonoBehaviour
    {
        [SerializeField]
        private SpriteSheet spriteSheet;

        private SpriteRenderer spriteRenderer;
        private Sprite[] sprites;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteSheet != null)
            {
                sprites = spriteSheet.GetSprites();
            }
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
