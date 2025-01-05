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
        private Texture2D fuelBarTexture;
        private Texture2D fuelTexture;
        private Texture2D driver;
        private float fuel;
        private bool consumeFuel;
        private float fuelCapacity; // in seconds
        private bool isAlive;
        private Vector2 head;
        private float headRadius;

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

        public bool ConsumeFuel
        {
            set { consumeFuel = value; }
        }

        public float Fuel
        {
            get { return fuel; }
        }

        public bool IsAlive
        {
            get { return isAlive; } 
        }

        private int deathReason;
        public int DeathReason
        {
            get { return deathReason; }
        }

        public Vehicle()
        {

        }

        public void HandleInput(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 rearPos = rearWheel.Position;
            Vector2 frontPos = frontWheel.Position;

            float rotationSpeed = 90;

            if (fuel > 0 && frontWheel.CanRotate && rearWheel.CanRotate && isAlive)
            {
                if (kstate.IsKeyDown(Keys.Left))
                {

                    rearPos.Y += rotationSpeed * elapsed * (float)Math.Sin(slope - Math.PI / 2);
                    rearPos.X -= rotationSpeed * elapsed * (float)Math.Cos(slope - Math.PI / 2);
                    frontPos.Y += rotationSpeed * elapsed * (float)Math.Sin(slope + Math.PI / 2);
                    frontPos.X -= rotationSpeed * elapsed * (float)Math.Cos(slope + Math.PI / 2);
                    if(frontWheel.Velocity != rearWheel.Velocity)
                    {
                        Vector2 avg = frontWheel.Velocity + rearWheel.Velocity;

                        avg /= 2;

                        frontWheel.Velocity = avg;
                        rearWheel.Velocity = avg;
                    }
                }
                if (kstate.IsKeyDown(Keys.Right))
                {

                    rearPos.Y -= rotationSpeed * elapsed * (float)Math.Sin(slope - Math.PI / 2);
                    rearPos.X += rotationSpeed * elapsed * (float)Math.Cos(slope - Math.PI / 2);
                    frontPos.Y -= rotationSpeed * elapsed * (float)Math.Sin(slope + Math.PI / 2);
                    frontPos.X += rotationSpeed * elapsed * (float)Math.Cos(slope + Math.PI / 2);
                    if (frontWheel.Velocity != rearWheel.Velocity)
                    {
                        Vector2 avg = frontWheel.Velocity + rearWheel.Velocity;

                        avg /= 2;

                        frontWheel.Velocity = avg;
                        rearWheel.Velocity = avg;
                    }
                }
            }
            else
            {
                return;
            }
            rearWheel.Position = rearPos;
            frontWheel.Position = frontPos;

            if (kstate.IsKeyDown(Keys.Space))
            {
                fuel = 0;
            }

        }

        private void UpdateHitbox(GameTime gameTime)
        {
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float height = 50;
            h1 = rearWheel.Position;
            h2 = frontWheel.Position;
            h3 = frontWheel.Position;
            h4 = rearWheel.Position;

            h1.Y += rearWheel.Radius * (float)Math.Cos(slope);
            h1.X += rearWheel.Radius * (float)Math.Sin(slope);

            h2.Y += frontWheel.Radius * (float)Math.Cos(slope);
            h2.X += frontWheel.Radius * (float)Math.Sin(slope);

            h3.Y -= height * (float)Math.Cos(slope);
            h3.X -= height * (float)Math.Sin(slope);
            h4.Y -= height * (float)Math.Cos(slope);
            h4.X -= height * (float)Math.Sin(slope);



            //h1.X -= rearWheel.Radius;
            //h2.X += frontWheel.Radius;
        }

        public void Initialize()
        {
            frontWheel.Initialize();
            rearWheel.Initialize();
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            texture = contentManager.Load<Texture2D>("Textures/car");
            //driver = contentManager.Load<Texture2D>("Textures/driver");
            fuelTexture = contentManager.Load<Texture2D>("Textures/fuel");
            fuelBarTexture = new Texture2D(graphicsDevice, 1, 1);
            fuelBarTexture.SetData(new Color[] { Color.White });

            frontWheel = new Wheel(Map, this);
            rearWheel = new Wheel(Map, this);
            frontWheel.LoadContent(contentManager);
            rearWheel.LoadContent(contentManager);

            wheelBase = 102;
            fuel = 100;
            consumeFuel = false;
            fuelCapacity = 30;

            frontWheel.Position = new Vector2(120 + wheelBase, 50);
            rearWheel.Position = new Vector2(120, 50);

            isAlive = true;
            headRadius = 17.5f;
            deathReason = -1; // -1 => alive, 0 => driver down, 1 => out of fuel, 2 => quit
        }

        public void Update(GameTime gameTime)
        {
            frontWheel.Update(gameTime);
            rearWheel.Update(gameTime);

            HandleInput(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(consumeFuel)
            {
                if (fuel > 0)
                {
                    fuel -= elapsed * 2.5f;
                }
            }

            //Debug.WriteLine(fuel);

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

            //Debug.WriteLine(rearWheel.IsOnGround.ToString() + frontWheel.IsOnGround.ToString());

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

            UpdateHitbox(gameTime);

            map.Coins.HandleHitbox(h1, h2, h3, h4);

            if(map.Fuel.HandleHitbox(h1, h2, h3, h4))
            {
                fuel = 100;
            }

            head = rearWheel.Position;
            head.Y += 37 * (float)Math.Cos(slope + Math.PI / 2);
            head.X += 37 * (float)Math.Sin(slope + Math.PI / 2);

            head.X -= 65 * (float)Math.Sin(slope);
            head.Y -= 65 * (float)Math.Cos(slope);

            foreach(Segment segment in map.Segments)
            {
                float dist = segment.calculateDistance(head);
                if(dist < headRadius)
                {
                    isAlive = false;
                    deathReason = 0;
                }
            }

            Debug.WriteLine(rearWheel.Velocity);

            if(fuel <= 0)
            {
                if(rearWheel.Velocity.Y < 0.01 && rearWheel.Velocity.Y > -0.01 && rearWheel.Velocity.X < 0.01 && rearWheel.Velocity.X > -0.01)
                {
                    if (frontWheel.Velocity.Y < 0.01 && frontWheel.Velocity.Y > -0.01 && frontWheel.Velocity.X < 0.01 && frontWheel.Velocity.X > -0.01)
                    {
                        isAlive = false;
                        deathReason = 1;

                    }
                }
            }

        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, OrthographicCamera camera)
        {
            frontWheel.Draw(spriteBatch, gameTime);
            rearWheel.Draw(spriteBatch, gameTime);
            //spriteBatch.DrawLine(frontWheel.Position, rearWheel.Position, Color.Black, 2);

            //spriteBatch.Draw(driver, new Vector2(rearWheel.Position.X + 50, rearWheel.Position.Y - 20), null, Color.White, -slope, new Vector2(0, 0), 0.65f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(texture, new Vector2(rearWheel.Position.X, rearWheel.Position.Y), null, Color.White, -slope, new Vector2(50, 125), 0.65f, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(fuelBarTexture, new Rectangle(camera.ScreenToWorld(50, 8).ToPoint(), new Vector2(2 * (int)fuel, 35).ToPoint()), Color.LimeGreen);
            spriteBatch.Draw(fuelTexture, camera.ScreenToWorld(new Vector2(5, 8)), null, Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);

            spriteBatch.DrawLine(camera.ScreenToWorld(new Vector2(50, 8)), camera.ScreenToWorld(new Vector2(250, 8)), Color.Black, 3);
            spriteBatch.DrawLine(camera.ScreenToWorld(new Vector2(50, 43)), camera.ScreenToWorld(new Vector2(250, 43)), Color.Black, 3);
            spriteBatch.DrawLine(camera.ScreenToWorld(new Vector2(50, 6)), camera.ScreenToWorld(new Vector2(50, 44)), Color.Black, 3);
            spriteBatch.DrawLine(camera.ScreenToWorld(new Vector2(250, 6)), camera.ScreenToWorld(new Vector2(250, 44)), Color.Black, 3);

            //spriteBatch.DrawCircle(head, 17.5f, 20, Color.Red);
            //spriteBatch.DrawLine(h1, h2, Color.Red);
            //spriteBatch.DrawLine(h2, h3, Color.Red);
            //spriteBatch.DrawLine(h3, h4, Color.Red);
            //spriteBatch.DrawLine(h1, h4, Color.Red);
        }

    }
}
