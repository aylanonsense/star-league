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
            points = new List<BezierSplinePoint>();
            points.Add(new BezierSplinePoint(new Vector3(0.0f, 0.0f, 100.0f)));
            points.Add(new BezierSplinePoint(new Vector3(0.0f, 0.0f, 50.0f)));
            points.Add(new BezierSplinePoint(new Vector3(0.0f, 0.0f, 0.0f)));
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
            anchorIn = new Vector3(position.x, position.y, position.z - 10.0f);
            anchorIn = new Vector3(position.x, position.y, position.z + 10.0f);
        }
    }
}
