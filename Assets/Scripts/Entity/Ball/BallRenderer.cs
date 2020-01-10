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
            // Figure out where the ball and shadow should be drawn
            Vector3 ballPixelPosition = PerspectiveManager.ToPixels(transform.position.x, transform.position.y, transform.position.z);
            Vector3 ballShadowPixelPosition = PerspectiveManager.ToPixels(transform.position.x, PerspectiveManager.GroundY(), transform.position.z);

            // Move the ball and shadow to those positions
            ballSprite.transform.position = new Vector3(ballPixelPosition.x, ballPixelPosition.y, 0.0f);
            ballShadowSprite.transform.position = new Vector3(ballShadowPixelPosition.x, ballShadowPixelPosition.y, 0.0f);

            // Update their frames to reflect their size
            int frame = Mathf.FloorToInt(Mathf.Clamp(15.0f * ballPixelPosition.z, 3.0f, 16.0f)) - 3;
            ballSpriteController.SetSprite(frame);
            ballShadowSpriteController.SetSprite(frame);
        }
    }
}