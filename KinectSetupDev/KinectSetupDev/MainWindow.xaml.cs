using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect; //I added this manually.
using Microsoft.Kinect.Toolkit; //Needed to reference this first

namespace KinectSetupDev
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private KinectSensorChooser _sensorChooser = new KinectSensorChooser();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensorChooser.KinectChanged += new EventHandler<KinectChangedEventArgs>(_sensorChooser_KinectChanged);
            _sensorChooser.Start();
        }

        void _sensorChooser_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            //throw new NotImplementedException();

            KinectSensor oldSensor = e.OldSensor;

            StopKinect(oldSensor);

            KinectSensor newSensor = e.NewSensor;

            newSensor.ColorStream.Enable();
            newSensor.DepthStream.Enable();
            newSensor.SkeletonStream.Enable();
            //newSensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(_sensor_AllFramesReady);
            try
            {
                newSensor.Start();
            }
            catch (System.IO.IOException)
            {
                _sensorChooser.TryResolveConflict();
            }
        }

        void _sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            //throw new NotImplementedException();

            /*using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame == null) return;

                byte[] pixels = new byte[colorFrame.PixelDataLength];

                colorFrame.CopyPixelDataTo(pixels);

                int stride = colorFrame.Width * 4;
                image1.Source = BitmapSource.Create(colorFrame.Width, colorFrame.Height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
            }*/
        }

        void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                sensor.Stop();
                sensor.AudioSource.Stop();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopKinect(_sensorChooser.Kinect);
        }
    }
}
