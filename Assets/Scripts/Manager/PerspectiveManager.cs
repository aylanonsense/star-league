using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class PerspectiveManager
    {
        private static readonly float pitchersMoundPixelY = 25.5f;
        private static readonly float strikeZoneCenterPixelY = -46.0f;
        private static readonly float homePlatePixelY = -101.5f;
        private static readonly float minPixelY = -9999.0f;
        private static readonly float strikeZoneCellPixelWidth = 25.0f;
        private static readonly float strikeZoneCellPixelHeight = 25.0f;

        private static readonly float pitchersMoundDepth = 100.0f;
        private static readonly float homePlateDepth = 0.0f;
        private static readonly float strikeZoneCellWidth = 10.0f;
        private static readonly float strikeZoneCellHeight = 10.0f;

        private static readonly float pitchersMoundRelativeSize = 0.2f;

        /* Perspective units
         * z = 0 is where the batter is standing
         * z = 100 is where the pitcher is standing
         * So the camera is looking in the positive Z direction
         *
         * x = 0 is the center of the strike zone
         * x = 10 is the middle of the right portion of the strike zone
         * x = -10 is the middle of the left portion of the strike zone
         * So to the right of the batter is the positive X direction
         *
         * y = 0 is the center of the strike zone
         * y = 10 is the middle of the top portion of the strike zone
         * y = -10 is the middle of the top portion of the strike zone
         * So up is the positive Y direction
         */

        /* Pixel units
         * x = 0 is the center of the screen
         * x = 128 is the right side of the screen
         * x = -128 is the left side of the screen
         *
         * y = 0 is the center of the screen
         * y = 120 is the top of the screen
         * y = -120 is the bottom of the screen
         */

        public static float GroundY()
        {
            return (homePlatePixelY - strikeZoneCenterPixelY) * strikeZoneCellHeight / strikeZoneCellPixelHeight;
        }

        public static float ToPerspectiveDepth(float pixelY)
        {
            return 0.0f;
        }

        public static Vector4 ToPerspective(float pixelX, float pixelY, float depth)
        {
            return new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        }

        public static Vector3 ToPixels(Vector3 position)
        {
            return ToPixels(position.x, position.y, position.z);
        }

        public static Vector3 ToPixels(float x, float y, float z)
        {
            // Figure out the vanishing point of this perspective
            float vanishingPointPixelY = homePlatePixelY + (pitchersMoundPixelY - homePlatePixelY) / (1 - pitchersMoundRelativeSize);

            // Solve a system of equations to figure out how far we are from the "camera"
            float a = ((vanishingPointPixelY - minPixelY) / (vanishingPointPixelY - pitchersMoundPixelY) + (minPixelY - vanishingPointPixelY) / (vanishingPointPixelY - homePlatePixelY)) / (pitchersMoundDepth - homePlateDepth);
            float b = (vanishingPointPixelY - minPixelY) / (vanishingPointPixelY - homePlatePixelY) - a * homePlateDepth;
            float distanceFromCamera = Mathf.Max(1.0f, a * z + b);

            // Using that we can determine the scale of things at that distance
            float scale = (a * homePlateDepth + b) / distanceFromCamera;

            // And from there we can figure out the pixel on the screen to use
            float pixelX = x * scale * strikeZoneCellPixelWidth / strikeZoneCellWidth;
            float pixelY = vanishingPointPixelY - (vanishingPointPixelY - minPixelY) / distanceFromCamera + (y - GroundY()) * scale * strikeZoneCellPixelHeight / strikeZoneCellHeight;

            // Return the pixel position and the scale at that distance
            return new Vector3(pixelX, pixelY, scale);
        }
    }
}
