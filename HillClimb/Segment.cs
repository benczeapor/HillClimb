using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Diagnostics;


namespace HillClimb
{
    internal class Segment
    {
        private Vector2 p1;
        private Vector2 p2;

        private Vector2 R1;
        private Vector2 R2;

        private float slope;
        private float distance;
        private Color color;
        //private int id;

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
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        //public int Id
        //{
        //    get { return id; }
        //}

        private void Init()
        {
            float hitboxHeight = 50;
            float length = (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            slope = (float)Math.Asin((p1.Y - p2.Y) / length);
            color = Color.Green;
            if (P1.Y <= P2.Y)
            {
                R1.X = P1.X + hitboxHeight * (float)Math.Cos(slope - (Math.PI / 2));
                R1.Y = P1.Y + hitboxHeight * (float)Math.Sin(slope - (Math.PI / 2));
                R2.X = P2.X + hitboxHeight * (float)Math.Cos(slope - (Math.PI / 2));
                R2.Y = P2.Y + hitboxHeight * (float)Math.Sin(slope - (Math.PI / 2));
            }
            else
            {
                R1.X = P1.X - hitboxHeight * (float)Math.Abs(Math.Cos(slope - (Math.PI / 2)));
                R1.Y = P1.Y - hitboxHeight * (float)Math.Abs(Math.Sin(slope - (Math.PI / 2)));
                R2.X = P2.X - hitboxHeight * (float)Math.Abs(Math.Cos(slope - (Math.PI / 2)));
                R2.Y = P2.Y - hitboxHeight * (float)Math.Abs(Math.Sin(slope - (Math.PI / 2)));
            }
            //Debug.WriteLine(id.ToString() + ": " + (180/Math.PI) * slope);
        }

        public Segment(Vector2 p1, Vector2 p2)
        {
            if (p1.X < p2.X)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
            else
            {
                this.p1 = p2;
                this.p2 = p1;
            }
            //this.id = id;
            Init();
            
        }

        public Segment(float x, float y, float z, float w)
        {
            if( x < z)
            {
                this.p1 = new Vector2(x, y);
                this.p2 = new Vector2(z, w);
            }
            else
            {
                this.p1 = new Vector2(z, w);
                this.p2 = new Vector2(x, y);
            }
            //this.id = id;
            //this.id = id;
            Init();
            
        }

        public float calculateDistance(Vector2 position)
        {
            distance = (float)(Math.Abs((p2.X - p1.X) * (position.Y - p1.Y) - (p2.Y - p1.Y) * (position.X - p1.X)) / Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y)));

            float d1 = Vector2.Distance(position, this.p1);
            float d2 = Vector2.Distance(position, this.p2);
            float ll = Vector2.Distance(this.p1, this.p2);

            if (d1 > ll || d2 > ll)
            {
                if (d1 < d2)
                    return d1;

                return d2;
            }

            return distance;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D texture)
        {
            float length = (float)Math.Sqrt((P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y));
            float rotation = (float)Math.Asin((P2.Y - P1.Y) / length);
            Rectangle rect = new Rectangle((int)P1.X, (int)P1.Y, (int)length, 20);
            spriteBatch.Draw(texture, rect, null, color, rotation, new Vector2(0, 0), SpriteEffects.None, 1);

            //spriteBatch.DrawLine(P1.X, P1.Y, P2.X, P2.Y, Color.Blue);

            //spriteBatch.DrawLine(R1.X, R1.Y, R2.X, R2.Y, Color.Red);
            //spriteBatch.DrawLine(R1.X, R1.Y, P1.X, P1.Y, Color.Red);
            //spriteBatch.DrawLine(P2.X, P2.Y, R2.X, R2.Y, Color.Red);
        }
    }
}
