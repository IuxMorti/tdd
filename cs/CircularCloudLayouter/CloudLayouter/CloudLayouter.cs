﻿using System;
using System.Collections.Generic;
using System.Drawing;


// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class CloudLayouter
    {
        private readonly List<Rectangle> rectangles;
        private readonly IEnumerator<Point> pointEnumerator;
        private readonly Point center;

        public Rectangle[] Rectangles  => rectangles.ToArray();

        public CloudLayouter(ICurve curve)
        {
            pointEnumerator = curve.GetEnumerator();
            rectangles = new List<Rectangle>();
            center = curve.Center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException("size cannot be less than or equal to zero");
            }

            var nextRectangle = new Rectangle(GetCenterPointRectangle(center, rectangleSize),
                rectangleSize);
            while (true)
            {
                if (!nextRectangle.AreIntersectedAny(rectangles))
                    break;
                pointEnumerator.MoveNext();
                var location = pointEnumerator.Current;
                location = GetCenterPointRectangle(location, rectangleSize);
                nextRectangle = new Rectangle(location, rectangleSize);
            }

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        private static Point GetCenterPointRectangle(Point location, Size size)
        {
            return new Point(
                location.X - size.Width / 2,
                location.Y - size.Height / 2);
        }

    }
}