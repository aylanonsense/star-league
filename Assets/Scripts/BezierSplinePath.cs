using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class BezierSplinePath : ScriptableObject
    {
        public List<BezierSplinePoint> points;

        public void Reset()
        {
            points = new List<BezierSplinePoint>
            {
                new BezierSplinePoint(new Vector3(0.0f, 0.0f, 100.0f)),
                new BezierSplinePoint(new Vector3(0.0f, 0.0f, 0.0f))
            };
        }

        public Vector3 GetPosition(float t)
        {
            t = Mathf.Clamp01(t);
            int numCurves = points.Count - 1;
            int curveNumber = (int)Mathf.Clamp(Mathf.Floor(t * numCurves), 0, numCurves - 1);
            BezierSplinePoint startPoint = points[curveNumber];
            BezierSplinePoint endPoint = points[curveNumber + 1];
            float p = (t - ((float)curveNumber / numCurves)) * numCurves;
            float q = 1.0f - p;
            return q * q * q * startPoint.position
                + q * q * p * startPoint.anchorOut * 3.0f
                + q * p * p * endPoint.anchorIn * 3.0f
                + p * p * p * endPoint.position;
        }
    }

    [Serializable]
    public class BezierSplinePoint
    {
        public Vector3 position;
        public Vector3 anchorIn;
        public Vector3 anchorOut;

        public BezierSplinePoint(Vector3 position)
        {
            this.position = position;
            anchorIn = new Vector3(position.x, position.y, position.z + 10.0f);
            anchorOut = new Vector3(position.x, position.y, position.z - 10.0f);
        }
    }
}
