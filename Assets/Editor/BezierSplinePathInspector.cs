using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(BezierSplinePath))]
    public class BezierSplinePathInspector : Editor
    {
        private int selectedPointIndex = -1;
        private SelectedPointType selectedPointType = SelectedPointType.None;
        bool hasSelectedGround = false;

        private enum SelectedPointType
        {
            None,
            Point,
            AnchorIn,
            AnchorOut
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            BezierSplinePath path = target as BezierSplinePath;
            List<BezierSplinePoint> points = path.points;

            // Darken the rest of the scene
            Handles.DrawSolidRectangleWithOutline(new Vector3[] {
                    new Vector3(-128.0f, -120.0f, 0.0f),
                    new Vector3(-128.0f, 120.0f, 0.0f),
                    new Vector3(128.0f, 120.0f, 0.0f),
                    new Vector3(128.0f, -120.0f, 0.0f),
                }, new Color(0.0f, 0.0f, 0.0f, 0.7f), new Color(0.0f, 0.0f, 0.0f, 0.0f));

            // Iterate through the list of points
            BezierSplinePoint prevPoint = null;
            Vector3 prevPixelPosition = Vector3.zero;
            for (int i = 0; i < points.Count; i++)
            {
                BezierSplinePoint point = points[i];
                bool isSelected = (selectedPointIndex == i);

                // Do some perspective math to figure out where the ball will be rendered at this point
                Vector3 pixelPosition = ToPixels(point.position);
                Vector3 groundPixelPosition = ToGroundPixels(point.position);
                float scale = ToScale(point.position);
                float radius = 15.0f * scale / 2;

                // Draw a line from this point to the previous point
                if (prevPoint != null)
                {
                    Handles.color = Color.cyan;
                    Handles.DrawLine(prevPixelPosition, pixelPosition);
                }

                if (isSelected)
                {
                    // Draw a handle at the anchor in position
                    Vector3 anchorInPixelPosition = ToPixels(point.anchorIn);
                    Vector3 anchorInGroundPixelPosition = ToGroundPixels(point.anchorIn);
                    float anchorInScale = ToScale(point.anchorIn);
                    float anchorInSize = 2.0f * anchorInScale;
                    Handles.color = Color.gray;
                    Handles.DrawLine(anchorInPixelPosition, anchorInGroundPixelPosition);
                    if (Handles.Button(anchorInGroundPixelPosition, Quaternion.identity, anchorInSize, Mathf.Max(2.0f, anchorInSize + 1.0f), Handles.DotHandleCap))
                    {
                        selectedPointIndex = i;
                        selectedPointType = SelectedPointType.AnchorIn;
                        hasSelectedGround = true;
                        Repaint();
                    }
                    Handles.color = Color.magenta;
                    Handles.DrawLine(anchorInPixelPosition, pixelPosition);
                    if (Handles.Button(anchorInPixelPosition, Quaternion.identity, anchorInSize, Mathf.Max(2.0f, anchorInSize + 1.0f), Handles.DotHandleCap))
                    {
                        selectedPointIndex = i;
                        selectedPointType = SelectedPointType.AnchorIn;
                        hasSelectedGround = false;
                        Repaint();
                    }
                }

                // Draw a circle representing where the ball will be at this point
                Handles.color = isSelected ? Color.yellow : Color.white;
                Handles.DrawSolidDisc(pixelPosition, Vector3.back, radius);

                // Draw a line from the ball to the ground
                Handles.DrawLine(pixelPosition, groundPixelPosition);

                // Draw a button where the ground under the ball is
                if (Handles.Button(groundPixelPosition, Quaternion.identity, radius / 2, Mathf.Max(2.0f, radius / 2 + 1.0f), Handles.DotHandleCap))
                {
                    selectedPointIndex = i;
                    selectedPointType = SelectedPointType.Point;
                    hasSelectedGround = true;
                    Repaint();
                }

                // Draw a button where the ball is
                if (Handles.Button(pixelPosition, Quaternion.identity, radius, Mathf.Max(2.0f, radius + 1.0f), Handles.CircleHandleCap))
                {
                    selectedPointIndex = i;
                    selectedPointType = SelectedPointType.Point;
                    hasSelectedGround = false;
                    Repaint();
                }

                if (isSelected)
                {
                    // Draw a handle at the anchor out position
                    Vector3 anchorOutPixelPosition = ToPixels(point.anchorOut);
                    Vector3 anchorOutGroundPixelPosition = ToGroundPixels(point.anchorOut);
                    float anchorOutScale = ToScale(point.anchorOut);
                    float anchorOutSize = 2.0f * anchorOutScale;
                    Handles.color = Color.gray;
                    Handles.DrawLine(anchorOutPixelPosition, anchorOutGroundPixelPosition);
                    if (Handles.Button(anchorOutGroundPixelPosition, Quaternion.identity, anchorOutSize, Mathf.Max(2.0f, anchorOutSize + 1.0f), Handles.DotHandleCap))
                    {
                        selectedPointIndex = i;
                        selectedPointType = SelectedPointType.AnchorOut;
                        hasSelectedGround = true;
                        Repaint();
                    }
                    Handles.color = Color.magenta;
                    Handles.DrawLine(pixelPosition, anchorOutPixelPosition);
                    if (Handles.Button(anchorOutPixelPosition, Quaternion.identity, anchorOutSize, Mathf.Max(2.0f, anchorOutSize + 1.0f), Handles.DotHandleCap))
                    {
                        selectedPointIndex = i;
                        selectedPointType = SelectedPointType.AnchorOut;
                        hasSelectedGround = false;
                        Repaint();
                    }
                }

                // Draw the position handle so the user can change any aspect of a point
                if (isSelected)
                {
                    Vector3 selectedVector;
                    if (selectedPointType == SelectedPointType.Point)
                    {
                        selectedVector = point.position;
                    }
                    else if (selectedPointType == SelectedPointType.AnchorIn)
                    {
                        selectedVector = point.anchorIn;
                    }
                    else
                    {
                        selectedVector = point.anchorOut;
                    }

                    Vector3 selectedPixelPosition;
                    if (hasSelectedGround)
                    {
                        selectedPixelPosition = ToGroundPixels(selectedVector);
                    }
                    else
                    {
                        selectedPixelPosition = ToPixels(selectedVector);
                    }

                    EditorGUI.BeginChangeCheck();
                    Vector3 newPixelPosition = Handles.DoPositionHandle(selectedPixelPosition, Quaternion.identity);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Vector3 newPosition;
                        if (hasSelectedGround)
                        {
                            float depth = PerspectiveManager.ToPerspectiveDepth(newPixelPosition.y);
                            Vector3 newGroundPosition = PerspectiveManager.ToPerspective(newPixelPosition.x, newPixelPosition.y, depth);
                            newPosition = new Vector3(newGroundPosition.x, selectedVector.y, newGroundPosition.z);
                        }
                        else
                        {
                            newPosition = PerspectiveManager.ToPerspective(newPixelPosition.x, newPixelPosition.y, selectedVector.z);
                        }
                        Vector3 diff = newPosition - point.position;
                        if (selectedPointType == SelectedPointType.Point)
                        {
                            point.position = newPosition;
                            point.anchorIn += diff;
                            point.anchorOut += diff;
                        }
                        else if (selectedPointType == SelectedPointType.AnchorIn)
                        {
                            point.anchorIn = newPosition;
                            point.anchorOut = point.position - 2 * diff;
                        }
                        else
                        {
                            point.anchorOut = newPosition;
                            point.anchorIn = point.position - 2 * diff;
                        }
                        Undo.RecordObject(path, "Move Point");
                        EditorUtility.SetDirty(path);
                    }
                }

                prevPoint = point;
                prevPixelPosition = pixelPosition;
            }
        }

        public override void OnInspectorGUI()
        {
            BezierSplinePath path = target as BezierSplinePath;

            // Draw the normal fields and such
            DrawDefaultInspector();

            // Draw controls for the currently-selected point
            if (selectedPointIndex >= 0)
            {
                GUILayout.Label("Selected Point");
                if (GUILayout.Button("Add Point"))
                {
                    // TODO
                }
                if (GUILayout.Button("Delete Point"))
                {
                    // TODO
                }
            }
        }

        protected void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        protected void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            selectedPointIndex = -1;
            selectedPointType = SelectedPointType.None;
            hasSelectedGround = false;
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
