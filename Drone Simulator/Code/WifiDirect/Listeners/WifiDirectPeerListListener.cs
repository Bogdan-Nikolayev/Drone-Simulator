using System;
using Android.Net.Wifi.P2p;

namespace Drone_Simulator.WifiDirect.Listeners
{
    public class WifiDirectPeerListListener : Java.Lang.Object, WifiP2pManager.IPeerListListener
    {
        private readonly Action<WifiP2pDeviceList> _onPeersAvailable;

        public WifiDirectPeerListListener(Action<WifiP2pDeviceList> onPeersAvailable)
        {
            _onPeersAvailable = onPeersAvailable;
        }

        public void OnPeersAvailable(WifiP2pDeviceList? peers)
        {
            foreach (WifiP2pDevice device in peers.DeviceList)
                Log.Debug(device.DeviceName);

            _onPeersAvailable?.Invoke(peers);
        }
    }
}