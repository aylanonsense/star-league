using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class PerspectiveManager
    {
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

        private static readonly float pitchersMoundRelativeSize = 0.1f;

        // Solve a system of equations to figure out perspective constants
        private static readonly float vanishingPointPixelY = homePlatePixelY + (pitchersMoundPixelY - homePlatePixelY) / (1 - pitchersMoundRelativeSize);
        private static readonly float a = ((vanishingPointPixelY - minPixelY) / (vanishingPointPixelY - pitchersMoundPixelY) + (minPixelY - vanishingPointPixelY) / (vanishingPointPixelY - homePlatePixelY)) / (pitchersMoundDepth - homePlateDepth);
        private static readonly float b = (vanishingPointPixelY - minPixelY) / (vanishingPointPixelY - homePlatePixelY) - a * homePlateDepth;

        // Get the distance that an object at the given depth would be from the camera
        public static float DistanceFromCamera(float depth)
        {
            return Mathf.Max(1.0f, a * depth + b);
        }

        // Get the perspective y value of the surface of the ground
        public static float GroundY()
        {
            return (homePlatePixelY - strikeZoneCenterPixelY) * strikeZoneCellHeight / strikeZoneCellPixelHeight;
        }

        // Convert from <x, y> screen pixels to a 3D <x, y, z> vector
        public static Vector3 ToPerspective(Vector2 pixelPosition, float depth)
        {
            return ToPerspective(pixelPosition.x, pixelPosition.y, depth);
        }
        public static Vector3 ToPerspective(float pixelX, float pixelY, float depth)
        {
            Vector2 pixelBaseline = ToPixels(0.0f, 0.0f, depth);
            float scale = ToPixelScale(depth);
            float dx = pixelX - pixelBaseline.x;
            float dy = pixelY - pixelBaseline.y;
            float x = dx * (strikeZoneCellWidth / strikeZoneCellPixelWidth) / scale;
            float y = dy * (strikeZoneCellHeight / strikeZoneCellPixelHeight) / scale;
            float z = depth;
            return new Vector3(x, y, z);
        }

        // Convert from a pixel y value to a perspective depth value
        public static float ToPerspectiveDepth(float pixelY)
        {
            // Return the z value of the ground at that pixel y value
            return ((minPixelY - vanishingPointPixelY) / (pixelY - vanishingPointPixelY) - b) / a;
        }

        // Convert from a 3D <x, y, z> vector to <x, y> screen pixels
        public static Vector2 ToPixels(Vector3 position)
        {
            return ToPixels(position.x, position.y, position.z);
        }
        public static Vector2 ToPixels(float x, float y, float z)
        {
            float distanceFromCamera = DistanceFromCamera(z);
            float scale = ToPixelScale(z);
            float pixelX = x * scale * strikeZoneCellPixelWidth / strikeZoneCellWidth;
            float pixelY = vanishingPointPixelY - (vanishingPointPixelY - minPixelY) / distanceFromCamera + (y - GroundY()) * scale * strikeZoneCellPixelHeight / strikeZoneCellHeight;
            return new Vector2(pixelX, pixelY);
        }

        // Convenience function to find <x, y> pixel position of the ground under the point
        public static Vector2 ToGroundPixels(Vector3 position)
        {
            return ToGroundPixels(position.x, position.y, position.z);
        }
        public static Vector2 ToGroundPixels(float x, float y, float z)
        {
            return ToPixels(x, GroundY(), z);
        }

        // Convert from a perspective depth to a pixel scale
        public static float ToPixelScale(float depth)
        {
            return (a * homePlateDepth + b) / DistanceFromCamera(depth);
        }
    }
}
