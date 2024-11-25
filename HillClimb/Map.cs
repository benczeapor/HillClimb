using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Framework;
using MonoGame.Extended;
//using FarseerPhysics.Dynamics;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HillClimb
{
    internal class Map
    {
        private float level;//

        private Texture2D texture;
        

        private List<Segment> segments;
        public List<Segment> Segments
        {
            get { return segments; }
        }

        private Wheel wheel;
        public Wheel Wheel
        {
            get { return wheel; }
        }

        public Map()
        {

        }

        public void Initialize()
        {
            wheel.Initialize();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch , ContentManager contentManager)
        {   
            texture = new Texture2D(graphicsDevice, 1, 1);
            Color[] color = {Color.White};
            texture.SetData<Color>(color);

            segments = new List<Segment>();

            level = 50;
            //rects.Add(new Rectangle(0, (int)(graphicsDevice.Viewport.Height - level), graphicsDevice.Viewport.Width, (int)level));
            segments.Add(new Segment(0, (int)(graphicsDevice.Viewport.Height - level), 800, (int)(graphicsDevice.Viewport.Height - level)));
            //platforms.Add(new Vector4(200, 300, 500, 300));
            segments.Add(new Segment(0, 200, 400, 430));

            wheel = new Wheel(this);
            wheel.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            wheel.Update(gameTime);

            segments.Sort((x, y) => y.Distance.CompareTo(x.Distance));

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            wheel.Draw(spriteBatch, gameTime);

            spriteBatch.Begin();

            //spriteBatch.Draw(texture, new Rectangle(0, (int)(480 - level), 800, (int)level), Color.Black);

            foreach (Segment segment in segments)
            {
                float length = (float)Math.Sqrt((segment.X - segment.Z) * (segment.X - segment.Z) + (segment.Y - segment.W) * (segment.Y - segment.W));
                float rotation = (float)Math.Asin((Math.Abs(segment.W - segment.Y) / length));

                Rectangle rect = new Rectangle((int)segment.X, (int)segment.Y, (int)length, 20);

                spriteBatch.Draw(texture, rect, null, Color.Black, rotation, new Vector2(0, 0), SpriteEffects.None, 1);

                spriteBatch.DrawLine(segment.X, segment.Y, segment.Z, segment.W, Color.Blue);
            }

            //spriteBatch.Draw(texture, new Rectangle(49, (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Yellow);
            //spriteBatch.Draw(texture, new Rectangle(49 + (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Magenta);
            //spriteBatch.Draw(texture, new Rectangle(49 + 2 * (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Green);


            spriteBatch.End();
        }
    }
}
