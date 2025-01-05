using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HillClimb
{
    internal class Fuel
    {
        private float distance = 180 * 75;

        private Map map;
        private Texture2D texture;
        private bool isVisible;
        //private float previous;
        //private float current;
        private Vector2 position;
        private List<float> points;
        private int index;
        private bool makeMore;

        public Fuel(Map map)
        {
            this.map = map;
        }

        private bool Ccw(Vector2 a, Vector2 b, Vector2 c)
        {
            return (c.Y - a.Y) * (b.X - a.X) > (b.Y - a.Y) * (c.X - a.X);
        }

        private bool DoesIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            //return Math.Sign((p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X)) != Math.Sign((p2.X - p1.X) * (p4.Y - p1.Y) - (p2.Y - p1.Y) * (p4.X - p1.X));
            return Ccw(a, c, d) != Ccw(b, c, d) && Ccw(a, b, c) != Ccw(a, b, d);
        }

        public bool HandleHitbox(Vector2 h1, Vector2 h2, Vector2 h3, Vector2 h4)
        {
            int sum = 0;
            sum += DoesIntersect(h1, h2, position, new Vector2(-100, -100)) ? 1 : 0;
            sum += DoesIntersect(h2, h3, position, new Vector2(-100, -100)) ? 1 : 0;
            sum += DoesIntersect(h3, h4, position, new Vector2(-100, -100)) ? 1 : 0;
            sum += DoesIntersect(h4, h1, position, new Vector2(-100, -100)) ? 1 : 0;


            if (sum % 2 == 1)
            {
                if (points.Count > 0)
                {
                    float temp = points.Last();
                    points.RemoveAt(index);
                    points.Add(temp + distance);
                    position = new Vector2(-69420, -69420);
                    isVisible = false;
                    distance *= 1.1f;
                }

                return true;
            }

            if (h1.X > position.X && h2.X > position.X && h3.X > position.X && h4.X > position.X && position.X > -100)
            {
                if (points.Count < 5)
                {
                    points.Add(points.Last() + distance);
                    distance *= 1.1f;
                }
                
                //makeMore = false;
            }

            return false;
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager contentManager)
        {
            isVisible = false;
            texture = contentManager.Load<Texture2D>("Textures/fuel");
            //previous = map.CoinPos(1000);
            //previous = 0;
            points = new List<float>();
            //points.Add(new Vector2(distance, 0));
            points.Add(distance);
            position = new Vector2(-69420, -69420);
            index = 0;
            makeMore = true;
            //points[0] -= new Vector2(16, 32);
            //current = 
            //position = 
        }
        public void Update(GameTime gameTime, OrthographicCamera camera)
        {
            //current = previous + distance;
            //points.Add(points.Last() + distance);
            //position = map.CoinPos(points.Last());
            //position -= new Vector2(16, 32);

            float start = camera.ScreenToWorld(new Vector2(0, 0)).X;
            //Debug.WriteLine(points.Count);
            for(int i=0; i<points.Count; i++)
            {
                if (points[i] < start - 200)
                {
                    points.RemoveAt(i);
                }
                if (points[i] > start - 200 && points[i] < start + 1280 + 200)
                {
                    position = map.CoinPos(points[i]) - new Vector2(16, 32);
                    isVisible = true;
                    index = i;
                    break;
                }
            }

            //if (points.Count >= 3)
            //{
            //    points.RemoveAt(0);
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, previous, Color.White);
            if (isVisible && position.X > -69420)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0);
            }
        }
    }
}
