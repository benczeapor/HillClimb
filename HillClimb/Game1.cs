using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System.Data;


//using FarseerPhysics.Dynamics;
//using FarseerPhysics.Factories;
//using FarseerPhysics.Collision;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;

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
        private bool isPressed;

        private Button playButton;
        private Button restartButton;
        private Button resumeButton;
        private Button exitButton1;
        private Button exitButton2;
        private Button exitButton3;

        private string playerName;

        private int state; // 0 => menu, 1 => game, 2 => pause, 3 => game over

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
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 720);
            camera = new OrthographicCamera(viewportAdapter);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");

            gameOver = false;

            titleScreen = Content.Load<Texture2D>("Images/title");

            playButton = new Button(new Vector2(900, 200), "Play");
            //playButton.Camera = camera;
            playButton.LoadContent(Content, GraphicsDevice, camera);
            

            playButton.IsActive = true;

            restartButton = new Button(new Vector2(570, 310), "Restart");
            //restartButton.Camera = camera;
            restartButton.LoadContent(Content, GraphicsDevice, camera);

            resumeButton = new Button(new Vector2(570, 250), "Resume");
            resumeButton.LoadContent(Content, GraphicsDevice, camera);

            exitButton2 = new Button(new Vector2(570, 370), "Exit");
            exitButton2.LoadContent(Content, GraphicsDevice, camera);

            exitButton1 = new Button(new Vector2(900, 260), "Exit");
            exitButton1.LoadContent(Content, GraphicsDevice, camera);

            exitButton3 = new Button(new Vector2(570, 370), "Exit");
            exitButton3.LoadContent(Content, GraphicsDevice, camera);
            //groundTexture = new Texture2D(GraphicsDevice, 1, 1);
            //Color[] color = { Color.White };
            //groundTexture.SetData<Color>(color);
            isPressed = false;

            map.LoadContent(GraphicsDevice, spriteBatch, Content);
            playerName = "";


            //state = 3;
            //gameOver = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !gameOver)
            {
                if (!isPressed)
                {
                    if (state != 2 && state != 0)
                    {
                        state = 2;
                    }
                    else if(state != 0)
                    {
                        state = 1;
                    }
                }
                isPressed = true;
            }

            if(Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                isPressed = false;
            }

            base.Update(gameTime);
            fps = (int)(1 / gameTime.ElapsedGameTime.TotalSeconds);

            if(state == 0)
            {
                playButton.Update(gameTime, camera);
                exitButton1.Update(gameTime, camera);
                if(playButton.Pressed)
                {
                    state = 1;
                    playButton.IsActive = false;
                }
                if(exitButton1.Pressed)
                {
                    Exit();
                }
            }
            if(state == 1)
            {
                if (map.Vehicle.IsAlive)
                {
                    map.Update(gameTime, camera);
                }
                else
                {
                    gameOver = true;
                    state = 3;
                }
            }
            if(state == 2)
            {
                //restartButton.IsActive = true;
                restartButton.Update(gameTime, camera);
                resumeButton.Update(gameTime, camera);
                exitButton2.Update(gameTime, camera);
                if (restartButton.Pressed)
                {
                    map = new Map();
                    map.Camera = camera;

                    map.LoadContent(GraphicsDevice, spriteBatch, Content);
                    map.Initialize();
                    state = 1;
                    gameOver = false;
                }
                if(resumeButton.Pressed)
                {
                    state = 1;
                }
                if (exitButton2.Pressed)
                {
                    state = 0;
                    map = new Map();
                    map.Camera = camera;

                    map.LoadContent(GraphicsDevice, spriteBatch, Content);
                    map.Initialize();
                    gameOver = false;
                }

            }
            if (state == 3)
            {
                restartButton.Update(gameTime, camera);
                exitButton3.Update(gameTime, camera);
                if (restartButton.Pressed)
                {
                    map = new Map();
                    map.Camera = camera;

                    map.LoadContent(GraphicsDevice, spriteBatch, Content);
                    map.Initialize();
                    state = 1;
                    gameOver = false;
                }
                if(exitButton3.Pressed)
                {
                    state = 0;
                    map = new Map();
                    map.Camera = camera;

                    map.LoadContent(GraphicsDevice, spriteBatch, Content);
                    map.Initialize();
                    gameOver = false;
                }
                foreach(Keys key in Keyboard.GetState().GetPressedKeys())
                {
                    if((int)key >= 65 && (int) key <= 90 || (int)key >= 97 && (int)key <= 122 || (int)key >= 48 && (int)key <= 57)
                    {
                        playerName += (char)key;
                    }
                    if(key == Keys.Enter)
                    {
                        Debug.WriteLine(playerName);
                    }
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
                spriteBatch.Draw(titleScreen, new Rectangle((int)camera.ScreenToWorld(new Vector2(0, 0)).X, (int)camera.ScreenToWorld(new Vector2(0, 0)).Y, 1280, 720), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                playButton.Draw(spriteBatch);
                exitButton1.Draw(spriteBatch);
            }
            else
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
                    restartButton.Draw(spriteBatch);
                    exitButton3.Draw(spriteBatch);
                }
                if(state == 2)
                {
                    spriteBatch.DrawString(font, "Paused", camera.ScreenToWorld(new Vector2(580, 200)), Color.Red);
                    restartButton.Draw(spriteBatch);
                    resumeButton.Draw(spriteBatch);
                    exitButton2.Draw(spriteBatch);
                }
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
