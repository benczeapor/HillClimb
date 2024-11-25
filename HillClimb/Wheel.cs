using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using MonoGame.Extended;

using FarseerPhysics.Dynamics;

using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace HillClimb
{
    internal class Wheel
    {
        const float gravity = 250;
        const float touchThreshold = 3;
        const float maxRotationSpeed = 25;
        const float rotationAcceleration = 20;
        const float groundDrag = 0.5f;
        const float airDrag = 0.99f;

        float minD = 10000;

        private Texture2D texture;

        private float slope;

        private float rotationSpeed;
        private float wheelRadius;
        private float wheelRotation;
        private bool isDrive;

        private Vector2 projection;

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            //set { position = value; }
        }

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
        }

        private Map map;
        public Map Map
        {
            get { return map; }
            set { map = value; }
        }

        private bool isOnGround;
        public bool IsOnGround
        {
            get { return isOnGround; }
        }

        public Wheel(Map map)
        {
            Map = map;
        }

        public void handleCollision(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            //Rectangle rect = map.Rect;
            List<Segment> segments = map.Segments;

            foreach(Segment segment in segments)
            {

                if (position.Y > segment.Y && position.Y > segment.W)
                {
                    continue;
                }

                float d1 = (float)Math.Sqrt((position.X - segment.X) * (position.X - segment.X) + (position.Y - segment.Y) * (position.Y - segment.Y));
                float d2 = (float)Math.Sqrt((position.X - segment.Z) * (position.X - segment.Z) + (position.Y - segment.W) * (position.Y - segment.W));
                float d3 = (float)Math.Sqrt((segment.X - segment.Z) * (segment.X - segment.Z) + (segment.Y - segment.W) * (segment.Y - segment.W));

                float a1 = (float)Math.Acos((d1*d1 + d3*d3 - d2*d2) / (2 * d1 * d3));
                float a2 = (float)Math.Acos((d2*d2 + d3*d3 - d1*d1) / (2 * d2 * d3));

                if(a1 > Math.PI / 2 || a2 > Math.PI / 2 && velocity.X == 0) // if it doesn't intersect
                {
                    continue;
                }

                slope = (float)Math.Asin(Math.Abs(segment.Y - segment.W) / d3);

                //Debug.WriteLine(slope.ToString());

                //float distance = (float)(Math.Abs((segment.Z - segment.X) * (position.Y - segment.Y) - (segment.W - segment.Y) * (position.X - segment.X)) / Math.Sqrt((segment.Z - segment.X) * (segment.Z - segment.X) + (segment.W - segment.Y) * (segment.W - segment.Y)));
                //float nextDistance = (float)(Math.Abs((segment.Z - segment.X) * ((position.Y + velocity.Y * elapsed) - segment.Y) - (segment.W - segment.Y) * ((position.X + velocity.X * elapsed) - segment.X)) / Math.Sqrt((segment.Z - segment.X) * (segment.Z - segment.X) + (segment.W - segment.Y) * (segment.W - segment.Y)));

                float distance = segment.calculateDistance(position);
                float nextDistance = segment.calculateDistance(position + velocity * elapsed);

                //if(distance > 2 * wheelRadius)
                //{
                //    continue;
                //}

                float h = (float)(Math.Sin(a1) * d1 + Math.Sin(a2) * d2) / 2;

                float h1 = (float)Math.Sqrt(d1*d1 - h*h);
                float h2 = (float)Math.Sqrt(d2*d2 - h * h);

                projection.X = position.X + (100 * (float)Math.Cos(slope));
                projection.Y = position.Y + (100 * (float)Math.Sin(slope));

                //projection.X = (platform.X * h1 + platform.Z * h2) / (h1 + h2);
                //projection.Y = (platform.Y * h1 + platform.W * h2) / (h1 + h2);

                //projection.X = (platform.X + platform.Z) / 2;
                //projection.Y = (platform.Y + platform.W) / 2;

                //if(d1 > d3 || d2 > d3)
                //{
                //    distance = -1;
                //}

                //if(minD > distance)
                //{
                //    minD = distance;
                //}

                Debug.Write(h1);
                Debug.Write(", ");
                Debug.Write(h2);
                //Debug.Write("\n");
                Debug.Write(", ");
                Debug.WriteLine(d3);

                if (distance <= touchThreshold + wheelRadius)
                {
                    isOnGround = true;
                }
                else
                {
                    isOnGround = false;
                }
                //if()
                if (distance <= wheelRadius)
                {
                    velocity.Y = (float)(-velocity.Y * 0.5);
                    //velocity.Y = 0;
                }
                else if (nextDistance <= wheelRadius)
                {
                    //velocity.Y = 0;
                    velocity.Y = (float)(-velocity.Y * 0.5);
                    //position.Y = rect.Y - (wheelRadius);
                }
            }

            //temp
            //if (position.X - wheelRadius <= 0)
            //{
            //    velocity.X *= -1;
            //}
            //if (position.X + wheelRadius >= 800)
            //{
            //    velocity.X *= -1;
            //}
        }

        public void applyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.Y += gravity * elapsed;

            if(isOnGround)
            {
                //position.X += rotationSpeed * wheelRadius * elapsed;
                velocity.X = rotationSpeed * wheelRadius * (float)Math.Cos(slope);
                velocity.Y = rotationSpeed * wheelRadius * (float)Math.Sin(slope);
                //velocity.X = rotationSpeed * MathHelper.TwoPi * wheelRadius * elapsed;
            }
            //else
            //{
            //    rotationSpeed *= airDrag;
            //    velocity.X += rotationSpeed * (1 / airDrag) * elapsed;
            //}

            handleCollision(gameTime);
            position += velocity * elapsed;
            //position += velocity;

        }

        public void handleInput(GameTime gameTime)
        {
            //float updatedWheelSpeed = wheelSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float force = 100;

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                //velocity.Y -= force;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                //velocity.Y += force;
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                //velocity.X -= force;



                rotationSpeed -= rotationAcceleration * elapsed;
                if (rotationSpeed < -maxRotationSpeed)
                {
                    rotationSpeed = -maxRotationSpeed;
                }
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                //velocity.X += force;
                //rotationSpeed = 2;

                rotationSpeed += rotationAcceleration * elapsed;
                if (rotationSpeed > maxRotationSpeed)
                {
                    rotationSpeed = maxRotationSpeed;
                }
            }
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {
            position = new Vector2(50, 50);
            wheelRadius = 25;

            texture = content.Load<Texture2D>("Textures/ball");
            wheelRotation = 0;
            rotationSpeed = 0;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Debug.WriteLine(isOnGround);
            handleInput(gameTime);
            applyPhysics(gameTime);

            wheelRotation = (wheelRotation + rotationSpeed * elapsed) % MathHelper.TwoPi;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();

            //spriteBatch.Draw(texture, Vector2.Subtract(position, new Vector2(wheelRadius, wheelRadius)), Color.White);
            //spriteBatch.Draw(texture, Vector2.Subtract(position, new Vector2(wheelRadius, wheelRadius)), null, Color.White, wheelRotation, new Vector2(wheelRadius, wheelRadius), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, position, null, Color.White, wheelRotation, new Vector2(wheelRadius, wheelRadius), 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawLine(projection, position, Color.Red);

            spriteBatch.End();
        }
    }
}
