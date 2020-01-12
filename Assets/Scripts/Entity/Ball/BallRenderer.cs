using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BallRenderer : EntityComponent
    {
        [SerializeField] private SpriteController ballSprite = default;
        [SerializeField] private SpriteController ballShadowSprite = default;
        [SerializeField] private SpriteController ballReticleSprite = default;

        public override void PrepareToRender()
        {
            // Figure out where the ball and shadow should be drawn
            Vector3 ballPixelPosition = PerspectiveManager.ToPixels(transform.position.x, transform.position.y, transform.position.z);
            Vector3 ballShadowPixelPosition = PerspectiveManager.ToPixels(transform.position.x, PerspectiveManager.GroundY(), transform.position.z);
            Vector3 ballReticlePixelPosition = PerspectiveManager.ToPixels(transform.position.x, transform.position.y, 0.0f);

            // Need to adjust the frame position since not all ball frames are pixel-aligned
            int frame = Mathf.FloorToInt(Mathf.Clamp(15.0f * ballPixelPosition.z, 3.0f, 16.0f)) - 3;
            float frameOffset = (frame % 2 == 0) ? 0.5f : 0.0f;

            // Move the ball and shadow to those positions
            ballSprite.transform.position = new Vector3(ballPixelPosition.x + frameOffset, ballPixelPosition.y + frameOffset, 0.0f);
            ballShadowSprite.transform.position = new Vector3(ballShadowPixelPosition.x + frameOffset, ballShadowPixelPosition.y, 0.0f);
            ballReticleSprite.transform.position = new Vector3(ballReticlePixelPosition.x + frameOffset, ballReticlePixelPosition.y, 0.0f);

            // Update their frames to reflect their size
            ballSprite.SetSprite(frame);
            ballShadowSprite.SetSprite(frame);
            ballReticleSprite.SetSprite(frame);

            // Hide the reticle if out of range
            if (transform.position.z < 0.0f || transform.position.z > 100.0f)
            {
                ballReticleSprite.Hide();
            }
            else
            {
                ballReticleSprite.Show();
            }
        }
    }
}