#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Kinect;
#endregion

namespace MGSimonKinect
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Probably need to do checking to make sure one is connected/update this when it is reconnected.
        KinectSensor sensor;

        public enum GameState { StartMenu, HowToPlayMenu, OptionsMenu, Loading, PauseMenu, CreditsMenu, Playing }
        GameState currentGameState;
        GameState prevGameState; //May not need this one
        public GameState gameState
        {
            get { return currentGameState; }
            set { prevGameState = currentGameState; currentGameState = value; }
        }

        // Menus.
        StartMenu startMenu;

        Texture2D kinectCamera;
        byte[] kinectCameraByteArray;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            sensor = KinectSensor.KinectSensors[0];
            sensor.Start();
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.DepthStream.Enable();
            sensor.SkeletonStream.Enable();

            kinectCamera = new Texture2D(GraphicsDevice, 640, 480);

            gameState = GameState.StartMenu;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            startMenu = new StartMenu(this, GraphicsDevice.Viewport.Bounds, Content.Load<Texture2D>(@"Images\Menus\StartMenu\QuitButton"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Update all of these frame objects each update cycle.
            /*SkeletonFrame skeletonFrame = sensor.SkeletonStream.OpenNextFrame(0);
            ImageFrame depthFrame = sensor.DepthStream.OpenNextFrame(0);*/
            ColorImageFrame colorImageFrame = sensor.ColorStream.OpenNextFrame(0);

            if (colorImageFrame != null)
            {
                kinectCameraByteArray = new byte[colorImageFrame.PixelDataLength];
                colorImageFrame.CopyPixelDataTo(kinectCameraByteArray);
                //Convert RGBA to BGRA
                Byte[] bgraPixelData = new Byte[colorImageFrame.PixelDataLength];
                for (int i = 0; i < kinectCameraByteArray.Length; i += 4)
                {
                    bgraPixelData[i] = kinectCameraByteArray[i + 2];
                    bgraPixelData[i + 1] = kinectCameraByteArray[i + 1];
                    bgraPixelData[i + 2] = kinectCameraByteArray[i];
                    bgraPixelData[i + 3] = (Byte)255; //The video comes with 0 alpha so it is transparent
                }

                kinectCamera.SetData(bgraPixelData);
                colorImageFrame.Dispose();
            }

            switch (gameState)
            {
                case GameState.StartMenu:
                    startMenu.Update(gameTime);
                    break;
            }

            //Assumed that this all needs to be at the end
            //colorImageFrame.Dispose();
            //depthFrame.Dispose();
            //skeletonFrame.Dispose();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
            //spriteBatch.Draw(kinectCamera, new Rectangle(0, 0, 640, 480), Color.White);
            spriteBatch.Draw(kinectCamera, Vector2.Zero, new Rectangle(0, 0, kinectCamera.Width, kinectCamera.Height), Color.White, 0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0);
            spriteBatch.End();

            switch (gameState)
            {
                case GameState.StartMenu:
                    startMenu.Draw(gameTime, spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
