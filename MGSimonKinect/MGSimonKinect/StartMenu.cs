using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework; // Rectangle
using Microsoft.Xna.Framework.Graphics; // Texture2D
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio; // MouseState

namespace MGSimonKinect
{
	class StartMenu
	{
        Vector2 quitButtonPos;
        Texture2D quitButtonSprite;
        Rectangle quitButtonRect;

        Game1 game;
        MouseState mouseState;
        MouseState previousMouseState;
        Vector3 rightHandState;
        Vector3 previousRightHandState;

        public StartMenu(Game1 game, Rectangle gameWindow, Texture2D quitButtonSprite)
        {
            this.game = game;
            this.quitButtonSprite = quitButtonSprite;
            quitButtonPos = new Vector2(gameWindow.Width / 2 - quitButtonSprite.Width / 2, gameWindow.Height / 2 - quitButtonSprite.Height / 2 + 125);
            quitButtonRect = new Rectangle((int)quitButtonPos.X, (int)quitButtonPos.Y, quitButtonSprite.Width, quitButtonSprite.Height);
        }

        // Help for this method from: http://www.spikie.be/blog/page/Building-a-main-menu-and-loading-screens-in-XNA.aspx
        private void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 1, 1);

            if (mouseClickRect.Intersects(quitButtonRect)) game.Exit();
        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            rightHandState = game.GetRightHandState();

            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            //If Kinect hand made a click gesture
            //.00847
            if (-(rightHandState.Z - previousRightHandState.Z) > 0.1)
            {
                MouseClicked((int)game.rightHandVector3.X, (int)game.rightHandVector3.Y);
            }

            previousMouseState = mouseState;
            previousRightHandState = rightHandState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
            spriteBatch.Draw(quitButtonSprite, quitButtonPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
	}
}