using System;
using Android.Net.Wifi.P2p;
using Android.Util;

namespace Drone_Simulator.WifiDirect
{
    public class WifiDirectActionListener : Java.Lang.Object, WifiP2pManager.IActionListener
    {
        private readonly Action _onSuccess;
        private readonly Action<WifiP2pFailureReason> _onFailure;

        public WifiDirectActionListener(Action onSuccess, Action<WifiP2pFailureReason> onFailure)
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        public void OnSuccess()
        {
            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ",
                nameof(WifiDirectActionListener), nameof(OnSuccess)));

            _onSuccess?.Invoke();
        }

        public void OnFailure(WifiP2pFailureReason reason)
        {
            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ",
                nameof(WifiDirectActionListener), nameof(OnFailure), reason));

            _onFailure?.Invoke(reason);
        }
    }
}