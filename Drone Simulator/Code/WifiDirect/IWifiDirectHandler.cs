using System;
using Android.Net.Wifi.P2p;
using Drone_Simulator.WifiDirect.Listeners;

namespace Drone_Simulator.WifiDirect
{
    public interface IWifiDirectHandler
    {
        public bool IsWifiDirectEnabled { get; set; }
        public WifiDirectPeerListListener PeerListListener { get; }
        public WifiDirectConnectionInfoListener ConnectionInfoListener { get; }

        public event Action<WifiP2pInfo> Connected;
    }
}