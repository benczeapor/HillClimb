using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;



//using FarseerPhysics.Dynamics;
//using FarseerPhysics.Factories;
//using FarseerPhysics.Collision;

using System;
using System.Data;
using System.Diagnostics;

namespace HillClimb
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private OrthographicCamera camera;
        private Map map;

        //private World world;
        //private Body body;
        //private Body ground;
        const float unitToPixel = 100.0f;
        const float pixelToUnit = 1 / unitToPixel;
        private Texture2D texture;
        private Texture2D groundTexture;
        private int fps;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            map = new Map();


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            base.Initialize();

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 720);
            camera = new OrthographicCamera(viewportAdapter);


            map.Initialize();
            map.Camera = camera;
        }

        protected override void LoadContent()
        {
            //world = new World(new Vector2(0, 9.8f));

            //Vector2 size = new Vector2(50, 50);
            //body = BodyFactory.CreateCircle(world, size.X * pixelToUnit, size.Y * pixelToUnit, 1);
            //body.BodyType = BodyType.Dynamic;
            //body.Position = new Vector2((GraphicsDevice.Viewport.Width / 2.0f) * pixelToUnit, 0);


            //ground = BodyFactory.CreateRectangle(world, 600 * pixelToUnit, 20 * pixelToUnit, 1);
            //ground.BodyType = BodyType.Static;
            //ground.Position = new Vector2(0, 400 * pixelToUnit);

            //texture = Content.Load<Texture2D>("Textures/ball");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            //groundTexture = new Texture2D(GraphicsDevice, 1, 1);
            //Color[] color = { Color.White };
            //groundTexture.SetData<Color>(color);


            map.LoadContent(GraphicsDevice, spriteBatch, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
            fps = (int)(1 / gameTime.ElapsedGameTime.TotalSeconds);


            map.Update(gameTime, camera);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            var transformMatrix = camera.GetViewMatrix();

            

            spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: transformMatrix);

            map.Draw(spriteBatch, gameTime);

            //string output = "alma";
            string fpsS = fps.ToString();
            spriteBatch.DrawString(font, fpsS, camera.ScreenToWorld(new Vector2(1280 - (font.MeasureString(fpsS).X * 2), 0)), Color.Red, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            string distanceS = map.Distance.ToString();
            spriteBatch.DrawString(font, distanceS, camera.ScreenToWorld(new Vector2(5, 0)), Color.Red, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0.5f);

            //Vector2 scale = new Vector2(50 / (float)texture.Width, 50 / (float)texture.Height);

            //spriteBatch.Draw(texture, body.Position * unitToPixel, null, Color.White, body.Rotation, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), scale, SpriteEffects.None, 0);
            //spriteBatch.Draw(groundTexture, ground.Position * unitToPixel, null, Color.Black, ground.Rotation, new Vector2(0, 0), new Vector2(600, 20), SpriteEffects.None, 0);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
