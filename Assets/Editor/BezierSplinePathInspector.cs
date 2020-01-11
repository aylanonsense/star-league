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
        private SelectionType selectionType = SelectionType.None;

        private enum SelectionType
        {
            None,
            Point,
            Ground,
            AnchorInPoint,
            AnchorInGround,
            AnchorOutPoint,
            AnchorOutGround
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
                }, new Color(0.0f, 0.0f, 0.0f, 0.5f), new Color(0.0f, 0.0f, 0.0f, 0.0f));

            // Iterate through the list of points
            BezierSplinePoint prevPoint = null;
            Vector3 prevPixelPosition = Vector3.zero;
            for (int i = 0; i < points.Count; i++)
            {
                BezierSplinePoint point = points[i];
                bool isSelected = (selectedPointIndex == i);

                // Do some perspective math to figure out where the ball will be rendered at this point
                Vector3 position = point.position;
                Vector3 pixelCoordinates = PerspectiveManager.ToPixels(position);
                Vector3 pixelPosition = new Vector3(pixelCoordinates.x, pixelCoordinates.y, 0.0f);
                Vector3 groundPixelCoordinates = PerspectiveManager.ToPixels(position.x, PerspectiveManager.GroundY(), position.z);
                Vector3 groundPixelPosition = new Vector3(groundPixelCoordinates.x, groundPixelCoordinates.y, 0.0f);
                float pixelScale = pixelCoordinates.z;
                float radius = 15.0f * pixelScale / 2;

                // Draw a line from this point to the previous point
                if (prevPoint != null)
                {
                    Handles.color = Color.cyan;
                    Handles.DrawLine(prevPixelPosition, pixelPosition);
                }

                // Draw a circle representing where the ball will be at this point
                Handles.color = isSelected ? Color.yellow : Color.white;
                Handles.DrawSolidDisc(pixelPosition, Vector3.back, radius);

                // Draw a line from the ball to the ground
                Handles.DrawLine(pixelPosition, groundPixelPosition);

                // Draw a button where the ball is
                if (Handles.Button(pixelPosition, Quaternion.identity, radius, Mathf.Max(4.0f, 15.0f * pixelScale + 2.0f), Handles.CircleHandleCap))
                {
                    selectedPointIndex = i;
                    selectionType = SelectionType.Point;
                    Repaint();
                }

                // Draw a button where the ground under the ball is
                if (Handles.Button(groundPixelPosition, Quaternion.identity, radius / 2, radius / 2 + 2.0f, Handles.DotHandleCap))
                {
                    selectedPointIndex = i;
                    selectionType = SelectionType.Ground;
                    Repaint();
                }

                // Draw the position handle so the user can change any aspect of a point
                if (isSelected)
                {
                    if (selectionType == SelectionType.Point)
                    {
                        EditorGUI.BeginChangeCheck();
                        Vector3 newPixelPosition = Handles.DoPositionHandle(pixelPosition, Quaternion.identity);
                        if (EditorGUI.EndChangeCheck())
                        {
                            point.position = PerspectiveManager.ToPerspective(newPixelPosition.x, newPixelPosition.y, position.z);
                            Undo.RecordObject(path, "Move Point");
                            EditorUtility.SetDirty(path);
                        }
                    }
                    else if (selectionType == SelectionType.Ground)
                    {
                        EditorGUI.BeginChangeCheck();
                        Vector3 newPixelPosition = Handles.DoPositionHandle(groundPixelPosition, Quaternion.identity);
                        if (EditorGUI.EndChangeCheck())
                        {
                            float depth = PerspectiveManager.ToPerspectiveDepth(newPixelPosition.y);
                            Vector3 newGroundPosition = PerspectiveManager.ToPerspective(newPixelPosition.x, newPixelPosition.y, depth);
                            point.position = new Vector3(newGroundPosition.x, position.y, newGroundPosition.z);
                            Undo.RecordObject(path, "Move Point");
                            EditorUtility.SetDirty(path);
                        }
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
            selectionType = SelectionType.None;
        }
    }
}
