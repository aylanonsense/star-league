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

            // Draw the bezier curve
            Handles.color = Color.cyan;
            Vector2 prevBezierPixelPosition = Vector3.zero;
            for (float t = 0.0f; t <= 1.0f; t += 0.001f)
            {
                Vector3 bezierPosition = path.GetPosition(t);
                Vector2 bezierPixelPosition = PerspectiveManager.ToPixels(bezierPosition);
                if (t > 0.0f)
                {
                    Handles.DrawLine(prevBezierPixelPosition, bezierPixelPosition);
                }
                prevBezierPixelPosition = bezierPixelPosition;
            }

            // Iterate through the list of points
            BezierSplinePoint prevPoint = null;
            Vector3 prevPixelPosition = Vector3.zero;
            for (int i = 0; i < points.Count; i++)
            {
                BezierSplinePoint point = points[i];
                bool isSelected = (selectedPointIndex == i);

                // Do some perspective math to figure out where the ball will be rendered at this point
                Vector2 pixelPosition = PerspectiveManager.ToPixels(point.position);
                Vector2 groundPixelPosition = PerspectiveManager.ToGroundPixels(point.position);
                float scale = PerspectiveManager.ToPixelScale(point.position.z);
                float radius = 15.0f * scale / 2;

                if (isSelected)
                {
                    // Draw a handle at the anchor in position
                    Vector3 anchorIn = point.position + point.anchorIn;
                    Vector2 anchorInPixelPosition = PerspectiveManager.ToPixels(anchorIn);
                    Vector2 anchorInGroundPixelPosition = PerspectiveManager.ToGroundPixels(anchorIn);
                    float anchorInScale = PerspectiveManager.ToPixelScale(anchorIn.z);
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
                Handles.color = new Color(1.0f, 1.0f, isSelected ? 0.0f : 1.0f, 0.1f);
                Handles.DrawSolidDisc(pixelPosition, Vector3.back, radius);

                // Draw a line from the ball to the ground
                Handles.color = isSelected ? Color.yellow : Color.white;
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
                    Vector3 anchorOut = point.position + point.anchorOut;
                    Vector2 anchorOutPixelPosition = PerspectiveManager.ToPixels(anchorOut);
                    Vector2 anchorOutGroundPixelPosition = PerspectiveManager.ToGroundPixels(anchorOut);
                    float anchorOutScale = PerspectiveManager.ToPixelScale(anchorOut.z);
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
                        selectedVector = point.position + point.anchorIn;
                    }
                    else
                    {
                        selectedVector = point.position + point.anchorOut;
                    }

                    Vector2 selectedPixelPosition;
                    if (hasSelectedGround)
                    {
                        selectedPixelPosition = PerspectiveManager.ToGroundPixels(selectedVector);
                    }
                    else
                    {
                        selectedPixelPosition = PerspectiveManager.ToPixels(selectedVector);
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
                        if (selectedPointType == SelectedPointType.Point)
                        {
                            point.position = newPosition;
                        }
                        else if (selectedPointType == SelectedPointType.AnchorIn)
                        {
                            point.anchorIn = newPosition - point.position;
                            if (point.mirrorAnchors)
                            {
                                point.anchorOut = -point.anchorIn;
                            }
                        }
                        else
                        {
                            point.anchorOut = newPosition - point.position;
                            if (point.mirrorAnchors)
                            {
                                point.anchorIn = -point.anchorOut;
                            }
                        }
                        Undo.RegisterCompleteObjectUndo(path, "Move Point");
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
                BezierSplinePoint selectedPoint = path.points[selectedPointIndex];
                if (GUILayout.Button("Add Point"))
                {
                    path.points.Insert(selectedPointIndex + 1, new BezierSplinePoint(new Vector3(selectedPoint.position.x, selectedPoint.position.y, selectedPoint.position.z - 10.0f)));
                    selectedPointIndex += 1;
                    Undo.RegisterCompleteObjectUndo(path, "Add Point");
                    EditorUtility.SetDirty(path);
                }
                if (GUILayout.Button("Delete Point"))
                {
                    path.points.RemoveAt(selectedPointIndex);
                    selectedPointIndex -= 1;
                    Undo.RegisterCompleteObjectUndo(path, "Delete Point");
                    EditorUtility.SetDirty(path);
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
    }
}
