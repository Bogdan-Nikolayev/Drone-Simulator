using System;
using Android.Net.Wifi.P2p;

namespace Drone_Simulator.WifiDirect.Listeners
{
    public class WifiDirectConnectionInfoListener : Java.Lang.Object, WifiP2pManager.IConnectionInfoListener
    {
        private readonly Action<WifiP2pInfo> _onConnectionInfoAvailable;

        public WifiDirectConnectionInfoListener(Action<WifiP2pInfo> onConnectionInfoAvailable)
        {
            _onConnectionInfoAvailable = onConnectionInfoAvailable;
        }

        public void OnConnectionInfoAvailable(WifiP2pInfo? info)
        {
            Log.Debug("IsGroupOwner: " + info.IsGroupOwner);

            _onConnectionInfoAvailable?.Invoke(info);
        }
    }
}