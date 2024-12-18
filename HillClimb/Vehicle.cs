using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using MonoGame.Extended;
using System;
using System.Diagnostics;

namespace HillClimb
{
    internal class Vehicle
    {
        private float wheelBase;
        private float slope;

        private Map map;
        private Wheel frontWheel;
        private Wheel rearWheel;
        private Texture2D texture;

        public Map Map 
        { 
            get { return map; } 
            set { map = value; }
        }

        public Wheel FrontWheel
        {
            get { return frontWheel; }
            set { frontWheel = value; }
        }

        public Vehicle()
        {

        }
        
        public void Initialize()
        {
            frontWheel.Initialize();
            rearWheel.Initialize();
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("Textures/car");

            frontWheel = new Wheel(Map);
            rearWheel = new Wheel(Map);
            frontWheel.LoadContent(contentManager);
            rearWheel.LoadContent(contentManager);

            wheelBase = 102;

            frontWheel.Position = new Vector2(120 + wheelBase, 50);
            rearWheel.Position = new Vector2(120, 50);
        }

        public void Update(GameTime gameTime)
        {
            frontWheel.Update(gameTime);
            rearWheel.Update(gameTime);

            float distance = Vector2.Distance(frontWheel.Position, rearWheel.Position);
            slope = (float)(Math.Asin((frontWheel.Position.Y - rearWheel.Position.Y) / distance));

            float error = wheelBase - distance;

            Vector2 correction = new Vector2(0, 0);

            correction.X = (float)Math.Cos(slope);
            correction.Y = (float)Math.Sin(slope);

            Debug.WriteLine(correction.ToString() + ", " + error.ToString());

            //if (frontWheel.Position.X < rearWheel.Position.X)
            //{
            //    error *= -1;
            //}

            if (error > 0.1 || error < -0.1)
            {
                if (frontWheel.IsOnGround && !rearWheel.IsOnGround)
                {
                    rearWheel.Position -= correction * error;
                }
                else if (!frontWheel.IsOnGround && rearWheel.IsOnGround)
                {
                    frontWheel.Position += correction * error;
                }
                else
                {
                    frontWheel.Position += correction * error / 2;
                    rearWheel.Position -= correction * error / 2;
                }
            }

            frontWheel.correctPosition();
            rearWheel.correctPosition();

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            frontWheel.Draw(spriteBatch, gameTime);
            rearWheel.Draw(spriteBatch, gameTime);
            //spriteBatch.DrawLine(frontWheel.Position, rearWheel.Position, Color.Black, 2);

            spriteBatch.Draw(texture, new Vector2(rearWheel.Position.X, rearWheel.Position.Y), null, Color.White, slope, new Vector2(50, 100), 0.65f, SpriteEffects.None, 1);
        }

    }
}
