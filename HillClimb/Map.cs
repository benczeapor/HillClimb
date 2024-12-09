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
using System.Diagnostics;

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
            //segments.Add(new Segment(0, (int)(graphicsDevice.Viewport.Height - level), 800, (int)(graphicsDevice.Viewport.Height - level)));

            //platforms.Add(new Vector4(200, 300, 500, 300));
            //segments.Add(new Segment(0, 200, 400, 430));

            //segments.Add(new Segment(0, 480, 800, 0));

            //segments.Add(new Segment(100, 100, 600, 400, 1));

            segments.Add(new Segment(0, 100, 100, 400, 1));
            segments.Add(new Segment(100, 400, 300, 450, 2));
            //segments.Add(new Segment(300, 450, 350, 450, 3));
            segments.Add(new Segment(500, 300, 800, 300, 4));
            segments.Add(new Segment(300, 450, 500, 300, 5));

            //segments.Add(new Segment(200, 240, 600, 240, 5));

            wheel = new Wheel(this);
            wheel.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            wheel.Update(gameTime);

            //segments.Sort((x, y) => y.Distance.CompareTo(x.Distance));
            //MouseState ms = Mouse.GetState();

            //Debug.WriteLine(segments[0].calculateDistance(new Vector2(ms.X, ms.Y)));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            wheel.Draw(spriteBatch, gameTime);

            spriteBatch.Begin();

            //spriteBatch.Draw(texture, new Rectangle(0, (int)(480 - level), 800, (int)level), Color.Black);

            foreach (Segment segment in segments)
            {
                segment.Draw(spriteBatch, gameTime, texture);    
            }

            //spriteBatch.Draw(texture, new Rectangle(49, (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Yellow);
            //spriteBatch.Draw(texture, new Rectangle(49 + (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Magenta);
            //spriteBatch.Draw(texture, new Rectangle(49 + 2 * (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Green);


            spriteBatch.End();
        }
    }
}
