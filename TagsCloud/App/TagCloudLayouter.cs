﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloud.Infrastructure;

namespace TagsCloud.App
{
    public class TagCloudLayouter
    {
        private readonly ILayoutAlgorithm layoutAlgorithm;
        private readonly List<Rectangle> placedRectangles;

        public TagCloudLayouter(ILayoutAlgorithm layoutAlgorithm)
        {
            this.layoutAlgorithm = layoutAlgorithm;
            placedRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty)
                throw new ArgumentException();
            foreach (var point in layoutAlgorithm)
            {
                var topLeftCorner = new Point
                {
                    X = point.X - rectangleSize.Width / 2,
                    Y = point.Y - rectangleSize.Height / 2
                };
                var rectangle = new Rectangle(topLeftCorner, rectangleSize);
                if (IsRectangleIntersects(rectangle))
                    continue;
                placedRectangles.Add(rectangle);
                AddUsedPointsToAlgorithm(rectangle);
                return rectangle;
            }

            return new Rectangle();
        }

        private bool IsRectangleIntersects(Rectangle rectangle)
        {
            return placedRectangles.Any(rectangle.IntersectsWith);
        }

        private void AddUsedPointsToAlgorithm(Rectangle rectangle)
        {
            var usedPoints = new List<Point>();
            for (var i = 0; i < rectangle.Width; i++)
            for (var j = 0; j < rectangle.Height; j++)
                usedPoints.Add(new Point(rectangle.Left + i, rectangle.Top + j));
            layoutAlgorithm.AddUsedPoints(usedPoints);
        }
    }
}