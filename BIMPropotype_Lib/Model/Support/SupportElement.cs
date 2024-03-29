﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Support
{
    [Serializable]
    public abstract class SupportElement
    {
        public List<Point> Points { get; set; }
        public CoordinateSystem Base { get; set; }
        public SupportElement() { }

        protected void Add(Point point) 
        {
            if (Points == null)
            {
                Points = new List<Point>() { point };
            }
            else
            {
                Points.Add(point);
            }
        }

        public Point GetFirst() 
        {
            if (Points != null)
            {
                if (Points.Count > 0)
                {
                    return Points[0];
                }
            }
            return null;
        }
        public Point GetSecond()
        {
            if (Points != null)
            {
                if (Points.Count > 1)
                {
                    return Points[1];
                }
            }
            return null;
        }

        public void Transform(Matrix matrix) 
        {
            List<Point> points = new List<Point>();
            foreach (var point in Points)
            {
                points.Add(matrix.Transform(point));
            }
            Points = points;
        }
    }
}
