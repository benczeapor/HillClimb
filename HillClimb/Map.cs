using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        private List<Vector4> platforms;
        public List<Vector4> Platforms
        {
            get { return platforms; }
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

            platforms = new List<Vector4>();

            level = 50;
            //rects.Add(new Rectangle(0, (int)(graphicsDevice.Viewport.Height - level), graphicsDevice.Viewport.Width, (int)level));
            platforms.Add(new Vector4(0, (int)(graphicsDevice.Viewport.Height - level), graphicsDevice.Viewport.Width, (int)(graphicsDevice.Viewport.Height - level)));
            //platforms.Add(new Vector4(10, 100, 800, 400));

            wheel = new Wheel(this);
            wheel.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            wheel.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            wheel.Draw(spriteBatch, gameTime);

            spriteBatch.Begin();

            //spriteBatch.Draw(texture, new Rectangle(0, (int)(480 - level), 800, (int)level), Color.Black);

            foreach(Vector4 platform in platforms)
            {
                float length = (float)Math.Sqrt((platform.X - platform.Z) * (platform.X - platform.Z) + (platform.Y - platform.W) + (platform.Y - platform.W));
                float rotation = (float)Math.Asin((Math.Abs(platform.W - platform.Y) / length));

                Rectangle rect = new Rectangle((int)platform.X, (int)platform.Y, (int)length, 20);
                spriteBatch.Draw(texture, rect, null, Color.Black, rotation, new Vector2(0, 0), SpriteEffects.None, 1);
            }

            spriteBatch.Draw(texture, new Rectangle(49, (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Yellow);
            spriteBatch.Draw(texture, new Rectangle(49 + (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Magenta);
            spriteBatch.Draw(texture, new Rectangle(49 + 2 * (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Green);


            spriteBatch.End();
        }
    }
}
