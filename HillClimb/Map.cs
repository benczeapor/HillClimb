using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
//using FarseerPhysics.Dynamics;

using System;
//using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace HillClimb
{
    internal class Map
    {
        private float level;//
        private float distance;

        private Vehicle vehicle;
        private Texture2D texture;
        private Random random;
        private FastNoiseLite noise;


        private float first;
        private float second;
        private float firstHeight;
        private float secondHeight;

        private List<Segment> segments;
        public List<Segment> Segments
        {
            get { return segments; }
        }

        private Fuel fuel;
        public Fuel Fuel
        {
            get { return fuel; }
        }

        private Coins coins;
        public Coins Coins
        {
            get { return coins; }
        }

        private Wheel wheel;
        public Wheel Wheel
        {
            get { return wheel; }
        }

        public int Distance
        {
            get { return (int)distance; }
        }

        private OrthographicCamera camera;
        public OrthographicCamera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public Vehicle Vehicle
        { 
            get { return vehicle; } 
        }

        public Map()
        {

        }

        public Vector2 CoinPos(float x)
        {
            Segment above = new Segment(-1, -1, -1, -1);
            //Debug.WriteLine(x);
            foreach(Segment segment in Segments) 
            {
                if(x >= segment.X && x <= segment.Z)
                {
                    above = segment;
                    break;
                }
            }
            if(above.X == -1 && above.Y == -1)
            {
                return new Vector2(-69420, -69420);
            }
            //if(x == above.X)
            //{
            //    return above.Y - 50;
            //}
            //if(x == above.Z)
            //{
            //    return above.W - 50;
            //}

            Vector2 result = new Vector2(x, ((above.Y * (x - above.X) + above.W * (above.Z - x)) / (above.Z - above.X)));

            result.X -= 30 * (float)Math.Sin(above.Slope);
            result.Y -= 30 * (float)Math.Cos(above.Slope);

            return result;

            //return  - 50;

        }

        public void Initialize()
        {
            //wheel.Initialize();
            vehicle.Initialize();
            coins.Initialize();
            fuel.Initialize();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch , ContentManager contentManager)
        {
            

            random = new Random(69420);
            noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.Value);
            noise.SetFrequency(0.0025f);

            texture = new Texture2D(graphicsDevice, 1, 1);
            Color[] color = {Color.White};
            texture.SetData<Color>(color);

            segments = new List<Segment>();

            level = 50;

            segments.Add(new Segment(-200, 300, 0, 400));
            segments.Add(new Segment(0, 400, 400, 400));
            //segments.Add(new Segment(400, 400, 800, 300));

            coins = new Coins(this);
            coins.LoadContent(contentManager);

            fuel = new Fuel(this);
            fuel.LoadContent(contentManager);

            vehicle = new Vehicle();
            vehicle.Map = this;
            vehicle.LoadContent(contentManager, graphicsDevice);

            first = 400;
            second = 400;
            firstHeight = 400;
            secondHeight = 400;
        }

        public void Update(GameTime gameTime, OrthographicCamera camera)
        {
            
            //wheel.Update(gameTime);

            //segments.Sort((x, y) => y.Distance.CompareTo(x.Distance));
            //MouseState ms = Mouse.GetState();

            //Debug.WriteLine(segments[0].calculateDistance(new Vector2(ms.X, ms.Y)));
            vehicle.Update(gameTime);

            Vector2 worldPosition = (vehicle.FrontWheel.Position + vehicle.RearWheel.Position) / 2;

            Vector2 position = camera.WorldToScreen(worldPosition);

            distance = (worldPosition.X - 171) / 75;

            if(position.X > 400)
            {
                camera.Move(new Vector2(position.X - 400, 0));
            }
            if(position.X < 200)
            {
                camera.Move(new Vector2(position.X - 200, 0));
            }

            if (position.Y > 600)
            {
                camera.Move(new Vector2(0, position.Y - 600));
            }
            if(position.Y < 200)
            {
                camera.Move(new Vector2(0, position.Y - 200));
            }

            //Debug.WriteLine(camera.ScreenToWorld(new Vector2(0, 0)).X);

            //noise.GetNoise();

            //Vector2 lastSeg = segments.Last().P2;
            Segment lastSeg = segments.Last();
            Vector2 lastPoint = lastSeg.P2;

            while(camera.WorldToScreen(lastPoint).X < 1280)
            {
                float x = lastPoint.X + random.Next(20, 40);
                float y = (noise.GetNoise(x, 0) * 200) + 530;
                segments.Add(new Segment(lastPoint, new Vector2(x, y)));
                lastSeg = segments.Last();
                lastPoint = lastSeg.P2;
            }

            //Debug.WriteLine(segments.Count);

            if(segments.Count > 100)
            {
                segments.RemoveAt(0);
            }
            coins.Update(gameTime, camera);
            fuel.Update(gameTime, camera);

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //wheel.Draw(spriteBatch, gameTime);

            //spriteBatch.Begin();

            //spriteBatch.Draw(texture, new Rectangle(0, (int)(480 - level), 800, (int)level), Color.Black);

            

            vehicle.Draw(spriteBatch, gameTime, camera);

            foreach (Segment segment in segments)
            {
                segment.Draw(spriteBatch, gameTime, texture);    
            }

            coins.Draw(spriteBatch, gameTime, camera);
            fuel.Draw(spriteBatch);

            //spriteBatch.Draw(texture, new Rectangle(49, (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Yellow);
            //spriteBatch.Draw(texture, new Rectangle(49 + (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Magenta);
            //spriteBatch.Draw(texture, new Rectangle(49 + 2 * (int)(MathHelper.TwoPi * 25), (int)(480 - level), (int)(MathHelper.TwoPi * 25), 10), Color.Green);


            //spriteBatch.End();
        }
    }
}
