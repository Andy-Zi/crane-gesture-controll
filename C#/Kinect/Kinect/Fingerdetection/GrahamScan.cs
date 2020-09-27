﻿using System.Collections.Generic;
using System.Linq;

namespace Kinect.Fingerdetection
{
    internal class GrahamScan
    {
        private IList<DepthPointEx> _points;

        public IList<DepthPointEx> ConvexHull(IList<DepthPointEx> points)
        {
            if (points.Count <= 3) return points;

            _points = points;

            var pointsSortedByAngle = SortPoints();
            var index = 1;

            while (index + 1 < pointsSortedByAngle.Count)
            {
                var value = PointAngleComparer.Compare(pointsSortedByAngle[index - 1], pointsSortedByAngle[index + 1],
                    pointsSortedByAngle[index]);
                if (value < 0)
                {
                    index++;
                }
                else
                {
                    pointsSortedByAngle.RemoveAt(index);
                    if (index > 1) index--;
                }
            }

            pointsSortedByAngle.Add(pointsSortedByAngle.First());

            return pointsSortedByAngle;
        }

        private DepthPointEx GetMinimumPoint()
        {
            var minPoint = _points[0];

            for (var index = 1; index < _points.Count; index++) minPoint = GetMinimumPoint(minPoint, _points[index]);

            return minPoint;
        }

        private DepthPointEx GetMinimumPoint(DepthPointEx p1, DepthPointEx p2)
        {
            if (p1.Y < p2.Y)
                return p1;
            if (p1.Y == p2.Y)
                if (p1.X < p2.X)
                    return p1;

            return p2;
        }

        private IList<DepthPointEx> SortPoints()
        {
            var p0 = GetMinimumPoint();

            var comparer = new PointAngleComparer(p0);

            var sortedPoints = new List<DepthPointEx>(_points);
            sortedPoints.Remove(p0);
            sortedPoints.Insert(0, p0);
            sortedPoints.Sort(1, sortedPoints.Count - 1, comparer);

            return sortedPoints;
        }
    }
}