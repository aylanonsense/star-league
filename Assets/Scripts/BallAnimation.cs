using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu]
    public class BallAnimation : ScriptableObject
    {
        public BezierSplinePath path;
        public List<Vector3> points;
        public float distance;

        public void Reset()
        {
            if (path)
            {
                Generate();
            }
            else
            {
                points = new List<Vector3>
                {
                    new Vector3(0.0f, 0.0f, 100.0f),
                    new Vector3(0.0f, 0.0f, 0.0f)
                };
            }
        }

        public Vector3 GetPosition(float t)
        {
            int index = Mathf.Min(points.Count - 2, Mathf.FloorToInt(t * (points.Count - 1)));
            Vector3 startPoint = points[index];
            Vector3 endPoint = points[index + 1];
            float startTime = index / ((float)(points.Count - 1));
            float endTime = (index + 1) / ((float)(points.Count - 1));
            float p = (t - startTime) / (endTime - startTime);
            return (1 - p) * startPoint + p * endPoint;
        }

        public void Generate()
        {
            int numSteps = 100;

            // Calculate the total distance of the path
            float distOfPathSoFar = 0.0f;
            Vector3 prevPathPoint = path.GetPosition(0.0f);
            for (int i = 1; i <= numSteps; i++)
            {
                float t = ((float)i) / ((float)numSteps);
                Vector3 nextPathPoint = path.GetPosition(t);
                float distBetweenPoints = Vector3.Distance(prevPathPoint, nextPathPoint);
                distOfPathSoFar += distBetweenPoints;
                prevPathPoint = nextPathPoint;
            }
            distance = distOfPathSoFar;

            // Find evenly-placed points along the path to add to the list of points
            distOfPathSoFar = 0.0f;
            prevPathPoint = path.GetPosition(0.0f);
            points = new List<Vector3>();
            float distOfNextPointToAdd = 0.0f;
            for (int i = 1; i <= numSteps; i++)
            {
                float t = i / ((float)numSteps);
                Vector3 nextPathPoint = path.GetPosition(t);
                float distBetweenPoints = Vector3.Distance(prevPathPoint, nextPathPoint);
                while (distOfNextPointToAdd <= distOfPathSoFar + distBetweenPoints)
                {
                    float p = (distOfNextPointToAdd - distOfPathSoFar) / distBetweenPoints;
                    points.Add(prevPathPoint * (1 - p) + nextPathPoint * p);
                    distOfNextPointToAdd = distance * ((float)points.Count) / ((float)numSteps);
                }
                distOfPathSoFar += distBetweenPoints;
                prevPathPoint = nextPathPoint;
            }
            points.Add(path.GetPosition(1.0f));
        }
    }
}
