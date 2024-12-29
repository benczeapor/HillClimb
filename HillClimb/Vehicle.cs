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

        private Vector2 h1, h2, h3, h4; // points for item collection hitbox

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

        public Wheel RearWheel
        {
            get { return rearWheel; }
            set { rearWheel = value; }
        }

        public void HandleInput(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 rearPos = rearWheel.Position;
            Vector2 frontPos = frontWheel.Position;

            float rotationSpeed = 100;

            if (!frontWheel.IsOnGround && !rearWheel.IsOnGround)
            {
                if (kstate.IsKeyDown(Keys.Left))
                {

                    rearPos.Y -= rotationSpeed * elapsed * (float)Math.Sin(slope - Math.PI / 2);
                    rearPos.X += rotationSpeed * elapsed * (float)Math.Cos(slope - Math.PI / 2);
                    frontPos.Y -= rotationSpeed * elapsed * (float)Math.Sin(slope + Math.PI / 2);
                    frontPos.X += rotationSpeed * elapsed * (float)Math.Cos(slope + Math.PI / 2);

                        
                    //Debug.WriteLine(rotationSpeed * elapsed * (float)Math.Cos(slope));
                }
                if (kstate.IsKeyDown(Keys.Right))
                {

                    rearPos.Y += rotationSpeed * elapsed * (float)Math.Sin(slope - Math.PI / 2);
                    rearPos.X -= rotationSpeed * elapsed * (float)Math.Cos(slope - Math.PI / 2);
                    frontPos.Y += rotationSpeed * elapsed * (float)Math.Sin(slope + Math.PI / 2);
                    frontPos.X -= rotationSpeed * elapsed * (float)Math.Cos(slope + Math.PI / 2);
                }
            }
            else if(frontWheel.IsOnGround)
            {
                if (kstate.IsKeyDown(Keys.Left))
                {

                }
                if (kstate.IsKeyDown(Keys.Right))
                {

                }
            }
            else if(rearWheel.IsOnGround)
            {
                if (kstate.IsKeyDown(Keys.Left))
                {

                }
                if (kstate.IsKeyDown(Keys.Right))
                {

                }
            }

            rearWheel.Position = rearPos;
            frontWheel.Position = frontPos;

        }

        private void UpdateHitbox(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            h1 = rearWheel.Position;
            h2 = frontWheel.Position;
            if(slope < Math.PI / 2)
            {
                h3.Y = rearWheel.Position.Y + 50 * (float)Math.Sin(slope + Math.PI / 2);
                h3.X = rearWheel.Position.X + 50 * (float)Math.Cos(slope + Math.PI / 2);
            }
            
            else
            {
                h3.X = 0;
                h3.Y = 0;
                //h3.Y = rearWheel.Position.Y + 50 * (float)Math.Abs(Math.Sin(slope - Math.PI / 2));
                //h3.X = rearWheel.Position.X + 50 * (float)Math.Abs(Math.Cos(slope - Math.PI / 2));
            }

            h4.Y = frontWheel.Position.Y + 50 * (float)Math.Sin(slope + Math.PI / 2);
            h4.X = frontWheel.Position.X + 50 * (float)Math.Cos(slope + Math.PI / 2);

            //h1.Y += rearWheel.Radius;
            //h2.Y += frontWheel.Radius;
            //h1.X -= rearWheel.Radius;
            //h2.X += frontWheel.Radius;
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

            HandleInput(gameTime);

            float distance = Vector2.Distance(frontWheel.Position, rearWheel.Position);

            //float slopeSine = ;
            slope = (float)(Math.Asin((rearWheel.Position.Y - frontWheel.Position.Y) / distance));


            if (frontWheel.Position.X > rearWheel.Position.X)
            {
                slope = (float)(Math.Asin((rearWheel.Position.Y - frontWheel.Position.Y) / distance));
                
            }
            else
            {
                slope = (float)(Math.Asin((frontWheel.Position.Y - rearWheel.Position.Y) / distance));
                slope += (float)Math.PI;
            }

            h1 = frontWheel.Position;
            h2 = rearWheel.Position;

            float error = wheelBase - distance;

            Vector2 correction = new Vector2(0, 0);

            correction.X = (float)((frontWheel.Position.X - rearWheel.Position.X) / wheelBase);
            correction.Y = (float)((frontWheel.Position.Y - rearWheel.Position.Y) / wheelBase);

            //Debug.WriteLine(correction.ToString() + ", " + error.ToString());

            //if (frontWheel.Position.X < rearWheel.Position.X)
            //{
            //    error *= -1;
            //}

            Debug.WriteLine(correction);
            h3 = frontWheel.Position - correction * 100;
            h4 = rearWheel.Position + correction * 100;


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



            frontWheel.CorrectPosition();
            rearWheel.CorrectPosition();

            //UpdateHitbox(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            frontWheel.Draw(spriteBatch, gameTime);
            rearWheel.Draw(spriteBatch, gameTime);
            //spriteBatch.DrawLine(frontWheel.Position, rearWheel.Position, Color.Black, 2);

            spriteBatch.Draw(texture, new Vector2(rearWheel.Position.X, rearWheel.Position.Y), null, Color.White, -slope, new Vector2(50, 100), 0.65f, SpriteEffects.None, 1);

            //spriteBatch.DrawLine(h1, h3, Color.Red);
            //spriteBatch.DrawLine(h2, h4, Color.Red);
        }

    }
}
