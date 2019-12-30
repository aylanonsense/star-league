using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BallRenderer : EntityComponent
    {
        [SerializeField]
        private GameObject ballSprite;
        [SerializeField]
        private GameObject ballShadowSprite;

        private SpriteController ballSpriteController;
        private SpriteController ballShadowSpriteController;

        public override void Created()
        {
            ballSpriteController = ballSprite.GetComponent<SpriteController>();
            ballShadowSpriteController = ballShadowSprite.GetComponent<SpriteController>();
        }

        public override void PrepareToRender()
        {
            // y = 77 is the vanishing point
            // y = 65 is second base
            // y = -100 is just over home plate
            // y = -120 is off the bottom of the screen
            Vector3 pos = transform.position;
            float distanceFromCamera = Mathf.Max(145.0f - pos.z, 1.0f);
            float x = 113.0f * pos.x / distanceFromCamera;
            float height = 111.0f * (pos.y + 23.6f) / distanceFromCamera;
            float y = 84.0f - 8385.0f / distanceFromCamera;
            int size = Mathf.CeilToInt(Mathf.Clamp(3.0f, 15.0f * 45.0f / distanceFromCamera, 16.0f));
            int frame = size - 3;
            float offset = frame % 2 == 0 ? 0.5f : 0.0f;
            ballSprite.transform.position = new Vector3(x + offset, y + height + offset, 0.0f);
            ballShadowSprite.transform.position = new Vector3(x + offset, y, 0.0f);
            ballSpriteController.SetSprite(frame);
            ballShadowSpriteController.SetSprite(frame);
        }
    }
}