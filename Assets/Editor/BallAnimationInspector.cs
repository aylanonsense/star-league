using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(BallAnimation))]
    public class BallAnimationInspector : Editor
    {
        public void OnSceneGUI(SceneView sceneView)
        {
            BallAnimation anim = target as BallAnimation;
            List<Vector3> points = anim.points;

            // Darken the rest of the scene
            Handles.DrawSolidRectangleWithOutline(new Vector3[] {
                    new Vector3(-128.0f, -120.0f, 0.0f),
                    new Vector3(-128.0f, 120.0f, 0.0f),
                    new Vector3(128.0f, 120.0f, 0.0f),
                    new Vector3(128.0f, -120.0f, 0.0f),
                }, new Color(0.0f, 0.0f, 0.0f, 0.7f), new Color(0.0f, 0.0f, 0.0f, 0.0f));

            // Draw the path
            Handles.color = Color.cyan;
            Vector3 prevBezierPixelPosition = Vector3.zero;
            Vector2? prevPixelPosition = null;
            foreach (Vector3 point in points)
            {
                Vector2 pixelPosition = PerspectiveManager.ToPixels(point);
                if (prevPixelPosition != null)
                {
                    Handles.DrawLine((UnityEngine.Vector3)prevPixelPosition, pixelPosition);
                }
                prevPixelPosition = pixelPosition;
            }

            // Draw circles representing where the ball will be along the path
            Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
            for (int i = 0; i <= 20; i++)
            {
                float t = i / 20.0f;
                Vector3 position = anim.GetPosition(t);
                Vector2 pixelPosition = ToPixels(position);
                Vector2 groundPixelPosition = ToGroundPixels(position);
                float scale = ToScale(position);
                float radius = 15.0f * scale / 2;
                Handles.DrawSolidDisc(pixelPosition, Vector3.back, radius);
                Handles.DrawLine(groundPixelPosition, pixelPosition);
            }
        }

        public override void OnInspectorGUI()
        {
            BallAnimation anim = target as BallAnimation;

            // Add a button to generate points from the given animation's path
            if (GUILayout.Button("Generate"))
            {
                anim.Generate();
                Undo.RegisterCompleteObjectUndo(anim, "Generate");
                EditorUtility.SetDirty(anim);
            }

            // Draw the normal fields and such
            DrawDefaultInspector();
        }

        protected void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        protected void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private Vector2 ToPixels(Vector3 position)
        {
            Vector3 pixels = PerspectiveManager.ToPixels(position);
            return new Vector2(pixels.x, pixels.y);
        }

        private Vector2 ToGroundPixels(Vector3 position)
        {
            Vector3 pixels = PerspectiveManager.ToPixels(position.x, PerspectiveManager.GroundY(), position.z);
            return new Vector2(pixels.x, pixels.y);
        }

        private float ToScale(Vector3 position)
        {
            Vector3 pixels = PerspectiveManager.ToPixels(position);
            return pixels.z;
        }
    }
}
