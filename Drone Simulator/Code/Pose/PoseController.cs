using System;
using Xamarin.Essentials;

namespace Drone_Simulator.Pose
{
    public class PoseController
    {
        private readonly SensorSpeed _speed = SensorSpeed.UI;
        private readonly PoseJavaScriptInterface _javaScriptInterface;
        private readonly PoseSocketDecorator _socketDecorator;

        public PoseController(PoseJavaScriptInterface javaScriptInterface, PoseSocketDecorator socketDecorator)
        {
            _javaScriptInterface = javaScriptInterface;
            _socketDecorator = socketDecorator;
            _socketDecorator.PoseReceived += SendPoseToWebView;

            OrientationSensor.ReadingChanged += SendOrientation;
        }

        private void SendPoseToWebView(Pose pose)
        {
            _javaScriptInterface.ReceivePose(pose);
        }

        private void SendOrientation(object sender, OrientationSensorChangedEventArgs e)
        {
            OrientationSensorData data = e.Reading;
            Console.WriteLine(
                $"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}, W: {data.Orientation.W}");

            _socketDecorator.SendPose(new Pose(data.Orientation.X, data.Orientation.Y, data.Orientation.Z));
            // Process Orientation quaternion (X, Y, Z, and W)
        }

        public void ToggleOrientationSensor()
        {
            try
            {
                if (OrientationSensor.IsMonitoring)
                    OrientationSensor.Stop();
                else
                    OrientationSensor.Start(_speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}