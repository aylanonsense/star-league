using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class SpriteSheet : ScriptableObject
    {
        [SerializeField]
        private Sprite[] sprites;

        public Sprite[] GetSprites()
        {
            return sprites;
        }
    }
}
