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

                // Draw a circle representing where the ball will be at this point
                //Handles.color = isSelected ? new Color(1.0f, 1.0f, 0.0f, 1.0f) : new Color(1.0f, 1.0f, 1.0f, 0.35f);
                Handles.color = isSelected ? Color.yellow : Color.white;
                Handles.DrawSolidDisc(pixelPosition, Vector3.back, radius);

                // Draw a line from the ball to the ground
                Handles.DrawLine(pixelPosition, groundPixelPosition);

                // Draw a button where the ball is
                if (Handles.Button(pixelPosition, Quaternion.identity, radius, Mathf.Max(4.0f, 15.0f * pixelScale + 2.0f), Handles.CircleHandleCap))
                {
                    selectedPointIndex = i;
                }

                // Draw a button where the ground under the ball is
                if (Handles.Button(groundPixelPosition, Quaternion.identity, radius / 2, radius / 2 + 2.0f, Handles.DotHandleCap))
                {
                    selectedPointIndex = i;
                }
            }

            //Vector3[] points = path.points;
            //Debug.Log("pointsz " + (points == null));
            //Debug.Log("Hello");

            //foreach (BezierSplinePoint point in points)
            //{
            //    Vector3 position = point.position;
            //    Handles.color = Color.magenta;
            //    Handles.Button(position, Quaternion.identity, 10.0f, 12.0f, Handles.CircleCap);

            //if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))
            //{
            //    selectedIndex = index;
            //    Repaint();
            //}
            //if (selectedIndex == index)
            //{
            //    EditorGUI.BeginChangeCheck();
            //    point = Handles.DoPositionHandle(point, handleRotation);
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        Undo.RecordObject(spline, "Move Point");
            //        EditorUtility.SetDirty(spline);
            //        spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
            //    }
            //}
            //return point;
            //}

            //Handles.color = Color.white;
            //Handles.DrawLine(new Vector3(-30.0f, 0.0f, 0.0f), new Vector3(0.0f, 20.0f, 0.0f));
        }

        public override void OnInspectorGUI()
        {
            BezierSplinePath path = target as BezierSplinePath;
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
    }
}
