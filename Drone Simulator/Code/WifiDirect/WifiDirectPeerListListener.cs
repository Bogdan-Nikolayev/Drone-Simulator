using System;
using Android.Net.Wifi.P2p;
using Android.Util;

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
            Log.Debug(Tag.DroneSimulator, string.Join(" ",
                nameof(WifiDirectPeerListListener), nameof(OnPeersAvailable), peers.DeviceList.Count));

            _onPeersAvailable?.Invoke(peers);
        }
    }
}