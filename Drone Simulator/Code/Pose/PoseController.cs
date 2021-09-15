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

                Vector3 angles = QuaternionToEuler(NormalizedToDegrees(orientation));
                // Log.Debug(angles.X + " " + angles.Y + " " + angles.Z);
                _socketDecorator.SendPose(new Pose(angles.X, angles.Y, angles.Z));

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
            _javaScriptInterface.ReceivePose(pose.ToJson());
        }

        private Quaternion NormalizedToDegrees(Quaternion q)
        {
            q.X *= 180;
            q.Y *= 180;
            q.Z *= 180;

            return q;
        }

        private Vector3 QuaternionToEuler(Quaternion q)
        {
            Vector3 eulerAngles = new Vector3();
            // X = Convert.ToSingle(Math.Atan2(2.0 * (q.Y * q.Z + q.W * q.X),
            //     q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z)),
            // Y = Convert.ToSingle(Math.Asin(2.0 * (q.X * q.Z - q.W * q.Y))),
            // Z = Convert.ToSingle(Math.Atan2(2.0 * (q.X * q.Y + q.W * q.Z),
            //     q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z))

            // double q2sqr = q.Z * q.Z;
            // double t0 = -2.0 * (q2sqr + q.W * q.W) + 1.0;
            // double t1 = +2.0 * (q.Y * q.Z + q.X * q.W);
            // double t2 = -2.0 * (q.Y * q.W - q.X * q.Z);
            // double t3 = +2.0 * (q.Z * q.W + q.X * q.Y);
            // double t4 = -2.0 * (q.Y * q.Y + q2sqr) + 1.0;
            //
            // t2 = t2 > 1.0 ? 1.0 : t2;
            // t2 = t2 < -1.0 ? -1.0 : t2;
            //
            // eulerAngles.X = Convert.ToSingle(Math.Asin(t2));
            // eulerAngles.Y = Convert.ToSingle(Math.Atan2(t3, t4));
            // eulerAngles.Z = Convert.ToSingle(Math.Atan2(t1, t0));


            // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles#Quaternion_to_Euler_angles_conversion
            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            eulerAngles.X = Convert.ToSingle(Math.Atan2(sinr_cosp, cosr_cosp));

            // pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                eulerAngles.Y = Convert.ToSingle(Math.PI / 2 * Math.Sign(sinp)); // use 90 degrees if out of range
            else
                eulerAngles.Y = Convert.ToSingle(Math.Asin(sinp));

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            eulerAngles.Z = Convert.ToSingle(Math.Atan2(siny_cosp, cosy_cosp));

            eulerAngles.X = q.X;
            eulerAngles.Y = q.Y;
            eulerAngles.Z = q.Z;

            return eulerAngles;
        }
    }
}