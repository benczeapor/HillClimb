using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

//using nkast.Aether.Physics2D.Dynamics;

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

        private float rotationSpeed;
        private float wheelRadius;
        private float wheelRotation;
        private bool isDrive;

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
            List<Vector4> platforms = map.Platforms;

            foreach(Vector4 platform in platforms)
            {

                float distance = (float)(Math.Abs((platform.Z - platform.X) * (position.Y - platform.Y) - (platform.W - platform.Y) * (position.X - platform.X)) / Math.Sqrt((platform.Z - platform.X) * (platform.Z - platform.X) + (platform.W - platform.Y) * (platform.W - platform.Y)));
                float nextDistance = (float)(Math.Abs((platform.Z - platform.X) * ((position.Y + velocity.Y * elapsed) - platform.Y) - (platform.W - platform.Y) * ((position.X + velocity.X * elapsed) - platform.X)) / Math.Sqrt((platform.Z - platform.X) * (platform.Z - platform.X) + (platform.W - platform.Y) * (platform.W - platform.Y)));

                //if(minD > distance)
                //{
                //    minD = distance;
                //}

                //Debug.Write(distance);
                //Debug.Write(", ");
                //Debug.Write(position.X);
                //Debug.Write(", ");
                //Debug.WriteLine(position.Y);

                if (distance <= touchThreshold + wheelRadius)
                {
                    isOnGround = true;
                }
                else
                {
                    isOnGround = false;
                }

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
            if (position.X - wheelRadius <= 0)
            {
                velocity.X *= -1;
            }
            if (position.X + wheelRadius >= 800)
            {
                velocity.X *= -1;
            }
        }

        public void applyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.Y += gravity * elapsed;

            if(isOnGround)
            {
                //position.X += rotationSpeed * wheelRadius * elapsed;
                velocity.X = rotationSpeed * wheelRadius;
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

            spriteBatch.End();
        }
    }
}
