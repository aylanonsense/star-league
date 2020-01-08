﻿using System.Collections;
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
            float minBallSize = 3.0f;
            float maxBallSize = 16.0f;
            float ballSizeAtStrikeZone = 14.0f;
            float perspective = 65.0f;
            float strikeZoneUnitZ = 100.0f;
            float strikeZoneCellUnitWidth = 10.0f;
            float strikeZoneCellPixelWidth = 25.0f;
            float strikeZoneCellUnitHeight = 10.0f;
            float strikeZoneCellPixelHeight = 25.0f;
            float vanishingPointPixelY = 85.0f;
            float strikeZoneCenterPixelY = -34.0f;
            float homePlatePixelY = -101.0f;
            float groundUnitY = (homePlatePixelY - strikeZoneCenterPixelY) * strikeZoneCellUnitHeight / strikeZoneCellPixelHeight;
            Vector3 pos = transform.position;
            float distanceFromCamera = Mathf.Max(strikeZoneUnitZ + perspective - pos.z, 1.0f);
            float pixelX = pos.x * (strikeZoneCellPixelWidth / strikeZoneCellUnitWidth) * (perspective / distanceFromCamera);
            float pixelY = vanishingPointPixelY - (vanishingPointPixelY - homePlatePixelY) * (perspective / distanceFromCamera);
            float pixelHeight = (pos.y - groundUnitY) * (strikeZoneCellPixelHeight / strikeZoneCellUnitHeight) * (perspective / distanceFromCamera);
            int ballSize = Mathf.CeilToInt(Mathf.Clamp(ballSizeAtStrikeZone * perspective / distanceFromCamera, minBallSize, maxBallSize));
            int frame = ballSize - 3;
            float pixelOffset = frame % 2 == 0 ? 0.5f : 0.0f;
            ballSprite.transform.position = new Vector3(pixelX + pixelOffset, pixelY + pixelHeight - pixelOffset, 0.0f);
            ballShadowSprite.transform.position = new Vector3(pixelX + pixelOffset, pixelY, 0.0f);
            ballSpriteController.SetSprite(frame);
            ballShadowSpriteController.SetSprite(frame);
        }
    }
}