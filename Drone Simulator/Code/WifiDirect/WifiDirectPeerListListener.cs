using System;
using Android.Net.Wifi.P2p;

namespace Drone_Simulator.WifiDirect
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
            _onPeersAvailable?.Invoke(peers);
        }
    }
}