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
        //Not entirely sure if these are needed yet
        //Hopefully used
        /*SkeletonFrame skeletonFrame;
        ImageFrame depthFrame;
        ColorImageFrame colorImageFrame;*/

        Texture2D kinectCamera;
        byte[] kinectCameraByteArray;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
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

            //Just putting this here for now.
            /*if (colorImageFrame != null)
            {
                kinectCameraByteArray = new byte[colorImageFrame.PixelDataLength];
                colorImageFrame.CopyPixelDataTo(kinectCameraByteArray);
                kinectCamera.SetData(kinectCameraByteArray);
            }*/

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

            //Hopefully this is the right thing to do. Update all of these frame objects each update cycle.
            SkeletonFrame skeletonFrame = sensor.SkeletonStream.OpenNextFrame(0);
            ImageFrame depthFrame = sensor.DepthStream.OpenNextFrame(0);
            ColorImageFrame colorImageFrame = sensor.ColorStream.OpenNextFrame(0);

            /*skeletonFrame = sensor.SkeletonStream.OpenNextFrame(0);
            depthFrame = sensor.DepthStream.OpenNextFrame(0);
            colorImageFrame = sensor.ColorStream.OpenNextFrame(0);*/

            if (colorImageFrame != null)
            {
                kinectCameraByteArray = new byte[colorImageFrame.PixelDataLength];
                colorImageFrame.CopyPixelDataTo(kinectCameraByteArray);
                kinectCamera.SetData(kinectCameraByteArray);
                //kinectCamera
            }

            //Assumed that this all needs to be at the end
            colorImageFrame.Dispose();
            depthFrame.Dispose();
            skeletonFrame.Dispose();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
            spriteBatch.Draw(kinectCamera, new Rectangle(0, 0, 640, 480), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
