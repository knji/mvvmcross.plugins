using System.Collections.Generic;
using Android.Graphics;
using Android.OS;
using Java.Util;

namespace MapQuest.Android.Maps
{
    public class SimplifierHandler : Handler
    {
        private static int SIMPLIFY = 0;
        private readonly Stack<int[]> reuse = new Stack<int[]>();
        private MapView mapView;
        private readonly IList<Point> _points;
        private readonly IList<GeoPoint> _data;
        private readonly int _epsilon;
        private readonly List<GeoPoint> _simplified = new List<GeoPoint>();

        public SimplifierHandler(MapView mapView, Looper looper, IList<Point> points, IList<GeoPoint> data , int epsilon )
        {
            this.mapView = mapView;
            _points = points;
            _data = data;
            _epsilon = epsilon;
        }

        private int[] GetIndices(int start, int end)
        {
            int[] indices = null;
            if (reuse.Count == 0)
            {
                indices = new[] {start, end};
            }
            else
            {
                indices = reuse.Pop();
                indices[0] = start;
                indices[1] = end;
            }
            return indices;
        }

        private IList<GeoPoint> Simplify(IList<Point> points, IList<GeoPoint> data, int epsilon)
        {
            var output = new List<GeoPoint>();

            var stack = new Stack<int[]>();
            var start = 0;
            var end = data.Count - 1;

            stack.Push(GetIndices(start, end));

            var indices = new List<int>();
            indices.Add(start);
            indices.Add(end);
            var pout = new Point();
            while (stack.Count != 0)
            {
                int[] startEnd = stack.Pop();
                start = startEnd[0];
                end = startEnd[1];

                reuse.Push(startEnd);
                if (start + 1 < end)
                {
                    int maxDistance = 0;
                    int farthestIndex = 0;

                    var startPoint = points[start];
                    var endPoint =  points[end];
                    for (int i = start + 1; i < end; i++)
                    {
                        var p =  points[i];
                        Util.ClosestPoint(p, startPoint, endPoint, pout );
                        int dist = Util.DistanceSquared(p.X, p.X, pout.X, pout.Y);
                        if (dist > maxDistance)
                        {
                            maxDistance = dist;
                            farthestIndex = i;
                        }
                    }
                    if (maxDistance > epsilon)
                    {
                        indices.Add(farthestIndex);
                        int[] indices1 = GetIndices(start, farthestIndex);
                        int[] indices2 = GetIndices(farthestIndex, end);

                        stack.Push(indices1);
                        stack.Push(indices2);
                    }
                }
            }

            Collections.Sort(indices);
            int previous = -1;
            foreach (var index in indices)
            {
                var i = index;
                if (i != previous)
                {
                    output.Add(data[i]);
                    previous = i;
                }
            }

            return output;
        }

        public override void HandleMessage(Message msg)
        {
            if (msg.What == 0)
            {
                _simplified.Clear();
                _simplified.AddRange(Simplify(_points, _data, _epsilon));

                // call this in layer itself
                mapView.PostInvalidate();
            }
            else
            {
                base.HandleMessage(msg);
            }
        }
    }
}