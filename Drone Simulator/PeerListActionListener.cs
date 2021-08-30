﻿using System;
using Android.Net.Wifi.P2p;
using Android.Util;

namespace Drone_Simulator
{
    public class PeerListActionListener : Java.Lang.Object, WifiP2pManager.IPeerListListener
    {
        private readonly Action<WifiP2pDeviceList> _onPeersAvailable;
        
        public PeerListActionListener(Action<WifiP2pDeviceList> onPeersAvailable)
        {
            _onPeersAvailable = onPeersAvailable;
        }

        public void OnPeersAvailable(WifiP2pDeviceList? peers)
        {
            Log.Debug("DroneSimulator", "PeerListActionListener OnPeersAvailable");
            
            _onPeersAvailable?.Invoke(peers);
        }
    }
}