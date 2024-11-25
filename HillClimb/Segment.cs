using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using System;


namespace HillClimb
{
    internal class Segment
    {
        private Vector2 p1;
        private Vector2 p2;

        private float slope;
        private float distance;

        public Vector2 P1
        {
            get { return p1; }
        }
        public Vector2 P2
        {
            get { return p2; } 
        }

        public float X
        {
            get { return p1.X; }
        }

        public float Y
        {
            get { return p1.Y; }
        }

        public float Z
        {
            get { return p2.X; }
        }

        public float W
        {
            get { return p2.Y; }
        }

        public float Slope
        { 
            get { return slope; } 
        }

        public float Distance
        {
            get { return distance; }
        }

        public Segment(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;

            float d3 = (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            slope = (float)Math.Asin(Math.Abs(p1.Y - p2.Y) / d3);
        }

        public Segment(float x, float y, float z, float w)
        {
            this.p1 = new Vector2(x, y);
            this.p2 = new Vector2(z, w);

            float d3 = (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            slope = (float)Math.Asin(Math.Abs(p1.Y - p2.Y) / d3);
        }

        public float calculateDistance(Vector2 position)
        {
            distance = (float)(Math.Abs((p2.X - p1.X) * (position.Y - p1.Y) - (p2.Y - p1.Y) * (position.X - p1.X)) / Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y)));
            return distance;
        }
    }
}
