using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using RenderingLibrary.Graphics;



namespace HillClimb
{
    internal class Button
    {
        private string text;
        private Vector2 position;
        private Vector2 drawPosition;
        private Texture2D texture;
        private int width = 120;
        private int height = 40;
        private Vector2 textPos;
        private Vector2 textDrawPos;
        private SpriteFont font;

        //private OrthographicCamera camera;

        private bool pressed;
        public bool Pressed
        {
            get { return pressed; }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        
        //public OrthographicCamera Camera
        //{
        //    get { return camera; }
        //    set { camera = value; }
        //}

        public Button(Vector2 position, string text)
        {
            this.text = text;
            this.position = position;
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice, OrthographicCamera camera)
        {
            font = contentManager.Load<SpriteFont>("Font");
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] {Color.White});
            drawPosition = new Vector2(0, 0);

            Vector2 textSize = font.MeasureString(text);

            textPos.X = position.X + textSize.X / 2;
            textPos.Y = position.Y;

            //textPos = textPos;

            pressed = false;
            isActive = false;

        }

        public void Update(GameTime gameTime, OrthographicCamera camera)
        {
            drawPosition = camera.ScreenToWorld(position);
            textDrawPos = camera.ScreenToWorld(textPos);
            pressed = false;

            MouseState ms = Mouse.GetState();

            Rectangle rect = new Rectangle((int)position.X, (int)position.Y, width, height);

            if(ms.LeftButton == ButtonState.Pressed && rect.Contains(ms.Position))
            {
                drawPosition -= new Vector2(1.5f, 1.5f);
                textDrawPos -= new Vector2(1.5f, 1.5f);
                pressed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)drawPosition.X - 5, (int)drawPosition.Y - 5, width + 10, height + 10), null, Color.Black, 0, new Vector2(0, 0), SpriteEffects.None, 0.1f);
            spriteBatch.Draw(texture, new Rectangle((int)drawPosition.X, (int)drawPosition.Y, width, height), null, Color.AliceBlue, 0, new Vector2(0, 0), SpriteEffects.None, 0.1f);
            spriteBatch.DrawString(font, text, textDrawPos, Color.Black);
        }
    }
}
