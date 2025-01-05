using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;



//using FarseerPhysics.Dynamics;
//using FarseerPhysics.Factories;
//using FarseerPhysics.Collision;

using System;
using System.ComponentModel;
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
        private Texture2D titleScreen;
        private int fps;

        private int state; // 0 => menu, 1 => game, 2 => game over

        private bool gameOver;

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

            gameOver = false;

            titleScreen = Content.Load<Texture2D>("Images/title");
            //groundTexture = new Texture2D(GraphicsDevice, 1, 1);
            //Color[] color = { Color.White };
            //groundTexture.SetData<Color>(color);


            map.LoadContent(GraphicsDevice, spriteBatch, Content);
            state = 0;
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            base.Update(gameTime);
            fps = (int)(1 / gameTime.ElapsedGameTime.TotalSeconds);

            if(state == 1)
            {
                if (map.Vehicle.IsAlive)
                {
                    map.Update(gameTime, camera);
                }
                else
                {
                    gameOver = true;
                    state = 2;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            var transformMatrix = camera.GetViewMatrix();

            

            spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: transformMatrix);

            if(state == 0)
            {
                spriteBatch.Draw(titleScreen, new Rectangle(0, 0, 1280, 720), Color.White);
            }

            else if (state == 1)
            {

                map.Draw(spriteBatch, gameTime);

                string fpsS = fps.ToString();
                spriteBatch.DrawString(font, fpsS, camera.ScreenToWorld(new Vector2(1280 - (font.MeasureString(fpsS).X * 2), 0)), Color.Red, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
                string distanceS = "Distance: " + map.Distance.ToString();
                spriteBatch.DrawString(font, distanceS, camera.ScreenToWorld(new Vector2(5, 100)), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0.5f);

                if (gameOver)
                {
                    string outS = "Game Over";

                    switch (map.Vehicle.DeathReason)
                    {
                        case 0:
                            outS += "\nDriver Down";
                            break;
                        case 1:
                            outS += "\nOut of Fuel";
                            break;
                        default:
                            break;
                    }

                    spriteBatch.DrawString(font, outS, camera.ScreenToWorld(new Vector2(500, 200)), Color.Red);
                }
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
