using System;
using Drone_Simulator.Extensions;
using Xamarin.Essentials;

namespace Drone_Simulator.Pose
{
    // TODO: Rename class to more meaningful name.
    public class PoseController
    {
        private readonly SensorSpeed _speed = SensorSpeed.UI;
        private readonly PoseJavaScriptInterface _javaScriptInterface;
        private readonly PoseSocketDecorator _socketDecorator;

        public PoseController(PoseJavaScriptInterface javaScriptInterface, PoseSocketDecorator socketDecorator)
        {
            _javaScriptInterface = javaScriptInterface;
            _socketDecorator = socketDecorator;

            Log.Debug(OrientationSensor.IsMonitoring);
        }

        public void StartCommunication()
        {
            _socketDecorator.PoseReceived += ReceivePoseByWebView;
            OrientationSensor.ReadingChanged += SendPose;
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

        private void SendPose(object sender, OrientationSensorChangedEventArgs e)
        {
            OrientationSensorData data = e.Reading;
            Log.Debug(
                $"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}, W: {data.Orientation.W}");

            _socketDecorator.SendPose(new Pose(data.Orientation.X, data.Orientation.Y, data.Orientation.Z));
        }

        private void ReceivePoseByWebView(Pose pose)
        {
            _javaScriptInterface.ReceivePose(pose.ToJson());
        }
    }
}