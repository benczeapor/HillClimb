using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HillClimb
{
    internal class Coins
    {
        private List<Vector2> coins;
        private Texture2D texture;
        private Random random;
        private Map map;
        private int collected;
        private SpriteFont font;

        public Coins(Map map) 
        {
            this.map = map;
        }

        private bool Ccw(Vector2 a, Vector2 b, Vector2 c)
        {
            return (c.Y - a.Y) * (b.X - a.X) > (b.Y - a.Y) * (c.X - a.X);
        }

        private bool DoesIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            //return Math.Sign((p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X)) != Math.Sign((p2.X - p1.X) * (p4.Y - p1.Y) - (p2.Y - p1.Y) * (p4.X - p1.X));
            return Ccw(a, c, d) != Ccw(b, c, d) && Ccw(a, b, c) != Ccw(a, b, d);
        }

        public void HandleHitbox(Vector2 h1, Vector2 h2, Vector2 h3, Vector2 h4)
        {
            int sum = 0;
            for(int i=0; i<coins.Count; i++)
            {
                sum = 0;

                sum += DoesIntersect(h1, h2, coins[i], new Vector2(-100, -100)) ? 1 : 0;
                sum += DoesIntersect(h2, h3, coins[i], new Vector2(-100, -100)) ? 1 : 0;
                sum += DoesIntersect(h3, h4, coins[i], new Vector2(-100, -100)) ? 1 : 0;
                sum += DoesIntersect(h4, h1, coins[i], new Vector2(-100, -100)) ? 1 : 0;

                if(sum % 2 == 1)
                {
                    coins.RemoveAt(i);
                    collected ++;
                    //val ++;
                }

            }
            //return val;
        }

        private void GenerateCoins()
        {
            float nextBatch = random.Next(250, 500);
            float numbCoins = random.Next(1, 5);

            nextBatch += coins.Last().X;

            if(map.Segments.Last().X < nextBatch)
            {
                return;
            }

            for(int i=0; i<numbCoins; i++)
            {
                //float y = map.CoinHeight(nextBatch);
                Vector2 newPos = map.CoinPos(nextBatch);

                if (newPos.X != -69420)
                {
                    coins.Add(newPos);
                }
                nextBatch += 40;
            }

        }

        public void Initialize()
        {
            coins = new List<Vector2>();
            coins.Add(new Vector2(300, 350));
            //coins.Add(map.CoinPos(-50));
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("Textures/coin");
            random = new Random(13);
            collected = 0;
            font = contentManager.Load<SpriteFont>("Font");
        }
        public void Update(GameTime gameTime, OrthographicCamera camera)
        {
            if(camera.WorldToScreen(coins.Last()).X < 1280)
            {
                GenerateCoins();
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, OrthographicCamera camera)
        {
            foreach(Vector2 coin in coins)
            {
                spriteBatch.Draw(texture, coin - new Vector2(16, 16), null, Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
                //spriteBatch.DrawLine(coin, new Vector2(-100, -100), Color.Red);
            }
            spriteBatch.Draw(texture, camera.ScreenToWorld(new Vector2(5, 55)), null, Color.White, 0, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
            string collectedS = ": " + collected.ToString();
            spriteBatch.DrawString(font, collectedS, camera.ScreenToWorld(new Vector2(40, 52)), Color.Black, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0.5f);
        }
    }
}
