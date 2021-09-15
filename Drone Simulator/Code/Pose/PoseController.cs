using System;
using System.Numerics;
using Drone_Simulator.Extensions;
using Java.Util.Concurrent;
using Xamarin.Essentials;

namespace Drone_Simulator.Pose
{
    // TODO: Rename class to more meaningful name.
    // TODO: Refactoring.
    public class PoseController
    {
        private readonly SensorSpeed _speed = SensorSpeed.UI;
        private readonly PoseJavaScriptInterface _javaScriptInterface;
        private readonly PoseSocketDecorator _socketDecorator;
        private readonly MutableRunnable _runnable = new MutableRunnable(() => { });
        // private readonly Thread _thread;
        private readonly IExecutorService _executorService = Executors.NewFixedThreadPool(16);

        private bool _isRunning = false;

        public PoseController(PoseJavaScriptInterface javaScriptInterface, PoseSocketDecorator socketDecorator)
        {
            _javaScriptInterface = javaScriptInterface;
            _socketDecorator = socketDecorator;

            // _thread = new Thread(_runnable);
            // _thread.Start();
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
            catch (FeatureNotSupportedException featureNotSupportedException)
            {
                // Feature not supported on device
            }
            catch (Exception exception)
            {
                // Other error has occurred.
            }
        }

        private void SendPose(object sender, OrientationSensorChangedEventArgs e)
        {
            _runnable.Action = () =>
            {
                Quaternion orientation = e.Reading.Orientation;

                _socketDecorator.SendPose(new Pose(orientation.X, orientation.Y, orientation.Z));

                _isRunning = false;
            };

            // TODO: Refactor this way of checking.
            if (!_isRunning)
            {
                _executorService.Execute(_runnable);

                _isRunning = false;
            }
        }

        private void ReceivePoseByWebView(Pose pose)
        {
            Log.Debug("Pose received: " + pose.ToJson());
            _javaScriptInterface.ReceivePose(pose.ToJson());
        }
    }
}