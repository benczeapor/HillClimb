using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using MonoGame.Extended;

using FarseerPhysics.Dynamics;

using System;
using System.Diagnostics;
using System.Collections.Generic;
using MonoGame.Extended.Graphics;
//using System.Drawing;

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

        private Segment currentSegment;
        private Segment nextSegment;

        private Vector2 projection;

        private bool willPhaseTrough;

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

        public float Radius
        {
            get { return wheelRadius; }
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

        public void correctPosition()
        {
            Vector2 perpendicular;

            slope = Math.Abs(slope);

            if(currentSegment == null)
            {
                return;
            }

            if(currentSegment.Y > currentSegment.W)
            {
                //Debug.WriteLine("1");
                perpendicular.X = - (float)Math.Abs(Math.Cos(slope + (Math.PI / 2)));
                perpendicular.Y = - (float)Math.Abs(Math.Sin(slope + (Math.PI / 2)));
            }
            else
            {
                //Debug.WriteLine("2");
                perpendicular.X = (float)Math.Cos(slope - (Math.PI / 2));
                perpendicular.Y = (float)Math.Sin(slope - (Math.PI / 2));
            }

            

            
            //currentSegment.Color = Color.White;

            

            float error = wheelRadius - currentSegment.calculateDistance(position);

            if (error > 0)
            {
                position += perpendicular * error;
            }

            //Debug.WriteLine(distance);

            //projection.X = position.X + 100 * (float)Math.Cos(slope + Math.PI / 2);
            //projection.Y = position.Y + 100 * (float)Math.Sin(slope + Math.PI / 2);
        }

        public void handleCollision(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;


            //Rectangle rect = map.Rect;
            List<Segment> segments = map.Segments;
            float minDist = -1;
            foreach (Segment segment in segments)
            {
                float distance = segment.calculateDistance(position);

                if(distance > 4 * wheelRadius)
                {
                    continue;
                }

                if(minDist == -1 || minDist > distance)
                {
                    minDist = distance;
                    currentSegment = segment;
                    slope = segment.Slope;
                }
            }

            if(minDist == -1)
            {
                isOnGround = false;
                currentSegment = null;
                return;
            }

            if (minDist <= wheelRadius + 2.5)
            {
                //Debug.WriteLine("fasz");
                isOnGround = true;

                //segment.Color = Color.Red;
            }
            else
            {
                isOnGround = false;
            }

            if (minDist <= wheelRadius)
            {
                velocity.Y = (float)(-velocity.Y * 0.5);
            }
        }

        public void applyPhysics(GameTime gameTime)
        {
            handleCollision(gameTime);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.Y += gravity * elapsed;

            if (isOnGround)
            {
                
                //position.X += rotationSpeed * wheelRadius * elapsed;

                if(slope <= 0)
                {
                    Debug.WriteLine("1");
                    velocity.X = rotationSpeed * wheelRadius * (float)Math.Abs(Math.Cos(slope));
                    velocity.Y = rotationSpeed * wheelRadius * (float)Math.Abs(Math.Sin(slope));
                }
                else
                {
                    Debug.WriteLine("2");
                    velocity.X = rotationSpeed * wheelRadius * (float)Math.Cos(-slope);
                    velocity.Y = rotationSpeed * wheelRadius * (float)Math.Sin(-slope);
                }
                

                


                //projection = position + 10 * velocity;
                //velocity.X = rotationSpeed * MathHelper.TwoPi * wheelRadius * elapsed;
            }
            else
            {
                //Debug.WriteLine("asder");
            }

            position += velocity * elapsed;
            correctPosition();

            //else
            //{
            //    rotationSpeed *= airDrag;
            //    velocity.X += rotationSpeed * (1 / airDrag) * elapsed;
            //}

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

            if (kstate.IsKeyDown(Keys.Space))
            {
                velocity = Vector2.Zero;
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
            //spriteBatch.DrawLine(currentSegment.X, currentSegment.Y, currentSegment.Z, currentSegment.W, Color.Blue);

            spriteBatch.End();
        }
    }
}
